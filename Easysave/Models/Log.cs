using System;
using System.Text.Json;
using Easysave.ViewModels;

namespace Easysave.Models
{
    public class Log
    {
        private readonly string ProjectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private Config configObj = Config.getConfig();
        public string LogFilePath;

        public DataLog LogObj { get; set; }

        private static Log logInstance;

        private Log()
        {
            CreateLogFile();
        }

        public static Log getLog()
        {
            logInstance ??= new Log();
            return logInstance;
        }

        public void CreateLogFile()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
            LogFilePath = configObj.SaveLogDir + $"/log_{currentDate}.json";
            File.Create(LogFilePath).Close();
            using StreamWriter sw = File.CreateText(LogFilePath);
            sw.Write("[]");
        }

        public void WriteLog()
        {
            string jsonString = File.ReadAllText(LogFilePath);

            List<DataLog> logList = JsonSerializer.Deserialize<List<DataLog>>(jsonString);
            logList.Add(LogObj);

            string serializedJSON = JsonSerializer.Serialize(logList.ToArray()) + Environment.NewLine;
            File.WriteAllText(LogFilePath, serializedJSON);

        }
    }



}




