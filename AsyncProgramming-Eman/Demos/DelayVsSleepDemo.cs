using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates the differences between Task.Delay and Thread.Sleep
    /// </summary>
    public class DelayVsSleepDemo
    {
        /// <summary>
        /// Runs the Task.Delay vs Thread.Sleep demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Task.Delay() Vs Thread.Sleep()", "تنويم المسار مقابل تاخير المهمة");
            
            ConsoleHelper.WriteInfo("This demo shows the key differences between Task.Delay() and Thread.Sleep().");
            ConsoleHelper.WriteInfo("Understanding these differences is crucial for writing efficient async code.\n");
            
            // Basic comparison
            CompareBasicBehavior();
            
            // Thread blocking comparison
            CompareThreadBlocking();
            
            // Performance comparison
            ComparePerformance();
            
            // Practical examples
            ShowPracticalExamples();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Task.Delay() vs Thread.Sleep()");
            Console.WriteLine("Key differences:");
            Console.WriteLine("✓ Thread.Sleep blocks the current thread");
            Console.WriteLine("✓ Task.Delay is non-blocking when used with await");
            Console.WriteLine("✓ Thread.Sleep is synchronous, Task.Delay is asynchronous");
            Console.WriteLine("✓ Task.Delay integrates with the async/await pattern");
            Console.WriteLine("✓ Task.Delay supports cancellation");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Compares the basic behavior of Task.Delay and Thread.Sleep
        /// </summary>
        private void CompareBasicBehavior()
        {
            ConsoleHelper.WriteSubheader("Basic Behavior Comparison");
            
            Console.WriteLine("1. Thread.Sleep - Synchronous, blocks the current thread:");
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            Console.WriteLine("   Before Thread.Sleep");
            Thread.Sleep(2000); // Blocks for 2 seconds
            Console.WriteLine($"   After Thread.Sleep - Elapsed: {sw.ElapsedMilliseconds}ms");
            
            sw.Restart();
            
            Console.WriteLine("\n2. Task.Delay - Asynchronous, doesn't block when awaited:");
            
            // We need to use an async local function to demonstrate await
            async Task DelayDemoAsync()
            {
                Console.WriteLine("   Before Task.Delay");
                await Task.Delay(2000); // Non-blocking when awaited
                Console.WriteLine($"   After Task.Delay - Elapsed: {sw.ElapsedMilliseconds}ms");
            }
            
            // Run the async demo and wait for it to complete
            DelayDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey difference: Thread.Sleep completely blocks the current thread,");
            ConsoleHelper.WriteInfo("while Task.Delay allows the thread to do other work when used with await.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Compares how Thread.Sleep and Task.Delay affect thread blocking
        /// </summary>
        private void CompareThreadBlocking()
        {
            ConsoleHelper.WriteSubheader("Thread Blocking Comparison");
            
            Console.WriteLine("This demo shows how Thread.Sleep blocks the thread while Task.Delay doesn't:");
            
            Console.WriteLine("\n1. Thread.Sleep in a UI or server scenario (simulated):");
            
            // Simulate a UI thread with work to do
            Thread uiThread = new Thread(() =>
            {
                Console.WriteLine("   UI Thread: Starting work");
                
                // Simulate UI update
                Console.WriteLine("   UI Thread: Updating UI");
                
                // Block with Sleep
                Console.WriteLine("   UI Thread: Sleeping for 3 seconds (UI is frozen)");
                Thread.Sleep(3000);
                
                // After sleep
                Console.WriteLine("   UI Thread: UI responsive again after sleep");
            });
            
            uiThread.Start();
            uiThread.Join(); // Wait for the thread to complete
            
            Console.WriteLine("\n2. Task.Delay in a UI or server scenario (simulated):");
            
            // We need to use an async local function
            async Task AsyncUiWorkAsync()
            {
                Console.WriteLine("   UI Thread: Starting work");
                
                // Simulate UI update
                Console.WriteLine("   UI Thread: Updating UI");
                
                // Non-blocking delay
                Console.WriteLine("   UI Thread: Delaying for 3 seconds (UI remains responsive)");
                
                // Start a task that simulates UI responsiveness during the delay
                Task uiResponsiveness = Task.Run(() =>
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        Thread.Sleep(500); // Simulate time passing
                        Console.WriteLine($"   UI Thread: Still responsive, handling other events... ({i}/6)");
                    }
                });
                
                await Task.Delay(3000);
                
                // After delay
                Console.WriteLine("   UI Thread: Delay completed, continuing work");
                
                await uiResponsiveness; // Ensure the UI responsiveness simulation completes
            }
            
            // Run the async demo and wait for it to complete
            AsyncUiWorkAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nIn real applications:");
            ConsoleHelper.WriteInfo("- Thread.Sleep would freeze the UI or block server threads");
            ConsoleHelper.WriteInfo("- Task.Delay with await allows the thread to process other work");
            ConsoleHelper.WriteInfo("- This is especially important in UI applications and high-throughput servers");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Compares the performance implications of Thread.Sleep and Task.Delay
        /// </summary>
        private void ComparePerformance()
        {
            ConsoleHelper.WriteSubheader("Performance Comparison");
            
            Console.WriteLine("Let's compare how Thread.Sleep and Task.Delay affect throughput:");
            
            int operationCount = 10;
            
            // Thread.Sleep approach
            Console.WriteLine("\n1. Sequential operations with Thread.Sleep:");
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            for (int i = 1; i <= operationCount; i++)
            {
                Console.WriteLine($"   Operation {i}/{operationCount} starting");
                Thread.Sleep(300); // Each operation takes 300ms
                Console.WriteLine($"   Operation {i}/{operationCount} completed");
            }
            
            sw.Stop();
            Console.WriteLine($"   Total time with Thread.Sleep: {sw.ElapsedMilliseconds}ms");
            
            // Task.Delay approach
            Console.WriteLine("\n2. Parallel operations with Task.Delay:");
            
            sw.Restart();
            
            // Create tasks for all operations
            Task[] tasks = new Task[operationCount];
            for (int i = 0; i < operationCount; i++)
            {
                int taskId = i + 1;
                tasks[i] = Task.Run(async () =>
                {
                    Console.WriteLine($"   Operation {taskId}/{operationCount} starting");
                    await Task.Delay(300); // Each operation takes 300ms
                    Console.WriteLine($"   Operation {taskId}/{operationCount} completed");
                });
            }
            
            // Wait for all tasks to complete
            Task.WaitAll(tasks);
            
            sw.Stop();
            Console.WriteLine($"   Total time with parallel Task.Delay: {sw.ElapsedMilliseconds}ms");
            
            ConsoleHelper.WriteInfo("\nKey performance implications:");
            ConsoleHelper.WriteInfo("- Thread.Sleep blocks threads, limiting concurrency");
            ConsoleHelper.WriteInfo("- Task.Delay allows multiple operations to run concurrently");
            ConsoleHelper.WriteInfo("- In I/O-bound scenarios, Task.Delay significantly improves throughput");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Shows practical examples of when to use Thread.Sleep vs Task.Delay
        /// </summary>
        private void ShowPracticalExamples()
        {
            ConsoleHelper.WriteSubheader("Practical Examples");
            
            Console.WriteLine("When to use Thread.Sleep:");
            Console.WriteLine("1. When you actually want to block the current thread");
            Console.WriteLine("2. In console applications where asynchrony isn't needed");
            Console.WriteLine("3. In synchronous code where you can't use async/await");
            Console.WriteLine("4. When implementing throttling in a synchronous context");
            
            Console.WriteLine("\nWhen to use Task.Delay:");
            Console.WriteLine("1. In async methods with await");
            Console.WriteLine("2. In UI applications to avoid freezing the interface");
            Console.WriteLine("3. In server applications to maintain throughput");
            Console.WriteLine("4. When you need cancellation support");
            Console.WriteLine("5. When implementing timeouts in async code");
            
            Console.WriteLine("\nExample: Implementing a timeout with cancellation:");
            
            CancellationTokenSource cts = new CancellationTokenSource();
            
            async Task TimeoutDemoAsync()
            {
                try
                {
                    Console.WriteLine("Starting operation with timeout...");
                    
                    // Start a task that can be canceled
                    Task operationTask = Task.Run(async () =>
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            // Check for cancellation
                            cts.Token.ThrowIfCancellationRequested();
                            
                            Console.WriteLine($"Operation in progress... {i * 10}%");
                            await Task.Delay(300, cts.Token);
                        }
                        
                        Console.WriteLine("Operation completed successfully");
                    }, cts.Token);
                    
                    // Create a timeout task
                    Task timeoutTask = Task.Delay(2000, cts.Token);
                    
                    // Wait for either the operation to complete or the timeout to occur
                    Task completedTask = await Task.WhenAny(operationTask, timeoutTask);
                    
                    if (completedTask == timeoutTask)
                    {
                        // Timeout occurred
                        Console.WriteLine("Operation timed out!");
                        cts.Cancel(); // Cancel the operation
                    }
                    else
                    {
                        // Operation completed before timeout
                        cts.Cancel(); // Cancel the timeout
                        await operationTask; // Propagate any exceptions
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Operation was canceled");
                }
            }
            
            // Run the async demo and wait for it to complete
            TimeoutDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nTask.Delay supports cancellation, making it ideal for");
            ConsoleHelper.WriteInfo("implementing timeouts and cancelable waiting operations.");
            
            ConsoleHelper.WaitForKey();
        }
    }
}
