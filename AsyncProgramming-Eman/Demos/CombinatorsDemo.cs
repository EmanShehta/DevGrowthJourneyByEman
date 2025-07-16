using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates task combinators in asynchronous programming
    /// </summary>
    public class CombinatorsDemo
    {
        /// <summary>
        /// Runs the Combinators demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Combinators", "التباديل");
            
            ConsoleHelper.WriteInfo("This demo shows how to use task combinators to work with multiple tasks.");
            ConsoleHelper.WriteInfo("Combinators allow you to coordinate and compose multiple asynchronous operations.\n");
            
            // Task.WhenAll
            DemonstrateWhenAll();
            
            // Task.WhenAny
            DemonstrateWhenAny();
            
            // Task.WaitAll and Task.WaitAny
            DemonstrateWaitMethods();
            
            // Custom combinators
            DemonstrateCustomCombinators();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Task Combinators");
            Console.WriteLine("Key points about task combinators:");
            Console.WriteLine("✓ Task.WhenAll - Wait for all tasks to complete asynchronously");
            Console.WriteLine("✓ Task.WhenAny - Wait for any task to complete asynchronously");
            Console.WriteLine("✓ Task.WaitAll - Wait for all tasks to complete synchronously (blocking)");
            Console.WriteLine("✓ Task.WaitAny - Wait for any task to complete synchronously (blocking)");
            Console.WriteLine("✓ Custom combinators can be created for specific scenarios");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates Task.WhenAll combinator
        /// </summary>
        private void DemonstrateWhenAll()
        {
            ConsoleHelper.WriteSubheader("Task.WhenAll");
            
            Console.WriteLine("Task.WhenAll waits for all tasks to complete asynchronously:");
            
            // We need to use an async local function
            async Task WhenAllDemoAsync()
            {
                Console.WriteLine("Starting multiple tasks...");
                
                // Create several tasks with different durations
                Task<string> task1 = SimulateWorkAsync("Task 1", 2000);
                Task<string> task2 = SimulateWorkAsync("Task 2", 1000);
                Task<string> task3 = SimulateWorkAsync("Task 3", 3000);
                
                Console.WriteLine("Waiting for all tasks to complete...");
                
                // Wait for all tasks to complete
                string[] results = await Task.WhenAll(task1, task2, task3);
                
                Console.WriteLine("\nAll tasks completed!");
                Console.WriteLine("Results:");
                foreach (string result in results)
                {
                    Console.WriteLine($"- {result}");
                }
            }
            
            // Run the async demo and wait for it to complete
            WhenAllDemoAsync().GetAwaiter().GetResult();
            
            Console.WriteLine("\nTask.WhenAll with exception handling:");
            
            async Task WhenAllExceptionDemoAsync()
            {
                // Create tasks where some will fail
                Task<string> successTask = SimulateWorkAsync("Success Task", 1000);
                Task<string> failTask1 = SimulateFailingWorkAsync("Fail Task 1", 1500);
                Task<string> failTask2 = SimulateFailingWorkAsync("Fail Task 2", 2000);
                
                try
                {
                    // Wait for all tasks to complete
                    string[] results = await Task.WhenAll(successTask, failTask1, failTask2);
                    Console.WriteLine("This line won't execute if any task fails");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nCaught exception: {ex.Message}");
                    
                    // Check the status of all tasks
                    Console.WriteLine($"Success Task status: {successTask.Status}");
                    Console.WriteLine($"Fail Task 1 status: {failTask1.Status}");
                    Console.WriteLine($"Fail Task 2 status: {failTask2.Status}");
                    
                    // We can still get results from successful tasks
                    if (successTask.IsCompletedSuccessfully)
                    {
                        Console.WriteLine($"Success Task result: {successTask.Result}");
                    }
                    
                    // Get all exceptions
                    var allExceptions = new List<Exception>();
                    if (failTask1.IsFaulted && failTask1.Exception != null)
                        allExceptions.AddRange(failTask1.Exception.InnerExceptions);
                    if (failTask2.IsFaulted && failTask2.Exception != null)
                        allExceptions.AddRange(failTask2.Exception.InnerExceptions);
                    
                    Console.WriteLine($"\nAll exceptions ({allExceptions.Count}):");
                    foreach (var exception in allExceptions)
                    {
                        Console.WriteLine($"- {exception.Message}");
                    }
                }
            }
            
            // Run the async exception demo and wait for it to complete
            WhenAllExceptionDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey points about Task.WhenAll:");
            ConsoleHelper.WriteInfo("- Returns a Task that completes when all input tasks complete");
            ConsoleHelper.WriteInfo("- Collects all results into an array if tasks return values");
            ConsoleHelper.WriteInfo("- Throws the first exception if any task fails");
            ConsoleHelper.WriteInfo("- You can still access individual task results and exceptions");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates Task.WhenAny combinator
        /// </summary>
        private void DemonstrateWhenAny()
        {
            ConsoleHelper.WriteSubheader("Task.WhenAny");
            
            Console.WriteLine("Task.WhenAny waits for any task to complete asynchronously:");
            
            // We need to use an async local function
            async Task WhenAnyDemoAsync()
            {
                Console.WriteLine("Starting multiple tasks...");
                
                // Create several tasks with different durations
                Task<string> task1 = SimulateWorkAsync("Task 1", 2000);
                Task<string> task2 = SimulateWorkAsync("Task 2", 1000); // This will complete first
                Task<string> task3 = SimulateWorkAsync("Task 3", 3000);
                
                var tasks = new[] { task1, task2, task3 };
                
                // Process tasks as they complete
                while (tasks.Any(t => !t.IsCompleted))
                {
                    // Wait for any task to complete
                    Task<string> completedTask = await Task.WhenAny(tasks.Where(t => !t.IsCompleted));
                    
                    // Get the result from the completed task
                    string result = await completedTask;
                    
                    Console.WriteLine($"\nTask completed: {result}");
                }
                
                Console.WriteLine("\nAll tasks have completed!");
            }
            
            // Run the async demo and wait for it to complete
            WhenAnyDemoAsync().GetAwaiter().GetResult();
            
            Console.WriteLine("\nTask.WhenAny for implementing timeouts:");
            
            async Task TimeoutDemoAsync()
            {
                // Create a task that takes a while to complete
                Task<string> longRunningTask = SimulateWorkAsync("Long Running Task", 5000);
                
                // Create a timeout task
                Task timeoutTask = Task.Delay(2000);
                
                // Wait for either the task to complete or the timeout to occur
                Task completedTask = await Task.WhenAny(longRunningTask, timeoutTask);
                
                if (completedTask == timeoutTask)
                {
                    Console.WriteLine("\nOperation timed out!");
                    
                    // We could cancel the long-running task here if it supports cancellation
                }
                else
                {
                    string result = await longRunningTask;
                    Console.WriteLine($"\nOperation completed before timeout: {result}");
                }
            }
            
            // Run the async timeout demo and wait for it to complete
            TimeoutDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey points about Task.WhenAny:");
            ConsoleHelper.WriteInfo("- Returns a Task that completes when any input task completes");
            ConsoleHelper.WriteInfo("- Useful for implementing timeouts and race conditions");
            ConsoleHelper.WriteInfo("- Can be used to process tasks as they complete");
            ConsoleHelper.WriteInfo("- Doesn't cancel other tasks when one completes");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates Task.WaitAll and Task.WaitAny methods
        /// </summary>
        private void DemonstrateWaitMethods()
        {
            ConsoleHelper.WriteSubheader("Task.WaitAll and Task.WaitAny");
            
            Console.WriteLine("Task.WaitAll and Task.WaitAny are synchronous (blocking) versions of WhenAll and WhenAny:");
            
            Console.WriteLine("\n1. Task.WaitAll - Blocks until all tasks complete:");
            
            // Create several tasks with different durations
            Task<string> task1 = SimulateWorkAsync("Task 1", 2000);
            Task<string> task2 = SimulateWorkAsync("Task 2", 1000);
            Task<string> task3 = SimulateWorkAsync("Task 3", 3000);
            
            Console.WriteLine("Waiting for all tasks to complete (blocking)...");
            
            // Wait for all tasks to complete (blocking)
            Task.WaitAll(task1, task2, task3);
            
            Console.WriteLine("\nAll tasks completed!");
            Console.WriteLine("Results:");
            Console.WriteLine($"- {task1.Result}");
            Console.WriteLine($"- {task2.Result}");
            Console.WriteLine($"- {task3.Result}");
            
            Console.WriteLine("\n2. Task.WaitAny - Blocks until any task completes:");
            
            // Create several tasks with different durations
            Task<string> taskA = SimulateWorkAsync("Task A", 2000);
            Task<string> taskB = SimulateWorkAsync("Task B", 1000); // This will complete first
            Task<string> taskC = SimulateWorkAsync("Task C", 3000);
            
            var tasks = new[] { taskA, taskB, taskC };
            
            Console.WriteLine("Waiting for any task to complete (blocking)...");
            
            // Wait for any task to complete (blocking)
            int completedIndex = Task.WaitAny(tasks);
            
            Console.WriteLine($"\nTask at index {completedIndex} completed first: {tasks[completedIndex].Result}");
            
            // Wait for remaining tasks
            Task.WaitAll(tasks);
            
            Console.WriteLine("All tasks have now completed");
            
            ConsoleHelper.WriteInfo("\nKey differences between Wait* and When* methods:");
            ConsoleHelper.WriteInfo("- Wait* methods block the current thread (synchronous)");
            ConsoleHelper.WriteInfo("- When* methods don't block and return a Task (asynchronous)");
            ConsoleHelper.WriteInfo("- Wait* methods should be avoided in async methods");
            ConsoleHelper.WriteInfo("- When* methods should be used with await in async methods");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates custom task combinators
        /// </summary>
        private void DemonstrateCustomCombinators()
        {
            ConsoleHelper.WriteSubheader("Custom Combinators");
            
            Console.WriteLine("You can create custom combinators for specific scenarios:");
            
            // We need to use an async local function
            async Task CustomCombinatorsDemoAsync()
            {
                Console.WriteLine("\n1. WhenAllOrFirstException - Completes when all complete or first exception:");
                
                // Create tasks where one will fail
                Task<string> task1 = SimulateWorkAsync("Task 1", 2000);
                Task<string> task2 = SimulateFailingWorkAsync("Task 2", 1000); // This will fail first
                Task<string> task3 = SimulateWorkAsync("Task 3", 3000);
                
                try
                {
                    var results = await WhenAllOrFirstExceptionAsync(task1, task2, task3);
                    Console.WriteLine("All tasks completed successfully");
                    foreach (var result in results)
                    {
                        Console.WriteLine($"- {result}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An exception occurred: {ex.Message}");
                }
                
                Console.WriteLine("\n2. WhenAtLeast - Completes when a minimum number of tasks complete:");
                
                // Create several tasks
                Task<string> taskA = SimulateWorkAsync("Task A", 1000);
                Task<string> taskB = SimulateWorkAsync("Task B", 2000);
                Task<string> taskC = SimulateWorkAsync("Task C", 3000);
                Task<string> taskD = SimulateWorkAsync("Task D", 4000);
                
                // Wait for at least 2 tasks to complete
                var firstTwoResults = await WhenAtLeastAsync(2, taskA, taskB, taskC, taskD);
                
                Console.WriteLine("\nAt least 2 tasks have completed:");
                foreach (var result in firstTwoResults)
                {
                    Console.WriteLine($"- {result}");
                }
                
                // Wait for remaining tasks
                await Task.WhenAll(taskA, taskB, taskC, taskD);
                Console.WriteLine("\nAll tasks have now completed");
            }
            
            // Run the async demo and wait for it to complete
            CustomCombinatorsDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nCustom combinators allow you to:");
            ConsoleHelper.WriteInfo("- Create specialized behavior for specific scenarios");
            ConsoleHelper.WriteInfo("- Combine existing combinators in new ways");
            ConsoleHelper.WriteInfo("- Implement more complex coordination patterns");
            
            ConsoleHelper.WaitForKey();
        }
        
        #region Helper Methods
        
        /// <summary>
        /// Simulates an asynchronous operation that returns a result
        /// </summary>
        private async Task<string> SimulateWorkAsync(string name, int delayMs)
        {
            Console.WriteLine($"  {name} started");
            await Task.Delay(delayMs);
            Console.WriteLine($"  {name} completed");
            return $"{name} result after {delayMs}ms";
        }
        
        /// <summary>
        /// Simulates an asynchronous operation that fails
        /// </summary>
        private async Task<string> SimulateFailingWorkAsync(string name, int delayMs)
        {
            Console.WriteLine($"  {name} started");
            await Task.Delay(delayMs);
            Console.WriteLine($"  {name} failed");
            throw new InvalidOperationException($"{name} failed after {delayMs}ms");
        }
        
        /// <summary>
        /// Custom combinator that completes when all tasks complete or the first exception occurs
        /// </summary>
        private async Task<IEnumerable<T>> WhenAllOrFirstExceptionAsync<T>(params Task<T>[] tasks)
        {
            var taskCompletionSource = new TaskCompletionSource<IEnumerable<T>>();
            
            // Count of completed tasks
            int remainingTasks = tasks.Length;
            var results = new T[tasks.Length];
            
            // For each task
            for (int i = 0; i < tasks.Length; i++)
            {
                int index = i; // Capture the index for the lambda
                
                // Add a continuation to each task
                tasks[i].ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        // If any task fails, complete the TCS with the exception
                        taskCompletionSource.TrySetException(t.Exception.InnerExceptions);
                    }
                    else if (t.IsCompleted)
                    {
                        // Store the result
                        results[index] = t.Result;
                        
                        // Decrement the counter
                        if (Interlocked.Decrement(ref remainingTasks) == 0)
                        {
                            // If all tasks completed successfully, complete the TCS with the results
                            taskCompletionSource.TrySetResult(results);
                        }
                    }
                });
            }
            
            return await taskCompletionSource.Task;
        }
        
        /// <summary>
        /// Custom combinator that completes when at least N tasks complete
        /// </summary>
        private async Task<IEnumerable<T>> WhenAtLeastAsync<T>(int count, params Task<T>[] tasks)
        {
            if (count > tasks.Length)
                throw new ArgumentException("Count cannot be greater than the number of tasks", nameof(count));
            
            var results = new List<T>(count);
            var completedTasks = 0;
            var tcs = new TaskCompletionSource<IEnumerable<T>>();
            
            foreach (var task in tasks)
            {
                task.ContinueWith(t =>
                {
                    if (t.IsCompletedSuccessfully)
                    {
                        lock (results)
                        {
                            results.Add(t.Result);
                            completedTasks++;
                            
                            if (completedTasks >= count)
                            {
                                tcs.TrySetResult(results);
                            }
                        }
                    }
                });
            }
            
            return await tcs.Task;
        }
        
        #endregion
    }
}
