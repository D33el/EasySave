using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace EasySave.ViewModels
{
    public class Cloud
    {
        private SaveViewModel _viewModel = new SaveViewModel();
        private Socket serverSocket;
        private List<Socket> clients = new List<Socket>();
        private bool isRunning = true;
        private object clientsLock = new object();

        public void StartServer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 667));
            serverSocket.Listen(10);

            Thread listenerThread = new Thread(ListenForClients);
            listenerThread.Start();
        }

        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    Socket clientSocket = serverSocket.Accept();

                    lock (clientsLock)
                    {
                        clients.Add(clientSocket);
                    }

                    Thread DataThread = new Thread(()=>SendSaveInfo());
                    DataThread.Start();

                    DataThread.Join();
                }
                catch (SocketException ex)
                {
                    // Handle socket-specific exceptions
                }
            }
        }

        public void SendSaveInfo()
        {
            lock (clientsLock)
            {

            var saveList = _viewModel.Saves;
            string jsonData = JsonSerializer.Serialize(saveList); // Serialize the save list to JSON

            Trace.WriteLine("==========SENDSAVEINFO=====================");
            Trace.WriteLine(jsonData);
            SendDataToAllClients(jsonData);
            }
        }
        public void SendDataToAllClients(string data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            Trace.WriteLine("==========SendDataToAllClients=====================");
            Trace.WriteLine(buffer.Length);
            lock (clientsLock)
            {
                try
                {
                    foreach (Socket client in clients)
                    {
                        client.Send(buffer);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending data to client: {ex.Message}");
                }
                
            }
        }
        private void HandleClientComm(object clientObj)
        {
            Socket clientSocket = (Socket)clientObj;

            try
            {
                // Implement your communication logic here
            }
            catch (Exception ex)
            {
                // Handle communication error
                Console.WriteLine($"Communication error with client: {ex.Message}");
            }
            finally
            {
                lock (clientsLock)
                {
                    clients.Remove(clientSocket);
                    clientSocket.Close();
                }
            }
        }

        public void StopServer()
        {
            isRunning = false;

            lock (clientsLock)
            {
                foreach (Socket client in clients)
                {
                    client.Close();
                }
            }

            serverSocket.Close();
        }
    }
}
