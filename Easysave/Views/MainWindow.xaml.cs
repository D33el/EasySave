using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpftest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WelcomePage.Visibility = Visibility.Collapsed;
            HomePage.Visibility = Visibility.Visible;

            SettingsPage.Visibility = Visibility.Collapsed;
            SavePage.Visibility = Visibility.Collapsed;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            HomePage.Visibility = Visibility.Visible;

            SettingsPage.Visibility = Visibility.Collapsed;
            SavePage.Visibility = Visibility.Collapsed;
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage.Visibility = Visibility.Visible;

            HomePage.Visibility = Visibility.Collapsed;
            SavePage.Visibility = Visibility.Collapsed;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SavePage.Visibility = Visibility.Visible;

            HomePage.Visibility = Visibility.Collapsed;
            SettingsPage.Visibility = Visibility.Collapsed;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OpenExplorer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
