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


        public Save() { }

        public void CreateSave() // Ajouter un paramètre (objet)
        {
            Config configObj = Config.getConfig();
            try
            {
                // Combine the saveSourcePath and saveName to get the full path of the save folder
                string saveFolderPath = Path.Combine(configObj.TargetDir, SaveName);
                

                // Check if the source directory exists before creating a save
                if (Directory.Exists(SaveSourcePath))
                {
                    // Check if the destination directory exists; if not, create it
                    if (!Directory.Exists(saveFolderPath))
                    {
                        Directory.CreateDirectory(saveFolderPath);
                        Console.WriteLine($"Save folder '{SaveName}' created successfully.");
                    }

                    // Copy files from the source directory to the destination directory
                    string[] filesToCopy = Directory.GetFiles(SaveSourcePath);
                    foreach (string file in filesToCopy)
                    {
                        string destinationFile = Path.Combine(saveFolderPath, Path.GetFileName(file));
                        File.Copy(file, destinationFile, true);
                    }

                    Console.WriteLine($"Save '{SaveName}' created successfully.");
                }
                else
                {
                    Console.WriteLine($"Source directory '{SaveSourcePath}' not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., if there are permission issues
                Console.WriteLine($"Error creating save '{SaveName}': {ex.Message}");
            }
        }

        public void DeleteSave(int saveId) // Ajouter un paramètre (id)
        {
            try
            {
                // Combine the saveSourcePath and saveName to get the full path of the save file
                string saveFilePath = Path.Combine(SaveSourcePath, SaveName);

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
    }
}
