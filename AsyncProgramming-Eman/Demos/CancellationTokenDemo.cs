using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates cancellation token usage in asynchronous programming
    /// </summary>
    public class CancellationTokenDemo
    {
        /// <summary>
        /// Runs the Cancellation Token demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Cancellation Token", "رمز الالغاء");
            
            ConsoleHelper.WriteInfo("This demo shows how to use cancellation tokens to cancel asynchronous operations.");
            ConsoleHelper.WriteInfo("Cancellation tokens provide a standardized way to cancel tasks cooperatively.\n");
            
            // Basic cancellation
            DemonstrateBasicCancellation();
            
            // Cancellation propagation
            DemonstrateCancellationPropagation();
            
            // Cancellation with timeout
            DemonstrateCancellationWithTimeout();
            
            // Cancellation registration
            DemonstrateCancellationRegistration();
            
            // Linked cancellation tokens
            DemonstrateLinkedCancellationTokens();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Cancellation Tokens");
            Console.WriteLine("Key points about cancellation tokens:");
            Console.WriteLine("✓ CancellationTokenSource creates and controls cancellation tokens");
            Console.WriteLine("✓ Pass CancellationToken to async methods to support cancellation");
            Console.WriteLine("✓ Check token.IsCancellationRequested or call token.ThrowIfCancellationRequested()");
            Console.WriteLine("✓ Cancellation is cooperative - the operation must check for cancellation");
            Console.WriteLine("✓ Use linked tokens to combine multiple cancellation sources");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates basic cancellation token usage
        /// </summary>
        private void DemonstrateBasicCancellation()
        {
            ConsoleHelper.WriteSubheader("Basic Cancellation");
            
            Console.WriteLine("Creating a cancellable task...");
            
            // Create a cancellation token source
            CancellationTokenSource cts = new CancellationTokenSource();
            
            // Start a task that can be cancelled
            Task task = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        // Check if cancellation was requested
                        if (cts.Token.IsCancellationRequested)
                        {
                            Console.WriteLine("Cancellation detected. Stopping work.");
                            break; // Cooperative cancellation
                        }
                        
                        Console.WriteLine($"Working... {i + 1}/10");
                        Thread.Sleep(500);
                    }
                    
                    Console.WriteLine("Task completed normally");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task was cancelled via exception");
                }
            }, cts.Token);
            
            // Wait a bit then cancel the task
            Thread.Sleep(2000);
            Console.WriteLine("\nCancelling the task...");
            cts.Cancel();
            
            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException is OperationCanceledException)
                {
                    Console.WriteLine("Task was cancelled (caught from Wait)");
                }
            }
            
            Console.WriteLine($"Task status: {task.Status}");
            
            ConsoleHelper.WriteInfo("\nThere are two ways to handle cancellation:");
            ConsoleHelper.WriteInfo("1. Check token.IsCancellationRequested and exit gracefully");
            ConsoleHelper.WriteInfo("2. Call token.ThrowIfCancellationRequested() to throw OperationCanceledException");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates cancellation token propagation through async methods
        /// </summary>
        private void DemonstrateCancellationPropagation()
        {
            ConsoleHelper.WriteSubheader("Cancellation Propagation");
            
            Console.WriteLine("Demonstrating cancellation propagation through async methods...");
            
            // Create a cancellation token source
            CancellationTokenSource cts = new CancellationTokenSource();
            
            // We need to use an async local function
            async Task PropagationDemoAsync()
            {
                try
                {
                    Console.WriteLine("Starting operation with nested async calls...");
                    
                    // Call an async method that accepts a cancellation token
                    await DoWorkAsync(cts.Token);
                    
                    Console.WriteLine("Operation completed successfully");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Operation was cancelled");
                }
            }
            
            // Start the async operation
            Task demoTask = PropagationDemoAsync();
            
            // Wait a bit then cancel
            Thread.Sleep(2000);
            Console.WriteLine("\nCancelling the operation...");
            cts.Cancel();
            
            // Wait for the demo to complete
            try
            {
                demoTask.Wait();
            }
            catch (AggregateException)
            {
                // Ignore exceptions
            }
            
            ConsoleHelper.WriteInfo("\nCancellation tokens should be passed through the call stack");
            ConsoleHelper.WriteInfo("to allow cancellation at any level of nested async operations.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates cancellation with timeout
        /// </summary>
        private void DemonstrateCancellationWithTimeout()
        {
            ConsoleHelper.WriteSubheader("Cancellation with Timeout");
            
            Console.WriteLine("Demonstrating automatic cancellation after a timeout...");
            
            // Create a cancellation token source with timeout
            CancellationTokenSource cts = new CancellationTokenSource(3000); // 3 second timeout
            
            Console.WriteLine("Starting a long-running operation with a 3-second timeout...");
            
            // Start a task that will run longer than the timeout
            Task task = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        // Check for cancellation
                        cts.Token.ThrowIfCancellationRequested();
                        
                        Console.WriteLine($"Working... {i + 1}/10");
                        Thread.Sleep(500);
                    }
                    
                    Console.WriteLine("Task completed successfully");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task was cancelled due to timeout");
                }
            }, cts.Token);
            
            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                if (ae.InnerException is OperationCanceledException)
                {
                    Console.WriteLine("Task was cancelled (caught from Wait)");
                }
            }
            
            Console.WriteLine("\nAlternative approach using Task.Delay for timeout:");
            
            // We need to use an async local function
            async Task TimeoutDemoAsync()
            {
                // Create a new cancellation token source
                using (CancellationTokenSource timeoutCts = new CancellationTokenSource())
                {
                    try
                    {
                        // Create a task representing the operation
                        Task operationTask = Task.Run(async () =>
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                // Check for cancellation
                                timeoutCts.Token.ThrowIfCancellationRequested();
                                
                                Console.WriteLine($"Working... {i + 1}/10");
                                await Task.Delay(500, timeoutCts.Token);
                            }
                        }, timeoutCts.Token);
                        
                        // Create a task representing the timeout
                        Task timeoutTask = Task.Delay(3000);
                        
                        // Wait for either the operation or the timeout to complete
                        Task completedTask = await Task.WhenAny(operationTask, timeoutTask);
                        
                        if (completedTask == timeoutTask)
                        {
                            // Timeout occurred
                            Console.WriteLine("\nOperation timed out!");
                            timeoutCts.Cancel();
                            
                            try
                            {
                                await operationTask; // Observe any exceptions
                            }
                            catch (OperationCanceledException)
                            {
                                Console.WriteLine("Operation was cancelled due to timeout");
                            }
                        }
                        else
                        {
                            // Operation completed before timeout
                            Console.WriteLine("\nOperation completed successfully before timeout");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
            
            // Run the async demo and wait for it to complete
            TimeoutDemoAsync().GetAwaiter().GetResult();
            
            ConsoleHelper.WriteInfo("\nTwo ways to implement timeouts:");
            ConsoleHelper.WriteInfo("1. CancellationTokenSource with timeout parameter");
            ConsoleHelper.WriteInfo("2. Task.WhenAny with Task.Delay for more control");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates cancellation token registration
        /// </summary>
        private void DemonstrateCancellationRegistration()
        {
            ConsoleHelper.WriteSubheader("Cancellation Registration");
            
            Console.WriteLine("Demonstrating cancellation token registration for cleanup...");
            
            // Create a cancellation token source
            CancellationTokenSource cts = new CancellationTokenSource();
            
            // Register actions to execute when cancellation is requested
            CancellationToken token = cts.Token;
            
            // Register multiple callbacks
            token.Register(() => Console.WriteLine("Cancellation callback 1: Cleaning up resources"));
            token.Register(() => Console.WriteLine("Cancellation callback 2: Logging cancellation"));
            token.Register(() => Console.WriteLine("Cancellation callback 3: Notifying other components"));
            
            // Start a task that can be cancelled
            Task task = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        // Check for cancellation
                        token.ThrowIfCancellationRequested();
                        
                        Console.WriteLine($"Working... {i + 1}/10");
                        Thread.Sleep(500);
                    }
                    
                    Console.WriteLine("Task completed successfully");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task was cancelled");
                }
            }, token);
            
            // Wait a bit then cancel
            Thread.Sleep(2000);
            Console.WriteLine("\nCancelling the operation...");
            cts.Cancel();
            
            // Wait for the task to complete
            try
            {
                task.Wait();
            }
            catch (AggregateException)
            {
                // Ignore exceptions
            }
            
            ConsoleHelper.WriteInfo("\nCancellation token registration allows you to:");
            ConsoleHelper.WriteInfo("- Execute cleanup code when cancellation is requested");
            ConsoleHelper.WriteInfo("- Release resources and perform necessary cleanup");
            ConsoleHelper.WriteInfo("- Notify other components about the cancellation");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates linked cancellation tokens
        /// </summary>
        private void DemonstrateLinkedCancellationTokens()
        {
            ConsoleHelper.WriteSubheader("Linked Cancellation Tokens");
            
            Console.WriteLine("Demonstrating linked cancellation tokens...");
            
            // Create two independent cancellation token sources
            CancellationTokenSource timeoutCts = new CancellationTokenSource();
            CancellationTokenSource userCts = new CancellationTokenSource();
            
            // Create a linked token source that combines both
            CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                timeoutCts.Token, userCts.Token);
            
            // Start a task with the linked token
            Task task = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        // Check for cancellation from either source
                        linkedCts.Token.ThrowIfCancellationRequested();
                        
                        Console.WriteLine($"Working... {i + 1}/10");
                        Thread.Sleep(500);
                    }
                    
                    Console.WriteLine("Task completed successfully");
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task was cancelled");
                }
            }, linkedCts.Token);
            
            // Schedule a timeout cancellation
            Task.Delay(2000).ContinueWith(_ =>
            {
                Console.WriteLine("\nTimeout occurred, cancelling via timeout source...");
                timeoutCts.Cancel();
                
                Console.WriteLine($"Linked token IsCancellationRequested: {linkedCts.Token.IsCancellationRequested}");
                Console.WriteLine($"User token IsCancellationRequested: {userCts.Token.IsCancellationRequested}");
            });
            
            // Wait for the task to complete
            try
            {
                task.Wait();
            }
            catch (AggregateException)
            {
                // Ignore exceptions
            }
            
            // Clean up
            linkedCts.Dispose();
            timeoutCts.Dispose();
            userCts.Dispose();
            
            ConsoleHelper.WriteInfo("\nLinked cancellation tokens allow you to:");
            ConsoleHelper.WriteInfo("- Combine multiple cancellation sources (user cancel, timeout, etc.)");
            ConsoleHelper.WriteInfo("- Cancel an operation if any of the source tokens are cancelled");
            ConsoleHelper.WriteInfo("- Create more complex cancellation scenarios");
            
            ConsoleHelper.WaitForKey();
        }
        
        #region Helper Methods
        
        /// <summary>
        /// Simulates a multi-level async operation that supports cancellation
        /// </summary>
        private async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("DoWorkAsync: Starting first level operation");
            
            // First level work
            await Task.Delay(500, cancellationToken);
            
            // Check for cancellation
            cancellationToken.ThrowIfCancellationRequested();
            
            // Call nested async method
            await DoNestedWorkAsync(cancellationToken);
            
            Console.WriteLine("DoWorkAsync: Completed");
        }
        
        /// <summary>
        /// Simulates a nested async operation that supports cancellation
        /// </summary>
        private async Task DoNestedWorkAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("DoNestedWorkAsync: Starting second level operation");
            
            // Second level work
            for (int i = 0; i < 5; i++)
            {
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();
                
                Console.WriteLine($"DoNestedWorkAsync: Working... {i + 1}/5");
                await Task.Delay(500, cancellationToken);
            }
            
            Console.WriteLine("DoNestedWorkAsync: Completed");
        }
        
        #endregion
    }
}
