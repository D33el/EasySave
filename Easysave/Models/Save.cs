using System;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace EasySave.Models
{
    public class Save
    {
        public int SaveId { get; set; }
        public string SaveName { get; set; }
        public string SaveSourcePath { get; set; }
        public string Type { get; set; }

        private Config _config = Config.GetConfig();
        private Log _log = Log.GetLog();
        private State _state = new State();
        private Stopwatch Duration = new Stopwatch();

        public Save() { }

        public void CreateSave()
        {
            string saveTargetPath = Path.Combine(_config.TargetDir, SaveName);
            string lang = _config.Language;

            _state.SaveId = SaveId;
            _state.SaveName = SaveName;
            _state.Type = Type;
            _state.Time = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            _state.SourcePath = SaveSourcePath;
            _state.TargetPath = saveTargetPath;
            _state.FilesSize = DirSize(new DirectoryInfo(SaveSourcePath));
            _state.FilesNumber = Directory.GetFiles(SaveSourcePath).Length;

            try
            {
                if (Directory.Exists(SaveSourcePath))
                {
                    if (!Directory.Exists(saveTargetPath)) { Directory.CreateDirectory(saveTargetPath); }

                    if (Type == "full") { FullSave(saveTargetPath); }
                    else { DiffSave(saveTargetPath); }

                    if (lang == "fr") { Console.WriteLine($"Sauvegarde '{SaveName}' créée avec succès."); }
                    else { Console.WriteLine($"Backup '{SaveName}' created with success."); }
                }
                else
                {
                    if (lang == "fr") { Console.WriteLine($"Répertoire source '{SaveSourcePath}' introuvable."); }
                    else { Console.WriteLine($"Source directory '{SaveSourcePath}' not found."); }
                }
            }
            catch (Exception ex)
            {
                if (lang == "fr") { Console.WriteLine($"Erreur lors de la création de la sauvegarde '{SaveName}': {ex.Message}"); }
                else { Console.WriteLine($"Error creating '{SaveName}' backup : {ex.Message}"); }
            }
        }

        private void FullSave(string folderPath)
        {
            if (Directory.Exists(folderPath) && Directory.GetFiles(folderPath).Length != 0)
            {
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            string[] filesToCopy = Directory.GetFiles(SaveSourcePath);

            foreach (string file in filesToCopy)
            {
                FileInfo fileInfo = new FileInfo(file);

                Duration.Start();
                string destinationFile = Path.Combine(folderPath, Path.GetFileName(file));
                File.Copy(file, destinationFile, true);
                Duration.Stop();

                _log.FileSaveDuration = Duration.ElapsedMilliseconds;
                _log.FileName = file;
                _log.FileSize = fileInfo.Length;
                _log.SourceDir = SaveSourcePath;
                _log.TargetDir = destinationFile;
                _log.Timestamp = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                _log.WriteLog();
            }
            // Updating state.json file by adding a new entry
            _state.AddState();
        }

        private void DiffSave(string folderPath)
        {
            string[] destinationFiles = Directory.GetFiles(folderPath);
            string[] sourceFiles = Directory.GetFiles(SaveSourcePath);
            string[] newModifiedFiles = CompareFiles(sourceFiles, destinationFiles);

            foreach (string file in newModifiedFiles)
            {

                FileInfo fileInfo = new FileInfo(file);
                Duration.Restart();

                string fileName = Path.GetFileName(file);
                string destinationFile = Path.Combine(folderPath, fileName);
                File.Copy(file, destinationFile, true);
                Duration.Stop();

                _log.FileSaveDuration = Duration.ElapsedMilliseconds;
                _log.FileName = file;
                _log.FileSize = fileInfo.Length;
                _log.SourceDir = SaveSourcePath;
                _log.TargetDir = destinationFile;
                _log.Timestamp = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");

                _log.WriteLog();

            }
            // Updating state.json file by adding a new entry
            _state.UpdateState();
        }

        public void DeleteSave()
        {
            string lang = _config.Language;
            try
            {
                string saveFilePath = Path.Combine(_config.TargetDir, SaveName);

                if (Directory.Exists(saveFilePath))
                {
                    string[] files = Directory.GetFiles(saveFilePath);
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                    Directory.Delete(saveFilePath);

                    // Updating state.json file
                    _state.SaveId = SaveId;
                    _state.DeleteState();

                    if (lang == "fr") { Console.WriteLine($"Sauvegarde '{SaveName}' supprimée avec succés"); }
                    else { Console.WriteLine($"Backup '{SaveName}' deleted successfully."); }
                }
                else
                {
                    if (lang == "fr") { Console.WriteLine($"Sauvegarde '{SaveName}' introuvable."); }
                    else { Console.WriteLine($"Backup '{SaveName}' not found"); }
                }
            }
            catch (Exception ex)
            {
                if (lang == "fr") { Console.WriteLine($"Erreur lors de la suppression de '{SaveName}': {ex.Message}"); }
                else { Console.WriteLine($"Error deleting save file '{SaveName}': {ex.Message}"); }
            }
        }

        public void GetSaveProgress()
        {
            string lang = _config.Language;
            try
            {
                string[] filesToCopy = Directory.GetFiles(SaveSourcePath);
                string saveFolderPath = Path.Combine(_config.TargetDir, SaveName);

                int totalFiles = filesToCopy.Length;
                int remainingFiles = totalFiles - Directory.GetFiles(saveFolderPath).Length;

                // Calculate the total size of files to be copied
                long totalSize = filesToCopy.Sum(file => new FileInfo(file).Length);

                // Calculate the remaining size to be copied
                long remainingSize = filesToCopy
                    .Where(file => !File.Exists(Path.Combine(saveFolderPath, Path.GetFileName(file))))
                    .Sum(file => new FileInfo(file).Length);

                double progress = 100.0 * (totalFiles - remainingFiles) / totalFiles;

                _state.Progress = progress;
                _state.RemainingFiles = remainingFiles;
                _state.RemainingFilesSize = remainingSize;
            }
            catch (Exception ex)
            {
                if (lang == "fr") { Console.WriteLine($"Erreur lors du calcul du progres: {ex.Message}"); }
                else { Console.WriteLine($"Error calculating save progress: {ex.Message}"); }
            }
        }

        private static string[] CompareFiles(string[] sourceFiles, string[] destinationFiles)
        {
            List<string> newModifiedFiles = new List<string>();

            foreach (string sourceFile in sourceFiles)
            {
                string sourceFileName = Path.GetFileName(sourceFile);

                foreach (var destinationFile in destinationFiles)
                {
                    string destinationFileName = Path.GetFileName(sourceFile);
                    if (sourceFileName != destinationFileName || File.GetLastWriteTimeUtc(sourceFile) > File.GetLastWriteTimeUtc(destinationFile))
                    {
                        newModifiedFiles.Add(sourceFile);
                    }
                }
            }
            return newModifiedFiles.ToArray();
        }

        private static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        public static void CalculateStorageStatistics()
        {
            long totalSizeBytes = 0;
            int totalFiles = 0;

            State[] existingStates = State.GetStateArr();

            foreach (var existingState in existingStates)
            {
                totalSizeBytes += existingState.FilesSize;
                totalFiles += existingState.FilesNumber;
            }

            double totalSizeGB = ConvertBytesToGigabytes(totalSizeBytes);

            Console.WriteLine($"Nombre total de gigaoctets stockés : {totalSizeGB} Go");
            Console.WriteLine($"Nombre total de fichiers stockés : {totalFiles} fichiers");
        }

        private static double ConvertBytesToGigabytes(long bytes)
        {
            const double BytesInGigabyte = 1.0 / (1024 * 1024 * 1024);
            return bytes * BytesInGigabyte;
        }

        public static void CalculateSaveTypesCount()
        {
            

            int fullSaveCount = 0;
            int diffSaveCount = 0;
           

            State[] existingStates = State.GetStateArr();
           

            foreach (var existingState in existingStates)
            {
                

                if (existingState.Type == "full")
                {
                    fullSaveCount++;
                }
                else if (existingState.Type == "diff")
                {
                    diffSaveCount++;
                }
                
            }

            Console.WriteLine($"Nombre de sauvegardes complètes : {fullSaveCount}");
            Console.WriteLine($"Nombre de sauvegardes différentielles : {diffSaveCount}");
            

            
        }

        public void Encrypt(string sourceDir, string targetDir)
        {
            using Process process = new Process();
            process.StartInfo.FileName = @"/CryptoSoft/CryptoSoft.exe";
            process.StartInfo.Arguments = String.Format("\"{0}\"", sourceDir) + " " + String.Format("\"{0}\"", targetDir);
            process.Start();
            process.Close();

        }
    }
}
