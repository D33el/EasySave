using System;
using System.IO;
using EasySave.ViewModels;

namespace EasySave.Models
{
    public class Save
    {
        public string saveId;
        public string saveName { get; set; }
        public string saveSourcePath { get; set; }
        public int saveFilesNumber { get; set; }
        public string type { get; } 

        public Save(string SaveId, string SaveName, string SaveSourcePath, int SaveFilesNumber, string Type)
        {
            saveId = SaveId;
            saveName = SaveName;
            saveSourcePath = SaveSourcePath;
            saveFilesNumber = SaveFilesNumber;
            type = Type;
        }

        public void CreateSave()
        {
            Config configObj = Config.getConfig();
            try
            {
                // Combine the saveSourcePath and saveName to get the full path of the save folder
                string saveFolderPath = Path.Combine(configObj.targetDir, saveName);

                // Check if the source directory exists before creating a save
                if (Directory.Exists(saveSourcePath))
                {
                    // Check if the destination directory exists; if not, create it
                    if (!Directory.Exists(saveFolderPath))
                    {
                        Directory.CreateDirectory(saveFolderPath);
                        Console.WriteLine($"Save folder '{saveName}' created successfully.");
                    }

                    // Copy files from the source directory to the destination directory
                    string[] filesToCopy = Directory.GetFiles(saveSourcePath);
                    foreach (string file in filesToCopy)
                    {
                        string destinationFile = Path.Combine(saveFolderPath, Path.GetFileName(file));
                        File.Copy(file, destinationFile, true);
                    }

                    Console.WriteLine($"Save '{saveName}' created successfully.");
                }
                else
                {
                    Console.WriteLine($"Source directory '{saveSourcePath}' not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., if there are permission issues
                Console.WriteLine($"Error creating save '{saveName}': {ex.Message}");
            }
        }

        public void DeleteSave()
        {
            try
            {
                // Combine the saveSourcePath and saveName to get the full path of the save file
                string saveFilePath = Path.Combine(saveSourcePath, saveName);

                // Check if the file exists before attempting to delete it
                if (File.Exists(saveFilePath))
                {
                    // Delete the save file
                    File.Delete(saveFilePath);
                    Console.WriteLine($"Save file '{saveName}' deleted successfully.");
                }
                else
                {
                    Console.WriteLine($"Save file '{saveName}' not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., if there are permission issues
                Console.WriteLine($"Error deleting save file '{saveName}': {ex.Message}");
            }
        }

        public string[] GetSaveProgress()
        {
            string[] arr = { };

            return arr ;
        }

        public string[] GetFileNames()
        {
            try
            {
                // Combine the saveSourcePath and saveName to get the full path of the save folder
                string saveFolderPath = Path.Combine(saveSourcePath, saveName);

                // Check if the directory exists
                if (Directory.Exists(saveFolderPath))
                {
                    // Get the names of all files in the save folder
                    string[] fileNames = Directory.GetFiles(saveFolderPath).Select(Path.GetFileName).ToArray();

                    return fileNames;
                }
                else
                {
                    Console.WriteLine($"Save folder '{saveName}' not found.");
                    return Array.Empty<string>();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, e.g., if there are permission issues
                Console.WriteLine($"Error getting file names for save '{saveName}': {ex.Message}");
                return Array.Empty<string>();
            }
        }


    }


   
  
}
