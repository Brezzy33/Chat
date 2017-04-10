using System;
using System.Windows.Forms;
using System.IO;

namespace Chat
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ipTextBox.Text != "" && portTextBox.Text !="")
            {
                try
                {
                    DirectoryInfo data = new DirectoryInfo("Client_info");
                    data.Create();
                    var sw = new StreamWriter(@"Client_info/data_info.txt");
                    sw.WriteLine(ipTextBox.Text + ":" + portTextBox.Text);
                    sw.Close();
                    Application.Restart();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error!"+ex.Message);
                }
            }
            this.Close();
        }
    }
}
