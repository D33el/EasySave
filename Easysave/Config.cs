using System.Text.Json;

namespace EasySave
{
    public sealed class Config
    {
        private readonly string FilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"/config.json";
        public string Language { get; set; }
        public string TargetDir { get; set; }
        public string LogsDir { get; set; }
        public string StateFilePath { get; set; }
        public string LogsType { get; set; }

        private static Config configInstance;

        private Config()
        {
            
        }

        public static Config getConfig()
        {
            configInstance ??= new Config();
            if (configInstance.CheckConfig()) { configInstance.LoadConfig(); }
            return configInstance;
        }

        public void SaveConfig()
        {
            string JSONtext = JsonSerializer.Serialize(this);
            File.WriteAllText(FilePath, JSONtext);
        }

        public bool CheckConfig()
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
                StateFilePath = configObj.StateFilePath;
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
            public string StateFilePath { get; set; }
            public string LogsType { get; set; }
        }
    }
}

