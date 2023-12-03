using System;
using System.IO;
using System.Text.Json;

namespace Easysave.ViewModels
{
    public sealed class Config
    {
        private readonly string FilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"/config.json";
        public string Language { get; set; }
        public string TargetDir { get; set; }
        public string SaveLogDir { get; set; }
        public string SaveStateDir { get; set; }

        public DataConfig DataConfig { get; set; }

        private static Config configInstance;

        private Config()
        {
            if (checkConfig())
            {
                DataConfig configObj = LoadConfig();
                Language = configObj.Language;
                TargetDir = configObj.TargetDir;
                SaveLogDir = configObj.SaveLogDir;
                SaveStateDir = configObj.SaveStateDir;
            }
        }

        public static Config getConfig()
        {
            configInstance ??= new Config();
            return configInstance;
        }

        public void SaveConfig()
        {
            string JSONtext = JsonSerializer.Serialize(DataConfig);
            File.WriteAllText(FilePath, JSONtext);
        }

        public bool checkConfig()
        {
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
                using StreamWriter sw = File.CreateText(FilePath);
                sw.Write("[]");
                return false;
            }
            string JSONtext = File.ReadAllText(FilePath);
            int fileLength = JSONtext.Length;
            if (fileLength > 32) { return true; } else { return false; }
        }

        public DataConfig LoadConfig()
        {
            string JSONtext = File.ReadAllText(FilePath);
            DataConfig configObj = JsonSerializer.Deserialize<DataConfig>(JSONtext);
            Language = configObj.Language;
            TargetDir = configObj.TargetDir;
            SaveLogDir = configObj.SaveLogDir;
            SaveStateDir = configObj.SaveStateDir;
            return configObj;
        }

    }
}

