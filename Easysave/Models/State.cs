using System.Text.Json;

namespace EasySave.Models
{
    public class State
    {
        public int SaveId { get; set; }
        public string SaveName { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public bool SaveState { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public int FilesNumber { get; set; }
        public long FilesSize { get; set; }
        public double Progress { get; set; }
        public long RemainingFiles { get; set; }
        public long RemainingFilesSize { get; set; }

        private Config ConfigObj = Config.getConfig();

        public State()
        {
            // checks if the state.json files exists and create one if not
            if (!File.Exists(ConfigObj.StateFilePath))
            {
                File.Create(ConfigObj.StateFilePath).Close();

                // Writes in the file so its in JSON format
                using StreamWriter sw = File.CreateText(ConfigObj.StateFilePath);
                sw.Write("[]");
            }
        }

        public void AddState()
        {
            string stateFilePath = ConfigObj.StateFilePath;
            string jsonString = File.ReadAllText(stateFilePath);

            State[] statesArr = JsonSerializer.Deserialize<State[]>(jsonString);
            List<State> stateList = new List<State>();

            // Adding the old state entries to the list
            stateList.AddRange(statesArr);
            // Adding the new state entry to the list
            stateList.Add(this);

            string serializedJSON = JsonSerializer.Serialize(stateList) + Environment.NewLine;
            File.WriteAllText(ConfigObj.StateFilePath, serializedJSON);
        }

        // TODO
        public void UpdateState()
        {
            List<State> stateList = new List<State>();

            string jsonString = File.ReadAllText(ConfigObj.StateFilePath);
            State[] dataStateList = JsonSerializer.Deserialize<State[]>(jsonString);

            foreach (var save in dataStateList)
            {
                if (save.SaveId == SaveId)
                {
                    save.SaveName = SaveName;
                    save.Time = Time;
                    save.Type = Type;
                    save.SaveState = SaveState;
                    save.SourcePath = SourcePath;
                    save.TargetPath = TargetPath;
                    save.FilesNumber = FilesNumber;
                    save.FilesSize = FilesSize;
                    save.Progress = Progress;
                    save.RemainingFiles = RemainingFiles;
                    save.RemainingFilesSize = RemainingFilesSize;
                }
                stateList.Add(save);
            }

            string serializedJSON = JsonSerializer.Serialize(stateList.ToArray()) + Environment.NewLine;
            File.WriteAllText(ConfigObj.StateFilePath, serializedJSON);
        }

        // TODO
        public void DeleteState()
        {
            string jsonString = File.ReadAllText(ConfigObj.StateFilePath);
            List<State> stateList = JsonSerializer.Deserialize<List<State>>(jsonString);

            stateList.RemoveAll(save => save.SaveId == SaveId);

            string serializedJSON = JsonSerializer.Serialize(stateList.ToArray()) + Environment.NewLine;
            File.WriteAllText(ConfigObj.StateFilePath, serializedJSON);
        }

        public static State[] GetStateArr()
        {
            Config configObj = Config.getConfig();
            string jsonString = File.ReadAllText(configObj.StateFilePath);
            State[] stateArr = JsonSerializer.Deserialize<State[]>(jsonString);

            return stateArr ??= Array.Empty<State>();
        }

    }
}

