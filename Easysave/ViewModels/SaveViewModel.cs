using System;
using System.Text;
using EasySave.Models;

namespace EasySave.ViewModels
{
    public class SaveViewModel
    {
        private BackupExecutor _backupExecutor = new BackupExecutor();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public bool AreBackupsActive { get; set; }
        public bool AreBackupsPaused { get; private set; } = false;

        public SaveViewModel() { }

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

        public void InitializeSave(string saveName, string saveType, string sourcePath, int saveId)
        {
            var save = new Save(_backupExecutor.BackupsProgress)
            {
                SaveId = saveId,
                SaveName = saveName,
                SaveSourcePath = sourcePath,
                Type = saveType
            };

            _backupExecutor.EnqueueBackup(save);
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
            CheckAllBackupsCompleted(); // Additional method to confirm all backups are complete
        }

        public async Task ReexecuteAllBackupsAsync()
        {
            State[] allBackups = State.GetStateArr();
            await _backupExecutor.ExecuteBackupsAsync(_cancellationTokenSource.Token);
            AreBackupsActive = false;
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

            AreBackupsActive = true;
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

        public object[] GetSaveList()
        {
            State[] statesArr = State.GetStateArr();

            List<object> saveList = new List<object>();

            foreach (var state in statesArr)
            {
                if (state.Type == "full") { state.Type = "Complète"; } else { state.Type = "Diffèrentielle"; }
                if (state.SaveState == true) { state.SaveStateString = "En cours"; } else { state.SaveStateString = "Terminée"; }
                state.FilesSizeString = FormatFileSize(state.FilesSize);
                saveList.Add(new
                {
                    state.SaveId,
                    state.TargetPath,
                    state.SaveName,
                    state.Time,
                    state.FilesSizeString,
                    state.Type,
                    state.SaveStateString,
                    state.FilesNumber
                });
            }

            return saveList.ToArray();
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

        public static Dictionary<string,string[]> GetAcls()
        {
            AccessList acl = AccessList.GetAccessList();
            Dictionary<string, string[]> aclList = new Dictionary<string, string[]>();
            aclList["encryptableFiles"] = acl.EncryptableFiles;
            aclList["ignoredFiles"] = acl.IgnoredFiles;
            return aclList;
        }

        public Dictionary<string,int> GetSavesStats()
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

