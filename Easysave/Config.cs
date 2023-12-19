using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace EasySave
{
    public sealed class Config
    {
        private readonly string FilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"/Assets/config.json";
        public string Language { get; set; } = "";
        public string TargetDir { get; set; } = "";
        public string LogsDir { get; set; } = "";
        public string LogsType { get; set; } = "";
        public string BlockingApp { get; set; } = "";

        private static Config ConfigInstance;

        private Config()
        {
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
                using StreamWriter sw = File.CreateText(FilePath);
                sw.Write("[]");
            }
        }

        public static Config GetConfig()
        {
            ConfigInstance ??= new Config();
            if (ConfigInstance.CheckConfig()) { ConfigInstance.LoadConfig(); }
            return ConfigInstance;
        }

        public void SaveConfig()
        {
            try
            {
                string JSONtext = JsonSerializer.Serialize(this);
                File.WriteAllText(FilePath, JSONtext);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
            }
        }

        public bool CheckConfig()
        {
            try
            {
                string JSONtext = File.ReadAllText(FilePath);
                int fileLength = JSONtext.Length;
                if (fileLength > 32) { return true; } else { return false; }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex);
                return false;
            }
        }

        public void LoadConfig()
        {
            string JSONtext = File.ReadAllText(FilePath);
            try
            {
                ConfigData configObj = JsonSerializer.Deserialize<ConfigData>(JSONtext);

                Language = configObj.Language;
                TargetDir = configObj.TargetDir;
                LogsDir = configObj.LogsDir;
                LogsType = configObj.LogsType;
                LogsType = configObj.LogsType;
                BlockingApp = configObj.BlockingApp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private class ConfigData
        {
            public string Language { get; set; }
            public string TargetDir { get; set; }
            public string LogsDir { get; set; }
            public string LogsType { get; set; }
            public string BlockingApp { get; set; }
        }
    }
}

