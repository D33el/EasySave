using EasySave.ViewModels;
//using EasySave.Views;


namespace EasySave
{
    class Program
    {

        static int SavesCounter = 0;
        static async Task Main(string[] args)
        {
            var viewModel = new SaveViewModel();
            while (true)
            {
                Console.WriteLine("=========================== V3.0");
                Console.WriteLine("1. Create a new backup");
                Console.WriteLine("2. Re-execute all backup");
                Console.WriteLine("3. Delete a backup");
                Console.WriteLine("4. Cancel all backups");
                Console.WriteLine("5. Exit");

                switch (Console.ReadLine())
                {
                    case "1":
                        SavesCounter++;
                        CreateNewBackup(viewModel);
                        _ = MonitorBackupProgressAsync(viewModel); // Start monitoring in a separate task
                        await viewModel.ExecuteEnqueuedBackupsAsync(); // Execute backups
                        break;
                    case "2":
                        await viewModel.ReexecuteAllBackupsAsync();
                        _ = MonitorBackupProgressAsync(viewModel); // Start monitoring in a separate task
                        await viewModel.ExecuteEnqueuedBackupsAsync(); // Execute backups
                        break;
                    case "3":
                        viewModel.CancelAllBackups();
                        Console.WriteLine("All backups have been canceled.");
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        private static void CreateNewBackup(SaveViewModel viewModel)
        {
            // Example inputs - modify as needed for your testing
            string saveName = "TestBackup" + SavesCounter;
            string saveType = "full"; // or "diff"
            string sourcePath = "/users/adel/desktop/files/file" + SavesCounter;
            int saveId = SavesCounter;
            Console.Clear();
            Console.WriteLine($"=== Creating a new backup: {saveName}");
            viewModel.InitializeSave(saveName, saveType, sourcePath, saveId);
        }

        private static void DrawProgressBar(int percentage, int width, char progressCharacter = '#', char backgroundCharacter = '-')
        {
            int progressWidth = (int)((width * percentage) / 100.0);
            string progressBar = new string(progressCharacter, progressWidth) + new string(backgroundCharacter, width - progressWidth);

            Console.Write("\r[{0}] {1}%", progressBar, percentage);
        }

        private static async Task MonitorBackupProgressAsync(SaveViewModel viewModel)
        {
            // Remember the initial cursor position
            int initialCursorTop = Console.CursorTop;

            while (viewModel.AreBackupsActive)
            {
                // Reset cursor position to overwrite existing progress lines
                Console.SetCursorPosition(0, initialCursorTop);
                Console.WriteLine("\nCurrent Backup Progress:");

                var backupProgress = viewModel.GetBackupProgress();
                foreach (var progress in backupProgress)
                {
                    Console.Write($"Backup ID {progress.Key}: ");
                    DrawProgressBar((int)progress.Value, 50); // 50 is the width of the progress bar
                    Console.WriteLine();
                }

                // Check if all backups are complete (100%)
                if (backupProgress.All(p => p.Value >= 100))
                {
                    viewModel.AreBackupsActive = false;
                }

                await Task.Delay(1000); // Update every 1 second
            }

            Console.WriteLine("\nAll backups are completed.");
        }




    }
}

