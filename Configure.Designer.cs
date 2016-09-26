namespace PHPAnalyzer
{
    partial class Configure
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
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.textBox_PHPApp_Location = new System.Windows.Forms.TextBox();
            this.button_FolderPicker = new System.Windows.Forms.Button();
            this.folderBrowserDialog_Configure = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_Save
            // 
            this.button_Save.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_Save.Location = new System.Drawing.Point(109, 436);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 23);
            this.button_Save.TabIndex = 0;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button_Cancel.Location = new System.Drawing.Point(190, 436);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // textBox_DBApp_Location
            // 
            this.textBox_PHPApp_Location.Location = new System.Drawing.Point(16, 80);
            this.textBox_PHPApp_Location.Name = "textBox_DBApp_Location";
            this.textBox_PHPApp_Location.Size = new System.Drawing.Size(206, 20);
            this.textBox_PHPApp_Location.TabIndex = 3;
            // 
            // button_FolderPicker
            // 
            this.button_FolderPicker.Location = new System.Drawing.Point(231, 80);
            this.button_FolderPicker.Name = "button_FolderPicker";
            this.button_FolderPicker.Size = new System.Drawing.Size(106, 23);
            this.button_FolderPicker.TabIndex = 4;
            this.button_FolderPicker.Text = "Choose Folder";
            this.button_FolderPicker.UseVisualStyleBackColor = true;
            this.button_FolderPicker.Click += new System.EventHandler(this.button_FolderPicker_Click);
            // 
            // folderBrowserDialog_Configure
            // 
            this.folderBrowserDialog_Configure.Description = "Choose the folder to scan";
            this.folderBrowserDialog_Configure.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "DB App Root Location:";
            // 
            // Configure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 471);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_FolderPicker);
            this.Controls.Add(this.textBox_PHPApp_Location);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Save);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Configure";
            this.Text = "Database Auto Awesome - Configure";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Configure_FormClosing_1);
            this.Load += new System.EventHandler(this.Configure_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TextBox textBox_PHPApp_Location;
        private System.Windows.Forms.Button button_FolderPicker;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_Configure;
        private System.Windows.Forms.Label label2;
    }
}