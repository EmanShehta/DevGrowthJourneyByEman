using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Models;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates the differences between synchronous and asynchronous programming
    /// </summary>
    public class SyncVsAsyncDemo
    {
        private readonly DataService _dataService = new DataService();
        private readonly FileService _fileService = new FileService();
        private readonly NetworkService _networkService = new NetworkService();
        
        /// <summary>
        /// Runs the Sync vs Async demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Sync Vs. Async", "التزامن مقابل اللاتزامن");
            
            ConsoleHelper.WriteInfo("This demo compares synchronous and asynchronous programming approaches.");
            ConsoleHelper.WriteInfo("You'll see the benefits of async programming for I/O-bound operations.\n");
            
            // Basic comparison
            CompareBasicApproaches();
            
            // UI responsiveness comparison
            CompareUiResponsiveness();
            
            // Throughput comparison
            CompareThroughput();
            
            // Real-world scenarios
            DemonstrateRealWorldScenarios();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Sync vs Async");
            Console.WriteLine("Key differences between synchronous and asynchronous programming:");
            Console.WriteLine("✓ Sync code is simpler but blocks threads during I/O operations");
            Console.WriteLine("✓ Async code is more complex but allows better resource utilization");
            Console.WriteLine("✓ Async improves responsiveness in UI applications");
            Console.WriteLine("✓ Async improves throughput in server applications");
            Console.WriteLine("✓ Async is essential for modern, scalable applications");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Compares basic synchronous and asynchronous approaches
        /// </summary>
        private void CompareBasicApproaches()
        {
            ConsoleHelper.WriteSubheader("Basic Comparison");
            
            Console.WriteLine("1. Synchronous approach (blocking):");
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            // Synchronous file read
            string fileContent = _fileService.ReadFileSync("example.txt");
            Console.WriteLine($"   File read completed: {fileContent.Substring(0, 20)}...");
            
            // Synchronous data fetch
            var data = _dataService.GetDataSync(3);
            Console.WriteLine($"   Data fetch completed: {data.Count} items retrieved");
            
            // Synchronous network operation
            byte[] downloadedData = _networkService.DownloadSync("http://example.com");
            Console.WriteLine($"   Download completed: {downloadedData.Length} bytes");
            
            sw.Stop();
            Console.WriteLine($"   Total time (sync): {sw.ElapsedMilliseconds}ms");
            
            Console.WriteLine("\n2. Asynchronous approach (non-blocking):");
            
            // We need to use an async local function
            async Task AsyncDemoAsync()
            {
                sw.Restart();
                
                // Asynchronous file read
                string asyncFileContent = await _fileService.ReadFileAsync("example.txt");
                Console.WriteLine($"   File read completed: {asyncFileContent.Substring(0, 20)}...");
                
                // Asynchronous data fetch
                var asyncData = await _dataService.GetDataAsync(3);
                Console.WriteLine($"   Data fetch completed: {asyncData.Count} items retrieved");
                
                // Asynchronous network operation
                byte[] asyncDownloadedData = await _networkService.DownloadAsync("http://example.com");
                Console.WriteLine($"   Download completed: {asyncDownloadedData.Length} bytes");
                
                sw.Stop();
                Console.WriteLine($"   Total time (async): {sw.ElapsedMilliseconds}ms");
            }
            
            // Run the async demo and wait for it to complete
            AsyncDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nNote: The timing difference may not be significant in this demo");
            ConsoleHelper.WriteInfo("because operations run sequentially in both cases.");
            ConsoleHelper.WriteInfo("The real benefit of async comes when running multiple operations concurrently.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Compares UI responsiveness with synchronous and asynchronous approaches
        /// </summary>
        private void CompareUiResponsiveness()
        {
            ConsoleHelper.WriteSubheader("UI Responsiveness Comparison");
            
            Console.WriteLine("This demo simulates how sync and async affect UI responsiveness:");
            
            Console.WriteLine("\n1. Synchronous approach (UI freezes):");
            
            // Simulate a UI thread with work to do
            Thread uiThread = new Thread(() =>
            {
                Console.WriteLine("   UI Thread: Application started");
                
                // Simulate button click that triggers a long operation
                Console.WriteLine("   UI Thread: User clicked 'Download' button");
                
                // Perform a long-running synchronous operation
                Console.WriteLine("   UI Thread: Starting download (UI is now frozen)");
                Thread.Sleep(3000); // Simulate a long download
                Console.WriteLine("   UI Thread: Download completed");
                
                // UI is responsive again
                Console.WriteLine("   UI Thread: UI is responsive again");
            });
            
            // Start a separate thread to show that the UI is frozen
            Thread interactionThread = new Thread(() =>
            {
                // Give the UI thread time to start
                Thread.Sleep(500);
                
                // Try to interact with the "frozen" UI
                for (int i = 1; i <= 5; i++)
                {
                    Console.WriteLine("   User: Trying to interact with UI... (no response)");
                    Thread.Sleep(500);
                }
            });
            
            uiThread.Start();
            interactionThread.Start();
            
            uiThread.Join();
            interactionThread.Join();
            
            Console.WriteLine("\n2. Asynchronous approach (UI remains responsive):");
            
            // We need to use an async local function
            async Task AsyncUiDemoAsync()
            {
                Console.WriteLine("   UI Thread: Application started");
                
                // Simulate button click that triggers a long operation
                Console.WriteLine("   UI Thread: User clicked 'Download' button");
                
                // Start an asynchronous operation
                Console.WriteLine("   UI Thread: Starting download asynchronously (UI remains responsive)");

                // Create a task to simulate the download
                Task<string> downloadTask = Task.Run(async () =>
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        await Task.Delay(500);
                        Console.WriteLine($"   Background: Download progress {i * 16.67:F1}%");
                    }

                    return "Download result";
                });
                // Simulate UI interactions while the download is in progress
                for (int i = 1; i <= 5; i++)
                {
                    Console.WriteLine("   User: Interacting with UI... (responsive)");
                    await Task.Delay(500);
                }

                // Wait for the download to complete
                string result = await downloadTask;
                Console.WriteLine($"   UI Thread: Download completed: {result}");
            }
            
            // Run the async demo and wait for it to complete
            AsyncUiDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey benefit for UI applications:");
            ConsoleHelper.WriteInfo("- Synchronous operations freeze the UI until they complete");
            ConsoleHelper.WriteInfo("- Asynchronous operations allow the UI to remain responsive");
            ConsoleHelper.WriteInfo("- This creates a much better user experience");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Compares throughput with synchronous and asynchronous approaches
        /// </summary>
        private void CompareThroughput()
        {
            ConsoleHelper.WriteSubheader("Throughput Comparison");
            
            Console.WriteLine("This demo compares throughput for multiple operations:");
            
            int operationCount = 5;
            
            Console.WriteLine("\n1. Synchronous sequential operations:");
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            for (int i = 1; i <= operationCount; i++)
            {
                Console.WriteLine($"   Starting operation {i}/{operationCount}");
                
                // Simulate an I/O operation
                Thread.Sleep(1000);
                
                Console.WriteLine($"   Completed operation {i}/{operationCount}");
            }
            
            sw.Stop();
            Console.WriteLine($"   Total time for sequential operations: {sw.ElapsedMilliseconds}ms");
            
            Console.WriteLine("\n2. Asynchronous parallel operations:");
            
            // We need to use an async local function
            async Task AsyncParallelDemoAsync()
            {
                sw.Restart();
                
                // Create tasks for all operations
                Task[] tasks = new Task[operationCount];
                
                for (int i = 0; i < operationCount; i++)
                {
                    int operationId = i + 1;
                    
                    tasks[i] = Task.Run(async () =>
                    {
                        Console.WriteLine($"   Starting operation {operationId}/{operationCount}");
                        
                        // Simulate an I/O operation
                        await Task.Delay(1000);
                        
                        Console.WriteLine($"   Completed operation {operationId}/{operationCount}");
                    });
                }
                
                // Wait for all operations to complete
                await Task.WhenAll(tasks);
                
                sw.Stop();
                Console.WriteLine($"   Total time for parallel operations: {sw.ElapsedMilliseconds}ms");
            }
            
            // Run the async demo and wait for it to complete
            AsyncParallelDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey benefit for server applications:");
            ConsoleHelper.WriteInfo("- Synchronous operations run sequentially, limiting throughput");
            ConsoleHelper.WriteInfo("- Asynchronous operations can run concurrently, improving throughput");
            ConsoleHelper.WriteInfo("- This allows servers to handle more requests with fewer threads");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates real-world scenarios where async is beneficial
        /// </summary>
        private void DemonstrateRealWorldScenarios()
        {
            ConsoleHelper.WriteSubheader("Real-World Scenarios");
            
            Console.WriteLine("Common scenarios where async programming is beneficial:");
            
            Console.WriteLine("\n1. Web API calls:");
            
            // We need to use an async local function
            async Task WebApiDemoAsync()
            {
                Console.WriteLine("   Making multiple API calls...");
                
                // Create tasks for multiple API calls
                Task<string> userApiTask = SimulateApiCallAsync("users/123", 1500);
                Task<string> orderApiTask = SimulateApiCallAsync("orders?userId=123", 2000);
                Task<string> productApiTask = SimulateApiCallAsync("products/featured", 1000);
                
                // Wait for all API calls to complete
                await Task.WhenAll(userApiTask, orderApiTask, productApiTask);
                
                // Process the results
                string userData = await userApiTask;
                string orderData = await orderApiTask;
                string productData = await productApiTask;
                
                Console.WriteLine("   All API calls completed");
                Console.WriteLine($"   User data: {userData}");
                Console.WriteLine($"   Order data: {orderData}");
                Console.WriteLine($"   Product data: {productData}");
            }
            
            // Run the async demo and wait for it to complete
            WebApiDemoAsync().GetAwaiter().GetResult();
            
            Console.WriteLine("\n2. Database operations:");
            
            async Task DatabaseDemoAsync()
            {
                Console.WriteLine("   Performing database operations...");
                
                // Simulate database operations
                string result = await SimulateDatabaseOperationAsync("SELECT * FROM Users", 1500);
                
                Console.WriteLine($"   Database operation completed: {result}");
            }
            
            // Run the async demo and wait for it to complete
            DatabaseDemoAsync().GetAwaiter().GetResult();
            
            Console.WriteLine("\n3. File operations:");
            
            async Task FileDemoAsync()
            {
                Console.WriteLine("   Performing file operations...");
                
                // Simulate reading multiple files concurrently
                Task<string> file1Task = SimulateFileReadAsync("file1.txt", 1000);
                Task<string> file2Task = SimulateFileReadAsync("file2.txt", 1200);
                
                string file1Content = await file1Task;
                string file2Content = await file2Task;
                
                Console.WriteLine("   File operations completed");
                Console.WriteLine($"   File 1: {file1Content}");
                Console.WriteLine($"   File 2: {file2Content}");
            }
            
            // Run the async demo and wait for it to complete
            FileDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nIn all these scenarios, async programming allows:");
            ConsoleHelper.WriteInfo("- Multiple I/O operations to run concurrently");
            ConsoleHelper.WriteInfo("- Better resource utilization");
            ConsoleHelper.WriteInfo("- Improved application responsiveness");
            ConsoleHelper.WriteInfo("- Higher throughput and scalability");
            
            ConsoleHelper.WaitForKey();
        }
        
        #region Helper Methods
        
        /// <summary>
        /// Simulates an API call with a delay
        /// </summary>
        private async Task<string> SimulateApiCallAsync(string endpoint, int delayMs)
        {
            Console.WriteLine($"   API call to {endpoint} started");
            await Task.Delay(delayMs);
            Console.WriteLine($"   API call to {endpoint} completed");
            return $"Data from {endpoint}";
        }
        
        /// <summary>
        /// Simulates a database operation with a delay
        /// </summary>
        private async Task<string> SimulateDatabaseOperationAsync(string query, int delayMs)
        {
            Console.WriteLine($"   Executing query: {query}");
            await Task.Delay(delayMs);
            return "Database result";
        }
        
        /// <summary>
        /// Simulates reading a file with a delay
        /// </summary>
        private async Task<string> SimulateFileReadAsync(string fileName, int delayMs)
        {
            Console.WriteLine($"   Reading file: {fileName}");
            await Task.Delay(delayMs);
            return $"Content of {fileName}";
        }
        
        #endregion
    }
}
