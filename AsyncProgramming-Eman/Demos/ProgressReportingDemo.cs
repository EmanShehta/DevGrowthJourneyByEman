using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates progress reporting from asynchronous functions
    /// </summary>
    public class ProgressReportingDemo
    {
        /// <summary>
        /// Runs the Progress Reporting demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Report a Progress from Async Function", "اعطاء تقرير بنسبة الانجاز");
            
            ConsoleHelper.WriteInfo("This demo shows how to report progress from asynchronous operations.");
            ConsoleHelper.WriteInfo("Progress reporting allows you to update the UI during long-running tasks.\n");
            
            // Basic progress reporting
            DemonstrateBasicProgressReporting();
            
            // Progress with async/await
            DemonstrateProgressWithAsyncAwait();
            
            // Progress with cancellation
            DemonstrateProgressWithCancellation();
            
            // Multiple progress reporters
            DemonstrateMultipleProgressReporters();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Progress Reporting");
            Console.WriteLine("Key points about progress reporting:");
            Console.WriteLine("✓ Use IProgress<T> and Progress<T> to report progress from async methods");
            Console.WriteLine("✓ Progress callbacks run on the original context (UI thread in GUI apps)");
            Console.WriteLine("✓ Progress reporting works well with cancellation tokens");
            Console.WriteLine("✓ You can use multiple progress reporters for different types of progress");
            Console.WriteLine("✓ Progress reporting is thread-safe and doesn't block the async operation");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates basic progress reporting
        /// </summary>
        private void DemonstrateBasicProgressReporting()
        {
            ConsoleHelper.WriteSubheader("Basic Progress Reporting");
            
            Console.WriteLine("Demonstrating basic progress reporting from an async operation...");
            
            // Create a progress reporter
            IProgress<int> progress = new Progress<int>(percent =>
            {
                ConsoleHelper.ClearCurrentLine();
                Console.Write($"Progress: {percent}% ");
                ConsoleHelper.DisplayProgressBar(percent, 100);
            });
            
            // We need to use an async local function
            async Task BasicProgressDemoAsync()
            {
                Console.WriteLine("Starting a long-running operation with progress reporting...\n");
                
                // Call an async method that reports progress
                await DoWorkWithProgressAsync(progress, 10);
                
                Console.WriteLine("\nOperation completed successfully");
            }
            
            // Run the async demo and wait for it to complete
            BasicProgressDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nThe Progress<T> class:");
            ConsoleHelper.WriteInfo("- Captures the current synchronization context when created");
            ConsoleHelper.WriteInfo("- Ensures callbacks run on the original context (UI thread in GUI apps)");
            ConsoleHelper.WriteInfo("- Provides a thread-safe way to report progress from any thread");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates progress reporting with async/await
        /// </summary>
        private void DemonstrateProgressWithAsyncAwait()
        {
            ConsoleHelper.WriteSubheader("Progress with Async/Await");
            
            Console.WriteLine("Demonstrating progress reporting with async/await pattern...");
            
            // Create a progress reporter
            IProgress<ProgressData> progress = new Progress<ProgressData>(data =>
            {
                ConsoleHelper.ClearCurrentLine();
                Console.Write($"Processing file {data.CurrentItem} of {data.TotalItems} ({data.PercentComplete}%) ");
                ConsoleHelper.DisplayProgressBar(data.PercentComplete, 100);
            });
            
            // We need to use an async local function
            async Task ProcessFilesDemoAsync()
            {
                Console.WriteLine("Starting file processing operation...\n");
                
                // Call an async method that processes files and reports progress
                int processedCount = await ProcessFilesAsync(10, progress);
                
                Console.WriteLine($"\nOperation completed. Processed {processedCount} files.");
            }
            
            // Run the async demo and wait for it to complete
            ProcessFilesDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nWith async/await:");
            ConsoleHelper.WriteInfo("- Progress reporting doesn't block the async operation");
            ConsoleHelper.WriteInfo("- You can report complex progress data using custom types");
            ConsoleHelper.WriteInfo("- Progress callbacks run on the appropriate context automatically");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates progress reporting with cancellation
        /// </summary>
        private void DemonstrateProgressWithCancellation()
        {
            ConsoleHelper.WriteSubheader("Progress with Cancellation");
            
            Console.WriteLine("Demonstrating progress reporting with cancellation support...");
            
            // Create a progress reporter
            IProgress<int> progress = new Progress<int>(percent =>
            {
                ConsoleHelper.ClearCurrentLine();
                Console.Write($"Progress: {percent}% ");
                ConsoleHelper.DisplayProgressBar(percent, 100);
            });
            
            // Create a cancellation token source
            CancellationTokenSource cts = new CancellationTokenSource();
            
            // We need to use an async local function
            async Task CancellableProgressDemoAsync()
            {
                try
                {
                    Console.WriteLine("Starting a cancellable operation with progress reporting...\n");
                    
                    // Call an async method that reports progress and supports cancellation
                    await DoWorkWithProgressAsync(progress, 20, cts.Token);
                    
                    Console.WriteLine("\nOperation completed successfully");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("\nOperation was cancelled");
                }
            }
            
            // Start the async operation
            Task demoTask = CancellableProgressDemoAsync();
            
            // Wait a bit then cancel
            Thread.Sleep(3000);
            Console.WriteLine("\nCancelling the operation...");
            cts.Cancel();
            
            // Wait for the demo to complete
            try
            {
                demoTask.Wait();
            }
            catch (AggregateException)
            {
                // Ignore exceptions
            }
            
            ConsoleHelper.WriteInfo("\nCombining progress reporting with cancellation:");
            ConsoleHelper.WriteInfo("- Allows users to cancel long-running operations");
            ConsoleHelper.WriteInfo("- Provides feedback during the operation");
            ConsoleHelper.WriteInfo("- Creates a better user experience for long-running tasks");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates using multiple progress reporters
        /// </summary>
        private void DemonstrateMultipleProgressReporters()
        {
            ConsoleHelper.WriteSubheader("Multiple Progress Reporters");
            
            Console.WriteLine("Demonstrating multiple progress reporters for different aspects...");
            
            // Create progress reporters for different aspects
            IProgress<int> overallProgress = new Progress<int>(percent =>
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Overall progress: {percent}% ");
                ConsoleHelper.DisplayProgressBar(percent, 100);
            });
            
            IProgress<string> statusProgress = new Progress<string>(status =>
            {
                Console.SetCursorPosition(0, Console.CursorTop + 1);
                Console.Write($"Current status: {status}".PadRight(50));
            });
            
            // We need to use an async local function
            async Task MultiProgressDemoAsync()
            {
                Console.WriteLine("Starting a complex operation with multiple progress reporters...\n");
                Console.WriteLine(); // Reserve line for overall progress
                Console.WriteLine(); // Reserve line for status progress
                
                // Call an async method that reports multiple types of progress
                await DoComplexWorkAsync(overallProgress, statusProgress);
                
                Console.WriteLine("\n\nOperation completed successfully");
            }
            
            // Run the async demo and wait for it to complete
            MultiProgressDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nUsing multiple progress reporters:");
            ConsoleHelper.WriteInfo("- Report different aspects of progress (overall, current step, etc.)");
            ConsoleHelper.WriteInfo("- Use different types for different progress information");
            ConsoleHelper.WriteInfo("- Create a richer progress reporting experience");
            
            ConsoleHelper.WaitForKey();
        }
        
        #region Helper Methods
        
        /// <summary>
        /// Simulates a long-running operation that reports progress
        /// </summary>
        private async Task DoWorkWithProgressAsync(IProgress<int> progress, int steps, CancellationToken cancellationToken = default)
        {
            for (int i = 0; i <= steps; i++)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();
                
                // Calculate progress percentage
                int percentComplete = (i * 100) / steps;
                
                // Report progress
                progress?.Report(percentComplete);
                
                // Simulate work
                await Task.Delay(500, cancellationToken);
            }
        }
        
        /// <summary>
        /// Simulates processing files with progress reporting
        /// </summary>
        private async Task<int> ProcessFilesAsync(int fileCount, IProgress<ProgressData> progress)
        {
            for (int i = 1; i <= fileCount; i++)
            {
                // Calculate progress
                int percentComplete = (i * 100) / fileCount;
                
                // Report progress with detailed information
                progress?.Report(new ProgressData
                {
                    CurrentItem = i,
                    TotalItems = fileCount,
                    PercentComplete = percentComplete,
                    ItemName = $"file{i}.txt"
                });
                
                // Simulate file processing
                await Task.Delay(500);
            }
            
            return fileCount;
        }
        
        /// <summary>
        /// Simulates a complex operation with multiple progress reporters
        /// </summary>
        private async Task DoComplexWorkAsync(IProgress<int> overallProgress, IProgress<string> statusProgress)
        {
            string[] phases = {
                "Initializing...",
                "Loading data...",
                "Processing items...",
                "Validating results...",
                "Saving output..."
            };
            
            for (int phase = 0; phase < phases.Length; phase++)
            {
                // Report current phase
                statusProgress?.Report(phases[phase]);
                
                // Calculate overall progress
                int overallPercent = (phase * 100) / phases.Length;
                
                // Process steps within this phase
                int steps = 5;
                for (int step = 0; step <= steps; step++)
                {
                    // Calculate detailed progress
                    int phasePercent = (step * 100) / steps;
                    int detailedPercent = overallPercent + (phasePercent / phases.Length);
                    
                    // Report overall progress
                    overallProgress?.Report(detailedPercent);
                    
                    // Simulate work
                    await Task.Delay(300);
                }
            }
            
            // Final progress update
            overallProgress?.Report(100);
            statusProgress?.Report("Completed");
        }
        
        #endregion
    }
    
    /// <summary>
    /// Represents detailed progress data
    /// </summary>
    public class ProgressData
    {
        public int CurrentItem { get; set; }
        public int TotalItems { get; set; }
        public int PercentComplete { get; set; }
        public string ItemName { get; set; }
    }
}
