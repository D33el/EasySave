using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace EasySaveCloud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window 
    {

        private Thread _pollingThread;
        private volatile bool _continuePolling = true;
        private App _app = (App)Application.Current;

        public MainWindow()
        {
            InitializeComponent();
            StartPollingThread();
        }

        private void StartPollingThread()
        {
            _pollingThread = new Thread(PollingMethod);
            _pollingThread.Start();
        }

        private void PollingMethod()
        {
            while (_continuePolling)
            {
                if (_app.ReceivedDataQueue.TryDequeue(out string data))
                {
                    Trace.WriteLine("==============fdfsfsdfsdfsdsfh===============");
                    Trace.WriteLine(data);
                    Dispatcher.Invoke(() => UpdateUI(data));
                }

                Thread.Sleep(500); // Polling interval
            }
        }

        private void UpdateUI(string data)
        {
            SaveHandling saveHandling = new SaveHandling();
            List<SaveItem> saves = saveHandling.DeserializeSavesJson(data);
            DisplaySaves(saves);
            Trace.WriteLine("================Saves Length===============");
            Trace.WriteLine(saves.Count);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _continuePolling = false;
            _pollingThread.Join();
        }

        private void DisplaySaves(List<SaveItem> saves)
        {
            SaveList.ItemsSource = saves;
        }

        private void SendDataToServer(string data)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 667); // Adresse et port du serveur
                NetworkStream stream = client.GetStream();
                byte[] message = Encoding.ASCII.GetBytes(data);
                stream.Write(message, 0, message.Length);
                client.Close();
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de communication
                MessageBox.Show($"Erreur de communication avec le serveur : {ex.Message}");
            }
        }

        private void stop_click(object sender, RoutedEventArgs e)
        {
            SendDataToServer("stop");
        }
        private void resume_Click(object sender, RoutedEventArgs e)
        {

            SendDataToServer("resume");
        }
        private void pause_Click(object sender, RoutedEventArgs e)
        {

            SendDataToServer("pause");
        }
    }

}
