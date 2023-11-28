using System;

namespace EasySave.Models
{
    public class Save
    {
        public string saveId;
        public string saveName { get; set; }
        public string saveSourcePath { get; set; }
        public int saveFilesNumber { get; set; }
        public string type { get; } 

        public Save(string SaveId, string SaveName, string SaveSourcePath, int SaveFilesNumber, string Type)
        {
            saveId = SaveId;
            saveName = SaveName;
            saveSourcePath = SaveSourcePath;
            saveFilesNumber = SaveFilesNumber;
            type = Type;
        }

        public void CreateSave()
        {
            
        }

        public void DeleteSave()
        {
            
        }

        public string[] GetSaveProgress()
        {
            string[] arr = { };

            return arr ;
        }

        public string[] GetFileNames()
        {
            string[] arr = { };

            return arr;
        }


    }


   
  
}
