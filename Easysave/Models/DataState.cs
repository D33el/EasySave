using System;
using System.Collections.Generic;
using System.Text;

namespace Easysave.Models
{
    public class DataState
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
        public float Progress { get; set; }
        public long RemainingFiles { get; set; }
        public long RemainingFilesSize { get; set; }

        public DataState()
        {

        }
    }
}
