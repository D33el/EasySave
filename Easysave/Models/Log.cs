using System;
using System.Text.Json;
using Easysave.ViewModels;

namespace Easysave.Models
{
    public class Log
    {
        private string ProjectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        private Config configObj = Config.getConfig();


        public DataLog LogObj { get; set; }

        public Log()
        {
            
        }


        public void CreateLogFile()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string logFilePath = ProjectDir + configObj.SaveLogDir + $"log_{currentDate}.json";

            if (!File.Exists(logFilePath))
            {
                try
                {
                    File.Create(logFilePath).Close();
                    Console.WriteLine($"Log file created successfully for {currentDate}.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating log file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"Log file for {currentDate} already exists.");
            }
        }
        
        public void WriteLog(string savedFileName, string sourceDir, string targetDir, string saveSize, long saveDuration)
        {
            string logFilePath = ProjectDir + configObj.SaveLogDir;

            List<DataLog> logList = new List<DataLog>();

            //if (!File.Exists(logFilePath))
            //{
            //    File.Create(logFilePath).Close();
            //}

            string jsonString = File.ReadAllText(logFilePath);
            Console.WriteLine(jsonString);

            if (jsonString.Length != 0)
            {
                DataLog[] logArray = JsonSerializer.Deserialize<DataLog[]>(jsonString);

                string currentTimestamp = DateTime.Now.ToString();

                DataLog logEntry = new DataLog(currentTimestamp);
               
                logEntry.SavedFileName = savedFileName;
                logEntry.SourceDir = sourceDir;
                logEntry.TargetDir = targetDir;
                logEntry.SaveSize = saveSize;
                logEntry.SaveDuration = saveDuration;


                logList.AddRange(logArray);
                logList.Add(logEntry);

                string serializedJSON = JsonSerializer.Serialize(logList.ToArray()) + Environment.NewLine;
                Console.WriteLine(serializedJSON);
                File.WriteAllText(logFilePath, serializedJSON);
            }
            else
            {
                Console.WriteLine("JSON file is empty");
            }

        }
    }



    }




