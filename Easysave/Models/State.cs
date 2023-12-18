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

        private static readonly string StateFilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"/Assets/state.json";

        private static readonly object _stateLock = new object();

        public State()
        {
            // checks if the state.json files exists and create one if not
            if (!File.Exists(StateFilePath))
            {
                File.Create(StateFilePath).Close();

                // Writes in the file so its in JSON format
                using StreamWriter sw = File.CreateText(StateFilePath);
                sw.Write("[]");
            }
        }

        public void AddState()
        {
            lock (_stateLock)
            {
                string jsonString = File.ReadAllText(StateFilePath);

                State[] statesArr = JsonSerializer.Deserialize<State[]>(jsonString);
                List<State> stateList = new List<State>();

                // Adding the old state entries to the list
                stateList.AddRange(statesArr);

                if (stateList.Any(s => s.SaveId == SaveId)) { return; }

                // Adding the new state entry to the list
                stateList.Add(this);

                string serializedJSON = JsonSerializer.Serialize(stateList) + Environment.NewLine;
                File.WriteAllText(StateFilePath, serializedJSON);
            }
        }

        public void UpdateState()
        {
            DeleteState();
            AddState();
        }

        public void DeleteState()
        {
            lock (_stateLock)
            {
                string jsonString = File.ReadAllText(StateFilePath);
                List<State> stateList = JsonSerializer.Deserialize<List<State>>(jsonString);

                stateList.RemoveAll(save => save.SaveId == SaveId);

                string serializedJSON = JsonSerializer.Serialize(stateList.ToArray()) + Environment.NewLine;
                File.WriteAllText(StateFilePath, serializedJSON);
            }
        }

        public static State[] GetStateArr()
        {
            lock (_stateLock)
            {
            string jsonString = File.ReadAllText(StateFilePath);
            State[] stateArr = JsonSerializer.Deserialize<State[]>(jsonString);

            return stateArr ??= Array.Empty<State>();
            }
        }

    }
}

