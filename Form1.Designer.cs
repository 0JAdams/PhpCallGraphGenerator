namespace PHPAnalyzer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.currentOperationProgressBar = new System.Windows.Forms.ProgressBar();
            this.folderScanProgressBarLabel = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl_FunctionTabs = new System.Windows.Forms.TabControl();
            this.tabGraphAll = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.button_GenerateFullGraph = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.elementHost_FullPHPGraphHost = new System.Windows.Forms.Integration.ElementHost();
            this.tabGraphFunction = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.elementHost_GraphXWPFHost = new System.Windows.Forms.Integration.ElementHost();
            this.button_GraphFunction = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_GraphFunction_FunctionName = new System.Windows.Forms.TextBox();
            this.label_GraphFunction_ClassName = new System.Windows.Forms.Label();
            this.textBox_GraphFunctionClassName = new System.Windows.Forms.TextBox();
            this.buildTreeButton = new System.Windows.Forms.Button();
            this.treeBuiltTextBox = new System.Windows.Forms.TextBox();
            this.label_treeBuilt = new System.Windows.Forms.Label();
            this.menuStrip_Master = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabControl_FunctionTabs.SuspendLayout();
            this.tabGraphAll.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabGraphFunction.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip_Master.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentOperationProgressBar
            // 
            this.currentOperationProgressBar.Location = new System.Drawing.Point(267, 51);
            this.currentOperationProgressBar.Name = "currentOperationProgressBar";
            this.currentOperationProgressBar.Size = new System.Drawing.Size(518, 23);
            this.currentOperationProgressBar.TabIndex = 8;
            // 
            // folderScanProgressBarLabel
            // 
            this.folderScanProgressBarLabel.AutoSize = true;
            this.folderScanProgressBarLabel.Location = new System.Drawing.Point(264, 35);
            this.folderScanProgressBarLabel.Name = "folderScanProgressBarLabel";
            this.folderScanProgressBarLabel.Size = new System.Drawing.Size(115, 13);
            this.folderScanProgressBarLabel.TabIndex = 9;
            this.folderScanProgressBarLabel.Text = "Current Task Progress:";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "php";
            this.saveFileDialog1.Filter = "PHP File (*.php)|*.php";
            this.saveFileDialog1.InitialDirectory = "C:\\ThesisStuffs";
            this.saveFileDialog1.Title = "Specify PHP File To Save Results To";
            // 
            // tabControl_FunctionTabs
            // 
            this.tabControl_FunctionTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_FunctionTabs.Controls.Add(this.tabGraphFunction);
            this.tabControl_FunctionTabs.Controls.Add(this.tabGraphAll);
            this.tabControl_FunctionTabs.Enabled = false;
            this.tabControl_FunctionTabs.Location = new System.Drawing.Point(12, 95);
            this.tabControl_FunctionTabs.Name = "tabControl_FunctionTabs";
            this.tabControl_FunctionTabs.SelectedIndex = 0;
            this.tabControl_FunctionTabs.Size = new System.Drawing.Size(1508, 805);
            this.tabControl_FunctionTabs.TabIndex = 38;
            // 
            // tabGraphAll
            // 
            this.tabGraphAll.BackColor = System.Drawing.Color.GhostWhite;
            this.tabGraphAll.Controls.Add(this.label1);
            this.tabGraphAll.Controls.Add(this.button_GenerateFullGraph);
            this.tabGraphAll.Controls.Add(this.panel1);
            this.tabGraphAll.Location = new System.Drawing.Point(4, 22);
            this.tabGraphAll.Name = "tabGraphAll";
            this.tabGraphAll.Size = new System.Drawing.Size(1500, 779);
            this.tabGraphAll.TabIndex = 4;
            this.tabGraphAll.Text = "Full Code Graph";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1133, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // button_GenerateFullGraph
            // 
            this.button_GenerateFullGraph.Location = new System.Drawing.Point(7, 27);
            this.button_GenerateFullGraph.Name = "button_GenerateFullGraph";
            this.button_GenerateFullGraph.Size = new System.Drawing.Size(110, 23);
            this.button_GenerateFullGraph.TabIndex = 8;
            this.button_GenerateFullGraph.Text = "Generate Full Graph";
            this.button_GenerateFullGraph.UseVisualStyleBackColor = true;
            this.button_GenerateFullGraph.Click += new System.EventHandler(this.button_GenerateFullGraph_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.elementHost_FullPHPGraphHost);
            this.panel1.Location = new System.Drawing.Point(4, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1493, 720);
            this.panel1.TabIndex = 7;
            // 
            // elementHost_FullPHPGraphHost
            // 
            this.elementHost_FullPHPGraphHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost_FullPHPGraphHost.BackColor = System.Drawing.Color.White;
            this.elementHost_FullPHPGraphHost.Location = new System.Drawing.Point(3, 3);
            this.elementHost_FullPHPGraphHost.Name = "elementHost_FullPHPGraphHost";
            this.elementHost_FullPHPGraphHost.Size = new System.Drawing.Size(1485, 712);
            this.elementHost_FullPHPGraphHost.TabIndex = 6;
            this.elementHost_FullPHPGraphHost.Text = "elementHost1";
            this.elementHost_FullPHPGraphHost.Child = null;
            // 
            // tabGraphFunction
            // 
            this.tabGraphFunction.BackColor = System.Drawing.Color.GhostWhite;
            this.tabGraphFunction.Controls.Add(this.panel2);
            this.tabGraphFunction.Controls.Add(this.button_GraphFunction);
            this.tabGraphFunction.Controls.Add(this.label11);
            this.tabGraphFunction.Controls.Add(this.textBox_GraphFunction_FunctionName);
            this.tabGraphFunction.Controls.Add(this.label_GraphFunction_ClassName);
            this.tabGraphFunction.Controls.Add(this.textBox_GraphFunctionClassName);
            this.tabGraphFunction.Location = new System.Drawing.Point(4, 22);
            this.tabGraphFunction.Name = "tabGraphFunction";
            this.tabGraphFunction.Size = new System.Drawing.Size(1500, 779);
            this.tabGraphFunction.TabIndex = 3;
            this.tabGraphFunction.Text = "Graph Specific Function";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.elementHost_GraphXWPFHost);
            this.panel2.Location = new System.Drawing.Point(4, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1493, 719);
            this.panel2.TabIndex = 6;
            // 
            // elementHost_GraphXWPFHost
            // 
            this.elementHost_GraphXWPFHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.elementHost_GraphXWPFHost.BackColor = System.Drawing.Color.White;
            this.elementHost_GraphXWPFHost.Location = new System.Drawing.Point(3, 3);
            this.elementHost_GraphXWPFHost.Name = "elementHost_GraphXWPFHost";
            this.elementHost_GraphXWPFHost.Size = new System.Drawing.Size(1485, 711);
            this.elementHost_GraphXWPFHost.TabIndex = 5;
            this.elementHost_GraphXWPFHost.Text = "elementHost1";
            this.elementHost_GraphXWPFHost.Child = null;
            // 
            // button_GraphFunction
            // 
            this.button_GraphFunction.Location = new System.Drawing.Point(374, 27);
            this.button_GraphFunction.Name = "button_GraphFunction";
            this.button_GraphFunction.Size = new System.Drawing.Size(100, 23);
            this.button_GraphFunction.TabIndex = 4;
            this.button_GraphFunction.Text = "Build";
            this.button_GraphFunction.UseVisualStyleBackColor = true;
            this.button_GraphFunction.Click += new System.EventHandler(this.button_GraphFunction_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(197, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Function Name:";
            // 
            // textBox_GraphFunction_FunctionName
            // 
            this.textBox_GraphFunction_FunctionName.Location = new System.Drawing.Point(197, 29);
            this.textBox_GraphFunction_FunctionName.Name = "textBox_GraphFunction_FunctionName";
            this.textBox_GraphFunction_FunctionName.Size = new System.Drawing.Size(160, 20);
            this.textBox_GraphFunction_FunctionName.TabIndex = 2;
            this.textBox_GraphFunction_FunctionName.TextChanged += new System.EventHandler(this.textBox_GraphFunction_FunctionName_TextChanged);
            // 
            // label_GraphFunction_ClassName
            // 
            this.label_GraphFunction_ClassName.AutoSize = true;
            this.label_GraphFunction_ClassName.Location = new System.Drawing.Point(17, 10);
            this.label_GraphFunction_ClassName.Name = "label_GraphFunction_ClassName";
            this.label_GraphFunction_ClassName.Size = new System.Drawing.Size(66, 13);
            this.label_GraphFunction_ClassName.TabIndex = 1;
            this.label_GraphFunction_ClassName.Text = "Class Name:";
            // 
            // textBox_GraphFunctionClassName
            // 
            this.textBox_GraphFunctionClassName.Location = new System.Drawing.Point(17, 29);
            this.textBox_GraphFunctionClassName.Name = "textBox_GraphFunctionClassName";
            this.textBox_GraphFunctionClassName.Size = new System.Drawing.Size(160, 20);
            this.textBox_GraphFunctionClassName.TabIndex = 0;
            this.textBox_GraphFunctionClassName.TextChanged += new System.EventHandler(this.textBox_GraphFunctionClassName_TextChanged);
            // 
            // buildTreeButton
            // 
            this.buildTreeButton.Location = new System.Drawing.Point(20, 51);
            this.buildTreeButton.Name = "buildTreeButton";
            this.buildTreeButton.Size = new System.Drawing.Size(113, 23);
            this.buildTreeButton.TabIndex = 0;
            this.buildTreeButton.Text = "Process PHP";
            this.buildTreeButton.UseVisualStyleBackColor = true;
            this.buildTreeButton.Click += new System.EventHandler(this.buildTreeButton_Click);
            // 
            // treeBuiltTextBox
            // 
            this.treeBuiltTextBox.BackColor = System.Drawing.Color.LightCoral;
            this.treeBuiltTextBox.Enabled = false;
            this.treeBuiltTextBox.Location = new System.Drawing.Point(149, 51);
            this.treeBuiltTextBox.Name = "treeBuiltTextBox";
            this.treeBuiltTextBox.Size = new System.Drawing.Size(100, 20);
            this.treeBuiltTextBox.TabIndex = 39;
            this.treeBuiltTextBox.Text = "False";
            // 
            // label_treeBuilt
            // 
            this.label_treeBuilt.AutoSize = true;
            this.label_treeBuilt.Location = new System.Drawing.Point(146, 35);
            this.label_treeBuilt.Name = "label_treeBuilt";
            this.label_treeBuilt.Size = new System.Drawing.Size(88, 13);
            this.label_treeBuilt.TabIndex = 40;
            this.label_treeBuilt.Text = "PHP Processed?";
            // 
            // menuStrip_Master
            // 
            this.menuStrip_Master.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip_Master.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip_Master.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_Master.Name = "menuStrip_Master";
            this.menuStrip_Master.Size = new System.Drawing.Size(1532, 24);
            this.menuStrip_Master.TabIndex = 41;
            this.menuStrip_Master.Text = "Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.configureToolStripMenuItem.Text = "Configure";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceWebsiteToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // sourceWebsiteToolStripMenuItem
            // 
            this.sourceWebsiteToolStripMenuItem.Name = "sourceWebsiteToolStripMenuItem";
            this.sourceWebsiteToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.sourceWebsiteToolStripMenuItem.Text = "Source Website";
            this.sourceWebsiteToolStripMenuItem.Click += new System.EventHandler(this.sourceWebsiteToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1532, 903);
            this.Controls.Add(this.label_treeBuilt);
            this.Controls.Add(this.treeBuiltTextBox);
            this.Controls.Add(this.buildTreeButton);
            this.Controls.Add(this.tabControl_FunctionTabs);
            this.Controls.Add(this.folderScanProgressBarLabel);
            this.Controls.Add(this.currentOperationProgressBar);
            this.Controls.Add(this.menuStrip_Master);
            this.MainMenuStrip = this.menuStrip_Master;
            this.Name = "Form1";
            this.Text = "PHP Call Graph Builder";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tabControl_FunctionTabs.ResumeLayout(false);
            this.tabGraphAll.ResumeLayout(false);
            this.tabGraphAll.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabGraphFunction.ResumeLayout(false);
            this.tabGraphFunction.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.menuStrip_Master.ResumeLayout(false);
            this.menuStrip_Master.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar currentOperationProgressBar;
        private System.Windows.Forms.Label folderScanProgressBarLabel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabControl tabControl_FunctionTabs;
        private System.Windows.Forms.Button buildTreeButton;
        private System.Windows.Forms.TextBox treeBuiltTextBox;
        private System.Windows.Forms.Label label_treeBuilt;
        private System.Windows.Forms.TabPage tabGraphFunction;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_GraphFunction_FunctionName;
        private System.Windows.Forms.Label label_GraphFunction_ClassName;
        private System.Windows.Forms.TextBox textBox_GraphFunctionClassName;
        private System.Windows.Forms.Button button_GraphFunction;
        private System.Windows.Forms.Integration.ElementHost elementHost_GraphXWPFHost;
        private System.Windows.Forms.MenuStrip menuStrip_Master;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.TabPage tabGraphAll;
        private System.Windows.Forms.Integration.ElementHost elementHost_FullPHPGraphHost;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sourceWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_GenerateFullGraph;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

