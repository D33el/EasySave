using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Collections;
using EasySave.ViewModels;

namespace wpftest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SaveViewModel viewModel = new SaveViewModel();
        public MainWindow()
        {
            InitializeComponent();
            WelcomePage.Visibility = Visibility.Collapsed;
            HomePage.Visibility = Visibility.Visible;
            SettingsPage.Visibility = Visibility.Collapsed;
            SavePage.Visibility = Visibility.Collapsed;
            displaySaveList();
            
        }

        public void displaySaveList()
        {
            SaveList.ItemsSource = viewModel.GetSaveList();
            
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

        private void CreateSave_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = SaveType.SelectedItem as ComboBoxItem;
            string savename = SaveName.Text;
            string savetype = selectedComboBoxItem.Tag.ToString();
            string sourcepath = SourcePath.Text;
            viewModel.InitializeSave(savename,savetype,sourcepath,0);
            SaveName.Clear();
            SaveType.SelectedItem = null;
            SourcePath.Clear();
            displaySaveList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_click(object sender, RoutedEventArgs e)
        {
            foreach (var item in SaveList.Items)
            {
                var container = SaveList.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                CheckBox checkBox = FindChild<CheckBox>(container);

                if (checkBox != null && checkBox.IsChecked == true)
                {
                    // Access the SaveId from the Tag property of the checked CheckBox
                    if (checkBox.Tag is int saveId)
                    {
                        viewModel.InitializeDeleteSave(saveId);
                    }
                }
            }
            displaySaveList();
        }

        private T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T result)
                {
                    return result;
                }

                T descendant = FindChild<T>(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }

            return null;
        }
    }
}
