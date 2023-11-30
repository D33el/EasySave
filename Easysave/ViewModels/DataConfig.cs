using System;
namespace Easysave.ViewModels
{
	public class DataConfig
	{
        public string Language { get; set; }
        public string TargetDir { get; set; }
        public string SaveLogDir { get; set; }
        public string SaveStateDir { get; set; }

        public DataConfig()
		{
		}
	}
}

