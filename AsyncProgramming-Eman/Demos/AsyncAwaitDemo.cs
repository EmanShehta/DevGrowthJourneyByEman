using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates asynchronous functions using async and await keywords
    /// </summary>
    public class AsyncAwaitDemo
    {
        /// <summary>
        /// Runs the Asynchronous Function demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Asynchronous Function (async, await)", "دوال التزامن");
            
            ConsoleHelper.WriteInfo("This demo shows how to use async and await keywords for asynchronous programming.");
            ConsoleHelper.WriteInfo("These keywords simplify writing and understanding asynchronous code.\n");
            
            // Basic async/await
            DemonstrateBasicAsyncAwait();
            
            // Return values from async methods
            DemonstrateAsyncReturnValues();
            
            // Error handling in async methods
            DemonstrateAsyncErrorHandling();
            
            // Async method composition
            DemonstrateAsyncComposition();
            
            // Async lambdas and anonymous methods
            DemonstrateAsyncLambdas();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Async and Await");
            Console.WriteLine("Key points about async and await:");
            Console.WriteLine("✓ async marks a method as asynchronous");
            Console.WriteLine("✓ await suspends execution until the awaited task completes");
            Console.WriteLine("✓ await unwraps Task<T> results and exceptions");
            Console.WriteLine("✓ async methods can be composed to create complex asynchronous workflows");
            Console.WriteLine("✓ async/await simplifies asynchronous programming compared to raw tasks");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates basic async/await usage
        /// </summary>
        private void DemonstrateBasicAsyncAwait()
        {
            ConsoleHelper.WriteSubheader("Basic Async/Await");
            
            Console.WriteLine("The async and await keywords simplify asynchronous programming:");
            
            // We need to use an async local function
            async Task BasicDemoAsync()
            {
                Console.WriteLine("1. Starting asynchronous operation");
                
                // Await an asynchronous operation
                await Task.Delay(1500);
                
                Console.WriteLine("2. Asynchronous operation completed");
                
                // Do more work
                Console.WriteLine("3. Continuing with more work");
                
                // Await another asynchronous operation
                await Task.Delay(1000);
                
                Console.WriteLine("4. Second asynchronous operation completed");
            }
            
            // Run the async demo and wait for it to complete
            Console.WriteLine("Running basic async/await demo...\n");
            BasicDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey points about basic async/await:");
            ConsoleHelper.WriteInfo("- The 'async' keyword marks a method as asynchronous");
            ConsoleHelper.WriteInfo("- The 'await' keyword suspends execution until the awaited task completes");
            ConsoleHelper.WriteInfo("- Code after 'await' runs as a continuation when the task completes");
            ConsoleHelper.WriteInfo("- The method appears to execute sequentially, but is actually asynchronous");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates returning values from async methods
        /// </summary>
        private void DemonstrateAsyncReturnValues()
        {
            ConsoleHelper.WriteSubheader("Async Return Values");
            
            Console.WriteLine("Async methods can return values using Task<T>:");
            
            // We need to use an async local function
            async Task ReturnValuesDemoAsync()
            {
                Console.WriteLine("Calling async methods that return values...\n");
                
                // Call an async method that returns a string
                string message = await GetMessageAsync("World");
                Console.WriteLine($"Received message: {message}");
                
                // Call an async method that returns an int
                int number = await CalculateValueAsync(10);
                Console.WriteLine($"Calculated value: {number}");
                
                // Call an async method that returns a complex type
                var userData = await GetUserDataAsync(123);
                Console.WriteLine($"User data: ID={userData.Id}, Name={userData.Name}, Age={userData.Age}");
            }
            
            // Run the async demo and wait for it to complete
            ReturnValuesDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey points about async return values:");
            ConsoleHelper.WriteInfo("- Async methods return Task<T> where T is the return type");
            ConsoleHelper.WriteInfo("- The 'await' keyword unwraps the Task<T> to give you the T value");
            ConsoleHelper.WriteInfo("- You can return any type from an async method, including complex types");
            ConsoleHelper.WriteInfo("- The compiler handles the creation of the Task<T> for you");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates error handling in async methods
        /// </summary>
        private void DemonstrateAsyncErrorHandling()
        {
            ConsoleHelper.WriteSubheader("Async Error Handling");
            
            Console.WriteLine("Error handling in async methods works similarly to synchronous code:");
            
            // We need to use an async local function
            async Task ErrorHandlingDemoAsync()
            {
                // Try-catch with await
                try
                {
                    Console.WriteLine("1. Calling an async method that will throw an exception");
                    await ThrowExceptionAsync();
                    Console.WriteLine("This line won't execute");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"2. Caught exception: {ex.Message}");
                }
                
                // Multiple awaits in try-catch
                try
                {
                    Console.WriteLine("\n3. Calling multiple async methods in a try block");
                    await Task.Delay(500);
                    Console.WriteLine("4. First await completed successfully");
                    await ThrowExceptionAsync();
                    Console.WriteLine("This line won't execute");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"5. Caught exception from second await: {ex.Message}");
                }
                
                // Exception in called method
                try
                {
                    Console.WriteLine("\n6. Calling a method that catches exceptions internally");
                    string result = await MethodWithInternalTryCatchAsync();
                    Console.WriteLine($"7. Method returned: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"This won't execute because the exception is handled internally: {ex.Message}");
                }
            }
            
            // Run the async demo and wait for it to complete
            ErrorHandlingDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey points about async error handling:");
            ConsoleHelper.WriteInfo("- Exceptions in awaited tasks are automatically unwrapped");
            ConsoleHelper.WriteInfo("- You can use try-catch blocks around await expressions");
            ConsoleHelper.WriteInfo("- Exceptions propagate naturally through async method calls");
            ConsoleHelper.WriteInfo("- Error handling is much simpler than with raw Task continuations");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates composing async methods
        /// </summary>
        private void DemonstrateAsyncComposition()
        {
            ConsoleHelper.WriteSubheader("Async Method Composition");
            
            Console.WriteLine("Async methods can be composed to create complex workflows:");
            
            // We need to use an async local function
            async Task CompositionDemoAsync()
            {
                Console.WriteLine("Starting a complex workflow with multiple async operations...\n");
                
                // Call a method that composes multiple async operations
                string result = await ProcessWorkflowAsync(42);
                
                Console.WriteLine($"\nWorkflow completed with result: {result}");
            }
            
            // Run the async demo and wait for it to complete
            CompositionDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey points about async composition:");
            ConsoleHelper.WriteInfo("- Async methods can call other async methods");
            ConsoleHelper.WriteInfo("- This creates a natural composition of asynchronous operations");
            ConsoleHelper.WriteInfo("- The code reads sequentially even though it's asynchronous");
            ConsoleHelper.WriteInfo("- This makes complex asynchronous workflows much easier to understand");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates async lambdas and anonymous methods
        /// </summary>
        private void DemonstrateAsyncLambdas()
        {
            ConsoleHelper.WriteSubheader("Async Lambdas and Anonymous Methods");
            
            Console.WriteLine("You can use async with lambda expressions and anonymous methods:");
            
            // We need to use an async local function
            async Task LambdaDemoAsync()
            {
                // Async lambda with Task.Run
                Console.WriteLine("1. Using async lambda with Task.Run");
                
                Task<int> task = Task.Run(async () =>
                {
                    Console.WriteLine("   Async lambda is running");
                    await Task.Delay(1000);
                    Console.WriteLine("   Async lambda is completing");
                    return 42;
                });
                
                int result = await task;
                Console.WriteLine($"   Result from async lambda: {result}\n");
                
                // Async event handler (simulated)
                Console.WriteLine("2. Async event handler (simulated)");
                
                Func<Task> asyncEventHandler = async () =>
                {
                    Console.WriteLine("   Async event handler is running");
                    await Task.Delay(1000);
                    Console.WriteLine("   Async event handler completed");
                };
                
                await asyncEventHandler();
                
                // LINQ with async
                Console.WriteLine("\n3. LINQ with async (simulated)");
                
                var items = new[] { 1, 2, 3 };
                
                foreach (var item in items)
                {
                    await Task.Run(async () =>
                    {
                        Console.WriteLine($"   Processing item {item}");
                        await Task.Delay(500);
                        Console.WriteLine($"   Finished processing item {item}");
                    });
                }
            }
            
            // Run the async demo and wait for it to complete
            LambdaDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nKey points about async lambdas:");
            ConsoleHelper.WriteInfo("- Lambda expressions can be marked as async");
            ConsoleHelper.WriteInfo("- Async lambdas can use await inside them");
            ConsoleHelper.WriteInfo("- Useful for event handlers, callbacks, and LINQ operations");
            ConsoleHelper.WriteInfo("- Makes asynchronous programming more flexible");
            
            ConsoleHelper.WaitForKey();
        }
        
        #region Helper Methods
        
        /// <summary>
        /// Async method that returns a string after a delay
        /// </summary>
        private async Task<string> GetMessageAsync(string name)
        {
            await Task.Delay(1000);
            return $"Hello, {name}! The time is {DateTime.Now:HH:mm:ss}";
        }
        
        /// <summary>
        /// Async method that calculates a value after a delay
        /// </summary>
        private async Task<int> CalculateValueAsync(int input)
        {
            await Task.Delay(800);
            return input * 2;
        }
        
        /// <summary>
        /// Async method that returns a complex type after a delay
        /// </summary>
        private async Task<UserData> GetUserDataAsync(int userId)
        {
            await Task.Delay(1200);
            return new UserData
            {
                Id = userId,
                Name = "John Doe",
                Age = 30
            };
        }
        
        /// <summary>
        /// Async method that throws an exception
        /// </summary>
        private async Task ThrowExceptionAsync()
        {
            await Task.Delay(500);
            throw new InvalidOperationException("This is a simulated error");
        }
        
        /// <summary>
        /// Async method with internal try-catch
        /// </summary>
        private async Task<string> MethodWithInternalTryCatchAsync()
        {
            try
            {
                await Task.Delay(500);
                throw new InvalidOperationException("Internal error occurred");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Exception caught internally: {ex.Message}");
                return "Fallback value after error";
            }
        }
        
        /// <summary>
        /// Async method that composes multiple async operations
        /// </summary>
        private async Task<string> ProcessWorkflowAsync(int input)
        {
            // Step 1: Get user data
            Console.WriteLine("Step 1: Getting user data");
            var userData = await GetUserDataAsync(input);
            Console.WriteLine($"   User data retrieved: {userData.Name}");
            
            // Step 2: Calculate a value based on user data
            Console.WriteLine("\nStep 2: Calculating value");
            int calculatedValue = await CalculateValueAsync(userData.Age);
            Console.WriteLine($"   Calculated value: {calculatedValue}");
            
            // Step 3: Format a message with the results
            Console.WriteLine("\nStep 3: Formatting message");
            string message = await GetMessageAsync(userData.Name);
            
            // Combine all results
            return $"{message} | User ID: {userData.Id} | Calculated Value: {calculatedValue}";
        }
        
        /// <summary>
        /// Simple class to represent user data
        /// </summary>
        private class UserData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
        
        #endregion
    }
}
