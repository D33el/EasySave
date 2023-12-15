using System;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

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
        private AccessList _accessList = AccessList.GetAccessList();
        private State _state = new State();
        private Stopwatch Duration = new Stopwatch();

        private ConcurrentDictionary<int, double> _progressTracker;
        public bool IsCompleted { get; private set; } = false;

        public Save(ConcurrentDictionary<int, double> progressTracker)
        {
            _progressTracker = progressTracker;
        }

        private void SetSaveState()
        {
            _state.SaveId = SaveId;
            _state.SaveName = SaveName;
            _state.Type = Type;
            _state.Time = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            _state.SourcePath = SaveSourcePath;
            _state.TargetPath = Path.Combine(_config.TargetDir, SaveName);
            _state.FilesSize = DirSize(new DirectoryInfo(SaveSourcePath));
            _state.FilesNumber = Directory.GetFiles(SaveSourcePath).Length;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
            UpdateSaveProgress(100); // Ensure 100% progress on completion
        }

        public void CreateSave(CancellationToken cancellationToken)
        {
            string saveTargetPath = Path.Combine(_config.TargetDir, SaveName);
            SetSaveState();

            try
            {
                if (Directory.Exists(SaveSourcePath))
                {
                    if (!Directory.Exists(saveTargetPath)) { Directory.CreateDirectory(saveTargetPath); }

                    if (Type == "full")
                    {
                        FullSave(saveTargetPath, cancellationToken);
                    }
                    else
                    {
                        DiffSave(saveTargetPath, cancellationToken);
                    }

                    //Console.WriteLine($"+++ Model Success creating : {SaveName} ");
                }
                else
                {
                    Console.WriteLine($"Error : Source directory '{SaveSourcePath}' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating '{SaveName}' backup : {ex.Message}");
            }
        }

        private void FullSave(string folderPath, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Backup operation was canceled.");
                return; // Exit the method
            }
            if (Directory.Exists(folderPath) && Directory.GetFiles(folderPath).Length != 0)
            {
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            string[] filesToCopy = Directory.GetFiles(SaveSourcePath);
            int count = 0;

            Duration.Start();

            int totalFiles = filesToCopy.Length;
            foreach (string file in filesToCopy)
            {
                count++;
                FileInfo fileInfo = new FileInfo(file);

                if (_accessList.FileIsInList("ignored", fileInfo)) return;

                Duration.Restart();
                string destinationFile = Path.Combine(folderPath, Path.GetFileName(file));

                if (_accessList.FileIsInList("encryptable", fileInfo))
                {
                    _log.EncryptionTime = EncryptedCopy(file, destinationFile);
                }
                else
                {
                    File.Copy(file, destinationFile, true);
                }
                Duration.Stop();

                _log.FileSaveDuration = Duration.ElapsedMilliseconds;
                _log.FileName = file;
                _log.FileSize = fileInfo.Length;
                _log.SourceDir = SaveSourcePath;
                _log.TargetDir = destinationFile;
                _log.Timestamp = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                _log.WriteLog();

                UpdateSaveProgress((count + 1) * 100.0 / totalFiles);
            }
            // Updating state.json file by adding a new entry
            UpdateSaveProgress(100);
            _state.UpdateState();
        }

        private void DiffSave(string folderPath, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Backup operation was canceled.");
                return; // Exit the method
            }
            string[] destinationFiles = Directory.GetFiles(folderPath);
            string[] sourceFiles = Directory.GetFiles(SaveSourcePath);
            List<string> newModifiedFiles = CompareFiles(sourceFiles, destinationFiles);

            Duration.Start();
            foreach (string file in newModifiedFiles)
            {

                FileInfo fileInfo = new FileInfo(file);
                Duration.Restart();

                string fileName = Path.GetFileName(file);
                string destinationFile = Path.Combine(folderPath, fileName);
                if (_accessList.FileIsInList("encryptable", fileInfo))
                {
                    _log.EncryptionTime = EncryptedCopy(file, destinationFile);
                }
                else
                {
                    File.Copy(file, destinationFile, true);
                }
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

        private static List<string> CompareFiles(string[] sourceFiles, string[] destinationFiles)
        {
            List<string> newModifiedFiles = new List<string>();

            foreach (string sourceFile in sourceFiles)
            {
                string sourceFileName = Path.GetFileName(sourceFile);

                // Find corresponding file in destinationFiles array
                string destinationFile = destinationFiles.FirstOrDefault(df =>
                    string.Equals(Path.GetFileName(df), sourceFileName, StringComparison.OrdinalIgnoreCase));

                if (destinationFile != null)
                {
                    // Compare last write times of source and destination files
                    DateTime sourceLastWriteTime = File.GetLastWriteTimeUtc(sourceFile);
                    DateTime destinationLastWriteTime = File.GetLastWriteTimeUtc(destinationFile);

                    if (sourceLastWriteTime > destinationLastWriteTime)
                    {
                        newModifiedFiles.Add(sourceFile);
                    }
                }
                else
                {
                    // File exists in source but not in destination
                    newModifiedFiles.Add(sourceFile);
                }
            }

            return newModifiedFiles;
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

        //public void UpdateSaveProgress()
        //{
        //    double progress = 0;
        //    try
        //    {
        //        string[] filesToCopy = Directory.GetFiles(SaveSourcePath);
        //        string saveFolderPath = Path.Combine(_config.TargetDir, SaveName);

        //        int totalFiles = filesToCopy.Length;
        //        int remainingFiles = totalFiles - Directory.GetFiles(saveFolderPath).Length;

        //        // Calculate the total size of files to be copied
        //        long totalSize = filesToCopy.Sum(file => new FileInfo(file).Length);

        //        // Calculate the remaining size to be copied
        //        long remainingSize = filesToCopy
        //            .Where(file => !File.Exists(Path.Combine(saveFolderPath, Path.GetFileName(file))))
        //            .Sum(file => new FileInfo(file).Length);

        //        progress = Math.Round(100.0 * (totalFiles - remainingFiles) / totalFiles, 2);
        //        _progressTracker[SaveId] = progress;

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error calculating save progress: {ex.Message}");
        //    }
        //}

        public void UpdateSaveProgress(double progress)
        {
            progress = Math.Min(progress, 100.0); // Cap the progress at 100%
            _progressTracker[SaveId] = progress;
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

        public static int[] GetSavesTypesNumber()
        {
            int fullSaveCount = 0;
            int diffSaveCount = 0;

            State[] existingStates = State.GetStateArr();

            foreach (var existingState in existingStates)
            {
                if (existingState.Type == "full") { fullSaveCount++; }
                else if (existingState.Type == "diff") { diffSaveCount++; }
            }

            return new int[] { fullSaveCount, diffSaveCount };
        }

        public static int GetEncryptedFilesNumber()
        {
            int count = 0;
            Config _config = Config.GetConfig();
            AccessList lists = AccessList.GetAccessList();

            DirectoryInfo targetDir = new DirectoryInfo(_config.TargetDir);
            foreach (var dir in targetDir.GetDirectories())
            {
                foreach (var file in dir.GetFiles())
                {
                    if (lists.FileIsInList("encryptable", file)) { count++; }
                }
            }

            return count;
        }

        private static long EncryptedCopy(string sourceFile, string targetFile)
        {
            Stopwatch EncryptionDuration = new Stopwatch();
            string appDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + @"/Assets/CryptoSoft/";
            string arguments = $"\"{sourceFile}\" \"{targetFile}\"";
            try
            {
                EncryptionDuration.Start();

                using Process process = new Process();

                process.StartInfo.FileName = Path.Combine(appDir, "Cryptosoft.exe");
                process.StartInfo.Arguments = arguments;
                Console.WriteLine("Process start info");
                Console.WriteLine($"fileName = ${process.StartInfo.FileName}");
                Console.WriteLine($"arguments = ${process.StartInfo.Arguments}");
                process.Start();

                process.WaitForExit();
                //TODO : finir l'eventhandler (WS 5)
                process.EnableRaisingEvents = true;

                EncryptionDuration.Stop();

                return EncryptionDuration.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Encryption failed : {ex}");
                return -1;
            }



        }
    }
}
