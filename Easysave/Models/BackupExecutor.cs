using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace EasySave.Models
{
    public class BackupExecutor
    {
        public ConcurrentDictionary<int, double> BackupsProgress = new ConcurrentDictionary<int, double>();

        private readonly ConcurrentQueue<Save> _backupQueue;
        private readonly SemaphoreSlim _maxParallelBackups;
        private readonly int _maxBackupLimit = 2;
        private CancellationTokenSource _cancellationTokenSource;
        private ManualResetEventSlim _pauseEvent;
        private bool _stopRequested = false;

        public BackupExecutor()
        {
            _backupQueue = new ConcurrentQueue<Save>();
            _maxParallelBackups = new SemaphoreSlim(_maxBackupLimit, _maxBackupLimit);
            _cancellationTokenSource = new CancellationTokenSource();
            _pauseEvent = new ManualResetEventSlim(true);
        }

        // Method to pause backups
        public void PauseBackups()
        {
            _pauseEvent.Reset();
        }

        // Method to resume backups
        public void ResumeBackups()
        {
            _pauseEvent.Set();
        }


        // Method to enqueue a backup task
        public void EnqueueBackup(Save saveTask)
        {
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
                _pauseEvent.Wait(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();

                saveTask.CreateSave(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Backup operation was canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing backup: {ex.Message}");
            }
            finally
            {
                saveTask.MarkAsCompleted();

                // Release the semaphore slot
                _maxParallelBackups.Release();
            }
        }


        public void StopAllBackups()
        {
            _stopRequested = true;

            _pauseEvent.Set();
            _maxParallelBackups.Wait();
            _cancellationTokenSource.Cancel();

            while (_backupQueue.TryDequeue(out _)) { }

            // Reset the CancellationTokenSource
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();

            // Reset the stop requested flag and semaphore
            _stopRequested = false;
            _maxParallelBackups.Release(_maxBackupLimit);
        }


    }
}
