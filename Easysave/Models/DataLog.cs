using System;
namespace Easysave.Models
{
	public class DataLog
	{
        public string Timestamp;
        public string SavedFileName;
        public string SourceDir;
        public string TargetDir;
        public string SaveSize;
        public long SaveDuration;

        public DataLog(string timestamp)
		{
            Timestamp = timestamp;
		}
	}
}

