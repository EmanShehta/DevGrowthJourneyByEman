using System;
using System.Threading;

namespace ThreadingConsoleDemo.Demos
{
    /// <summary>
    /// Demonstrates basic threading concepts including creation, starting, and joining threads
    /// </summary>
    public class BasicThreadingDemo
    {
        /// <summary>
        /// Demonstrates the sequential synchronous approach
        /// </summary>
        public void DemonstrateSequentialApproach()
        {
            ConsoleHelper.WriteHeader("Sequential Synchronous Approach", "");
            
            ConsoleHelper.WriteInfo("This demo shows how operations are executed sequentially in a single thread.");
            ConsoleHelper.WriteInfo("We'll simulate three tasks that take time to complete.\n");
            
            Console.WriteLine("Starting sequential execution of tasks...\n");
            
            DateTime startTime = DateTime.Now;
            
            // First task
            Console.WriteLine("Starting Task 1...");
            SimulateProcessingWork("Task 1", 2000);
            Console.WriteLine("Task 1 completed.\n");
            
            // Second task
            Console.WriteLine("Starting Task 2...");
            SimulateProcessingWork("Task 2", 3000);
            Console.WriteLine("Task 2 completed.\n");
            
            // Third task
            Console.WriteLine("Starting Task 3...");
            SimulateProcessingWork("Task 3", 1500);
            Console.WriteLine("Task 3 completed.\n");
            
            TimeSpan totalTime = DateTime.Now - startTime;
            
            ConsoleHelper.WriteSuccess($"All tasks completed sequentially in {totalTime.TotalSeconds:F2} seconds.");
            ConsoleHelper.WriteInfo("\nIn a sequential approach, each task must complete before the next one starts.");
            ConsoleHelper.WriteInfo("This is simple but inefficient when tasks could run in parallel.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates creating and configuring threads
        /// </summary>
        public void DemonstrateThreadCreation()
        {
            ConsoleHelper.WriteHeader("Creating a Thread", "");
            
            ConsoleHelper.WriteInfo("This demo shows different ways to create threads in C#.\n");
            
            // Method 1: Using ThreadStart delegate
            ConsoleHelper.WriteSubheader("Method 1: Using ThreadStart delegate");
            Console.WriteLine("Creating a thread with a named method:");
            Thread thread1 = new Thread(new ThreadStart(ThreadMethod));
            Console.WriteLine($"Thread created: ID={thread1.ManagedThreadId}, State={thread1.ThreadState}");
            
            // Method 2: Using ParameterizedThreadStart delegate
            ConsoleHelper.WriteSubheader("Method 2: Using ParameterizedThreadStart delegate");
            Console.WriteLine("Creating a thread with a parameterized method:");
            Thread thread2 = new Thread(new ParameterizedThreadStart(ParameterizedThreadMethod));
            Console.WriteLine($"Thread created: ID={thread2.ManagedThreadId}, State={thread2.ThreadState}");
            
            // Method 3: Using lambda expression
            ConsoleHelper.WriteSubheader("Method 3: Using lambda expression");
            Console.WriteLine("Creating a thread with a lambda expression:");
            Thread thread3 = new Thread(() => 
            {
                Console.WriteLine($"Lambda thread running on thread ID: {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine("Lambda thread completed.");
            });
            Console.WriteLine($"Thread created: ID={thread3.ManagedThreadId}, State={thread3.ThreadState}");
            
            // Thread properties
            ConsoleHelper.WriteSubheader("Thread Properties and Configuration");
            Console.WriteLine("Threads have various properties that can be configured:");
            
            thread3.Name = "DemoThread";
            thread3.IsBackground = false; // Foreground thread
            thread3.Priority = ThreadPriority.Normal;
            
            Console.WriteLine($"Name: {thread3.Name}");
            Console.WriteLine($"IsBackground: {thread3.IsBackground}");
            Console.WriteLine($"Priority: {thread3.Priority}");
            Console.WriteLine($"State: {thread3.ThreadState}");
            
            ConsoleHelper.WriteInfo("\nNote: Threads are not started automatically when created.");
            ConsoleHelper.WriteInfo("You must call the Start() method to begin execution.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates the difference between foreground and background threads
        /// </summary>
        public void DemonstrateForegroundVsBackground()
        {
            ConsoleHelper.WriteHeader("Foreground Vs Background Threads", "");
            
            ConsoleHelper.WriteInfo("This demo shows the difference between foreground and background threads.");
            ConsoleHelper.WriteInfo("- Foreground threads keep the application running until they complete");
            ConsoleHelper.WriteInfo("- Background threads are terminated when all foreground threads exit\n");
            
            // Create a foreground thread
            Thread foregroundThread = new Thread(() =>
            {
                Console.WriteLine("Foreground thread started.");
                for (int i = 1; i <= 5; i++)
                {
                    Console.WriteLine($"Foreground thread: Count {i}/5");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Foreground thread completed.");
            });
            foregroundThread.Name = "ForegroundThread";
            foregroundThread.IsBackground = false;  // This is the default
            
            // Create a background thread
            Thread backgroundThread = new Thread(() =>
            {
                Console.WriteLine("Background thread started.");
                for (int i = 1; i <= 10; i++)
                {
                    Console.WriteLine($"Background thread: Count {i}/10");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Background thread completed.");
            });
            backgroundThread.Name = "BackgroundThread";
            backgroundThread.IsBackground = true;
            
            // Start both threads
            Console.WriteLine("Starting both threads...");
            foregroundThread.Start();
            backgroundThread.Start();
            
            // Wait for the foreground thread to complete
            foregroundThread.Join();
            
            ConsoleHelper.WriteSuccess("\nForeground thread has completed.");
            ConsoleHelper.WriteWarning("If this were a real application with no other foreground threads,");
            ConsoleHelper.WriteWarning("the application would exit now, terminating any background threads.");
            ConsoleHelper.WriteInfo("\nIn this demo, we'll wait a bit longer to see the background thread...");
            
            // Wait a bit more to see some of the background thread's output
            Thread.Sleep(3000);
            
            ConsoleHelper.WriteInfo("\nDemo complete. Notice that the background thread didn't finish its full count.");
            ConsoleHelper.WriteInfo("In a real application, it would have been terminated when the main thread exited.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates starting threads and observing their execution
        /// </summary>
        public void DemonstrateStartingThreads()
        {
            ConsoleHelper.WriteHeader("Start a Thread", "");
            
            ConsoleHelper.WriteInfo("This demo shows how to start threads and observe their execution.\n");
            
            Console.WriteLine("Creating multiple threads...");
            
            // Create several threads with different priorities
            Thread lowPriorityThread = new Thread(() => CountWithPriority("Low Priority", 5));
            lowPriorityThread.Priority = ThreadPriority.Lowest;
            
            Thread normalPriorityThread = new Thread(() => CountWithPriority("Normal Priority", 5));
            normalPriorityThread.Priority = ThreadPriority.Normal;
            
            Thread highPriorityThread = new Thread(() => CountWithPriority("High Priority", 5));
            highPriorityThread.Priority = ThreadPriority.Highest;
            
            // Display thread information before starting
            Console.WriteLine("Threads created but not yet started:");
            Console.WriteLine($"Low Priority Thread: ID={lowPriorityThread.ManagedThreadId}, State={lowPriorityThread.ThreadState}");
            Console.WriteLine($"Normal Priority Thread: ID={normalPriorityThread.ManagedThreadId}, State={normalPriorityThread.ThreadState}");
            Console.WriteLine($"High Priority Thread: ID={highPriorityThread.ManagedThreadId}, State={highPriorityThread.ThreadState}");
            
            Console.WriteLine("\nStarting all threads...\n");
            
            // Start all threads
            DateTime startTime = DateTime.Now;
            lowPriorityThread.Start();
            normalPriorityThread.Start();
            highPriorityThread.Start();
            
            // Wait for all threads to complete
            lowPriorityThread.Join();
            normalPriorityThread.Join();
            highPriorityThread.Join();
            
            TimeSpan totalTime = DateTime.Now - startTime;
            
            ConsoleHelper.WriteSuccess($"\nAll threads completed in {totalTime.TotalSeconds:F2} seconds.");
            
            ConsoleHelper.WriteInfo("\nNotes about thread priorities:");
            ConsoleHelper.WriteInfo("- Thread priorities are hints to the operating system scheduler");
            ConsoleHelper.WriteInfo("- The actual execution order depends on many factors");
            ConsoleHelper.WriteInfo("- Higher priority threads are not guaranteed to finish first");
            ConsoleHelper.WriteInfo("- Use priorities sparingly and carefully");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates joining threads to wait for their completion
        /// </summary>
        public void DemonstrateJoiningThreads()
        {
            ConsoleHelper.WriteHeader("Join a Thread", "");
            
            ConsoleHelper.WriteInfo("This demo shows how to wait for thread completion using Join().\n");
            
            Console.WriteLine("Creating worker threads...");
            
            // Create worker threads
            Thread worker1 = new Thread(() => PerformWork("Worker 1", 3));
            Thread worker2 = new Thread(() => PerformWork("Worker 2", 5));
            Thread worker3 = new Thread(() => PerformWork("Worker 3", 2));
            
            Console.WriteLine("Starting all worker threads...\n");
            
            // Start all threads
            DateTime startTime = DateTime.Now;
            worker1.Start();
            worker2.Start();
            worker3.Start();
            
            Console.WriteLine("Main thread continues execution...");
            Console.WriteLine("Doing some work in the main thread...");
            Thread.Sleep(1000);
            Console.WriteLine("Main thread work completed.");
            
            Console.WriteLine("\nNow waiting for Worker 1 to complete (Join)...");
            worker1.Join();
            Console.WriteLine("Worker 1 has completed.");
            
            Console.WriteLine("\nNow waiting for Worker 2 to complete with timeout (Join with timeout)...");
            bool worker2Completed = worker2.Join(2000); // Wait up to 2 seconds
            if (worker2Completed)
            {
                Console.WriteLine("Worker 2 completed within the timeout period.");
            }
            else
            {
                Console.WriteLine("Worker 2 did not complete within the timeout period.");
            }
            
            Console.WriteLine("\nNow waiting for Worker 3 to complete (Join)...");
            worker3.Join();
            Console.WriteLine("Worker 3 has completed.");
            
            // Wait for any remaining threads to complete
            if (!worker2Completed)
            {
                Console.WriteLine("\nStill waiting for Worker 2 to complete...");
                worker2.Join();
                Console.WriteLine("Worker 2 has now completed.");
            }
            
            TimeSpan totalTime = DateTime.Now - startTime;
            
            ConsoleHelper.WriteSuccess($"\nAll threads completed in {totalTime.TotalSeconds:F2} seconds.");
            
            ConsoleHelper.WriteInfo("\nKey points about Join:");
            ConsoleHelper.WriteInfo("- Join() blocks the calling thread until the target thread completes");
            ConsoleHelper.WriteInfo("- Join(timeout) blocks for at most the specified time");
            ConsoleHelper.WriteInfo("- Join() is useful for coordinating work between threads");
            ConsoleHelper.WriteInfo("- Join() helps implement a fork-join pattern of parallel execution");
            
            ConsoleHelper.WaitForKey();
        }
        
        #region Helper Methods
        
        /// <summary>
        /// Simulates processing work with a progress bar
        /// </summary>
        private void SimulateProcessingWork(string taskName, int duration)
        {
            int steps = 20;
            int stepDuration = duration / steps;
            
            for (int i = 1; i <= steps; i++)
            {
                ConsoleHelper.DisplayProgressBar(i, steps);
                Thread.Sleep(stepDuration);
            }
            
            Console.WriteLine();
        }
        
        /// <summary>
        /// Method used for ThreadStart delegate demonstration
        /// </summary>
        private void ThreadMethod()
        {
            Console.WriteLine($"ThreadMethod running on thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);
            Console.WriteLine("ThreadMethod completed.");
        }
        
        /// <summary>
        /// Method used for ParameterizedThreadStart delegate demonstration
        /// </summary>
        private void ParameterizedThreadMethod(object parameter)
        {
            Console.WriteLine($"ParameterizedThreadMethod running on thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Parameter received: {parameter}");
            Thread.Sleep(1000);
            Console.WriteLine("ParameterizedThreadMethod completed.");
        }
        
        /// <summary>
        /// Counts with the specified thread priority
        /// </summary>
        private void CountWithPriority(string threadName, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine($"{threadName} thread: Count {i}/{count}");
                Thread.Sleep(500);
            }
            Console.WriteLine($"{threadName} thread completed.");
        }
        
        /// <summary>
        /// Performs work for a specified number of seconds
        /// </summary>
        private void PerformWork(string workerName, int seconds)
        {
            Console.WriteLine($"{workerName} started, will run for {seconds} seconds.");
            
            for (int i = 1; i <= seconds; i++)
            {
                Console.WriteLine($"{workerName}: Working... ({i}/{seconds})");
                Thread.Sleep(1000);
            }
            
            Console.WriteLine($"{workerName} completed its work.");
        }
        
        #endregion
    }
}
