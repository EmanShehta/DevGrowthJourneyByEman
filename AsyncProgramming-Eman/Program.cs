using System;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Demos;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo
{
    /// <summary>
    /// Main program class for the Asynchronous Programming Demo
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point for the application
        /// </summary>
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;


            // Display welcome message
            DisplayWelcomeMessage();
            
            // Main menu loop
            bool exit = false;
            while (!exit)
            {
                DisplayMainMenu();
                
                // Get user choice
                Console.Write("\nEnter your choice (1-14, or 0 to exit): ");
                string input = Console.ReadLine();
                
                if (int.TryParse(input, out int choice))
                {
                    Console.Clear();
                    
                    switch (choice)
                    {
                        case 0:
                            exit = true;
                            break;
                        case 1:
                            new TaskVsThreadsDemo().Run();
                            break;
                        case 2:
                            new TaskReturnValueDemo().Run();
                            break;
                        case 3:
                            new LongRunningTasksDemo().Run();
                            break;
                        case 4:
                            new ExceptionPropagationDemo().Run();
                            break;
                        case 5:
                            new TaskContinuationDemo().Run();
                            break;
                        case 6:
                            new DelayVsSleepDemo().Run();
                            break;
                        case 7:
                            new SyncVsAsyncDemo().Run();
                            break;
                        case 8:
                            new AsyncAwaitDemo().Run();
                            break;
                        case 9:
                            new CancellationTokenDemo().Run();
                            break;
                        case 10:
                            new ProgressReportingDemo().Run();
                            break;
                        case 11:
                            new CombinatorsDemo().Run();
                            break;
                        case 12:
                            new TaskBasedAsyncPatternDemo().Run();
                            break;
                        case 13:
                            new ConcurrencyParallelismDemo().Run();
                            break;
                        case 14:
                            RunAllDemos();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            ConsoleHelper.WaitForKey();
                            break;
                    }
                    
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    ConsoleHelper.WaitForKey();
                    Console.Clear();
                }
            }
            
            // Display goodbye message
            DisplayGoodbyeMessage();
        }
        
        /// <summary>
        /// Displays the welcome message
        /// </summary>
        private static void DisplayWelcomeMessage()
        {
            Console.Clear();
            ConsoleHelper.WriteHeader("Asynchronous Programming in C#", "البرمجة غير المتزامنة في سي شارب");

            Console.WriteLine("Welcome to this comprehensive demonstration of asynchronous programming concepts in C#.");
            Console.WriteLine("This application provides interactive examples of various async programming techniques.");
            Console.WriteLine("\nUse the menu to explore different topics and see them in action.");
            
            ConsoleHelper.WaitForKey();
            Console.Clear();
        }
        
        /// <summary>
        /// Displays the main menu
        /// </summary>
        private static void DisplayMainMenu()
        {
            ConsoleHelper.WriteHeader("Main Menu", "القائمة الرئيسية");
            
            Console.WriteLine("Select a topic to explore:");
            Console.WriteLine();
            Console.WriteLine(" 1. Task Vs. Threads | المهام مقابل مسار العمليات");
            Console.WriteLine(" 2. Task Return Value | مهمة ترجع قيمة للمستدعي");
            Console.WriteLine(" 3. Long Running Tasks | المهام التي تتطلب وقت طويل للتنفيذ");
            Console.WriteLine(" 4. Exception Propagation | تصاعد الاحداث الاستثنائية");
            Console.WriteLine(" 5. Task Continuation | عندما تكتمل المهمة");
            Console.WriteLine(" 6. Task.Delay() Vs Thread.Sleep() | تنويم المسار مقابل تاخير المهمة");
            Console.WriteLine(" 7. Sync Vs. Async | التزامن مقابل اللاتزامن");
            Console.WriteLine(" 8. Asynchronous Function (async, await) | دوال التزامن");
            Console.WriteLine(" 9. Cancellation Token | رمز الالغاء");
            Console.WriteLine("10. Report a Progress from Async Function | اعطاء تقرير بنسبة الانجاز");
            Console.WriteLine("11. Combinators | التباديل");
            Console.WriteLine("12. Task-Based Asynchronous Pattern | انماط المهام غير المتزامنة");
            Console.WriteLine("13. Concurrency and Parallelism | التنفيذ بالتزامن او بالتوازي");
            Console.WriteLine("14. Run All Demos | تشغيل جميع العروض التوضيحية");
            Console.WriteLine(" 0. Exit | خروج");
        }
        
        /// <summary>
        /// Runs all demos in sequence
        /// </summary>
        private static void RunAllDemos()
        {
            Console.Clear();
            ConsoleHelper.WriteHeader("Running All Demos", "تشغيل جميع العروض التوضيحية");
            
            Console.WriteLine("This will run all demos in sequence. Press any key to start...");
            ConsoleHelper.WaitForKey();
            
            // Run each demo
            new TaskVsThreadsDemo().Run();
            new TaskReturnValueDemo().Run();
            new LongRunningTasksDemo().Run();
            new ExceptionPropagationDemo().Run();
            new TaskContinuationDemo().Run();
            new DelayVsSleepDemo().Run();
            new SyncVsAsyncDemo().Run();
            new AsyncAwaitDemo().Run();
            new CancellationTokenDemo().Run();
            new ProgressReportingDemo().Run();
            new CombinatorsDemo().Run();
            new TaskBasedAsyncPatternDemo().Run();
            new ConcurrencyParallelismDemo().Run();
            
            Console.WriteLine("\nAll demos completed! Press any key to return to the main menu...");
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Displays the goodbye message
        /// </summary>
        private static void DisplayGoodbyeMessage()
        {
            Console.Clear();
            ConsoleHelper.WriteHeader("Thank You!", "شكرا لك!");
            
            Console.WriteLine("Thank you for exploring asynchronous programming concepts in C#.");
            Console.WriteLine("We hope these demonstrations have helped you understand these important concepts.");
            Console.WriteLine("\nPress any key to exit...");
            
            ConsoleHelper.WaitForKey();
        }
    }
}
