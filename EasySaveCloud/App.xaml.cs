using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace EasySaveCloud
{
    public partial class App : Application
    {
        private Socket _client;
        private Thread _receiveThread;
        public ConcurrentQueue<string> ReceivedDataQueue { get; private set; }


        public event Action<string> OnDataReceived;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ReceivedDataQueue = new ConcurrentQueue<string>();
            bool isConnected = ConnectToServer();
            while (!isConnected)
            {
                ConnectToServer();
            }
        }


        public bool ConnectToServer()
        {
            try
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 667));

                _receiveThread = new Thread(()=>ReceiveData());
                _receiveThread.Start();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to server: {ex.Message}");
                return false;
            }
        }

        public void SendData(string data)
        {
            try
            {
                byte[] message = Encoding.ASCII.GetBytes(data);
                _client.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending data: {ex.Message}");
            }
        }

        private void ReceiveData()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;
                while (_client.Connected)
                {
                    bytesRead = _client.Receive(buffer);
                    if (bytesRead > 0)
                    {
                        string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Trace.WriteLine("Received Data: " + receivedData);
                        ReceivedDataQueue.Enqueue(receivedData);
                        Trace.WriteLine("==============================");
                        Trace.WriteLine(ReceivedDataQueue.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving data: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            try
            {
                _client.Shutdown(SocketShutdown.Both);
                _client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during disconnection: {ex.Message}");
            }
        }
    }
}

