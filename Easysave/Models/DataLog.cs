using System;
namespace Easysave.Models
{
	public class DataLog
	{
        public string Timestamp { get; set; }
        public string SavedFileName { get; set; }
        public string SourceDir { get; set; }
        public string TargetDir { get; set; }
        public long SaveSize { get; set; }
        public long SaveDuration { get; set; }

        public DataLog()
		{

		}
	}
}

