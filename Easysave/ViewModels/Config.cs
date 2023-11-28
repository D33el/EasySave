using System;
using System.IO;
using System.Text.Json;

namespace EasySave.ViewModels
{
	public class Config
	{
		public string langage { get; set; }
        public string targetDir { get; set; }
		public string saveLogDir { get; set; }
		public string saveStateDir { get; set; }
        private Config configInstance;

        private Config()
        {

        }



		public void SaveConfig() 
		{
            Console.WriteLine("langage");
            string langage = Console.ReadLine();

            Console.WriteLine("targetDir");
            string targetDir = Console.ReadLine();

            Console.WriteLine("saveLogDir");
            string saveLogDir = Console.ReadLine();

            Console.WriteLine("saveStateDir");
            string saveStateDir = Console.ReadLine();

            Console.WriteLine("configInstance");
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
		
        public Config getConfig() 
        {
            if(configInstance == null)
            {
                configInstance = new Config();
            } 
            return configInstance;
        }

	}
}

