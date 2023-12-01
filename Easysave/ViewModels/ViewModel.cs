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
        Config ConfigObj = Config.getConfig(); 

        public  ViewModel(){
        
        }

        public void InitializeSave(DataState inputObj) {
           save.SaveName = inputObj.SaveName;
           save.SaveSourcePath = inputObj.SourcePath;
           save.Type = inputObj.Type;
           save.CreateSave();
        }

        public void InitializeDeleteSave() { }

        public String[] GetSaveInfo(int saveNb)
        {

            string[] saveList = GetSavelist();
            string[] save = { };


            return save;
        }

        public String[] GetSavelist()
        {
            string JSONtext = File.ReadAllText(@"../Config/state.json");
            //var save = JsonSerializer.Deserialize<State>(JSONtext);


            string[] saveList = { };

            return saveList;
        }

        public void deleteSave(int saveId)
        {
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
                    DataConfig data = view.GetParametersInput(false);
                    ConfigObj.DataConfig = data;
                    ConfigObj.SaveConfig();
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

