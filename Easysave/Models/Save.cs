using System;//bibliothéque (classes,methodes...)
using System.IO;// bibliotheque pour lire ou ecrire pour avoir acces a un file ou directories
using System.Linq.Expressions;
using Easysave.ViewModels;

namespace Easysave.Models
{
    public class Save
    {
        public int SaveId;
        public string SaveName { get; set; }
        public string SaveSourcePath { get; set; }
        public int SaveFilesNumber { get; set; }
        public string Type { get; set; }

        private DataState saveDataState = new();


        Config configObj = Config.getConfig();

        public Save() { }

        public void CreateSave()
        {
            string saveFolderPath = Path.Combine(configObj.TargetDir, SaveName);
            string lang = configObj.Language;
            saveDataState.SaveId = SaveId;
            saveDataState.SaveName = SaveName;
            saveDataState.Time = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            saveDataState.SourcePath = SaveSourcePath;
            saveDataState.TargetPath = saveFolderPath;
            saveDataState.Type = Type;
            saveDataState.FilesSize = DirSize(new DirectoryInfo(SaveSourcePath));
            saveDataState.FilesNumber = Directory.GetFiles(SaveSourcePath).Length;

            try
            {
                if (Directory.Exists(SaveSourcePath))
                {
                    if (!Directory.Exists(saveFolderPath))
                    {
                        Directory.CreateDirectory(saveFolderPath);
                    }
                    if (Type == "full")
                    {
                        string[] filesToCopy = Directory.GetFiles(SaveSourcePath);
                        saveDataState.FilesNumber = filesToCopy.Length;
                        foreach (string file in filesToCopy)
                        {
                            string destinationFile = Path.Combine(saveFolderPath, Path.GetFileName(file));
                            File.Copy(file, destinationFile, true);
                        }
                        // Updating state.json file by adding a new entry
                        State stateObj = new State
                        {
                            DataState = saveDataState
                        };
                        stateObj.CreateState();
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
                        // Updating state.json file by adding a new entry
                        State stateObj = new State
                        {
                            DataState = saveDataState
                        };
                        stateObj.UpdateState();
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
                if (lang == "fr")
                {
                    Console.WriteLine($"Erreur lors de la création de la sauvegarde '{SaveName}': {ex.Message}");
                }
                else
                {
                    Console.WriteLine($"Error creating '{SaveName}' backup : {ex.Message}");
                }

            }
        }


        public void DeleteSave()
        {
            string lang = configObj.Language;
            try
            {
                // Combine the TargetPath and saveName to get the full path of the save file
                string saveFilePath = Path.Combine(configObj.TargetDir, SaveName);

                // Check if the directory exists before attempting to delete it
                if (Directory.Exists(saveFilePath))
                {
                    // Empty the directory before deleting it
                    string[] files = Directory.GetFiles(saveFilePath);

                    foreach (string file in files)
                    {
                        File.Delete(file);
                        Console.WriteLine($"Deleted file: {file}");
                    }

                    // Delete the save directory
                    Directory.Delete(saveFilePath);

                    // Updating state.json file

                    saveDataState.SaveId = SaveId;
                    State stateObj = new()
                    {
                        DataState = saveDataState
                    };
                    stateObj.DeleteState();

                    if (lang == "fr")
                    {
                        Console.WriteLine($"Sauvegarde '{SaveName}' supprimée avec succés");
                    }
                    else
                    {
                        Console.WriteLine($"Backup '{SaveName}' deleted successfully.");
                    }
                }
                else
                {
                    if (lang == "fr")
                    {
                        Console.WriteLine($"Sauvegarde '{SaveName}' introuvable.");
                    }
                    else
                    {
                        Console.WriteLine($"Backup '{SaveName}' not found");
                    }
                }
            }
            catch (Exception ex)
            {
                if (lang == "fr")
                {
                    Console.WriteLine($"Erreur lors de la suppression de '{SaveName}': {ex.Message}");
                }
                else
                {
                    Console.WriteLine($"Error deleting save file '{SaveName}': {ex.Message}");
                }
            }
        }

       public DataState GetSaveProgress()
{
    try
    {
        // Retrieve the list of full paths for all files in the source directory
        string[] filesToCopy = Directory.GetFiles(SaveSourcePath);

        // Create the full path of the destination directory
        string saveFolderPath = Path.Combine(configObj.TargetDir, SaveName);

        // Calculate the total number of files and the number of remaining files to be copied
        int totalFiles = filesToCopy.Length;
        int remainingFiles = totalFiles - Directory.GetFiles(saveFolderPath).Length;

        // Calculate the total size of files to be copied
        long totalSize = filesToCopy.Sum(file => new FileInfo(file).Length);

        // Calculate the remaining size to be copied
        long remainingSize = filesToCopy
            .Where(file => !File.Exists(Path.Combine(saveFolderPath, Path.GetFileName(file))))
            .Sum(file => new FileInfo(file).Length);

        // Calculate the progress percentage
        double progress = 100.0 * (totalFiles - remainingFiles) / totalFiles;

        // Create a DataState object with progress details
        DataState progressDetails = new DataState() {
            Progress = progress,
            RemainingFiles = remainingFiles,
            RemainingFilesSize = remainingSize
        };

        // Return the DataState object with progress details
        return progressDetails;
    }
    catch (Exception ex)
    {
        // Handle errors and print an error message in case of failure
        Console.WriteLine($"Error calculating save progress: {ex.Message}");

        // Return a new DataState object in case of an exception
        return new DataState();
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
    }
}
