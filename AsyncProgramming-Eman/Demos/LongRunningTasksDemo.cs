using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates long-running tasks in asynchronous programming
    /// </summary>
    public class LongRunningTasksDemo
    {
        /// <summary>
        /// Runs the Long Running Tasks demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Long Running Tasks", "المهام التي تتطلب وقت طويل للتنفيذ");
            
            ConsoleHelper.WriteInfo("This demo shows how to handle long-running operations with tasks.");
            ConsoleHelper.WriteInfo("Long-running tasks require special consideration to avoid blocking the thread pool.\n");
            
            // Standard task vs long-running task
            CompareStandardAndLongRunningTasks();
            
            // Using TaskCreationOptions.LongRunning
            DemonstrateLongRunningOption();
            
            // CPU-bound vs I/O-bound operations
            CompareCpuAndIoBoundOperations();
            
            // Best practices for long-running tasks
            ShowBestPractices();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Long Running Tasks");
            Console.WriteLine("Key points about long-running tasks:");
            Console.WriteLine("✓ Use TaskCreationOptions.LongRunning for CPU-intensive operations");
            Console.WriteLine("✓ For I/O-bound operations, use async/await instead");
            Console.WriteLine("✓ Consider breaking large operations into smaller chunks");
            Console.WriteLine("✓ Provide cancellation support for long-running operations");
            Console.WriteLine("✓ Report progress to keep the user informed");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Compares standard tasks with long-running tasks
        /// </summary>
        private void CompareStandardAndLongRunningTasks()
        {
            ConsoleHelper.WriteSubheader("Standard Task vs Long-Running Task");
            
            Console.WriteLine("Creating a standard task (uses thread pool)...");
            Task standardTask = Task.Run(() =>
            {
                Console.WriteLine($"Standard task running on thread ID: {Thread.CurrentThread.ManagedThreadId}");
                SimulateWork("Standard task", 3);
            });
            
            Console.WriteLine("\nCreating a long-running task...");
            Task longRunningTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Long-running task running on thread ID: {Thread.CurrentThread.ManagedThreadId}");
                SimulateWork("Long-running task", 3);
            }, TaskCreationOptions.LongRunning);
            
            Console.WriteLine("\nWaiting for both tasks to complete...");
            Task.WaitAll(standardTask, longRunningTask);
            
            ConsoleHelper.WriteInfo("\nThe key difference:");
            ConsoleHelper.WriteInfo("- Standard tasks use the thread pool, which is optimized for short operations");
            ConsoleHelper.WriteInfo("- Long-running tasks may get their own dedicated thread, avoiding thread pool starvation");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates the TaskCreationOptions.LongRunning option
        /// </summary>
        private void DemonstrateLongRunningOption()
        {
            ConsoleHelper.WriteSubheader("Using TaskCreationOptions.LongRunning");
            
            Console.WriteLine("When to use TaskCreationOptions.LongRunning:");
            Console.WriteLine("1. For CPU-intensive operations that may run for several seconds or longer");
            Console.WriteLine("2. When you want to avoid blocking thread pool threads");
            Console.WriteLine("3. For operations that might otherwise cause thread pool starvation");
            
            Console.WriteLine("\nExample of a CPU-intensive operation:");
            
            Task<long> computeTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Computing task started on thread ID: {Thread.CurrentThread.ManagedThreadId}");
                
                // Simulate CPU-intensive calculation
                long result = 0;
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Computing... Step {i + 1}/10");
                    Thread.Sleep(300); // Simulating computation time
                    result += i * 1000000;
                }
                
                Console.WriteLine("Computation completed");
                return result;
            }, TaskCreationOptions.LongRunning);
            
            Console.WriteLine("\nWaiting for computation to complete...");
            long computeResult = computeTask.Result;
            Console.WriteLine($"Computation result: {computeResult}");
            
            ConsoleHelper.WriteInfo("\nNote: TaskCreationOptions.LongRunning is just a hint to the scheduler.");
            ConsoleHelper.WriteInfo("The actual behavior may vary depending on the runtime and system load.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Compares CPU-bound and I/O-bound operations
        /// </summary>
        private void CompareCpuAndIoBoundOperations()
        {
            ConsoleHelper.WriteSubheader("CPU-Bound vs I/O-Bound Operations");
            
            Console.WriteLine("Different types of long-running operations require different approaches:");
            
            // CPU-bound example
            Console.WriteLine("\n1. CPU-bound operation (uses processor intensively):");
            Task cpuBoundTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Starting CPU-bound operation...");
                
                // Simulate CPU-intensive work
                long sum = 0;
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"CPU work in progress... {(i + 1) * 20}%");
                    
                    // Simulate computation (in real code, this would be actual CPU work)
                    for (int j = 0; j < 10000000; j++)
                    {
                        sum += j;
                    }
                    
                    Thread.Sleep(200); // Just to slow down the demo
                }
                
                Console.WriteLine("CPU-bound operation completed");
            }, TaskCreationOptions.LongRunning);
            
            cpuBoundTask.Wait();
            
            // I/O-bound example
            Console.WriteLine("\n2. I/O-bound operation (waits for external resources):");
            
            async Task IoOperationAsync()
            {
                Console.WriteLine("Starting I/O-bound operation...");
                
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"I/O operation in progress... {(i + 1) * 20}%");
                    
                    // Simulate I/O operation (network, disk, etc.)
                    await Task.Delay(500);
                }
                
                Console.WriteLine("I/O-bound operation completed");
            }
            
            Task ioTask = IoOperationAsync();
            ioTask.Wait();
            
            ConsoleHelper.WriteInfo("\nBest practices for different operation types:");
            ConsoleHelper.WriteInfo("- CPU-bound: Use TaskCreationOptions.LongRunning or consider Parallel library");
            ConsoleHelper.WriteInfo("- I/O-bound: Use async/await pattern without LongRunning option");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Shows best practices for long-running tasks
        /// </summary>
        private void ShowBestPractices()
        {
            ConsoleHelper.WriteSubheader("Best Practices for Long-Running Tasks");
            
            Console.WriteLine("1. Provide cancellation support");
            
            CancellationTokenSource cts = new CancellationTokenSource();
            Task cancellableTask = Task.Factory.StartNew(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        // Check for cancellation
                        cts.Token.ThrowIfCancellationRequested();
                        
                        Console.WriteLine($"Working... Step {i + 1}/10");
                        Thread.Sleep(300);
                    }
                    
                    Console.WriteLine("Task completed successfully");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task was cancelled");
                }
            }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            
            // Simulate cancellation after a delay
            Thread.Sleep(1200);
            Console.WriteLine("\nCancelling the task...");
            cts.Cancel();
            
            try
            {
                cancellableTask.Wait();
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException is OperationCanceledException)
                {
                    Console.WriteLine("Task confirmed cancelled");
                }
            }
            
            Console.WriteLine("\n2. Report progress");
            
            IProgress<int> progress = new Progress<int>(percent =>
            {
                ConsoleHelper.ClearCurrentLine();
                Console.Write($"Progress: {percent}% ");
                ConsoleHelper.DisplayProgressBar(percent, 100);
            });
            
            Task progressTask = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i <= 10; i++)
                {
                    int percent = i * 10;
                    progress.Report(percent);
                    Thread.Sleep(200);
                }
                Console.WriteLine("\nProgress reporting task completed");
            }, TaskCreationOptions.LongRunning);
            
            progressTask.Wait();
            
            Console.WriteLine("\n3. Break large operations into smaller chunks");
            Console.WriteLine("4. Consider using a dedicated thread for very long operations");
            Console.WriteLine("5. Avoid blocking the UI thread in graphical applications");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Simulates work with progress updates
        /// </summary>
        private void SimulateWork(string taskName, int seconds)
        {
            int steps = seconds * 2;
            int stepDuration = seconds * 1000 / steps;
            
            for (int i = 1; i <= steps; i++)
            {
                Console.Write($"{taskName} progress: ");
                ConsoleHelper.DisplayProgressBar(i, steps);
                Thread.Sleep(stepDuration);
            }
            
            Console.WriteLine();
        }
    }
}
