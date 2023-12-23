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
using System.Windows.Markup.Localizer;
using System.Threading;
using System.ComponentModel;
using System.Globalization;

namespace EasySave.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class Langue
    {
    }
    public partial class MainWindow : Window
    {
        private bool backupsPaused;
        Config _config = Config.GetConfig();
        SaveViewModel viewModel = new SaveViewModel();


        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
            bool configExists = _config.CheckConfig();
            if (!configExists)
            {
                ShowWelcomePage();
            }
            else
            {
                if(!App.LanguageSet) { ChangeLanguage(_config.Language); }
            ShowHomePage();
            displaySaveList();
            displayParameters();
            displayAccessList();
            }

        }

        public static void ChangeLanguage(string cultureCode)
        {
            try
            {
                CultureInfo newCulture = new CultureInfo(cultureCode);
                CultureInfo.DefaultThreadCurrentCulture = newCulture;
                CultureInfo.DefaultThreadCurrentUICulture = newCulture;

                App.ChangeCulture(newCulture);
            }
            catch (Exception ex)
            {
                // Handle the exception, e.g., log it
                Console.WriteLine($"Error changing language: {ex.Message}");
            }
        }

        

        public void displayStats()
        {

            Dictionary<string, long> stats = viewModel.GetSavesStats();

            NumberOfFull.Text = stats["FullSavesNb"].ToString();
            NumberOfDiff.Text = stats["DiffSavesNb"].ToString() ;
            SavesSize.Text = FormatFileSize(stats["AllSavesSize"]);
            NumberOfEncrypted.Text = stats["EncryptedFilesNb"].ToString() ;

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
            SaveList.ItemsSource = viewModel.Saves;
            displayStats();
        }


        public void displayAccessList()
        {

            Dictionary<string, string[]> acls = SaveViewModel.GetAcls();

            string[] Encryptable = acls.GetValueOrDefault("encryptableFiles");
            string[] Ignored = acls.GetValueOrDefault("ignoredFiles");
            string[] Priority = acls.GetValueOrDefault("extensionsPriority");

            string extensionsEncryptable = string.Join(", ", Encryptable);
            string extensionsIgnored = string.Join(", ", Ignored);
            string extensionsPriority = string.Join(", ", Priority);


            SettingFilesCrypte.Text = extensionsEncryptable;
            SettingFilesIgnore.Text = extensionsIgnored;
            PrioritaryFiles.Text = extensionsPriority;

        }
        public void displayParameters()
        {
            SettingLanguage.SelectedItem = FindComboBoxItemByTag(SettingLanguage, _config.Language);
            SettingTargetSave.Text = _config.TargetDir;
            SettingLogsPath.Text = _config.LogsDir;
            SettingBlockingApp.Text = _config.BlockingApp;  
            SettingTypeLogs.SelectedItem = FindComboBoxItemByTag(SettingTypeLogs, _config.LogsType);
            SizeLimit.Text = _config.SizeLimit.ToString();
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
            ComboBoxItem selectedLanguage = LangueSetting.SelectedItem as ComboBoxItem;
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
            ShowHomePage();

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
            string priorityFiles = PrioritaryFiles.Text;

            string[] extensionsCryptArray = extensionsCrypt.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < extensionsCryptArray.Length; i++)
            {
                extensionsCryptArray[i] = extensionsCryptArray[i].Trim();
            }

            string[] extensionsIgnoreArray = extensionsIgnore.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < extensionsIgnoreArray.Length; i++)
            {
                extensionsIgnoreArray[i] = extensionsIgnoreArray[i].Trim();
            }

            string[] priorityFilesArray = priorityFiles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < priorityFilesArray.Length; i++)
            {
                priorityFilesArray[i] = priorityFilesArray[i].Trim();
            }

            SaveViewModel.WriteAcl(extensionsCryptArray, extensionsIgnoreArray,priorityFilesArray);
        }


        private async void CreateSave_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedComboBoxItem = SaveType.SelectedItem as ComboBoxItem;
            string savename = SaveName.Text;
            string savetype = selectedComboBoxItem.Tag.ToString();
            string sourcepath = SourcePath.Text;

            viewModel.InitializeSave(savename, savetype, sourcepath);
            viewModel.AreBackupsActive = true;
            viewModel.MonitorProgress();
            await viewModel.ExecuteEnqueuedBackupsAsync();

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
            string BlockingApp = SettingBlockingApp.Text;
            long MaxSize = long.Parse(SizeLimit.Text);

            _config.Language = Langue;
            _config.TargetDir = savesPath;
            _config.LogsDir = logsPath;
            _config.LogsType = LogsType;
            _config.BlockingApp = BlockingApp;
            _config.SizeLimit = MaxSize;

            _config.SaveConfig();

            ChangeLanguage(_config.Language);
            AccessList();
            displayParameters();
            displayAccessList();
        }

        private void LangueSetting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = LangueSetting.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                string selectedValue = selectedItem.Tag.ToString(); 
                ChangeLanguage(selectedValue);
            }
        }
        private void Delete_click(object sender, RoutedEventArgs e)
        {
            List<int> saveId = GetSelectedSaves();

            foreach (var id in saveId)
            {
                viewModel.InitializeDeleteSave(id);
            }
            displaySaveList();
        }

        private List<int> GetSelectedSaves ()
        {
            List<int> idList = new List<int>();

            foreach (var item in SaveList.Items)
            {
                var container = SaveList.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                CheckBox checkBox = FindChild<CheckBox>(container);

                if (checkBox != null && checkBox.IsChecked == true)
                {
                    if (checkBox.Tag is int saveId)
                    {
                        idList.Add(saveId);
                    }
                }
            }

            return idList;
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

        private async void play_click(object sender, RoutedEventArgs e)
        {
            if (backupsPaused)
            {
            viewModel.ResumeBackups();
                backupsPaused = false;
            }else
            {
            List<int> saveids = GetSelectedSaves();
            await viewModel.ReexecuteBackupsAsync(saveids);
            viewModel.AreBackupsActive = true;
            viewModel.MonitorProgress();

            }
        }
        private void pause_click(object sender, RoutedEventArgs e)
        {
            viewModel.PauseBackups();
            backupsPaused = true;
        }
        private void stop_click(object sender, RoutedEventArgs e)
        {
            viewModel.CancelAllBackups();
        }



    }
}
