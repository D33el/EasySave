using System;

namespace EasySave.Models
{
    public class Log
    {
        public string saveName;
        public string sourceDir;
        public string targetDir;
        public string saveSize;
        public long saveDuration;


        public Log(string SaveName, string SourceDir, string TargetDir, string SaveSize, long SaveDuration)
        {
            saveName = SaveName;
            sourceDir = SourceDir;
            targetDir = TargetDir;
            saveSize = SaveSize;
            saveDuration = SaveDuration;
        }


        public void CreateLogFile()
        {

        }

        public void WriteLog()
        {

        }



    }




}