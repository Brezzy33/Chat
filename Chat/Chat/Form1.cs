using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace Chat
{
    public partial class Form1 : Form
    {
        static public Socket Client;
        private IPAddress ip = null;
        public Form1()
        {
            InitializeComponent();
            chatTextBox.ReadOnly = true;
            msgTextBox.Enabled = false;
            sndBtn.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings stn = new Settings();
            stn.Show();
        }
    }
}
