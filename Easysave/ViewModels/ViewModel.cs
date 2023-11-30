using System;
using System.IO;
using System.Text.Json;

using Easysave.Views;

namespace Easysave.ViewModels
{
    public class ViewModel
    {

        public ViewModel(){ }

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

