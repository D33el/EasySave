
using EasySave.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using EasySave.Models;
using System.Text.Json;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace EasySave.ViewModels
{
    public class SaveViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SaveItem> Saves { get; private set; } = new ObservableCollection<SaveItem>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private BackupExecutor _backupExecutor = new BackupExecutor();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public bool AreBackupsActive { get; set; }
        public bool AreBackupsPaused { get; private set; } = false;

        public SaveViewModel() {
            LoadExistingSaves();
        }

        private Save SetSaveInfo(int saveId)
        {
            Save _save = new Save(_backupExecutor.BackupsProgress);
            State save = State.GetStateArr().FirstOrDefault(s => s.SaveId == saveId) ?? new State();
            if (save != null)
            {
                _save.SaveName = save.SaveName;
                _save.SaveSourcePath = save.SourcePath;
                _save.Type = save.Type;
                _save.SaveId = save.SaveId;
            }
            return _save;
        }

        private void LoadExistingSaves()
        {
            var statesArr = State.GetStateArr();

            foreach (var state in statesArr)
            {
                if (state.Type == "full") { state.Type = "Complète"; } else { state.Type = "Diffèrentielle"; }
                if (state.SaveState == true) { state.SaveStateString = "En cours"; } else { state.SaveStateString = "Terminée"; }

                state.FilesSizeString = FormatFileSize(state.FilesSize);
                Saves.Add(new SaveItem()
                {
                    SaveId = state.SaveId,
                    TargetPath = state.TargetPath,
                    SaveName = state.SaveName,
                    Time=  state.Time,
                    FilesSizeString= state.FilesSizeString,
                    Type = state.Type,
                    SaveStateString = state.SaveStateString,
                    FilesNumber= state.FilesNumber
                });
            }
        }


        public void InitializeSave(string saveName, string saveType, string sourcePath)
        {
            int saveId = GetSavesNumber() + 1;
        
            Save save = new Save(_backupExecutor.BackupsProgress)
            {
                SaveId = saveId,
                SaveName = saveName,
                SaveSourcePath = sourcePath,
                Type = saveType
            };
            save.SetSaveState();

            AddSaveToObservable();

            _backupExecutor.EnqueueBackup(save);
        }

        public void AddSaveToObservable()
        {
            State[] statesArr = State.GetStateArr();

            State state = statesArr[statesArr.Length - 1];
            
                if (state.Type == "full") { state.Type = "Complete"; } else { state.Type = "Differentielle"; }
                if (state.SaveState == true) { state.SaveStateString = "En cours"; } else { state.SaveStateString = "Terminee"; }

                state.FilesSizeString = FormatFileSize(state.FilesSize);
               // Saves.Clear();
                Saves.Add(new SaveItem()
                {
                    SaveId = state.SaveId,
                    TargetPath = state.TargetPath,
                    SaveName = state.SaveName,
                    Time = state.Time,
                    FilesSizeString = state.FilesSizeString,
                    Type = state.Type,
                    SaveStateString = state.SaveStateString,
                    FilesNumber = state.FilesNumber
                });
        }

        public async Task MonitorProgress()
        {
            while (AreBackupsActive)
            {
                var backupProgress = GetBackupProgress();
                foreach (var progress in backupProgress)
                {
                    Debug.WriteLine("========MONITOR PROGRESSS=========");
                    Debug.WriteLine(progress.Key);
                    Debug.WriteLine(progress.Value);
                    UpdateProgress(progress.Key, progress.Value);
                }

                if (backupProgress.All(p => p.Value >= 100))
                {
                    AreBackupsActive = false;
                }

                await Task.Delay(100); 
            }
            Trace.WriteLine("============");
            Trace.WriteLine("\nAll backups are completed.");
        }

        private void UpdateProgress(int saveId, double progress)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var saveItem = Saves.FirstOrDefault(s => s.SaveId == saveId);
                if (saveItem != null)
                {
                    saveItem.Progress = progress;
                }
            });
        }

        public void InitializeDeleteSave(int saveId)
        {
            Save _save = new Save(_backupExecutor.BackupsProgress);
            _save = SetSaveInfo(saveId);
            _save.DeleteSave();
        }

        public async Task ExecuteEnqueuedBackupsAsync()
        {
            await _backupExecutor.ExecuteBackupsAsync(_cancellationTokenSource.Token);
            CheckAllBackupsCompleted();
        }

        public async Task ReexecuteAllBackupsAsync()
        {
            State[] allBackups = State.GetStateArr();
            await _backupExecutor.ExecuteBackupsAsync(_cancellationTokenSource.Token);
            AreBackupsActive = true;
            foreach (var backupState in allBackups)
            {
                var saveTask = new Save(_backupExecutor.BackupsProgress)
                {
                    SaveName = backupState.SaveName,
                    SaveSourcePath = backupState.SourcePath,
                    Type = backupState.Type,
                    SaveId = backupState.SaveId
                };

                _backupExecutor.EnqueueBackup(saveTask);
            }

            AreBackupsActive = false;
        }

        public void CheckAllBackupsCompleted()
        {
            AreBackupsActive = _backupExecutor.BackupsProgress.Any(p => p.Value < 100);
        }

        public void PauseBackups()
        {
            _backupExecutor.PauseBackups();
            AreBackupsPaused = true;
        }

        public void ResumeBackups()
        {
            _backupExecutor.ResumeBackups();
            AreBackupsPaused = false;
        }

        public void CancelAllBackups()
        {
            // If backups are paused, first resume them to allow proper cancellation
            if (AreBackupsPaused)
            {
                ResumeBackups();
            }

            _cancellationTokenSource.Cancel();
            _backupExecutor.StopAllBackups();
            AreBackupsActive = false;
        }

        public Dictionary<int, double> GetBackupProgress()
        {
            return new Dictionary<int, double>(_backupExecutor.BackupsProgress);
        }

        public static int GetSavesNumber()
        {
            State[] statesArr = State.GetStateArr();
            return statesArr.Length;
        }

        public static void WriteAcl(string[] list, string type)
        {
            AccessList acl = AccessList.GetAccessList();
            if (type == "priority")
            {
                acl.ExtensionsPriority = list;
            }

            acl.WriteList();
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

        public static void WriteAcl(string[] listCrypt, string[] listIgnore)
        {
            AccessList acl = AccessList.GetAccessList();
            acl.EncryptableFiles = listCrypt;
            acl.IgnoredFiles = listIgnore;
            acl.WriteList();
        }

        public static Dictionary<string, string[]> GetAcls()
        {
            AccessList acl = AccessList.GetAccessList();
            Dictionary<string, string[]> aclList = new Dictionary<string, string[]>();
            aclList["encryptableFiles"] = acl.EncryptableFiles;
            aclList["ignoredFiles"] = acl.IgnoredFiles;
            return aclList;
        }

        public Dictionary<string, int> GetSavesStats()
        {
            Dictionary<string, int> stats = new Dictionary<string, int>();

            int[] savesTypesNumber = Save.GetSavesTypesNumber();
            stats["FullSavesNb"] = savesTypesNumber[0];
            stats["DiffSavesNb"] = savesTypesNumber[1];
            stats["EncryptedFilesNb"] = Save.GetEncryptedFilesNumber();
            stats["AllSavesSize"] = (int)GetAllSavesSize();

            return stats;
        }

        private static long GetAllSavesSize()
        {
            State[] stateArr = State.GetStateArr();
            long total = 0;
            foreach (State state in stateArr)
            {
                total += state.FilesSize;
            }
            return total;
        }
    }
}

public class SaveItem : INotifyPropertyChanged
{

    public int SaveId { get; set; }
    public string SaveName { get; set; }
    public string Time { get; set; }
    public string Type { get; set; }
    public bool SaveState { get; set; }
    public string SourcePath { get; set; }
    public string TargetPath { get; set; }
    public int FilesNumber { get; set; }
    public long FilesSize { get; set; }
    public string FilesSizeString { get; set; }
    public string SaveStateString { get; set; }

    private double _progress;
    public double Progress
    {
        get { return _progress; }
        set
        {
            _progress = value;
            Debug.WriteLine("==============inside object set=================");
            Debug.WriteLine(value);
            OnPropertyChanged(nameof(Progress));
        }
    }
  

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

