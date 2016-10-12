using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TCPExample
{
    public partial class Cliente : Form
    {
        private string IPADDRESS = "192.168.0.186";
        private int PORT = 5004;
        private bool estaConetado = false;
        string mensagem;

        public Cliente()
        {
            InitializeComponent();
        }

        private void Cliente_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void ConnectAsClient()
        {
            try
            {
                TcpClient client = new TcpClient();
                //Conecta no servidor
                client.Connect(IPAddress.Parse(IPADDRESS), PORT);
                updateUI("Conectado:");
                updateUI("  Endereço: " + IPADDRESS);
                updateUI("  Porta: " + PORT);
                estaConetado = true;

                NetworkStream stream = client.GetStream();

                string msg = "Conectado";
                //Envia uma mensagem pro servidor
                byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
                stream.Write(msgBytes, 0, msgBytes.Length);

                updateUI("Mensagem enviada: ");
                updateUI("  '" + msg + "'");

                //Fecha tudo assim que não for mais usar
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro: " + ex.Message);
            }
        }

        private void updateUI(string message)
        {
            Func<int> del = delegate()
            {
                textBox1.AppendText(message + System.Environment.NewLine);
                return 0;
            };
            Invoke(del);
        }

        private void btLigaServer_Click(object sender, EventArgs e)
        {
            IPADDRESS = textBox2.Text;
            PORT = int.Parse(textBox3.Text);

            Thread mThread = new Thread(new ThreadStart(ConnectAsClient));
            mThread.IsBackground = true;
            mThread.Start();
        }
    }
}
