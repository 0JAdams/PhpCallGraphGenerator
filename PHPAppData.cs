using PHPAnalyzer.JavascriptObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PHPAnalyzer
{
    class PHPAppData
    {
        public List<CodeFile> CodeFiles { get; private set; } = new List<CodeFile>();

        public PHPAppData(string pathToPHPCode, BackgroundWorker worker, DoWorkEventArgs e)
        {
            try
            {
                //Parse all PHP and JS files and build the file tree
                ParseAllFiles(pathToPHPCode, worker);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse all code files. ", ex);
            }            
          

        }

        

        private void ParseAllFiles(string pathToCode, BackgroundWorker worker)
        {

            //parse PHP files
            List<FileInfo> filesToParse = new List<FileInfo>(); //stores our list of files
            List<DirectoryInfo> directoriesSearched = new List<DirectoryInfo>(); //stores our list of directories
            //get the list of php files in the selected directory
            DirectoryInfo di = new DirectoryInfo(pathToCode);
            string searchPattern = "*.php";
            FullDirList(di, searchPattern, filesToParse, directoriesSearched);
            int totalCount = filesToParse.Count;
            int visitedCount = 0;
            foreach (FileInfo fi in filesToParse)
            {
                visitedCount++;
                worker.ReportProgress((int)((float)visitedCount / totalCount * 100));
                try
                {
                    if (fi.Exists)
                    {
                        using (StreamReader sr = fi.OpenText())
                        {
                            string entireFile = sr.ReadToEnd();
                            AddPHPFile(entireFile, fi.FullName, fi.Name);
                        }
                    }
                }
                catch (Exception exp)
                {
                    System.Windows.Forms.MessageBox.Show("An error ocurred while trying to open a file: \n" + exp.ToString());
                }
            }

           
            ////parse Javascript files
            //filesToParse = new List<FileInfo>(); //stores our list of files
            //directoriesSearched = new List<DirectoryInfo>(); //stores our list of directories
            ////get the list of php files in the selected directory
            //di = new DirectoryInfo(pathToCode);
            //searchPattern = "*.js";
            //FullDirList(di, searchPattern, filesToParse, directoriesSearched);

            //foreach (FileInfo fi in filesToParse)
            //{
            //    try
            //    {
            //        if (fi.Exists)
            //        {
            //            using (StreamReader sr = fi.OpenText())
            //            {
            //                string entireFile = sr.ReadToEnd();
            //                AddJavascriptFile(entireFile, fi.FullName, fi.Name);
            //            }
            //        }
            //    }
            //    catch (Exception exp)
            //    {
            //        System.Windows.Forms.MessageBox.Show("An error ocurred while trying to open a file: \n" + exp.ToString());
            //    }
            //}

        }

        private void FullDirList(DirectoryInfo dir, string searchPattern, List<FileInfo> filesToParse, List<DirectoryInfo> directoriesSearched)
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
                Console.WriteLine("Directory {0}  \n could not be accessed!", dir.FullName);
                return;  // We already got an error trying to access dir so dont try to access it again
            }

            // process each directory
            // If I have been able to see the files in the directory I should also be able 
            // to look at its directories so I dont think I should place this in a try catch block
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                directoriesSearched.Add(d);
                FullDirList(d, searchPattern, filesToParse, directoriesSearched);
            }

        }


   
        public void AddPHPFile(string fullFileContent, string fullFilePath, string fileName)
        {
            PhpFile newFile = new PhpFile(fullFileContent, fullFilePath, fileName, this);
            CodeFiles.Add(newFile);
        }

        public void AddJavascriptFile(string fullFileContent, string fullFilePath, string fileName)
        {
            JavascriptFile newFile = new JavascriptFile(fullFileContent, fullFilePath, fileName, this);
            CodeFiles.Add(newFile);
        }

        /// <summary>
        /// Gets the PhpFile object for a desired file.
        /// </summary>
        /// <param name="phpFileName">FileName, including extension</param>
        /// <returns>PhpFile for correct file. Returns null if not found</returns>
        public PhpFile FindPhpFileByName(string phpFileName)
        {
            foreach(PhpFile p in CodeFiles.OfType<PhpFile>())
            {
                if(phpFileName!=null && phpFileName.ToLower()==p.FileName.ToLower())
                {
                    return p;
                }
            }
            return null;
        }

        
        /// <summary>
        /// Finds the PhpFunction object relating to a given function within a given class
        /// </summary>
        /// <param name="functionName">Name of function to get</param>
        /// <param name="className">Name of class containing function to get</param>
        /// <returns>PhpFunction matching desired function.  Returns null if not found.</returns>
        public PhpFunction FindPHPFunction(string functionName, string className)
        {
            //this assumes that in each class the function name is unique
            foreach (PhpFile pf in CodeFiles.OfType<PhpFile>())
            {
                foreach (PhpClass pc in pf.Classes)
                {
                    if (pc.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (PhpFunction pfunc in pc.Functions)
                        {
                            if (pfunc.FunctionName.Equals(functionName, StringComparison.OrdinalIgnoreCase))
                            {
                                return pfunc;
                            }
                        }
                    }
                }
            }
            return null;
        }

        
      
        public List<PhpCodeObject> FindCallerObjects(PhpFunction callee)
        {
            List<PhpCodeObject> callerCodeBlocks = new List<PhpCodeObject>();
            foreach(PhpFile pf in CodeFiles.OfType<PhpFile>())
            {
                foreach(PhpClass pc in pf.Classes)
                {
                    foreach(PhpFunction pfunc in pc.Functions)
                    {
                        //check for calls to callee
                        if(pfunc.MakesCallToCallee(callee))
                        {
                            //if found add to list of caller functions
                            callerCodeBlocks.Add(pfunc);
                        }
                    }
                }
                //check snippet (code not found within a class) for calls to function
                if(pf.Snippet.MakesCallToCallee(callee))
                {
                    callerCodeBlocks.Add(pf.Snippet);
                }                
            }
            return callerCodeBlocks;
        }

        internal List<PhpCodeObject> FindCalleeObjects(PhpCodeObject caller)
        {
            List<PhpCodeObject> calleeCodeBlocks = new List<PhpCodeObject>();
            calleeCodeBlocks = caller.ListOfFunctionsCalledTo(this);
            return calleeCodeBlocks;
        }

        public PhpClassGraph BuildSQLFunctionCallGraph(string tableName, bool limitToTable, string attributeName, bool limitToAttribute,
                                                       bool includeSelect, bool includeInsert, bool includeUpdate, bool includeDelete)
        {
            PhpClassGraph graph = new PhpClassGraph();
            List<PhpFunction> startingFunctions = new List<PhpFunction>();
            
            foreach(PhpFile f in CodeFiles.OfType<PhpFile>()) //get all functions that match the desired SQL
            {
                startingFunctions.AddRange(f.ScanForSQL(tableName, limitToAttribute, attributeName, limitToAttribute, 
                                                        includeSelect, includeInsert, includeUpdate, includeDelete));
            }

            //build graph
            graph.CreateCallGraphForSpecificFunctions(this, startingFunctions);
             
            return graph;
        }
    }
}
