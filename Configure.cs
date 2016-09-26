using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PHPAnalyzer
{
    public partial class Configure : Form
    {
        static byte[] entropy = System.Text.Encoding.Unicode.GetBytes("td7CdrTGUU670k3tsEhw"); //salt value for storing/reading

        public Configure()
        {
            InitializeComponent();
        }
        private void Configure_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void Configure_FormClosing_1(object sender, FormClosingEventArgs e)
        {
              
        }



        private void SetSettings()
        {
            //save fields back to application settings
            Properties.Settings.Default.DBAppLocation = textBox_PHPApp_Location.Text;           
            Properties.Settings.Default.Save();
            
        }

        private void LoadSettings()
        {
            //load all stored settings into fields
            textBox_PHPApp_Location.Text = Properties.Settings.Default.DBAppLocation;
            
        }

        private void button_FolderPicker_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog_Configure.ShowDialog();

            if (result == DialogResult.OK)
            {
                string folder = folderBrowserDialog_Configure.SelectedPath;
                textBox_PHPApp_Location.Text = folder;
            }
        }

       

        private void button_Save_Click(object sender, EventArgs e)
        {
            if(textBox_PHPApp_Location.Text != "")
            {
                Properties.Settings.Default.UserHasConfigured = true;
                Properties.Settings.Default.Save();
                SetSettings();
                this.Close();
            }
            
        }

        private string DecryptStoredSetting(string data)
        {
            try
            {
                if(data!="")
                {
                    byte[] bdata = Convert.FromBase64String(data);
                    byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                        bdata,
                        entropy,
                        System.Security.Cryptography.DataProtectionScope.CurrentUser);
                    string results = Encoding.Unicode.GetString(decryptedData);
                    return results;
                }
                else
                {
                    return "";
                }
                
            }
            catch (Exception)
            {

                return "";
            }
        }

        private string EncryptSettingToStore(string data)
        {
            byte[] encryptedData = System.Security.Cryptography.ProtectedData.Protect(
                Encoding.Unicode.GetBytes(data),
                entropy,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
        
    }
}
