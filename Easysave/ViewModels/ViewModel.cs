using System;
using System.IO;
using System.Text.Json;
using Easysave.Models;
using Easysave.Views;

namespace Easysave.ViewModels
{
    public class ViewModel
    {
        Save save = new Save();
        State state = new State();
        Config configObj = Config.getConfig();

        public ViewModel()
        {

        }

        public void InitializeSave(DataState inputObj)
        {
            if (inputObj.Type == "full")
            {
                DataState[] saveArr = GetSavelist();
                int savesNb = saveArr.Length;
                if (savesNb < 5)
                {
                    int id = savesNb + 1;
                    save.SaveId = id;
                }
                else
                {
                    Console.WriteLine("TODO : erreur il existe 5 sauvegarde");
                    return;
                }
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

        public void NavigateTo(int page)
        {

            View view = new View();
            Navigation navigation = new Navigation();


            switch (page)
            {
                case 1: // Show the saves
                    navigation.ShowSaveList();
                    navigation.ShowMainMenu();
                    break;
                case 2: // Create a new save
                    navigation.ShowSaveMenu();
                    navigation.ShowMainMenu();
                    break;

                case 3: // Delete a save
                    navigation.ShowDeleteMenu();
                    navigation.ShowMainMenu();
                    break;

                case 4: // Configuration
                    int step = navigation.ShowParameters();
                    if (step == 5)
                    {
                        navigation.ShowMainMenu();
                    }
                    else
                    {
                        DataConfig data = view.GetParametersInput(false, step);
                        configObj.DataConfig = data;
                        configObj.SaveConfig();
                        NavigateTo(4);
                    }

                    break;

                case 5: // Exit
                    Environment.Exit(0);
                    break;

                default:
                    break;
            }
        }
    }
}

