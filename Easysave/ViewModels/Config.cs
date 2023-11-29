using System;
using System.IO;
using System.Text.Json;

namespace EasySave.ViewModels
{
    public sealed class Config
    {
        public string Langage { get; set; }
        public string TargetDir { get; set; }
        public string SaveLogDir { get; set; }
        public string SaveStateDir { get; set; }
        private static Config configInstance;

        private Config()
        {
        }

        public static Config getConfig()
        {
            configInstance ??= new Config();
            return configInstance;
        }



        public static void SaveConfig(object parametersInput)
        {
            string projectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string JSONtext = JsonSerializer.Serialize(parametersInput);
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

