using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace EasySaveCloud
{
	internal class SaveHandling
	{
        private object lck = new object();
        public List<SaveItem> DeserializeSavesJson(string jsonString)
        {
            lock (lck) {
            try
            {
                List<SaveItem> data = JsonSerializer.Deserialize<List<SaveItem>>(jsonString);
                return data ?? new List<SaveItem>(); 
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
                return new List<SaveItem>(); // Return an empty list in case of error
            }
            }
        }

    }

    public class SaveItem : INotifyPropertyChanged
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
        public string FilesSizeString { get; set; }
        public string SaveStateString { get; set; }

        private double _progress;
        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                Debug.WriteLine("==============inside object set=================");
                Debug.WriteLine(value);
                OnPropertyChanged(nameof(Progress));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
