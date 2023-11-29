using System;
using System.IO;
using System.Text.Json;

namespace EasySave.ViewModels
{
    public sealed class Config
    {
        public string langage { get; set; }
        public string targetDir { get; set; }
        public string saveLogDir { get; set; }
        public string saveStateDir { get; set; }
        private static Config configInstance;

        private Config()
        {

        }

        public static Config getConfig()
        {
            if (configInstance == null) { configInstance = new Config(); }
            return configInstance;
        }

        public void SaveConfig()
        {
            
            string langage = Console.ReadLine();

            
            string targetDir = Console.ReadLine();

            
            string saveLogDir = Console.ReadLine();

            
            string saveStateDir = Console.ReadLine();

            
            string configInstance = Console.ReadLine();

            var myObjet = new
            {
                langage = langage,
                targetDir = targetDir,
                saveLogDir = saveLogDir,
                saveStateDir = saveStateDir,
                configInstance = configInstance
            };

            string jsonString = JsonSerializer.Serialize(myObjet);
            string filePath = "C:\\Users\\DELL\\source\\repos\\EasySave\\EasySave\\Config\\Config.json";

            File.WriteAllText(filePath, jsonString);
        }

        public int checkConfig()
        {
            string JSONtext = File.ReadAllText("C:\\Users\\DELL\\source\\repos\\EasySave\\EasySave\\Config\\Config.json");
            int fileLength = JSONtext.Length;
            return fileLength;
        }

        public string LoadConfig()
        {
            return "";
        }

        

    }
}

