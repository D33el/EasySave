using System;
using System.Xml;
using System.Text.Json;
using EasySave.ViewModels;

using EasySaveApp.Models;

namespace EasySave.Models
{
    public class State
    {
        private string ProjectDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public DataState DataState { get; set; }
        private Config configObj = Config.getConfig();

        public State()
        {

        }

        public void CreateState(int id)
        {
            string jsonString = File.ReadAllText(ProjectDir + configObj.SaveStateDir);

            List<DataState> stateList = new List<DataState>();
            DataState[] dataStateList = JsonSerializer.Deserialize<DataState[]>(jsonString);

            DataState test = new DataState(id);
            stateList.Add(test);

            stateList.AddRange(dataStateList);


            string serializedJSON = JsonSerializer.Serialize(stateList) + Environment.NewLine;
            Console.WriteLine(serializedJSON);

            File.WriteAllText(ProjectDir + configObj.SaveStateDir, serializedJSON);
        }



        public void UpdateStatefile (int saveId, string saveName, string time, string type, bool saveState, string sourcePath, string targetPath, int fileNumber, long filesSize, float progress, int remainingFiles, long remainingFilesSize)
        {
            List<DataState> stateList = new List<DataState>();
            if (!File.Exists(ProjectDir + configObj.SaveStateDir))
            {
                File.Create(configObj.SaveStateDir).Close();
            }

            string jsonString = File.ReadAllText(ProjectDir + configObj.SaveStateDir);
            Console.WriteLine(jsonString);
            if (jsonString.Length != 0)
            {

                DataState[] dataStateList = JsonSerializer.Deserialize<DataState[]>(jsonString);

                foreach (var saveSlot in dataStateList)
                {
                    if (saveSlot.SaveId == saveId)
                    {
                        saveSlot.SaveName = saveName;
                        saveSlot.Time = time;
                        saveSlot.Type = type;
                        saveSlot.SaveState = saveState;
                        saveSlot.SourcePath = sourcePath;
                        saveSlot.TargetPath = targetPath;
                        saveSlot.FilesNumber = fileNumber;
                        saveSlot.FilesSize = filesSize;
                        if (saveSlot.SaveState)
                        {
                            saveSlot.Progress = progress;
                            saveSlot.RemainingFiles = remainingFiles;
                            saveSlot.RemainingFilesSize = remainingFilesSize;
                        }
                    }

                    stateList.Add(saveSlot);

                }

                string serializedJSON = JsonSerializer.Serialize(stateList.ToArray()) + Environment.NewLine;
                Console.WriteLine(serializedJSON);
                File.WriteAllText(ProjectDir + configObj.SaveStateDir, serializedJSON);
            }
            else
            {
                Console.WriteLine("JSON file is empty");
            }

        }

    }
}

