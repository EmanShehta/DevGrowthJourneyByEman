using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates the Task-Based Asynchronous Pattern (TAP)
    /// </summary>
    public class TaskBasedAsyncPatternDemo
    {
        /// <summary>
        /// Runs the Task-Based Asynchronous Pattern demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Task-Based Asynchronous Pattern", "انماط المهام غير المتزامنة");
            
            ConsoleHelper.WriteInfo("This demo shows the Task-Based Asynchronous Pattern (TAP).");
            ConsoleHelper.WriteInfo("TAP is the recommended pattern for asynchronous operations in .NET.\n");
            
            // TAP basics
            ExplainTapBasics();
            
            // Implementing TAP methods
            DemonstrateImplementingTap();
            
            // TAP method naming conventions
            ExplainNamingConventions();
            
            // TAP best practices
            ExplainBestPractices();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Task-Based Asynchronous Pattern");
            Console.WriteLine("Key points about the Task-Based Asynchronous Pattern:");
            Console.WriteLine("✓ Methods return Task or Task<T> to represent asynchronous operations");
            Console.WriteLine("✓ Async methods should have 'Async' suffix in their names");
            Console.WriteLine("✓ Methods should accept CancellationToken when appropriate");
            Console.WriteLine("✓ Progress reporting is done via IProgress<T> parameter");
            Console.WriteLine("✓ Exceptions are propagated through the returned Task");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Explains the basics of the Task-Based Asynchronous Pattern
        /// </summary>
        private void ExplainTapBasics()
        {
            ConsoleHelper.WriteSubheader("TAP Basics");
            
            Console.WriteLine("The Task-Based Asynchronous Pattern (TAP) is a pattern for asynchronous operations in .NET.");
            Console.WriteLine("It uses Task and Task<T> to represent asynchronous operations.\n");
            
            Console.WriteLine("Key characteristics of TAP:");
            Console.WriteLine("1. Methods return Task or Task<T>");
            Console.WriteLine("2. Methods have 'Async' suffix in their names");
            Console.WriteLine("3. Methods accept CancellationToken for cancellation support");
            Console.WriteLine("4. Methods accept IProgress<T> for progress reporting");
            Console.WriteLine("5. Exceptions are propagated through the returned Task");
            
            Console.WriteLine("\nExamples of TAP methods in .NET:");
            Console.WriteLine("- File.ReadAllTextAsync()");
            Console.WriteLine("- HttpClient.GetAsync()");
            Console.WriteLine("- Task.Delay()");
            Console.WriteLine("- DbContext.SaveChangesAsync()");
            
            Console.WriteLine("\nTAP is designed to work seamlessly with the async/await keywords.");
            
            // We need to use an async local function
            async Task TapExampleAsync()
            {
                Console.WriteLine("\nExample of using TAP methods with async/await:");
                
                // Simulate calling a TAP method
                Console.WriteLine("Calling a TAP method...");
                string result = await SimulateReadFileAsync("example.txt");
                Console.WriteLine($"Result: {result}");
            }
            
            // Run the async demo and wait for it to complete
            TapExampleAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates implementing TAP methods
        /// </summary>
        private void DemonstrateImplementingTap()
        {
            ConsoleHelper.WriteSubheader("Implementing TAP Methods");
            
            Console.WriteLine("There are several ways to implement TAP methods:");
            
            Console.WriteLine("\n1. Using async/await (recommended):");
            Console.WriteLine("   public async Task<string> ReadFileAsync(string path)");
            Console.WriteLine("   {");
            Console.WriteLine("       // Implementation using await");
            Console.WriteLine("       await Task.Delay(100); // Simulate I/O");
            Console.WriteLine("       return \"File content\";");
            Console.WriteLine("   }");
            
            Console.WriteLine("\n2. Using TaskCompletionSource:");
            Console.WriteLine("   public Task<string> ReadFileAsync(string path)");
            Console.WriteLine("   {");
            Console.WriteLine("       var tcs = new TaskCompletionSource<string>();");
            Console.WriteLine("       // Do work and set result or exception");
            Console.WriteLine("       tcs.SetResult(\"File content\");");
            Console.WriteLine("       return tcs.Task;");
            Console.WriteLine("   }");
            
            Console.WriteLine("\n3. Using Task.FromResult for synchronous operations:");
            Console.WriteLine("   public Task<string> GetCachedDataAsync(string key)");
            Console.WriteLine("   {");
            Console.WriteLine("       // If data is already available synchronously");
            Console.WriteLine("       return Task.FromResult(\"Cached data\");");
            Console.WriteLine("   }");
            
            // We need to use an async local function
            async Task ImplementationDemoAsync()
            {
                Console.WriteLine("\nDemonstrating different TAP implementations:");
                
                // Using async/await
                Console.WriteLine("\nCalling method implemented with async/await:");
                string result1 = await GetDataWithAsyncAwaitAsync("example");
                Console.WriteLine($"Result: {result1}");
                
                // Using TaskCompletionSource
                Console.WriteLine("\nCalling method implemented with TaskCompletionSource:");
                string result2 = await GetDataWithTcsAsync("example");
                Console.WriteLine($"Result: {result2}");
                
                // Using Task.FromResult
                Console.WriteLine("\nCalling method implemented with Task.FromResult:");
                string result3 = await GetCachedDataAsync("example");
                Console.WriteLine($"Result: {result3}");
            }
            
            // Run the async demo and wait for it to complete
            ImplementationDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nPreferred implementation approach:");
            ConsoleHelper.WriteInfo("- Use async/await for most scenarios");
            ConsoleHelper.WriteInfo("- Use TaskCompletionSource for event-based or callback-based APIs");
            ConsoleHelper.WriteInfo("- Use Task.FromResult for synchronous operations that need to conform to TAP");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Explains TAP method naming conventions
        /// </summary>
        private void ExplainNamingConventions()
        {
            ConsoleHelper.WriteSubheader("TAP Naming Conventions");
            
            Console.WriteLine("TAP methods follow specific naming conventions:");
            
            Console.WriteLine("\n1. Async suffix:");
            Console.WriteLine("   - ReadAsync instead of Read");
            Console.WriteLine("   - WriteAsync instead of Write");
            Console.WriteLine("   - ConnectAsync instead of Connect");
            
            Console.WriteLine("\n2. Pairs with synchronous methods:");
            Console.WriteLine("   - ReadAllText and ReadAllTextAsync");
            Console.WriteLine("   - SaveChanges and SaveChangesAsync");
            Console.WriteLine("   - GetResponse and GetResponseAsync");
            
            Console.WriteLine("\n3. Parameter conventions:");
            Console.WriteLine("   - CancellationToken should be the last parameter");
            Console.WriteLine("   - IProgress<T> should be before CancellationToken");
            Console.WriteLine("   - Optional parameters should come before required ones");
            
            Console.WriteLine("\nExample method signatures:");
            Console.WriteLine("Task<string> ReadFileAsync(string path);");
            Console.WriteLine("Task<string> ReadFileAsync(string path, CancellationToken cancellationToken);");
            Console.WriteLine("Task<string> ReadFileAsync(string path, IProgress<int> progress, CancellationToken cancellationToken);");
            
            ConsoleHelper.WriteInfo("\nConsistent naming is important for API usability and discoverability.");
            ConsoleHelper.WriteInfo("Following these conventions makes your code more predictable and easier to use.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Explains TAP best practices
        /// </summary>
        private void ExplainBestPractices()
        {
            ConsoleHelper.WriteSubheader("TAP Best Practices");
            
            Console.WriteLine("Best practices for implementing TAP methods:");
            
            Console.WriteLine("\n1. Provide cancellation support when appropriate:");
            
            // We need to use an async local function
            async Task CancellationDemoAsync()
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                
                // Start a task that can be cancelled
                Task<string> task = ProcessDataAsync("example", cts.Token);
                
                // Cancel after a delay
                await Task.Delay(1000);
                Console.WriteLine("Cancelling operation...");
                cts.Cancel();
                
                try
                {
                    string result = await task;
                    Console.WriteLine($"Result: {result}");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Operation was cancelled");
                }
            }
            
            Console.WriteLine("Demonstrating cancellation support:");
            CancellationDemoAsync().GetAwaiter().GetResult();
            
            Console.WriteLine("\n2. Provide progress reporting when appropriate:");
            
            async Task ProgressDemoAsync()
            {
                // Create a progress reporter
                IProgress<int> progress = new Progress<int>(percent =>
                {
                    ConsoleHelper.ClearCurrentLine();
                    Console.Write($"Progress: {percent}% ");
                    ConsoleHelper.DisplayProgressBar(percent, 100);
                });
                
                // Call a method that reports progress
                string result = await ProcessDataWithProgressAsync("example", progress);
                Console.WriteLine($"\nResult: {result}");
            }
            
            Console.WriteLine("\nDemonstrating progress reporting:");
            ProgressDemoAsync().GetAwaiter().GetResult();
            
            Console.WriteLine("\n3. Other best practices:");
            Console.WriteLine("- Avoid using Task.Run in library code");
            Console.WriteLine("- Propagate exceptions naturally (don't swallow them)");
            Console.WriteLine("- Provide synchronous alternatives when appropriate");
            Console.WriteLine("- Use ConfigureAwait(false) in library code");
            Console.WriteLine("- Avoid async void methods (except for event handlers)");
            Console.WriteLine("- Implement IDisposable for classes with async cleanup");
            
            ConsoleHelper.WriteInfo("\nFollowing these best practices ensures your asynchronous code is:");
            ConsoleHelper.WriteInfo("- Reliable and predictable");
            ConsoleHelper.WriteInfo("- Composable with other asynchronous operations");
            ConsoleHelper.WriteInfo("- Efficient and responsive");
            
            ConsoleHelper.WaitForKey();
        }
        
        #region Helper Methods
        
        /// <summary>
        /// Simulates reading a file asynchronously
        /// </summary>
        private async Task<string> SimulateReadFileAsync(string path)
        {
            await Task.Delay(1000); // Simulate I/O
            return $"Content of {path}";
        }
        
        /// <summary>
        /// Implements a TAP method using async/await
        /// </summary>
        private async Task<string> GetDataWithAsyncAwaitAsync(string key)
        {
            await Task.Delay(1000); // Simulate asynchronous work
            return $"Data for {key} (async/await implementation)";
        }
        
        /// <summary>
        /// Implements a TAP method using TaskCompletionSource
        /// </summary>
        private Task<string> GetDataWithTcsAsync(string key)
        {
            var tcs = new TaskCompletionSource<string>();
            
            // Simulate asynchronous work
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(1000); // Simulate work
                tcs.SetResult($"Data for {key} (TaskCompletionSource implementation)");
            });
            
            return tcs.Task;
        }
        
        /// <summary>
        /// Implements a TAP method using Task.FromResult
        /// </summary>
        private Task<string> GetCachedDataAsync(string key)
        {
            // Simulate data that's already available
            string cachedData = $"Cached data for {key} (Task.FromResult implementation)";
            return Task.FromResult(cachedData);
        }
        
        /// <summary>
        /// Implements a TAP method with cancellation support
        /// </summary>
        private async Task<string> ProcessDataAsync(string data, CancellationToken cancellationToken)
        {
            for (int i = 0; i < 5; i++)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();
                
                Console.WriteLine($"Processing step {i + 1}/5...");
                await Task.Delay(500, cancellationToken);
            }
            
            return $"Processed {data}";
        }
        
        /// <summary>
        /// Implements a TAP method with progress reporting
        /// </summary>
        private async Task<string> ProcessDataWithProgressAsync(string data, IProgress<int> progress)
        {
            for (int i = 0; i <= 10; i++)
            {
                int percent = i * 10;
                progress?.Report(percent);
                await Task.Delay(300);
            }
            
            return $"Processed {data} with progress reporting";
        }
        
        #endregion
    }
}
