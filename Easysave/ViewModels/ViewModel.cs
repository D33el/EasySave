using Easysave.Models;
using Easysave.Views;

namespace Easysave.ViewModels
{
    public class ViewModel
    {
        Save save = new Save();

        Config configObj = Config.getConfig();


        public ViewModel()
        {

        }

        public void InitializeSave(DataState inputObj)
        {
            if (inputObj.Type == "full")
            {
                DataState[] saveArr = GetSavelist();
                int MaxId = 0;

                foreach (var item in saveArr)
                {
                    if (item.SaveId > MaxId)
                    {
                        MaxId = item.SaveId;
                    }
                }

                int id = MaxId + 1;
                save.SaveId = id;

            }
            else
            {
                save.SaveId = inputObj.SaveId;
            }
            save.SaveName = inputObj.SaveName;
            save.SaveSourcePath = inputObj.SourcePath;
            save.Type = inputObj.Type;
            save.CreateSave();

        }

        public void InitializeDeleteSave(int saveId)
        {
            DataState saveInfo = GetSaveInfo(saveId);
            save.SaveId = saveInfo.SaveId;
            save.SaveName = saveInfo.SaveName;
            save.DeleteSave();
        }

        public DataState[] GetSavelist()
        {
            State state = new State();
            DataState[] myResult = state.GetStateArr();
            return myResult;
        }

        public DataState GetSaveInfo(int saveNb)
        {

            DataState[] list = GetSavelist();
            DataState result = new DataState();

            foreach (DataState save in list)
            {
                if (save.SaveId == saveNb)
                {
                    result = save;
                }

            }


            return result;
        }

        public int checkSavesNumber()
        {
            ViewModel viewmodel = new ViewModel();
            DataState[] saveArr = viewmodel.GetSavelist();
            int savesNb = saveArr.Length;
            return savesNb;
        }

        public void NavigateTo(int page)
        {

            View view = new View();
            Log logObj = Log.getLog();
            string lang = configObj.Language;

            int saveNumber = checkSavesNumber();

            switch (page)
            {
                case 1: // Show the saves
                    if (saveNumber == 0) { Console.ForegroundColor = ConsoleColor.Red; if (lang == "en") { Console.Clear(); Console.WriteLine(" No backup available !"); } else { Console.Clear(); Console.WriteLine(" Aucune sauvegarde disponible !"); } } else { view.ShowSaveList(); }
                    Console.ForegroundColor = ConsoleColor.White;
                    //view.ShowSaveExecutionMenu();
                    view.ShowMainMenu();
                    break;
                case 2: // Create a new save
                    if (saveNumber < 5) { view.ShowSaveMenu(); } else { Console.ForegroundColor = ConsoleColor.Red; if (lang == "en") { Console.Clear(); Console.WriteLine(" Error. There are 5 saves!"); } else { Console.Clear(); Console.WriteLine(" Erreur. Il existe 5 sauvegardes !"); } }
                    Console.ForegroundColor = ConsoleColor.White;
                    view.ShowMainMenu();
                    break;

                case 3: // Delete a save
                    if (saveNumber == 0) { if (lang == "en") { Console.Clear(); Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(" No backup available !"); } else { Console.Clear(); Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(" Aucune sauvegarde disponible !"); } } else { view.ShowDeleteMenu(); }
                    Console.ForegroundColor = ConsoleColor.White;
                    view.ShowMainMenu();
                    break;

                case 4: // Configuration
                    int step = view.ShowParameters();
                    if (step == 5)
                    {
                        view.ShowMainMenu();
                    }
                    else
                    {
                        DataConfig data = view.GetParametersInput(false, step);
                        configObj.DataConfig = data;
                        configObj.SaveConfig();
                        configObj.LoadConfig();
                        NavigateTo(4);
                    }
                    break;

                case 5: // Exit
                    FileInfo file = new(logObj.LogFilePath);
                    if (file.Length <= 30) { file.Delete(); }
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }
        }
    }
}

