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
        //Создаем переменные для Ip, порта и потока.
        static public Socket Client;
        private IPAddress ip = null;
        private int port = 0;
        private Thread th;

        public Form1()
        {
            InitializeComponent();
            chatTextBox.ReadOnly = true;
            msgTextBox.Enabled = false;
            sndBtn.Enabled = false;
            try
            {
                var sr = new StreamReader(@"Client_info/data_info.txt");
                string buffer = sr.ReadToEnd();
                sr.Close();
                String[] connect_info = buffer.Split(':');
                ip = IPAddress.Parse(connect_info[1]);
                label4.ForeColor = Color.Green;
                label4.Text = "IP Сервера: " + connect_info[0] + "Порта сервера: " + connect_info[1];
            }
            catch
            {
                label4.ForeColor = Color.Red;
                label4.Text = "Настройки не найдены!";
                Settings stform = new Settings();
                stform.Show();
            }
        }

        void SendMessage(string message)
        {
            if (message != "" && message != " ")
            {
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes(message);
                Client.Send(buffer);
            }
        }

        void ReceveMessage()
        {
            byte[] buffer = new byte[1024];
            for (int i=0; i< buffer.Length; i++)
            {
                buffer[i] = 0;
            }
            for (;;)
            {
                Client.Receive(buffer);
                string Message = Encoding.UTF8.GetString(buffer);
                for (int i=0; i<buffer.Length;i++)
                {
                    buffer[i] = 0;
                }
                this.Invoke((MethodInvoker)delegate ()
                {
                    chatTextBox.AppendText(Message);
                });
            }
        } 

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings stn = new Settings();
            stn.Show();
        }

        private void sndBtn_Click(object sender, EventArgs e)
        {
            SendMessage("\n" + textBox1.Text + ": " + msgTextBox.Text);
            msgTextBox.Clear();
        }

        private void joinBtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!=""&&textBox1.Text!=" ")
            {
                msgTextBox.Enabled = true;
                sndBtn.Enabled = true;
                sndBtn.Text = "Connect...";
                Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (ip !=null)
                {
                    Client.Connect(ip, port);
                    th = new Thread(delegate () { ReceveMessage(); });
                }
            }
        }
    }
}
