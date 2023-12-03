using System;
using System.Data;
using System.Text.Json;
using Easysave.ViewModels;

namespace Easysave.Models
{
    public class State
    {
        public DataState DataState { get; set; }
        private Config configObj = Config.getConfig();

        public State() {
            if (!File.Exists(configObj.SaveStateDir))
            {
                File.Create(configObj.SaveStateDir).Close();
                using StreamWriter sw = File.CreateText(configObj.SaveStateDir);
                sw.Write("[]");
            }
        }

        public void CreateState()
        {
            string stateFilePath = configObj.SaveStateDir;
            string jsonString = File.ReadAllText(stateFilePath);

            DataState[] dataStateArr = JsonSerializer.Deserialize<DataState[]>(jsonString);
            List<DataState> stateList = new List<DataState>();

            //Adding the old state entries to the list
            stateList.AddRange(dataStateArr);
            //Adding the new state entry to the list
            stateList.Add(DataState);

            string serializedJSON = JsonSerializer.Serialize(stateList) + Environment.NewLine;
            

            File.WriteAllText(configObj.SaveStateDir, serializedJSON);
        }

        public void UpdateState()
        {
            List<DataState> stateList = new List<DataState>();
            if (!File.Exists(configObj.SaveStateDir))
            {
                File.Create(configObj.SaveStateDir).Close();
            }

            string jsonString = File.ReadAllText(configObj.SaveStateDir);
            DataState[] dataStateList = JsonSerializer.Deserialize<DataState[]>(jsonString);
            foreach (var saveSlot in dataStateList)
            {
                if (saveSlot.SaveId == DataState.SaveId)
                {
                    saveSlot.SaveName = DataState.SaveName;
                    saveSlot.Time = DataState.Time;
                    saveSlot.Type = DataState.Type;
                    saveSlot.SaveState = DataState.SaveState;
                    saveSlot.SourcePath = DataState.SourcePath;
                    saveSlot.TargetPath = DataState.TargetPath;
                    saveSlot.FilesNumber = DataState.FilesNumber;
                    saveSlot.FilesSize = DataState.FilesSize;
                    if (saveSlot.SaveState)
                    {
                        saveSlot.Progress = DataState.Progress;
                        saveSlot.RemainingFiles = DataState.RemainingFiles;
                        saveSlot.RemainingFilesSize = DataState.RemainingFilesSize;
                    }
                }

                stateList.Add(saveSlot);

            }

            string serializedJSON = JsonSerializer.Serialize(stateList.ToArray()) + Environment.NewLine;
            File.WriteAllText(configObj.SaveStateDir, serializedJSON);

        }

        public void DeleteState()
        {
            if (!File.Exists(configObj.SaveStateDir))
            {
                File.Create(configObj.SaveStateDir).Close();
            }

            string jsonString = File.ReadAllText(configObj.SaveStateDir);
            List<DataState> stateList = JsonSerializer.Deserialize<List<DataState>>(jsonString);

            stateList.RemoveAll(save => save.SaveId == DataState.SaveId);

            string serializedJSON = JsonSerializer.Serialize(stateList.ToArray()) + Environment.NewLine;
            File.WriteAllText(configObj.SaveStateDir, serializedJSON);
        }


        public DataState[] GetStateArr()
        {
            string jsonString = File.ReadAllText(configObj.SaveStateDir);
            DataState[] dataStateArr = JsonSerializer.Deserialize<DataState[]>(jsonString);

            return dataStateArr;
        }

    }
}

