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

        public  ViewModel(){
        
        }

        public void InitializeSave(DataState inputObj) {
           save.SaveName = inputObj.SaveName;
           save.SaveSourcePath = inputObj.SourcePath;
           save.Type = inputObj.Type;
           save.CreateSave();
        }

        public void InitializeDeleteSave() { }



        public DataState[] GetSavelist()
        {
            DataState[] myResult = state.GetStateArr();
            return myResult;
        }

        public DataState GetSaveInfo(int saveNb)
        {
            
            DataState[] list = GetSavelist();
            DataState result = new DataState(saveNb);

            foreach (DataState save in list)
            {
                if(save.SaveId == saveNb)
                {
                    result = save;
                }
                
            }


            return result;
        }



        public void deleteSave(int saveId)
        {
            DataState saveInfo = GetSaveInfo(saveId);
            save.SaveName = saveInfo.SaveName;
            save.DeleteSave(saveId);
        }

        public void NavigateTo(int page)
        {

            View view = new View();
            Navigation navigation = new Navigation();


            switch (page)
            {
                case 1: // Show the saves
                    navigation.ShowSaveList();
                    break;
                case 2: // Create a new save
                    navigation.ShowSaveMenu();
                    break;

                case 3: // Delete a save
                    navigation.ShowDeleteMenu();
                    break;

                case 4: // Configuration
                    int step = navigation.ShowParameters();
                    if(step == 5)
                    {
                        navigation.ShowMainMenu();
                    }
                    else
                    {
                        DataConfig data = view.GetParametersInput(false,step);
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

