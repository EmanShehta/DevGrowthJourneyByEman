using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates concurrency and parallelism in asynchronous programming
    /// </summary>
    public class ConcurrencyParallelismDemo
    {
        /// <summary>
        /// Runs the Concurrency and Parallelism demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Concurrency and Parallelism", "التنفيذ بالتزامن او بالتوازي");
            
            ConsoleHelper.WriteInfo("This demo shows the differences between concurrency and parallelism.");
            ConsoleHelper.WriteInfo("Understanding these concepts is crucial for efficient asynchronous programming.\n");
            
            // Explain the difference between concurrency and parallelism
            ExplainConcurrencyVsParallelism();
            
            // Demonstrate concurrency with async/await
            DemonstrateConcurrency();
            
            // Demonstrate parallelism with Parallel class
            DemonstrateParallelism();
            
            // Demonstrate Parallel LINQ (PLINQ)
            DemonstratePlinq();
            
            // Demonstrate combining concurrency and parallelism
            DemonstrateCombined();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Concurrency and Parallelism");
            Console.WriteLine("Key points about concurrency and parallelism:");
            Console.WriteLine("✓ Concurrency is about dealing with multiple things at once");
            Console.WriteLine("✓ Parallelism is about doing multiple things at once");
            Console.WriteLine("✓ Async/await is primarily for concurrency (especially I/O operations)");
            Console.WriteLine("✓ Parallel class and PLINQ are for parallelism (CPU-bound operations)");
            Console.WriteLine("✓ Choose the right tool based on whether your work is I/O-bound or CPU-bound");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Explains the difference between concurrency and parallelism
        /// </summary>
        private void ExplainConcurrencyVsParallelism()
        {
            ConsoleHelper.WriteSubheader("Concurrency vs. Parallelism");
            
            Console.WriteLine("Concurrency and parallelism are related but distinct concepts:");
            
            Console.WriteLine("\nConcurrency:");
            Console.WriteLine("- Definition: Dealing with multiple things at once");
            Console.WriteLine("- Focus: Structure - organizing independent tasks");
            Console.WriteLine("- Example: A single chef preparing multiple dishes by switching between them");
            Console.WriteLine("- Best for: I/O-bound operations (network, disk, etc.)");
            Console.WriteLine("- Implementation: async/await, Task-based APIs");
            
            Console.WriteLine("\nParallelism:");
            Console.WriteLine("- Definition: Doing multiple things at once");
            Console.WriteLine("- Focus: Execution - utilizing multiple processors");
            Console.WriteLine("- Example: Multiple chefs each preparing a different dish simultaneously");
            Console.WriteLine("- Best for: CPU-bound operations (calculations, data processing)");
            Console.WriteLine("- Implementation: Parallel class, PLINQ, Thread pool");
            
            Console.WriteLine("\nVisual representation:");
            Console.WriteLine("Concurrency (single thread, switching between tasks):");
            Console.WriteLine("Thread 1: Task A ---> Task B ---> Task A ---> Task B ---> Task A ---> Task B");
            
            Console.WriteLine("\nParallelism (multiple threads, simultaneous execution):");
            Console.WriteLine("Thread 1: Task A ------------------------------------------------>");
            Console.WriteLine("Thread 2: Task B ------------------------------------------------>");
            
            ConsoleHelper.WriteInfo("\nKey insight: Concurrency is about structure, parallelism is about execution.");
            ConsoleHelper.WriteInfo("A system can be concurrent without being parallel, parallel without being");
            ConsoleHelper.WriteInfo("concurrent, both, or neither.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates concurrency with async/await
        /// </summary>
        private void DemonstrateConcurrency()
        {
            ConsoleHelper.WriteSubheader("Concurrency with Async/Await");
            
            Console.WriteLine("Demonstrating concurrency with async/await for I/O-bound operations:");
            
            // We need to use an async local function
            async Task ConcurrencyDemoAsync()
            {
                Console.WriteLine("Starting multiple I/O operations concurrently...");
                
                Stopwatch sw = new Stopwatch();
                sw.Start();
                
                // Sequential approach
                Console.WriteLine("\n1. Sequential execution (one after another):");
                sw.Restart();
                
                string result1 = await SimulateIoOperationAsync("Operation 1", 1000);
                string result2 = await SimulateIoOperationAsync("Operation 2", 1000);
                string result3 = await SimulateIoOperationAsync("Operation 3", 1000);
                
                sw.Stop();
                Console.WriteLine($"Sequential execution completed in {sw.ElapsedMilliseconds}ms");
                Console.WriteLine($"Results: {result1}, {result2}, {result3}");
                
                // Concurrent approach
                Console.WriteLine("\n2. Concurrent execution (start all, then await all):");
                sw.Restart();
                
                Task<string> task1 = SimulateIoOperationAsync("Operation 1", 1000);
                Task<string> task2 = SimulateIoOperationAsync("Operation 2", 1000);
                Task<string> task3 = SimulateIoOperationAsync("Operation 3", 1000);
                
                // Wait for all tasks to complete
                await Task.WhenAll(task1, task2, task3);
                
                sw.Stop();
                Console.WriteLine($"Concurrent execution completed in {sw.ElapsedMilliseconds}ms");
                Console.WriteLine($"Results: {task1.Result}, {task2.Result}, {task3.Result}");
            }
            
            // Run the async demo and wait for it to complete
            ConcurrencyDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey points about concurrency with async/await:");
            ConsoleHelper.WriteInfo("- Ideal for I/O-bound operations (network, disk, database)");
            ConsoleHelper.WriteInfo("- Improves throughput by not blocking threads during I/O waits");
            ConsoleHelper.WriteInfo("- May not utilize multiple CPU cores simultaneously");
            ConsoleHelper.WriteInfo("- Provides better resource utilization and responsiveness");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates parallelism with Parallel class
        /// </summary>
        private void DemonstrateParallelism()
        {
            ConsoleHelper.WriteSubheader("Parallelism with Parallel Class");
            
            Console.WriteLine("Demonstrating parallelism with Parallel class for CPU-bound operations:");
            
            // Generate a large dataset
            int[] numbers = Enumerable.Range(1, 10000000).ToArray();
            
            // Sequential approach
            Console.WriteLine("\n1. Sequential processing:");
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            long sequentialSum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                // Simulate CPU-intensive work
                sequentialSum += PerformCpuIntensiveOperation(numbers[i]);
            }
            
            sw.Stop();
            Console.WriteLine($"Sequential processing completed in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Result: {sequentialSum}");
            
            // Parallel approach
            Console.WriteLine("\n2. Parallel processing:");
            
            sw.Restart();
            
            long parallelSum = 0;
            object lockObj = new object();
            
            Parallel.For(0, numbers.Length, i =>
            {
                // Simulate CPU-intensive work
                long localSum = PerformCpuIntensiveOperation(numbers[i]);
                
                // Thread-safe addition to the total sum
                lock (lockObj)
                {
                    parallelSum += localSum;
                }
            });
            
            sw.Stop();
            Console.WriteLine($"Parallel processing completed in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Result: {parallelSum}");
            
            // Parallel.ForEach example
            Console.WriteLine("\n3. Parallel.ForEach example:");
            
            sw.Restart();
            
            List<string> items = new List<string> { "Item 1", "Item 2", "Item 3", "Item 4", "Item 5" };
            
            Parallel.ForEach(items, item =>
            {
                Console.WriteLine($"Processing {item} on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(100); // Simulate work
            });
            
            sw.Stop();
            Console.WriteLine($"Parallel.ForEach completed in {sw.ElapsedMilliseconds}ms");
            
            ConsoleHelper.WriteInfo("\nKey points about parallelism with Parallel class:");
            ConsoleHelper.WriteInfo("- Ideal for CPU-bound operations (calculations, data processing)");
            ConsoleHelper.WriteInfo("- Utilizes multiple CPU cores for better performance");
            ConsoleHelper.WriteInfo("- Requires thread synchronization for shared state");
            ConsoleHelper.WriteInfo("- Performance improvement depends on available cores and workload");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates Parallel LINQ (PLINQ)
        /// </summary>
        private void DemonstratePlinq()
        {
            ConsoleHelper.WriteSubheader("Parallel LINQ (PLINQ)");
            
            Console.WriteLine("Demonstrating Parallel LINQ for data processing:");
            
            // Generate a dataset
            int[] numbers = Enumerable.Range(1, 1000000).ToArray();
            
            // Sequential LINQ
            Console.WriteLine("\n1. Sequential LINQ:");
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            var sequentialResult = numbers
                .Where(n => n % 2 == 0)
                .Select(n => n * n)
                .Take(5)
                .ToList();
            
            sw.Stop();
            Console.WriteLine($"Sequential LINQ completed in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine("First 5 results: " + string.Join(", ", sequentialResult));
            
            // Parallel LINQ
            Console.WriteLine("\n2. Parallel LINQ (PLINQ):");
            
            sw.Restart();
            
            var parallelResult = numbers
                .AsParallel()
                .Where(n => n % 2 == 0)
                .Select(n => n * n)
                .Take(5)
                .ToList();
            
            sw.Stop();
            Console.WriteLine($"Parallel LINQ completed in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine("First 5 results: " + string.Join(", ", parallelResult));
            
            // PLINQ with execution options
            Console.WriteLine("\n3. PLINQ with execution options:");
            
            sw.Restart();
            
            var customResult = numbers
                .AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount) // Limit to number of cores
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Where(n => n % 2 == 0)
                .Select(n => n * n)
                .Take(5)
                .ToList();
            
            sw.Stop();
            Console.WriteLine($"Custom PLINQ completed in {sw.ElapsedMilliseconds}ms");
            Console.WriteLine("First 5 results: " + string.Join(", ", customResult));
            
            ConsoleHelper.WriteInfo("\nKey points about PLINQ:");
            ConsoleHelper.WriteInfo("- Parallel version of LINQ for data processing");
            ConsoleHelper.WriteInfo("- Simple to use - just add .AsParallel() to LINQ queries");
            ConsoleHelper.WriteInfo("- Results may be returned in different order than sequential LINQ");
            ConsoleHelper.WriteInfo("- Best for computationally intensive operations on large datasets");
            ConsoleHelper.WriteInfo("- Provides options to control parallelism degree and behavior");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates combining concurrency and parallelism
        /// </summary>
        private void DemonstrateCombined()
        {
            ConsoleHelper.WriteSubheader("Combining Concurrency and Parallelism");
            
            Console.WriteLine("Demonstrating how to combine concurrency and parallelism:");
            
            // We need to use an async local function
            async Task CombinedDemoAsync()
            {
                Console.WriteLine("Starting a complex operation with both concurrency and parallelism...");
                
                Stopwatch sw = new Stopwatch();
                sw.Start();
                
                // Step 1: Concurrent I/O operations (concurrency)
                Console.WriteLine("\n1. Concurrent I/O operations:");
                
                Task<string[]> dataTask1 = SimulateDataFetchAsync("Source 1", 1000);
                Task<string[]> dataTask2 = SimulateDataFetchAsync("Source 2", 1000);
                Task<string[]> dataTask3 = SimulateDataFetchAsync("Source 3", 1000);
                
                // Wait for all data to be fetched
                await Task.WhenAll(dataTask1, dataTask2, dataTask3);
                
                // Combine all data
                List<string> allData = new List<string>();
                allData.AddRange(dataTask1.Result);
                allData.AddRange(dataTask2.Result);
                allData.AddRange(dataTask3.Result);
                
                Console.WriteLine($"   Fetched {allData.Count} items from all sources");
                
                // Step 2: Parallel processing of the data (parallelism)
                Console.WriteLine("\n2. Parallel processing of the fetched data:");
                
                var processedData = allData.AsParallel()
                    .Select(item => ProcessDataItem(item))
                    .ToList();
                
                Console.WriteLine($"   Processed {processedData.Count} items in parallel");
                
                // Step 3: Concurrent saving of results (concurrency)
                Console.WriteLine("\n3. Concurrent saving of results:");
                
                Task saveTask1 = SimulateSaveDataAsync("Database", processedData.Take(processedData.Count / 3).ToList(), 1000);
                Task saveTask2 = SimulateSaveDataAsync("File System", processedData.Skip(processedData.Count / 3).Take(processedData.Count / 3).ToList(), 1000);
                Task saveTask3 = SimulateSaveDataAsync("Cloud Storage", processedData.Skip(2 * processedData.Count / 3).ToList(), 1000);
                
                // Wait for all save operations to complete
                await Task.WhenAll(saveTask1, saveTask2, saveTask3);
                
                sw.Stop();
                Console.WriteLine($"\nCombined operation completed in {sw.ElapsedMilliseconds}ms");
            }
            
            // Run the async demo and wait for it to complete
            CombinedDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nBest practices for combining concurrency and parallelism:");
            ConsoleHelper.WriteInfo("1. Use concurrency (async/await) for I/O-bound operations");
            ConsoleHelper.WriteInfo("2. Use parallelism (Parallel, PLINQ) for CPU-bound operations");
            ConsoleHelper.WriteInfo("3. Fetch data concurrently, process it in parallel, save results concurrently");
            ConsoleHelper.WriteInfo("4. Be mindful of thread synchronization when sharing state");
            ConsoleHelper.WriteInfo("5. Monitor performance to ensure you're getting the expected benefits");
            
            ConsoleHelper.WaitForKey();
        }
        
        #region Helper Methods
        
        /// <summary>
        /// Simulates an I/O operation that returns a result
        /// </summary>
        private async Task<string> SimulateIoOperationAsync(string name, int delayMs)
        {
            Console.WriteLine($"  {name} started");
            await Task.Delay(delayMs);
            Console.WriteLine($"  {name} completed");
            return $"{name} result";
        }
        
        /// <summary>
        /// Performs a CPU-intensive operation
        /// </summary>
        private long PerformCpuIntensiveOperation(int input)
        {
            // Simulate CPU-intensive work (very simplified)
            return input * input;
        }
        
        /// <summary>
        /// Simulates fetching data from a source
        /// </summary>
        private async Task<string[]> SimulateDataFetchAsync(string source, int delayMs)
        {
            Console.WriteLine($"  Fetching data from {source}...");
            await Task.Delay(delayMs);
            
            // Generate some sample data
            string[] data = Enumerable.Range(1, 100)
                .Select(i => $"{source} item {i}")
                .ToArray();
            
            Console.WriteLine($"  Fetched {data.Length} items from {source}");
            return data;
        }
        
        /// <summary>
        /// Processes a single data item
        /// </summary>
        private string ProcessDataItem(string item)
        {
            // Simulate some processing
            Thread.Sleep(10);
            return $"Processed: {item}";
        }
        
        /// <summary>
        /// Simulates saving data to a destination
        /// </summary>
        private async Task SimulateSaveDataAsync(string destination, List<string> data, int delayMs)
        {
            Console.WriteLine($"  Saving {data.Count} items to {destination}...");
            await Task.Delay(delayMs);
            Console.WriteLine($"  Saved {data.Count} items to {destination}");
        }
        
        #endregion
    }
}
