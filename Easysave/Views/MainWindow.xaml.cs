using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ookii.Dialogs.Wpf;
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
using EasySave;

namespace EasySave.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
       
        private delegate void  writeOnTextBlock(string textboxName, string value);




        Config _config = Config.GetConfig();
        SaveViewModel viewModel = new SaveViewModel();


        public MainWindow()
        {
            InitializeComponent();
            bool configExists = _config.CheckConfig();
            if (!configExists)
            {
                ShowWelcomePage();
            }
            else
            {
                ShowHomePage();
            }

            displaySaveList();
            displayParameters();
            displayAccessList();
        }



        public void displayStats()
        {
            NumberOfFull.Text = viewModel.statsNumberFull().ToString() + " Complètes";
            NumberOfDiff.Text = viewModel.statsNumberDiff().ToString() + " Différentielles";

            SavesSize.Text = FormatFileSize(viewModel.GetAllSavesSize());

            NumberOfEncrypted.Text = viewModel.statsEncryptedFilesNumber().ToString() ;

        }

        public static string FormatFileSize(long sizeInBytes)
        {
            string[] sizeSuffixes = { "B", "KB", "MB", "GB" };

            int i = 0;
            double size = sizeInBytes;

            while (size >= 1024 && i < sizeSuffixes.Length - 1)
            {
                size /= 1024;
                i++;
            }

            return $"{size:N2} {sizeSuffixes[i]}";
        }



        public void displaySaveList()
        {
                  
            SaveList.ItemsSource = viewModel.GetSaveList();
            
            displayStats();
        }


        public void displayAccessList() 
        {
            string[] Encryptable = viewModel.getAclEncryptableFiles();
            string[] Ignored = viewModel.getAclIgnoreFiles();

            string extensionsEncryptable = string.Join(", ", Encryptable);
            string extensionsIgnored = string.Join(", ", Ignored);

            SettingFilesCrypte.Text = extensionsEncryptable;
            SettingFilesIgnore.Text = extensionsIgnored;

        }
        public void displayParameters()
        {
            SettingLanguage.SelectedItem = FindComboBoxItemByTag(SettingLanguage, _config.Language);
            SettingTargetSave.Text = _config.TargetDir;
            SettingLogsPath.Text = _config.LogsDir;
            SettingTypeLogs.SelectedItem = FindComboBoxItemByTag(SettingTypeLogs,_config.LogsType);
        }
        private ComboBoxItem FindComboBoxItemByTag(ComboBox comboBox, string tag)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Tag.ToString() == tag)
                {
                    return item;
                }
            }
            return null; 
        }

        private void Commencer_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedLanguage= LangueSetting.SelectedItem as ComboBoxItem;
            ComboBoxItem selectedType = LogsType.SelectedItem as ComboBoxItem;

            string Langue = selectedLanguage.Tag.ToString();
            string targetsave = TargetSave.Text;
            string targetlogs = TargetLogs.Text;
            string logstype = selectedType.Tag.ToString();
            _config.Language = Langue;
            _config.TargetDir = targetsave;
            _config.LogsDir = targetlogs;
            _config.LogsType = logstype;
            _config.SaveConfig();
            ShowHomePage() ;

        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            ShowHomePage();
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            ShowSettingsPage();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ShowSavePage();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OpenExplorer_Click(object sender, RoutedEventArgs e)
        {

                var dialog = new VistaFolderBrowserDialog();
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            dialog.SelectedPath = desktopPath;

            if (dialog.ShowDialog() == true)
            {

                string selectedFolderPath = dialog.SelectedPath;

                Button button = sender as Button;

                if (button != null)
                {
                    string buttonTag = button.Tag.ToString();

                    TextBox associatedTextBox = this.FindName(buttonTag) as TextBox;

                    if (associatedTextBox != null)
                    {
                        associatedTextBox.Text = selectedFolderPath;
                    };
                    
                }

            }
        }

        public void AccessList()
        {
            string extensionsCrypt = SettingFilesCrypte.Text;
            string extensionsIgnore = SettingFilesIgnore.Text;
            string[] extensionsCryptArray = extensionsCrypt.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i<extensionsCryptArray.Length; i++)
                            {
                                extensionsCryptArray[i] = extensionsCryptArray[i].Trim();
                            }
            string[] extensionsIgnoreArray = extensionsIgnore.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < extensionsIgnoreArray.Length; i++)
                            {
                                extensionsIgnoreArray[i] = extensionsIgnoreArray[i].Trim();
                            }
            viewModel.WriteAcl(extensionsCryptArray, extensionsIgnoreArray);
        }


        private void CreateSave_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = SaveType.SelectedItem as ComboBoxItem;
            string savename = SaveName.Text;
            string savetype = selectedComboBoxItem.Tag.ToString();
            string sourcepath = SourcePath.Text;
            viewModel.InitializeSave(savename, savetype, sourcepath, 0);
            SaveName.Clear();
            SaveType.SelectedItem = null;
            SourcePath.Clear();
            displaySaveList();
        }

        private void Sauvegarder_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedLangue = SettingLanguage.SelectedItem as ComboBoxItem;
            ComboBoxItem selectedTypeLogs = SettingTypeLogs.SelectedItem as ComboBoxItem;

            string Langue = selectedLangue.Tag.ToString();
            string savesPath = SettingTargetSave.Text;
            string logsPath = SettingLogsPath.Text;
            string filesCrypt = SettingFilesCrypte.Text;
            string filesIgnore = SettingFilesIgnore.Text;
            string LogsType = selectedTypeLogs.Tag.ToString();

            _config.Language = Langue;
            _config.TargetDir = savesPath;
            _config.LogsDir = logsPath;
            _config.LogsType = LogsType;

            _config.SaveConfig();
            AccessList();
            displayParameters();
            displayAccessList();
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



        public void ShowHomePage()
        {
            WelcomePage.Visibility = Visibility.Collapsed;
            HomePage.Visibility = Visibility.Visible;
            SettingsPage.Visibility = Visibility.Collapsed;
            SavePage.Visibility = Visibility.Collapsed;
        }
        public void ShowSettingsPage()
        {
            WelcomePage.Visibility = Visibility.Collapsed;
            HomePage.Visibility = Visibility.Collapsed;
            SettingsPage.Visibility = Visibility.Visible;
            SavePage.Visibility = Visibility.Collapsed;
        }

        public void ShowSavePage()
        {
            WelcomePage.Visibility = Visibility.Collapsed;
            HomePage.Visibility = Visibility.Collapsed;
            SettingsPage.Visibility = Visibility.Collapsed;
            SavePage.Visibility = Visibility.Visible;
        }

        public void ShowWelcomePage()
        {
            WelcomePage.Visibility = Visibility.Visible;
            HomePage.Visibility = Visibility.Collapsed;
            SettingsPage.Visibility = Visibility.Collapsed;
            SavePage.Visibility = Visibility.Collapsed;
        }

    }
}
