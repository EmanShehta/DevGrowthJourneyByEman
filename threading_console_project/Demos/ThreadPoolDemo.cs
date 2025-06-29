using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingConsoleDemo.Demos
{
    /// <summary>
    /// Demonstrates advanced threading concepts including thread pool, tasks, and callbacks
    /// </summary>
    public class ThreadPoolDemo
    {
        /// <summary>
        /// Demonstrates the thread pool concept and usage
        /// </summary>
        public void DemonstrateThreadPool()
        {
            ConsoleHelper.WriteHeader("Thread Pool", "بركة شرائط العمليات");
            
            ConsoleHelper.WriteInfo("This demo shows how the thread pool manages and reuses threads.");
            ConsoleHelper.WriteInfo("The thread pool is a collection of worker threads that can be used to perform tasks in the background.\n");
            
            // Get thread pool information
            ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
            ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);
            ThreadPool.GetAvailableThreads(out int availableWorkerThreads, out int availableCompletionPortThreads);
            
            Console.WriteLine("Thread Pool Information:");
            Console.WriteLine($"Max Worker Threads: {maxWorkerThreads}");
            Console.WriteLine($"Max Completion Port Threads: {maxCompletionPortThreads}");
            Console.WriteLine($"Min Worker Threads: {minWorkerThreads}");
            Console.WriteLine($"Min Completion Port Threads: {minCompletionPortThreads}");
            Console.WriteLine($"Available Worker Threads: {availableWorkerThreads}");
            Console.WriteLine($"Available Completion Port Threads: {availableCompletionPortThreads}\n");
            
            // Demonstrate queuing work items to the thread pool
            ConsoleHelper.WriteSubheader("Queuing Work Items to the Thread Pool");
            
            int numberOfWorkItems = ConsoleHelper.GetIntInput("Enter number of work items to queue", 5, 20);
            int delayMilliseconds = ConsoleHelper.GetIntInput("Enter delay per work item (ms)", 100, 1000);
            
            Console.WriteLine($"\nQueuing {numberOfWorkItems} work items to the thread pool...\n");
            
            // Use ManualResetEvent to signal when all work items are complete
            ManualResetEvent allDone = new ManualResetEvent(false);
            
            // Track thread IDs to demonstrate thread reuse
            HashSet<int> threadIds = new HashSet<int>();
            object lockObject = new object();
            
            // Counter for completed work items
            int completedWorkItems = 0;
            
            // Queue work items
            for (int i = 1; i <= numberOfWorkItems; i++)
            {
                int workItemId = i; // Capture the loop variable
                
                ThreadPool.QueueUserWorkItem(state =>
                {
                    int threadId = Thread.CurrentThread.ManagedThreadId;
                    
                    Console.WriteLine($"Work item {workItemId} started on thread {threadId}");
                    
                    // Simulate work
                    Thread.Sleep(delayMilliseconds);
                    
                    Console.WriteLine($"Work item {workItemId} completed on thread {threadId}");
                    
                    // Track thread IDs and completed count
                    lock (lockObject)
                    {
                        threadIds.Add(threadId);
                        completedWorkItems++;
                        
                        // If all work items are complete, signal the event
                        if (completedWorkItems == numberOfWorkItems)
                        {
                            allDone.Set();
                        }
                    }
                });
            }
            
            // Wait for all work items to complete
            Console.WriteLine("\nWaiting for all work items to complete...");
            allDone.WaitOne();
            
            // Display thread reuse statistics
            Console.WriteLine($"\nAll {numberOfWorkItems} work items completed.");
            Console.WriteLine($"Number of unique threads used: {threadIds.Count}");
            Console.WriteLine($"Thread reuse ratio: {(float)numberOfWorkItems / threadIds.Count:F2} work items per thread");
            
            if (threadIds.Count < numberOfWorkItems)
            {
                ConsoleHelper.WriteSuccess("\nThread reuse demonstrated! The thread pool reused threads for multiple work items.");
            }
            else
            {
                ConsoleHelper.WriteWarning("\nNo thread reuse observed. Each work item used a different thread.");
            }
            
            ConsoleHelper.WriteInfo("\nBenefits of the thread pool:");
            ConsoleHelper.WriteInfo("1. Reduced overhead - Thread creation is expensive");
            ConsoleHelper.WriteInfo("2. Better resource management - Limits the number of active threads");
            ConsoleHelper.WriteInfo("3. Load balancing - Distributes work across available threads");
            ConsoleHelper.WriteInfo("4. Simplified programming - No need to manually create and manage threads");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates the Task class and its relationship with the thread pool
        /// </summary>
        public void DemonstrateTasks()
        {
            ConsoleHelper.WriteHeader("Task Class and Pooled Thread", "كلاس المهام وبركة الشرائط");
            
            ConsoleHelper.WriteInfo("This demo shows how the Task class works with the thread pool.");
            ConsoleHelper.WriteInfo("Tasks provide a higher-level abstraction over threads and are the recommended way");
            ConsoleHelper.WriteInfo("to work with asynchronous operations in modern .NET applications.\n");
            
            // Basic task creation and execution
            ConsoleHelper.WriteSubheader("Basic Task Creation and Execution");
            
            Console.WriteLine("Creating and starting a simple task...");
            Task simpleTask = Task.Run(() =>
            {
                Console.WriteLine($"Simple task running on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine("Simple task completed");
            });
            
            Console.WriteLine("Task started. Waiting for completion...");
            simpleTask.Wait();
            Console.WriteLine($"Task status: {simpleTask.Status}\n");
            
            // Task with return value
            ConsoleHelper.WriteSubheader("Task with Return Value");
            
            Console.WriteLine("Creating a task that returns a value...");
            Task<int> taskWithResult = Task.Run(() =>
            {
                Console.WriteLine($"Calculation task running on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                return 42;
            });
            
            Console.WriteLine("Task started. Waiting for result...");
            int result = taskWithResult.Result; // This will block until the task completes
            Console.WriteLine($"Task result: {result}\n");
            
            // Multiple tasks
            ConsoleHelper.WriteSubheader("Running Multiple Tasks in Parallel");
            
            int taskCount = ConsoleHelper.GetIntInput("Enter number of tasks to run", 3, 10);
            
            Console.WriteLine($"\nCreating and starting {taskCount} tasks...");
            
            Task[] tasks = new Task[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                int taskId = i + 1;
                tasks[i] = Task.Run(() =>
                {
                    Console.WriteLine($"Task {taskId} started on thread {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(1000);
                    Console.WriteLine($"Task {taskId} completed");
                });
            }
            
            Console.WriteLine("\nWaiting for all tasks to complete...");
            Task.WaitAll(tasks);
            Console.WriteLine("All tasks completed\n");
            
            // Task continuation
            ConsoleHelper.WriteSubheader("Task Continuation");
            
            Console.WriteLine("Creating a task with continuation...");
            Task firstTask = Task.Run(() =>
            {
                Console.WriteLine($"First task running on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine("First task completed");
                return "Result from first task";
            });
            
            Task continuationTask = firstTask.ContinueWith(antecedent =>
            {
                Console.WriteLine($"Continuation task running on thread {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Received: {antecedent}");
                Thread.Sleep(1000);
                Console.WriteLine("Continuation task completed");
            });
            
            Console.WriteLine("Waiting for continuation chain to complete...");
            continuationTask.Wait();
            Console.WriteLine("Continuation chain completed\n");
            
            // Task exception handling
            ConsoleHelper.WriteSubheader("Task Exception Handling");
            
            Console.WriteLine("Creating a task that throws an exception...");
            Task faultedTask = Task.Run(() =>
            {
                Console.WriteLine($"Faulted task running on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                throw new InvalidOperationException("Deliberate exception for demonstration");
            });
            
            try
            {
                Console.WriteLine("Waiting for faulted task...");
                faultedTask.Wait();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("Caught AggregateException:");
                foreach (var ex in ae.InnerExceptions)
                {
                    Console.WriteLine($"- {ex.GetType().Name}: {ex.Message}");
                }
            }
            
            Console.WriteLine($"Faulted task status: {faultedTask.Status}");
            
            ConsoleHelper.WriteInfo("\nAdvantages of Tasks over raw threads:");
            ConsoleHelper.WriteInfo("1. Return values - Tasks can return values (Task<T>)");
            ConsoleHelper.WriteInfo("2. Exception handling - Exceptions are captured and propagated");
            ConsoleHelper.WriteInfo("3. Cancellation - Built-in support for cancellation");
            ConsoleHelper.WriteInfo("4. Continuations - Easy to chain operations");
            ConsoleHelper.WriteInfo("5. Composability - Combine tasks with Task.WhenAll, Task.WhenAny");
            ConsoleHelper.WriteInfo("6. async/await - Seamless integration with async/await pattern");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates WaitCallback and real-world thread pool usage
        /// </summary>
        public void DemonstrateWaitCallback()
        {
            ConsoleHelper.WriteHeader("Real World WaitCallBack", "مثال واقعي على انتظار النتيجة");
            
            ConsoleHelper.WriteInfo("This demo shows how WaitCallback is used in real-world scenarios.");
            ConsoleHelper.WriteInfo("WaitCallback is a delegate used with ThreadPool.QueueUserWorkItem to queue work to the thread pool.\n");
            
            // Basic WaitCallback usage
            ConsoleHelper.WriteSubheader("Basic WaitCallback Usage");
            
            Console.WriteLine("Queuing a simple work item using WaitCallback...");
            
            // Create a ManualResetEvent to signal when the work is done
            ManualResetEvent workDone = new ManualResetEvent(false);
            
            // Queue a work item using WaitCallback
            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                Console.WriteLine($"Work item executing on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine("Work item completed");
                workDone.Set(); // Signal that the work is done
            }));
            
            Console.WriteLine("Waiting for work item to complete...");
            workDone.WaitOne();
            Console.WriteLine("Work item signaled completion\n");
            
            // Parameterized WaitCallback
            ConsoleHelper.WriteSubheader("Parameterized WaitCallback");
            
            Console.WriteLine("Queuing work items with parameters...");
            
            // Create a countdown event to wait for all work items
            int itemCount = 5;
            CountdownEvent countdown = new CountdownEvent(itemCount);
            
            for (int i = 1; i <= itemCount; i++)
            {
                string data = $"Item {i}";
                
                ThreadPool.QueueUserWorkItem(state =>
                {
                    string itemData = state as string;
                    Console.WriteLine($"Processing {itemData} on thread {Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(500);
                    Console.WriteLine($"Completed processing {itemData}");
                    countdown.Signal(); // Signal that this work item is complete
                }, data);
            }
            
            Console.WriteLine($"Waiting for all {itemCount} work items to complete...");
            countdown.Wait();
            Console.WriteLine("All parameterized work items completed\n");
            
            // Real-world example: Order processing system
            ConsoleHelper.WriteSubheader("Real-World Example: Order Processing System");
            
            Console.WriteLine("Simulating an order processing system using the thread pool...");
            
            // Create a queue of orders to process
            Queue<string> orderQueue = new Queue<string>();
            for (int i = 1; i <= 10; i++)
            {
                orderQueue.Enqueue($"Order-{i:D3}");
            }
            
            Console.WriteLine($"Created {orderQueue.Count} orders to process");
            
            // Create synchronization objects
            object queueLock = new object();
            CountdownEvent ordersProcessed = new CountdownEvent(orderQueue.Count);
            
            // Track processing statistics
            Dictionary<int, int> threadWorkload = new Dictionary<int, int>();
            object statsLock = new object();
            
            // Create and start worker threads
            int workerCount = Math.Min(Environment.ProcessorCount, 4);
            Console.WriteLine($"Starting {workerCount} worker threads...");
            
            for (int i = 0; i < workerCount; i++)
            {
                ThreadPool.QueueUserWorkItem(state =>
                {
                    int threadId = Thread.CurrentThread.ManagedThreadId;
                    
                    while (true)
                    {
                        string order = null;
                        
                        // Try to get an order from the queue
                        lock (queueLock)
                        {
                            if (orderQueue.Count > 0)
                            {
                                order = orderQueue.Dequeue();
                            }
                            else
                            {
                                // No more orders to process
                                break;
                            }
                        }
                        
                        if (order != null)
                        {
                            // Process the order
                            Console.WriteLine($"Thread {threadId} processing {order}");
                            
                            // Simulate processing time (random duration)
                            Random random = new Random();
                            int processingTime = random.Next(200, 800);
                            Thread.Sleep(processingTime);
                            
                            Console.WriteLine($"Thread {threadId} completed {order} in {processingTime}ms");
                            
                            // Update statistics
                            lock (statsLock)
                            {
                                if (!threadWorkload.ContainsKey(threadId))
                                {
                                    threadWorkload[threadId] = 0;
                                }
                                threadWorkload[threadId]++;
                            }
                            
                            // Signal that an order has been processed
                            ordersProcessed.Signal();
                        }
                    }
                });
            }
            
            // Wait for all orders to be processed
            Console.WriteLine("Waiting for all orders to be processed...");
            ordersProcessed.Wait();
            
            // Display statistics
            Console.WriteLine("\nOrder processing completed!");
            Console.WriteLine("\nThread workload distribution:");
            foreach (var kvp in threadWorkload)
            {
                Console.WriteLine($"Thread {kvp.Key}: Processed {kvp.Value} orders");
            }
            
            ConsoleHelper.WriteInfo("\nKey points about WaitCallback and thread pool in real-world applications:");
            ConsoleHelper.WriteInfo("1. Work queue pattern - Common for processing items asynchronously");
            ConsoleHelper.WriteInfo("2. Resource management - Thread pool manages thread creation and reuse");
            ConsoleHelper.WriteInfo("3. Scalability - Automatically scales based on system resources");
            ConsoleHelper.WriteInfo("4. Load balancing - Work is distributed across available threads");
            ConsoleHelper.WriteInfo("5. Modern alternatives - Consider using Task Parallel Library for new code");
            
            ConsoleHelper.WaitForKey();
        }
    }
}
