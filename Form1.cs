using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Security;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.OverlapRemoval;
using GraphX.PCL.Logic.Models;
using GraphX.Controls;
using QuickGraph;
using MySql.Data;
using MySql.Data.MySqlClient;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;

namespace PHPAnalyzer
{
    public partial class Form1 : Form
    {
        static List<FileInfo> filesToParse = new List<FileInfo>(); //stores our list of files
        static List<DirectoryInfo> directoriesSearched = new List<DirectoryInfo>(); //stores our list of directories
        static List<ParsedFile> foundFiles = new List<ParsedFile>(); //stores our list of files that do contain SQL
        static List<string> namesOfAllFunctions = new List<string>(); //stores a list of names of ALL functions found
        static List<string> namesOfAllDistinctFunctions = new List<string>();
        static List<string> namesOfAllDuplicateFunctions = new List<string>();
        static byte[] entropy = System.Text.Encoding.Unicode.GetBytes("td7CdrTGUU670k3tsEhw"); //salt value for storing/reading

        PhpClassGraph FullGraph = new PhpClassGraph();

        List<string> operations = new List<string>();
        static PHPAppData fileTree;
        bool treeBuilt = false;
       
        private ZoomControl _zoomctrl;
        private GraphAreaExample _gArea;
        private ZoomControl _zoomctrl_full;
        private GraphAreaExample _gArea_full;

        public Form1()
        {
            InitializeComponent();

            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

           


            textBox_GraphFunctionClassName.Text = Properties.Settings.Default.LastSearchedClass;
            textBox_GraphFunction_FunctionName.Text = Properties.Settings.Default.LastSearchedFunction;

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.UserHasConfigured)
            {
                Form config = new Configure();
                config.ShowDialog(this);
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
           
            
            
        }
  
      
        static void FullDirList(DirectoryInfo dir, string searchPattern)
        {
            // Console.WriteLine("Directory {0}", dir.FullName);
            // list the files
            try
            {
                foreach (FileInfo f in dir.GetFiles(searchPattern))
                {
                    //Console.WriteLine("File {0}", f.FullName);
                    filesToParse.Add(f);
                }
            }
            catch
            {
                Console.WriteLine("Directory {0}  \n could not be accessed!!!!", dir.FullName);
                return;  // We alredy got an error trying to access dir so dont try to access it again
            }

            // process each directory
            // If I have been able to see the files in the directory I should also be able 
            // to look at its directories so I dont think I should place this in a try catch block
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                directoriesSearched.Add(d);
                FullDirList(d, searchPattern);
            }

        }
        
        /// <summary>
        /// Decrypts a string stored in the user config settings and returns a secure string with its value
        /// </summary>
        /// <param name="data">Encrypted string from user config settings to decrypt</param>
        /// <returns>SecureString containing stored user config setting</returns>
        private SecureString DecryptStoredSettingToSecureString(string data)
        {
            try
            {
                byte[] bdata = Convert.FromBase64String(data);
                byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                    bdata,
                    entropy,
                    System.Security.Cryptography.DataProtectionScope.CurrentUser);
                string results = Encoding.Unicode.GetString(decryptedData);
                SecureString secure = new SecureString();
                foreach (char c in results)
                {
                    secure.AppendChar(c);
                }
                secure.MakeReadOnly();
                return secure;
            }
            catch (Exception)
            {

                return new SecureString();
            }
        }

        /// <summary>
        /// Builds a tree based on the PHP found in the selected folder
        /// </summary>
        /// <returns></returns>
        private void BuildDocumentTree()
        {            
            currentOperationProgressBar.Maximum = 100;
            currentOperationProgressBar.Value = 0;
            currentOperationProgressBar.Update();
            backgroundWorker1.RunWorkerAsync(); //build tree asyncronously            
        }


        /// <summary>
        /// Button event to build a tree based on the PHP found in the selected folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buildTreeButton_Click(object sender, EventArgs e)
        {
            if(!treeBuilt)
            {
                buildTreeButton.Enabled = false;
                BuildDocumentTree();               
                
            }                        
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            BackgroundWorker worker = sender as BackgroundWorker;
            //get stored data about DBApp and DBMS
            string fileLocation = Properties.Settings.Default.DBAppLocation;
            //create tree
            
            e.Result = new PHPAppData(fileLocation, worker, e);
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error !=null)
            {
                buildTreeButton.Enabled = true;
                System.Windows.Forms.MessageBox.Show(e.Error.Message);
                
            }
            else if (e.Cancelled)
            {
                //currently doesn't have cancelled enabled
                buildTreeButton.Enabled = true;
                
            }
            else
            {
                fileTree = (PHPAppData)e.Result;
                treeBuiltTextBox.Text = "True";
                treeBuiltTextBox.BackColor = Color.LightGreen;
                treeBuilt = true;
                tabControl_FunctionTabs.Enabled = true;
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            currentOperationProgressBar.Value = e.ProgressPercentage;
            currentOperationProgressBar.Update();
        }


      

        /// <summary>
        /// Button event to build a graph of a particular PHP function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_GraphFunction_Click(object sender, EventArgs e)
        {
            PhpFunction startingPoint = fileTree.FindPHPFunction(textBox_GraphFunction_FunctionName.Text, textBox_GraphFunctionClassName.Text);
            if(startingPoint!=null)
            {
                List<PhpFunction> functionAsList = new List<PhpFunction>();
                functionAsList.Add(startingPoint);
                PhpClassGraph graph = new PhpClassGraph();
                graph.CreateCallGraphForSpecificFunctions(fileTree, functionAsList);
                //generate GraphX graph
                
               
                elementHost_GraphXWPFHost.Child = GenerateWpfGraphVisuals(graph, ref _zoomctrl, ref _gArea);
                _gArea.GenerateGraph(true);
                _gArea.SetVerticesDrag(true, true);
                _zoomctrl.ZoomToFill();
            }
        }

        private ZoomControl GenerateWpfGraphVisuals(PhpClassGraph graph, ref ZoomControl zoomctrl, ref GraphAreaExample gArea)
        {
            zoomctrl = new ZoomControl();
            ZoomControl.SetViewFinderVisibility(zoomctrl, Visibility.Visible);
            /* ENABLES WINFORMS HOSTING MODE --- >*/
            var logic = new GXLogicCore<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>();
            gArea = new GraphAreaExample() { EnableWinFormsHostingMode = true, LogicCore = logic };
            gArea.RelayoutFinished += gArea_RelayoutFinished;

            logic.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.LinLog;
            logic.DefaultLayoutAlgorithmParams = logic.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.LinLog);

            ((LinLogLayoutParameters)logic.DefaultLayoutAlgorithmParams).AttractionExponent = 2;
            ((LinLogLayoutParameters)logic.DefaultLayoutAlgorithmParams).GravitationMultiplier = .5;
            ((LinLogLayoutParameters)logic.DefaultLayoutAlgorithmParams).IterationCount = 150;
            ((LinLogLayoutParameters)logic.DefaultLayoutAlgorithmParams).RepulsiveExponent = 0;
            logic.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            logic.DefaultOverlapRemovalAlgorithmParams = logic.AlgorithmFactory.CreateOverlapRemovalParameters(OverlapRemovalAlgorithmTypeEnum.FSA);

            ((OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams).HorizontalGap = 10;
            ((OverlapRemovalParameters)logic.DefaultOverlapRemovalAlgorithmParams).VerticalGap = 10;
            logic.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.None;
            logic.AsyncAlgorithmCompute = false;

            logic.Graph = GenerateGraph(graph);


            zoomctrl.Content = gArea;
            

            var myResourceDictionary = new ResourceDictionary { Source = new Uri("Templates\\template.xaml", UriKind.Relative) };
            zoomctrl.Resources.MergedDictionaries.Add(myResourceDictionary);

            return zoomctrl;
        }

       

        void gArea_RelayoutFinished(object sender, EventArgs e)
        {
            _zoomctrl.ZoomToFill();
            _zoomctrl_full.ZoomToFill();
        }


        /// <summary>
        /// Generates dataVertex nodes and 
        /// </summary>
        /// <param name="graph">PhpClassGraph object containing nodes and edges for converting into displayable graph</param>
        /// <returns>GraphExample - completed graph for displaying</returns>
        private GraphExample GenerateGraph(PhpClassGraph graph)
        {
            var dataGraph = new GraphExample();
            foreach(PhpCodeObject po in graph.PhpFunctionNodes)
            {
                DataVertex dataVertex = new DataVertex();
                if(po is PhpFunction)
                {
                    dataVertex = new DataVertex((po as PhpFunction).ParentClass.ClassName + "." + (po as PhpFunction).FunctionName+"\r\n"+ (po as PhpFunction).ParentClass.ParentFile.FullFilePath);                    
                }
                else if (po is PhpSnippet)
                {
                    dataVertex = new DataVertex("PHP Code Block From: \r\n" + (po as PhpSnippet).ParentFile.FullFilePath);                                     
                }
                dataVertex.PhpObjectNode = po;
                VertexControl vertexControl = new VertexControl(dataVertex);
                dataGraph.AddVertex(dataVertex);
            }
            //var vlist = dataGraph.Vertices.ToList();
            //create edges between vertices
            DataEdge dataEdge = new DataEdge();
            foreach (GraphEdge ge in graph.PhpFunctionEdges)
            {

                DataVertex fromVertex = new DataVertex();
                DataVertex toVertex = new DataVertex();

                foreach(DataVertex dv in dataGraph.Vertices)
                {
                    if(dv.MatchesPhpCodeObjectNode(ge.FromThisFunction))
                    {
                        fromVertex = dv;
                    }
                    if (dv.MatchesPhpCodeObjectNode(ge.ToThisFunction))
                    {
                        toVertex = dv;
                    }
                }

                dataEdge = new DataEdge(fromVertex, toVertex) { Text = string.Format("{0} -> {1}", fromVertex.Text, toVertex.Text) };
                dataGraph.AddEdge(dataEdge);
            }


            return dataGraph;
        }
        
        

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form config = new Configure();
            config.ShowDialog(this);           
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_GenerateFullGraph_Click(object sender, EventArgs e)
        {

            currentOperationProgressBar.Value = 0;
            currentOperationProgressBar.Update();
            _zoomctrl_full = new ZoomControl();
            
            PhpClassGraph graph = new PhpClassGraph();
            graph.CreateCallGraphForEntireTree(fileTree);
            //generate GraphX graph

            
            elementHost_FullPHPGraphHost.Child = GenerateWpfGraphVisuals(graph, ref _zoomctrl_full, ref _gArea_full);
            _gArea_full.GenerateGraph(true);
            _gArea_full.SetVerticesDrag(true, true);
            _zoomctrl_full.ZoomToFill();


        }

        private void textBox_GraphFunctionClassName_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastSearchedClass = textBox_GraphFunctionClassName.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox_GraphFunction_FunctionName_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastSearchedFunction = textBox_GraphFunction_FunctionName.Text;
            Properties.Settings.Default.Save();
        }

        private void sourceWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.bing.com");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form about = new About();
            about.ShowDialog(this);
        }

        
    }
}
