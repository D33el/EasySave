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

        //private Config _config = Config.GetConfig();
        private static string StateFilePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"/Assets/state.json";

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

        // FIXME : ajoute au lieu de modifier (je sais pas pourquoi ptn)
        public void UpdateState()
        {
            string jsonString = File.ReadAllText(StateFilePath);
            List<State> dataStateList = JsonSerializer.Deserialize<List<State>>(jsonString);

            State existingState = dataStateList.FirstOrDefault(save => save.SaveId == SaveId);

            if (existingState != null)
            {
                existingState.SaveName = SaveName;
                existingState.Time = Time;
                existingState.Type = Type;
                existingState.SaveState = SaveState;
                existingState.SourcePath = SourcePath;
                existingState.TargetPath = TargetPath;
                existingState.FilesNumber = FilesNumber;
                existingState.FilesSize = FilesSize;
                existingState.Progress = Progress;
                existingState.RemainingFiles = RemainingFiles;
                existingState.RemainingFilesSize = RemainingFilesSize;



                string serializedJSON = JsonSerializer.Serialize(dataStateList);
                try
                {
                    File.WriteAllText(StateFilePath, serializedJSON);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error writing to file: " + e.Message);
                }
            }
            else
            {
                Console.WriteLine("SaveId not found in dataStateList.");
            }
        }

        // TODO
        public void DeleteState()
        {
            string jsonString = File.ReadAllText(StateFilePath);
            List<State> stateList = JsonSerializer.Deserialize<List<State>>(jsonString);

            stateList.RemoveAll(save => save.SaveId == SaveId);

            string serializedJSON = JsonSerializer.Serialize(stateList.ToArray()) + Environment.NewLine;
            File.WriteAllText(StateFilePath, serializedJSON);
        }

        public static State[] GetStateArr()
        {
            string jsonString = File.ReadAllText(StateFilePath);
            State[] stateArr = JsonSerializer.Deserialize<State[]>(jsonString);

            return stateArr ??= Array.Empty<State>();
        }

    }
}

