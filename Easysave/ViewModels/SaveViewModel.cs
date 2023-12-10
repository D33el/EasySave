using EasySave.Models;

namespace EasySave.ViewModels
{
    public class SaveViewModel
    {
        private Save _save = new();

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
    }
}

