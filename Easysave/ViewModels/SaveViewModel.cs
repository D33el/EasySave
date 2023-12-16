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

        public SaveViewModel()
        {
        }

        public Dictionary<int, double> GetBackupProgress()
        {
            return new Dictionary<int, double>(_backupExecutor.BackupsProgress);
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

        public async Task ExecuteEnqueuedBackupsAsync()
        {
            await _backupExecutor.ExecuteBackupsAsync(_cancellationTokenSource.Token);
            CheckAllBackupsCompleted(); // Additional method to confirm all backups are complete
        }

        public void CheckAllBackupsCompleted()
        {
            // Check if any backup task is not completed
            AreBackupsActive = _backupExecutor.BackupsProgress.Any(p => p.Value < 100);
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

        public void InitializeDeleteSave(int saveId)
        {
            Save _save = new Save(_backupExecutor.BackupsProgress);
            _save = SetSaveInfo(saveId);
            _save.DeleteSave();
        }

        public void CancelAllBackups()
        {
            _cancellationTokenSource.Cancel();
            _backupExecutor.StopAllBackups();
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

        public static int[] ShowSaveList()
        {

            Config ConfigObj = Config.GetConfig();
            State[] statesArr = State.GetStateArr();
            List<int> savesIds = new();

            if (statesArr.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (ConfigObj.Language == "fr")
                {
                    Console.WriteLine("/!\\ Vous n'avez aucune sauvegarde.");
                }
                else
                {
                    Console.WriteLine("/!\\ You have no backups.");
                }
                Console.ForegroundColor = ConsoleColor.White;
                return savesIds.ToArray();
            }
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var save in statesArr)
            {
                Console.WriteLine($"|==== [{save.SaveId}] {save.SaveName} -- {save.Time}");
                savesIds.Add(save.SaveId);
            }

            Console.ForegroundColor = ConsoleColor.White;
            return savesIds.ToArray();
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

        
    }
}

