using System;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates exception propagation in asynchronous programming
    /// </summary>
    public class ExceptionPropagationDemo
    {
        /// <summary>
        /// Runs the Exception Propagation demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Exception Propagation", "تصاعد الاحداث الاستثنائية");
            
            ConsoleHelper.WriteInfo("This demo shows how exceptions are propagated in asynchronous code.");
            ConsoleHelper.WriteInfo("Understanding exception handling is crucial for robust async applications.\n");
            
            // Exception in Task.Run
            DemonstrateTaskRunException();
            
            // Exception in async method
            DemonstrateAsyncMethodException();
            
            // Multiple exceptions
            DemonstrateMultipleExceptions();
            
            // Exception handling strategies
            DemonstrateExceptionHandlingStrategies();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Exception Propagation");
            Console.WriteLine("Key points about exception propagation in async code:");
            Console.WriteLine("✓ Exceptions in tasks are wrapped in AggregateException when using .Wait() or .Result");
            Console.WriteLine("✓ When using await, exceptions are unwrapped and thrown directly");
            Console.WriteLine("✓ Always handle exceptions in async code to prevent unobserved exceptions");
            Console.WriteLine("✓ Use try/catch around await or Task.Wait() to catch exceptions");
            Console.WriteLine("✓ Consider using Task.WhenAll for handling exceptions from multiple tasks");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates exception propagation in Task.Run
        /// </summary>
        private void DemonstrateTaskRunException()
        {
            ConsoleHelper.WriteSubheader("Exception in Task.Run");
            
            Console.WriteLine("Creating a task that will throw an exception...");
            
            Task faultedTask = Task.Run(() =>
            {
                Console.WriteLine("Task started");
                System.Threading.Thread.Sleep(1000);
                
                // Throw an exception
                throw new InvalidOperationException("Something went wrong in the task!");
            });
            
            Console.WriteLine($"Task status: {faultedTask.Status}");
            Console.WriteLine("Waiting for task completion...");
            
            try
            {
                // This will throw an AggregateException
                faultedTask.Wait();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("\nCaught AggregateException:");
                foreach (var innerException in ae.InnerExceptions)
                {
                    Console.WriteLine($"- {innerException.GetType().Name}: {innerException.Message}");
                }
            }
            
            Console.WriteLine($"\nTask status after exception: {faultedTask.Status}");
            Console.WriteLine($"Task.IsFaulted: {faultedTask.IsFaulted}");
            Console.WriteLine($"Task.Exception type: {faultedTask.Exception?.GetType().Name}");
            
            ConsoleHelper.WriteInfo("\nNote: When using Task.Wait() or Task.Result, exceptions are wrapped");
            ConsoleHelper.WriteInfo("in an AggregateException, which may contain multiple inner exceptions.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates exception propagation in async methods
        /// </summary>
        private async void DemonstrateAsyncMethodException()
        {
            ConsoleHelper.WriteSubheader("Exception in Async Method");
            
            Console.WriteLine("Calling an async method that will throw an exception...");
            
            try
            {
                // Call the async method and await it
                string result = await GetDataAsync(true);
                Console.WriteLine($"Result: {result}"); // This line won't execute if exception occurs
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nCaught exception directly: {ex.GetType().Name}");
                Console.WriteLine($"Exception message: {ex.Message}");
            }
            
            ConsoleHelper.WriteInfo("\nNote: When using 'await', exceptions are unwrapped and thrown");
            ConsoleHelper.WriteInfo("directly, allowing you to catch the specific exception type.");
            
            // Now demonstrate unobserved exception
            Console.WriteLine("\nDemonstrating unobserved exception (fire and forget):");
            
            // Create a task that will throw but not be awaited or have its exception observed
            Task unobservedTask = GetDataAsync(true);
            
            Console.WriteLine("Task started but not awaited. Exception will be unobserved.");
            System.Threading.Thread.Sleep(2000); // Give the task time to complete and throw
            
            ConsoleHelper.WriteWarning("\nUnobserved exceptions can cause process termination in some scenarios.");
            ConsoleHelper.WriteWarning("Always handle exceptions in async code or observe the task's exception.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates handling multiple exceptions from parallel tasks
        /// </summary>
        private void DemonstrateMultipleExceptions()
        {
            ConsoleHelper.WriteSubheader("Multiple Exceptions");
            
            Console.WriteLine("Creating multiple tasks that will throw different exceptions...");
            
            Task task1 = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(500);
                throw new InvalidOperationException("Error in task 1");
            });
            
            Task task2 = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(700);
                throw new ArgumentException("Error in task 2");
            });
            
            Task task3 = Task.Run(() =>
            {
                System.Threading.Thread.Sleep(300);
                throw new NullReferenceException("Error in task 3");
            });
            
            Console.WriteLine("Waiting for all tasks to complete...");
            
            try
            {
                Task.WaitAll(task1, task2, task3);
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"\nCaught AggregateException with {ae.InnerExceptions.Count} inner exceptions:");
                
                foreach (var innerException in ae.InnerExceptions)
                {
                    Console.WriteLine($"- {innerException.GetType().Name}: {innerException.Message}");
                }
            }
            
            ConsoleHelper.WriteInfo("\nTask.WaitAll collects all exceptions from all tasks and");
            ConsoleHelper.WriteInfo("combines them into a single AggregateException.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates different strategies for handling exceptions in async code
        /// </summary>
        private async void DemonstrateExceptionHandlingStrategies()
        {
            ConsoleHelper.WriteSubheader("Exception Handling Strategies");
            
            // Strategy 1: Try-catch around individual await
            Console.WriteLine("Strategy 1: Try-catch around individual await");
            
            try
            {
                await Task.Run(() =>
                {
                    throw new InvalidOperationException("Error in strategy 1");
                });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Caught exception: {ex.Message}");
            }
            
            // Strategy 2: Try-catch in the async method itself
            Console.WriteLine("\nStrategy 2: Try-catch in the async method itself");
            
            string result = await GetDataWithErrorHandlingAsync();
            Console.WriteLine($"Result with internal error handling: {result}");
            
            // Strategy 3: Continuation with error handling
            Console.WriteLine("\nStrategy 3: Continuation with error handling");
            Task<string> task = Task.FromException<string>(new InvalidOperationException("Error in strategy 3"));


            Task continuation = task.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    Console.WriteLine($"Task faulted: {t.Exception.InnerException.Message}");
                }
                else
                {
                    Console.WriteLine($"Task succeeded: {t.Result}");
                }
            });
            
            await continuation;
            
            Console.WriteLine("\nBest practices for exception handling in async code:");
            Console.WriteLine("1. Always handle exceptions in async methods");
            Console.WriteLine("2. Use try/catch around await expressions");
            Console.WriteLine("3. Consider using Task.ContinueWith for error handling in task chains");
            Console.WriteLine("4. Use Task.WhenAll for handling exceptions from multiple tasks");
            Console.WriteLine("5. Avoid 'fire and forget' tasks without exception handling");
            
            ConsoleHelper.WaitForKey();
        }
        
        #region Helper Methods
        
        /// <summary>
        /// Async method that may throw an exception
        /// </summary>
        private async Task<string> GetDataAsync(bool shouldThrow)
        {
            await Task.Delay(1000);
            
            if (shouldThrow)
            {
                throw new InvalidOperationException("Error occurred while getting data!");
            }
            
            return "Data retrieved successfully";
        }
        
        /// <summary>
        /// Async method with internal error handling
        /// </summary>
        private async Task<string> GetDataWithErrorHandlingAsync()
        {
            try
            {
                await Task.Delay(1000);
                
                // Simulate an error
                throw new InvalidOperationException("Internal error occurred!");
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error logged: {ex.Message}");
                
                // Return a fallback value
                return "Fallback data (error handled internally)";
            }
        }
        
        #endregion
    }
}
