using System;
using System.IO;
using System.Text.Json;

using EasySave.Views;

namespace EasySave.ViewModels
{
    public class ViewModel
    {
        View view = new View();

        public ViewModel()
        {
        }

        public void InitializeSave() { }

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
    }
}

