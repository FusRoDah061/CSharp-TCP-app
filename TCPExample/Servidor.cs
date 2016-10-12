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
    public partial class Servidor : Form
    {
        private int PORT = 5004;

        public Servidor()
        {
            InitializeComponent();
        }

        private void Servidor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btLigaServer_Click(object sender, EventArgs e)
        {
            PORT = int.Parse(textBox3.Text);

            Thread tcpServerRunThread = new Thread(new ThreadStart(TcpServerRun));
            tcpServerRunThread.IsBackground = true;
            tcpServerRunThread.Start();
        }

        private void TcpServerRun()
        {
            try
            {
                TcpListener tcpListener = new TcpListener(IPAddress.Any, PORT);
                tcpListener.Start();
                updateUI("Aguardando conexão...");
                updateUI("Porta: " + PORT.ToString());
                while (true)
                {
                    //Escuta por conexões
                    TcpClient client = tcpListener.AcceptTcpClient();

                    IPEndPoint endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
                    // .. or LocalEndPoint - depending on which end you want to identify

                    IPAddress ipAddress = endPoint.Address;

                    // get the hostname
                    IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
                    string hostName = hostEntry.HostName;

                    // get the port
                    int port = endPoint.Port;

                    updateUI("Cliente conectado:");
                    updateUI("  Endereço:" + ipAddress.ToString());
                    updateUI("  Host: " + hostName);
                    updateUI("  Porta: " + port.ToString());

                    Thread tcpHandlerThread = new Thread(new ParameterizedThreadStart(tcpHandler));
                    tcpHandlerThread.IsBackground = true;
                    tcpHandlerThread.Start(client);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro: " + ex.Message);
            }
        }

        //Gerencia a conexão
        private void tcpHandler(object client)
        {
            try
            {
                TcpClient mClient = (TcpClient)client;
                NetworkStream stream = mClient.GetStream();

                byte[] message = new byte[1024];
                stream.Read(message, 0, message.Length);
                updateUI("Mensagem recebida: ");
                updateUI("  '" + Encoding.UTF8.GetString(message) + "'");

                stream.Close();
                mClient.Close();
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
            try
            {
                Invoke(del);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro: " + ex.Message);
            }
        }
    }
}
