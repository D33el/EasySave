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



        public static void SaveConfig(string Lang, string TargetDir, string SaveLogDir, string SaveStateDir)
        {
            var myObjet = new
            {
                lang = Lang,
                targetDir = TargetDir,
                saveLogDir = SaveLogDir,
                saveStateDir = SaveStateDir
            };
            string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string JSONtext = JsonSerializer.Serialize(myObjet);
            string filePath = projectDir + @"/Config/config.json";

            File.WriteAllText(filePath, JSONtext);
        }

        public bool checkConfig()
        {
            string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string JSONtext = File.ReadAllText(projectDir + @"/Config/config.json");
            int fileLength = JSONtext.Length;
            if (fileLength > 10) { return true; } else { return false; }
        }

        public string LoadConfig()
        {
            return "";
        }

    }
}

