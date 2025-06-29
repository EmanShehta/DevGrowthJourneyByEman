using System;
using System.Collections.Generic;
using System.Threading;
using ThreadingConsoleDemo.Demos;

namespace ThreadingConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set console properties
            Console.Title = "C# Threading Demo";
            Console.OutputEncoding = System.Text.Encoding.UTF8; 

            // Display welcome message
            DisplayWelcomeMessage();

            // Main menu loop
            bool exit = false;
            while (!exit)
            {
                // Display main menu
                int choice = DisplayMainMenu();

                switch (choice)
                {
                    case 1: // Introduction
                        ShowIntroduction();
                        break;
                    case 2: // Modern Applications and Concurrency
                        ShowModernApplications();
                        break;
                    case 3: // Process and Thread
                        ShowProcessAndThread();
                        break;
                    case 4: // Basic Threading Demos
                        RunBasicThreadingDemos();
                        break;
                    case 5: // Synchronization Demos
                        RunSynchronizationDemos();
                        break;
                    case 6: // Advanced Threading Demos
                        RunAdvancedThreadingDemos();
                        break;
                    case 0: // Exit
                        exit = true;
                        break;
                }
            }

            // Display goodbye message
            Console.Clear();
            ConsoleHelper.WriteHeader("Thank You", "ok");
            ConsoleHelper.WriteSuccess("Thank you for exploring C# threading concepts!");
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Displays the welcome message
        /// </summary>
        static void DisplayWelcomeMessage()
        {
            Console.Clear();
            ConsoleHelper.WriteHeader("C# Threading Demo", "c#");
            
            Console.WriteLine("Welcome to the C# Threading Demo Console Application!");
            Console.WriteLine("This application demonstrates various threading concepts in C#.");
            Console.WriteLine("Use the menu to navigate through different threading topics and demos.");
            
            ConsoleHelper.WaitForKey();
        }

        /// <summary>
        /// Displays the main menu and returns the user's choice
        /// </summary>
        static int DisplayMainMenu()
        {
            List<string> options = new List<string>
            {
                "Introduction ",
                "Modern Application and Concurrency ",
                "Process And Thread ",
                "Basic Threading Demos (Sequential, Creation, Foreground/Background, Start, Join)",
                "Synchronization Demos (Race Condition, Locks, Deadlock)",
                "Advanced Threading Demos (Thread Pool, Tasks, WaitCallback)"
            };

            return ConsoleHelper.DisplayMenu("Main Menu", options);
        }

        /// <summary>
        /// Shows the introduction to threading
        /// </summary>
        static void ShowIntroduction()
        {
            Console.Clear();
            ConsoleHelper.WriteHeader("Introduction", "Introduction");
            
            Console.WriteLine("Threading is a technique that enables your program to perform multiple operations concurrently.");
            Console.WriteLine("In C#, threading is supported through the System.Threading namespace.");
            Console.WriteLine();
            Console.WriteLine("Key concepts in threading:");
            Console.WriteLine("- Thread: A thread is the smallest unit of execution within a process");
            Console.WriteLine("- Multithreading: Running multiple threads simultaneously");
            Console.WriteLine("- Concurrency: Making progress on multiple tasks at the same time");
            Console.WriteLine("- Parallelism: Executing multiple tasks simultaneously");
            Console.WriteLine("- Synchronization: Coordinating access to shared resources");
            Console.WriteLine();
            Console.WriteLine("Benefits of threading:");
            Console.WriteLine("- Improved responsiveness in applications");
            Console.WriteLine("- Better resource utilization");
            Console.WriteLine("- Increased throughput for I/O-bound operations");
            Console.WriteLine("- Ability to perform multiple tasks simultaneously");
            Console.WriteLine();
            Console.WriteLine("Challenges of threading:");
            Console.WriteLine("- Race conditions when accessing shared data");
            Console.WriteLine("- Deadlocks where threads wait indefinitely for each other");
            Console.WriteLine("- Increased complexity in program design and debugging");
            Console.WriteLine("- Overhead of thread creation and context switching");
            
            ConsoleHelper.WaitForKey();
        }

        /// <summary>
        /// Shows information about modern applications and concurrency
        /// </summary>
        static void ShowModernApplications()
        {
            Console.Clear();
            ConsoleHelper.WriteHeader("Modern Application and Concurrency", "");
            
            Console.WriteLine("Modern applications often need to handle multiple operations simultaneously:");
            Console.WriteLine();
            Console.WriteLine("1. Responsive User Interfaces");
            Console.WriteLine("   - Keeping the UI responsive while performing background work");
            Console.WriteLine("   - Updating the UI with progress information");
            Console.WriteLine();
            Console.WriteLine("2. Server Applications");
            Console.WriteLine("   - Handling multiple client requests concurrently");
            Console.WriteLine("   - Processing data in parallel for better throughput");
            Console.WriteLine();
            Console.WriteLine("3. Data Processing");
            Console.WriteLine("   - Parallel processing of large datasets");
            Console.WriteLine("   - Distributed computing across multiple nodes");
            Console.WriteLine();
            Console.WriteLine("4. I/O Operations");
            Console.WriteLine("   - Non-blocking I/O for file, network, and database operations");
            Console.WriteLine("   - Asynchronous programming patterns");
            Console.WriteLine();
            Console.WriteLine("Evolution of Threading in .NET:");
            Console.WriteLine("- .NET 1.0: Basic Thread class");
            Console.WriteLine("- .NET 2.0: ThreadPool improvements");
            Console.WriteLine("- .NET 4.0: Task Parallel Library (TPL) and Task class");
            Console.WriteLine("- .NET 4.5: async/await pattern");
            Console.WriteLine("- .NET Core: Further improvements in performance and scalability");
            Console.WriteLine();
            Console.WriteLine("Modern Best Practices:");
            Console.WriteLine("- Prefer Task over Thread for most scenarios");
            Console.WriteLine("- Use async/await for asynchronous operations");
            Console.WriteLine("- Use high-level abstractions like Parallel.ForEach when possible");
            Console.WriteLine("- Consider thread safety from the beginning of design");
            
            ConsoleHelper.WaitForKey();
        }

        /// <summary>
        /// Shows information about processes and threads
        /// </summary>
        static void ShowProcessAndThread()
        {
            Console.Clear();
            ConsoleHelper.WriteHeader("Process And Thread", "");
            
            Console.WriteLine("Understanding the difference between processes and threads is fundamental:");
            Console.WriteLine();
            Console.WriteLine("Process:");
            Console.WriteLine("- An instance of a running program");
            Console.WriteLine("- Has its own memory space (virtual address space)");
            Console.WriteLine("- Contains one or more threads");
            Console.WriteLine("- Isolated from other processes");
            Console.WriteLine("- Expensive to create and terminate");
            Console.WriteLine("- Examples: Each instance of a browser, word processor, or this console app");
            Console.WriteLine();
            Console.WriteLine("Thread:");
            Console.WriteLine("- The smallest unit of execution within a process");
            Console.WriteLine("- Shares the process's memory space with other threads");
            Console.WriteLine("- Has its own execution path and stack");
            Console.WriteLine("- Less expensive to create than processes");
            Console.WriteLine("- Can communicate with other threads via shared memory");
            Console.WriteLine();
            Console.WriteLine("Thread Components:");
            Console.WriteLine("- Thread ID: Unique identifier");
            Console.WriteLine("- Program Counter: Current position in the code");
            Console.WriteLine("- Register Set: Working variables");
            Console.WriteLine("- Stack: Method calls and local variables");
            Console.WriteLine();
            Console.WriteLine("Thread States:");
            Console.WriteLine("- Unstarted: Created but not yet started");
            Console.WriteLine("- Running: Currently executing");
            Console.WriteLine("- WaitSleepJoin: Temporarily paused");
            Console.WriteLine("- Suspended: Paused by explicit suspension");
            Console.WriteLine("- Stopped: Completed execution");
            Console.WriteLine("- Aborted: Terminated abnormally");
            
            ConsoleHelper.WaitForKey();
        }

        /// <summary>
        /// Runs the basic threading demos
        /// </summary>
        static void RunBasicThreadingDemos()
        {
            BasicThreadingDemo demo = new BasicThreadingDemo();
            
            bool returnToMainMenu = false;
            while (!returnToMainMenu)
            {
                List<string> options = new List<string>
                {
                    "Sequential Synchronous Approach ",
                    "Creating a thread ",
                    "Foreground Vs Background Threads ",
                    "Start a Thread ",
                    "Join a thread "
                };
                
                int choice = ConsoleHelper.DisplayMenu("Basic Threading Demos", options);
                
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        demo.DemonstrateSequentialApproach();
                        break;
                    case 2:
                        Console.Clear();
                        demo.DemonstrateThreadCreation();
                        break;
                    case 3:
                        Console.Clear();
                        demo.DemonstrateForegroundVsBackground();
                        break;
                    case 4:
                        Console.Clear();
                        demo.DemonstrateStartingThreads();
                        break;
                    case 5:
                        Console.Clear();
                        demo.DemonstrateJoiningThreads();
                        break;
                    case 0:
                        returnToMainMenu = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Runs the synchronization demos
        /// </summary>
        static void RunSynchronizationDemos()
        {
            SynchronizationDemo demo = new SynchronizationDemo();
            
            bool returnToMainMenu = false;
            while (!returnToMainMenu)
            {
                List<string> options = new List<string>
                {
                    "Race Condition ",
                    "Lock to prevent Race condition ",
                    "Deadlock "
                };
                
                int choice = ConsoleHelper.DisplayMenu("Synchronization Demos", options);
                
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        demo.DemonstrateRaceCondition();
                        break;
                    case 2:
                        Console.Clear();
                        demo.DemonstrateLockPrevention();
                        break;
                    case 3:
                        Console.Clear();
                        demo.DemonstrateDeadlock();
                        break;
                    case 0:
                        returnToMainMenu = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Runs the advanced threading demos
        /// </summary>
        static void RunAdvancedThreadingDemos()
        {
            ThreadPoolDemo demo = new ThreadPoolDemo();
            
            bool returnToMainMenu = false;
            while (!returnToMainMenu)
            {
                List<string> options = new List<string>
                {
                    "Thread Pool ",
                    "Task class and Pooled thread ",
                    "Real World WaitCallBack "
                };
                
                int choice = ConsoleHelper.DisplayMenu("Advanced Threading Demos", options);
                
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        demo.DemonstrateThreadPool();
                        break;
                    case 2:
                        Console.Clear();
                        demo.DemonstrateTasks();
                        break;
                    case 3:
                        Console.Clear();
                        demo.DemonstrateWaitCallback();
                        break;
                    case 0:
                        returnToMainMenu = true;
                        break;
                }
            }
        }
    }
}
