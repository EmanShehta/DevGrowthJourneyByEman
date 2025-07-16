using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates Task return values in asynchronous programming
    /// </summary>
    public class TaskReturnValueDemo
    {
        /// <summary>
        /// Runs the Task Return Value demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Task Return Value", "مهمة ترجع قيمة للمستدعي");
            
            ConsoleHelper.WriteInfo("This demo shows how Tasks can return values to the caller.");
            ConsoleHelper.WriteInfo("Unlike threads, Tasks can directly return results when they complete.\n");
            
            // Basic task with return value
            DemonstrateBasicTaskWithResult();
            
            // Multiple tasks with results
            DemonstrateMultipleTasksWithResults();
            
            // Async method with return value
            DemonstrateAsyncMethodWithResult();
            
            // Chaining tasks with results
            DemonstrateTaskChainingWithResults();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Task Return Values");
            Console.WriteLine("Key points about Task return values:");
            Console.WriteLine("✓ Use Task<T> to return values from asynchronous operations");
            Console.WriteLine("✓ Access results using the .Result property (blocking) or await (non-blocking)");
            Console.WriteLine("✓ Task results are cached after completion");
            Console.WriteLine("✓ Results can be passed to continuation tasks");
            Console.WriteLine("✓ Exceptions during task execution are captured and rethrown when accessing .Result");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates a basic Task with a return value
        /// </summary>
        private void DemonstrateBasicTaskWithResult()
        {
            ConsoleHelper.WriteSubheader("Basic Task with Return Value");
            
            Console.WriteLine("Creating a Task<int> that returns a computed value...");
            
            Task<int> calculationTask = Task.Run(() =>
            {
                Console.WriteLine($"Task running on thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                
                // Simulate computation
                System.Threading.Thread.Sleep(1000);
                
                int result = 42;
                Console.WriteLine($"Task computed result: {result}");
                return result;
            });
            
            Console.WriteLine("Task started. Status: " + calculationTask.Status);
            Console.WriteLine("Waiting for result...");
            
            // Get the result (this will block until the task completes)
            int taskResult = calculationTask.Result;
            
            Console.WriteLine($"Received result: {taskResult}");
            Console.WriteLine("Task status after completion: " + calculationTask.Status);
            
            ConsoleHelper.WriteInfo("\nNote: Using .Result will block the current thread until the task completes.");
            ConsoleHelper.WriteInfo("In real applications, prefer using 'await' with async methods to avoid blocking.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates multiple tasks with different return value types
        /// </summary>
        private void DemonstrateMultipleTasksWithResults()
        {
            ConsoleHelper.WriteSubheader("Multiple Tasks with Different Return Types");
            
            Console.WriteLine("Creating tasks with different return types...");
            
            // Task returning string
            Task<string> stringTask = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(800);
                return "Hello from Task!";
            });
            
            // Task returning double
            Task<double> doubleTask = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1200);
                return 3.14159;
            });
            
            // Task returning DateTime
            Task<DateTime> dateTimeTask = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                return DateTime.Now.AddDays(7);
            });
            
            Console.WriteLine("All tasks started. Waiting for results...");
            
            // Wait for all tasks to complete
            Task.WaitAll(stringTask, doubleTask, dateTimeTask);
            
            // Access results
            Console.WriteLine($"\nString task result: {stringTask.Result}");
            Console.WriteLine($"Double task result: {doubleTask.Result}");
            Console.WriteLine($"DateTime task result: {dateTimeTask.Result:yyyy-MM-dd}");
            
            ConsoleHelper.WriteInfo("\nTask<T> can return any type T, including reference types, value types,");
            ConsoleHelper.WriteInfo("and even custom classes or collections.");
            
            ConsoleHelper.WaitForKey();
        }

        /// <summary>
        /// Demonstrates an async method with return value
        /// </summary>
        private async void DemonstrateAsyncMethodWithResult()
        {
            ConsoleHelper.WriteSubheader("Async Method with Return Value");

            Console.WriteLine($"[Main] Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Calling an async method that returns a value...");

            try
            {
                string result = await GetMessageAsync("World");
                Console.WriteLine($"Received result: {result}");
                Console.WriteLine($"[After GetMessageAsync] Thread ID: {Thread.CurrentThread.ManagedThreadId}");

                int count = await CountCharactersAsync(result);
                Console.WriteLine($"Character count: {count}");
                Console.WriteLine($"[After CountCharactersAsync] Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            ConsoleHelper.WriteInfo("\nAsync methods can return Task<T> to provide results when they complete.");
            ConsoleHelper.WriteInfo("Using 'await' unwraps the Task<T> and gives you the T value directly.");

            ConsoleHelper.WaitForKey();
        }

        private void DemonstrateTaskChainingWithResults()
        {
            ConsoleHelper.WriteSubheader("Chaining Tasks with Results");

            Console.WriteLine($"[Main] Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Creating a chain of tasks that pass results to each other...");

            Task<int> generateNumberTask = Task.Run(() =>
            {
                Console.WriteLine($"First task: Generating a number (Thread ID: {Thread.CurrentThread.ManagedThreadId})");
                Thread.Sleep(1000);
                return 10;
            });

            Task<int> doubleNumberTask = generateNumberTask.ContinueWith(antecedent =>
            {
                int number = antecedent.Result;
                Console.WriteLine($"Second task: Doubling {number} (Thread ID: {Thread.CurrentThread.ManagedThreadId})");
                Thread.Sleep(1000);
                return number * 2;
            });

            Task<string> formatResultTask = doubleNumberTask.ContinueWith(antecedent =>
            {
                int number = antecedent.Result;
                Console.WriteLine($"Third task: Formatting {number} (Thread ID: {Thread.CurrentThread.ManagedThreadId})");
                Thread.Sleep(1000);
                return $"The final result is {number}";
            });

            Console.WriteLine("Task chain created. Waiting for final result...");
            Console.WriteLine($"[Before waiting] Thread ID: {Thread.CurrentThread.ManagedThreadId}");

            string finalResult = formatResultTask.Result;
            Console.WriteLine($"[After waiting] Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"\nFinal result: {finalResult}");

            ConsoleHelper.WriteInfo("\nTask chaining allows you to create a pipeline of operations,");
            ConsoleHelper.WriteInfo("where each task takes the result of the previous task as input.");

            ConsoleHelper.WaitForKey();
        }


        #region Helper Methods

        /// <summary>
        /// Async method that returns a string after a delay
        /// </summary>
        private async Task<string> GetMessageAsync(string name)
        {
            Console.WriteLine($"GetMessageAsync: Starting... (Thread ID: {Thread.CurrentThread.ManagedThreadId})");
            await Task.Delay(1500);
            Console.WriteLine($"GetMessageAsync: Completed (Thread ID: {Thread.CurrentThread.ManagedThreadId})");
            return $"Hello, {name}! Current time is {DateTime.Now:HH:mm:ss}";
        }

        private async Task<int> CountCharactersAsync(string text)
        {
            Console.WriteLine($"CountCharactersAsync: Starting... (Thread ID: {Thread.CurrentThread.ManagedThreadId})");
            await Task.Delay(800);
            Console.WriteLine($"CountCharactersAsync: Completed (Thread ID: {Thread.CurrentThread.ManagedThreadId})");
            return text.Length;
        }


        #endregion
    }
}
