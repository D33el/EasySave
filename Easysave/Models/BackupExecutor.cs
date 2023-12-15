using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace EasySave.Models
{
    public class BackupExecutor
    {
        private readonly ConcurrentQueue<Save> _backupQueue;
        private readonly SemaphoreSlim _maxParallelBackups;
        private readonly int _maxBackupLimit = 2;
        private CancellationTokenSource _cancellationTokenSource;

        public ConcurrentDictionary<int, double> BackupsProgress = new ConcurrentDictionary<int, double>();

        public BackupExecutor()
        {
            _backupQueue = new ConcurrentQueue<Save>();
            _maxParallelBackups = new SemaphoreSlim(_maxBackupLimit, _maxBackupLimit);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        // Method to enqueue a backup task
        public void EnqueueBackup(Save saveTask)
        {
            Console.WriteLine($"+++ Model.BackupExecutor Enqueued : {saveTask.SaveName}");
            _backupQueue.Enqueue(saveTask);
            BackupsProgress[saveTask.SaveId] = 0;
        }

        // Method to start executing backups
        public async Task ExecuteBackupsAsync(CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"+++ Model.BackupExecutor Number of backups to be done {_backupQueue.Count}");
            while (!_backupQueue.IsEmpty)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    //TODO Handle cancellation
                    break;
                }

                if (_backupQueue.TryDequeue(out Save saveTask))
                {
                    // Wait for an available slot
                    await _maxParallelBackups.WaitAsync(cancellationToken);

                    // Run the backup task in a separate task
                    Console.WriteLine($"+++ Model.BackupExecutor Executing : {saveTask.SaveName}");
                    Task.Run(() => RunBackupAsync(saveTask, cancellationToken));
                    
                }
            }
        }

        // Method to run an individual backup task
        private async Task RunBackupAsync(Save saveTask, CancellationToken cancellationToken)
        {
            try
            {
                //Check for cancelation token before 
                cancellationToken.ThrowIfCancellationRequested();
                saveTask.CreateSave(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Backup operation was canceled.");
                // TODO : envoyer un message pour le front 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing backup: {ex.Message}");
            }
            finally
            {
                saveTask.MarkAsCompleted(); // Mark the task as completed
                _maxParallelBackups.Release();
            }
        }


        // A method to stop all ongoing and queued backups
        public void StopAllBackups()
        {
            // Signal cancellation to all running backups
            _cancellationTokenSource.Cancel();

            // Clear the queue to prevent further backups from starting
            while (_backupQueue.TryDequeue(out _))
            {
                // Optionally, handle the dequeued tasks, e.g., logging or cleanup
            }

            // Reset the CancellationTokenSource for future use
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }


    }
}
