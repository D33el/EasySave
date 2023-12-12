using System.Text.Json;
using System.Xml.Serialization;

namespace EasySave.Models
{
    public sealed class Log
    {
        public string Timestamp { get; set; }
        public string FileName { get; set; }
        public string SourceDir { get; set; }
        public string TargetDir { get; set; }
        public long FileSize { get; set; }
        public long FileSaveDuration { get; set; }
        public long EncryptionTime { get; set; } = 0;

        private static readonly string LogFilePath;
        private Config _config = Config.GetConfig();
        private static Log LogInstance;

        static Log()
        {
            string logFileType = Config.GetConfig().LogsType;
            string currentTimestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            LogFilePath = Config.GetConfig().LogsDir + $"/log_{currentTimestamp}.{logFileType}";

            File.Create(LogFilePath).Close();

            if (logFileType == "json")
            {
                using StreamWriter sw = File.CreateText(LogFilePath);
                sw.Write("[]");
            }
            else if (logFileType == "xml")
            {
                using StreamWriter sw = File.CreateText(LogFilePath);
                sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?><Logs></Logs>");
            }
        }

        private Log()
        {
        }

        public static Log GetLog()
        {
            try
            {
            LogInstance ??= new Log();
            return LogInstance;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Log();
            }
        }

        public void WriteLog()
        {
            string logFileType = _config.LogsType;
            if (logFileType == "json") { WriteJsonLog(); }
            else { WriteXmlLog(); }
        }

        private void WriteJsonLog()
        {
            try
            {
                string jsonString = File.ReadAllText(LogFilePath);

                List<object> logList = JsonSerializer.Deserialize<List<object>>(jsonString);
                logList.Add(this);

                string serializedJSON = JsonSerializer.Serialize(logList.ToArray()) + Environment.NewLine;
                File.WriteAllText(LogFilePath, serializedJSON);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing JSON log: {ex}");
            }
        }

        private void WriteXmlLog()
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogWrapper));
                LogWrapper logWrapper;

                if (File.Exists(LogFilePath))
                {
                    using FileStream fileStream = new FileStream(LogFilePath, FileMode.Open);
                    logWrapper = (LogWrapper)xmlSerializer.Deserialize(fileStream);
                }
                else
                {
                    logWrapper = new LogWrapper();
                }

                logWrapper.Logs.Add(this);

                using FileStream fileStreamWrite = new FileStream(LogFilePath, FileMode.Create);
                xmlSerializer.Serialize(fileStreamWrite, logWrapper);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing XML log: {ex}");
            }
        }

        // Class to structure the XML file
        [XmlRoot("Logs")]
        public class LogWrapper
        {
            [XmlElement("Log")]
            public List<Log> Logs { get; set; } = new List<Log>();
        }
    }
}




