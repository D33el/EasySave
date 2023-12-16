using EasySave.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
namespace EasySave.ViewModels
{
    public class SaveViewModel
    {
        private Save _save = new Save();

        public SaveViewModel() { }

        public void InitializeSave(string saveName, string saveType, string sourcePath, int saveId)
        {
            State[] statesArr = State.GetStateArr();
            _save.SaveName = saveName;
            _save.SaveSourcePath = sourcePath;
            _save.Type = saveType;

            if (saveType == "full")
            {
                if(statesArr.Length == 0 )
                {
                    _save.SaveId = 1;
                } else { _save.SaveId = statesArr.Max(item => item.SaveId) + 1; }
                
            }
            else if (saveType == "diff")
            {
                SetSaveInfo(saveId);
            }
            _save.CreateSave();
        }

        public void InitializeSaveReexecution(int saveId)
        {
            SetSaveInfo(saveId);
            _save.CreateSave();
        }

        public void InitializeDeleteSave(int saveId)
        {
            SetSaveInfo(saveId);
            _save.DeleteSave();
        }

        public static int GetSavesNumber()
        {
            State[] statesArr = State.GetStateArr();
            return statesArr.Length;
        }

        public static int[] ShowSaveList()
        {

            Config ConfigObj = Config.GetConfig();
            State[] statesArr = State.GetStateArr();
            List<int> savesIds = new List<int>();

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


        public object[] GetSaveList()
        {
            State[] statesArr = State.GetStateArr();

            List<object> saveList = new List<object>();

            foreach (var state in statesArr)
            {
                if(state.Type == "full") { state.Type = "Complète"; } else { state.Type = "Diffèrentielle"; }
                if(state.SaveState == true) { state.SaveStateString = "En cours";  } else { state.SaveStateString = "Terminée"; }
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
        private void SetSaveInfo(int saveId)
        {
            State save = State.GetStateArr().FirstOrDefault(s => s.SaveId == saveId) ?? new State();
            if (save != null)
            {
                _save.SaveName = save.SaveName;
                _save.SaveSourcePath = save.SourcePath;
                _save.Type = save.Type;
                _save.SaveId = save.SaveId;
            }
        }

        public void WriteAcl(string[] listCrypt, string[] listIgnore)
        {
            AccessList acl = AccessList.GetAccessList();
            acl.EncryptableFiles = listCrypt;
            acl.IgnoredFiles = listIgnore;
            acl.WriteList();
        }

        public string[] getAclEncryptableFiles()
        {
            AccessList acl = AccessList.GetAccessList();
            return acl.EncryptableFiles;
        }

        public  string[] getAclIgnoreFiles()
        {
            AccessList acl = AccessList.GetAccessList();
            return acl.IgnoredFiles;
        }

        public int statsNumberFull()
        {
          return  _save.GetFullSaveCount();
        }

        public int statsNumberDiff()
        {
            return _save.GetDiffSaveCount();
        }

        public int statsEncryptedFilesNumber()
        {
            return _save.GetEncryptedFilesNumber();
        }

        public long GetAllSavesSize()
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

