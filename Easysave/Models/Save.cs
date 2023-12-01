using System;
using System.IO;
using Easysave.ViewModels;

namespace Easysave.Models
{
    public class Save
    {
        public string SaveId;
        public string SaveName { get; set; }
        public string SaveSourcePath { get; set; }
        public int SaveFilesNumber { get; set; }
        public string Type { get; set; }
        public DataState DataState { get; set; }

        Config configObj = Config.getConfig();

        public Save() { }

        public void CreateSave()
        {
            string lang = configObj.Language;
            try
            {
                string saveFolderPath = Path.Combine(configObj.TargetDir, SaveName);

                if (Directory.Exists(SaveSourcePath))
                {
                    if (!Directory.Exists(saveFolderPath))
                    {
                        Directory.CreateDirectory(saveFolderPath);
                    }
                    if (Type == "full")
                    {
                        string[] filesToCopy = Directory.GetFiles(SaveSourcePath);
                        foreach (string file in filesToCopy)
                        {
                            string destinationFile = Path.Combine(saveFolderPath, Path.GetFileName(file));
                            File.Copy(file, destinationFile, true);
                        }
                    }
                    else if (Type == "diff")
                    {
                        string[] destinationFiles = Directory.GetFiles(saveFolderPath);
                        string[] sourceFiles = Directory.GetFiles(SaveSourcePath);
                        string[] newModifiedFiles = CompareFiles(sourceFiles, destinationFiles, saveFolderPath);

                        foreach (string file in newModifiedFiles)
                        {
                            string destinationFile = Path.Combine(saveFolderPath, Path.GetFileName(file));
                            File.Copy(file, destinationFile, true);
                        }
                    }
                    if (lang == "fr")
                    {
                    Console.WriteLine($"Sauvegarde '{SaveName}' créée avec succès.");
                    }
                    else
                    {
                    Console.WriteLine($"Backup '{SaveName}' created with success.");
                    }
                }
                else
                {
                    if (lang == "fr")
                    {
                        Console.WriteLine($"Répertoire source '{SaveSourcePath}' introuvable.");
                    }
                    else
                    {
                        Console.WriteLine($"Source directory '{SaveSourcePath}' not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création de la sauvegarde '{SaveName}': {ex.Message}");
            }
        }


        public void DeleteSave(int saveId) // Ajouter un paramètre (id)
        {

            try
            {
                // Combine the TargetPath and saveName to get the full path of the save file
                string saveFilePath = Path.Combine(configObj.TargetDir, SaveName);

                // Check if the file exists before attempting to delete it
                if (File.Exists(saveFilePath))
                {
                    // Delete the save file
                    File.Delete(saveFilePath);
                    Console.WriteLine($"Save file '{SaveName}' deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Save file '{SaveName}' not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., if there are permission issues
                Console.WriteLine($"Error deleting save file '{SaveName}': {ex.Message}");
            }
        }

        public string[] GetSaveProgress()
        {
            string[] arr = { };
            return arr;
        }

        public string[] GetFileNames()
        {
            try
            {
                // Combine the saveSourcePath and saveName to get the full path of the save folder
                string saveFolderPath = Path.Combine(SaveSourcePath, SaveName);

                // Check if the directory exists
                if (Directory.Exists(saveFolderPath))
                {
                    // Get the names of all files in the save folder
                    string[] fileNames = Directory.GetFiles(saveFolderPath).Select(Path.GetFileName).ToArray();

                    return fileNames;
                }
                else
                {
                    Console.WriteLine($"Save folder '{SaveName}' not found.");
                    return Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., if there are permission issues
                Console.WriteLine($"Error getting file names for save '{SaveName}': {ex.Message}");
                return Array.Empty<string>();
            }
        }

        private string[] CompareFiles(string[] sourceFiles, string[] destinationFiles, string saveFolderPath)
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
    }
}
