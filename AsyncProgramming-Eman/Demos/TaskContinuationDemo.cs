using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates task continuation in asynchronous programming
    /// </summary>
    public class TaskContinuationDemo
    {
        /// <summary>
        /// Runs the Task Continuation demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Task Continuation", "عندما تكتمل المهمة");
            
            ConsoleHelper.WriteInfo("This demo shows how to use task continuations to chain operations.");
            ConsoleHelper.WriteInfo("Task continuations allow you to specify what happens after a task completes.\n");
            
            // Basic continuation
            DemonstrateBasicContinuation();
            
            // Continuation with result
            DemonstrateContinuationWithResult();
            
            // Conditional continuations
            DemonstrateConditionalContinuations();
            
            // Multiple continuations
            DemonstrateMultipleContinuations();
            
            // Continuation options
            DemonstrateContinuationOptions();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Task Continuation");
            Console.WriteLine("Key points about task continuations:");
            Console.WriteLine("✓ Use ContinueWith to specify what happens after a task completes");
            Console.WriteLine("✓ Continuations can access the result of the previous task");
            Console.WriteLine("✓ Conditional continuations run only when specific conditions are met");
            Console.WriteLine("✓ Multiple continuations can be chained together");
            Console.WriteLine("✓ TaskContinuationOptions provide control over when and how continuations run");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates basic task continuation
        /// </summary>
        private void DemonstrateBasicContinuation()
        {
            ConsoleHelper.WriteSubheader("Basic Continuation");
            
            Console.WriteLine("Creating a task with a simple continuation...");
            
            Task firstTask = Task.Run(() =>
            {
                Console.WriteLine("First task is running...");
                Thread.Sleep(1000);
                Console.WriteLine("First task completed");
            });
            
            Task continuationTask = firstTask.ContinueWith(antecedent =>
            {
                Console.WriteLine("Continuation task is running...");
                Thread.Sleep(1000);
                Console.WriteLine("Continuation task completed");
            });
            
            Console.WriteLine("Waiting for all tasks to complete...");
            continuationTask.Wait();
            
            ConsoleHelper.WriteInfo("\nThe continuation task automatically starts when the first task completes.");
            ConsoleHelper.WriteInfo("This allows you to chain operations without blocking the current thread.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates continuation with result passing
        /// </summary>
        private void DemonstrateContinuationWithResult()
        {
            ConsoleHelper.WriteSubheader("Continuation with Result");
            
            Console.WriteLine("Creating a task that returns a result and passes it to a continuation...");
            
            Task<int> calculationTask = Task.Run(() =>
            {
                Console.WriteLine("Calculating a value...");
                Thread.Sleep(1000);
                int result = 42;
                Console.WriteLine($"Calculation result: {result}");
                return result;
            });
            
            Task<string> formattingTask = calculationTask.ContinueWith(antecedent =>
            {
                int value = antecedent.Result; // Gets the result from the previous task
                Console.WriteLine($"Formatting the value: {value}");
                Thread.Sleep(1000);
                string formatted = $"The answer is {value}";
                Console.WriteLine($"Formatting result: {formatted}");
                return formatted;
            });
            
            Console.WriteLine("Waiting for the task chain to complete...");
            string finalResult = formattingTask.Result;
            
            Console.WriteLine($"\nFinal result: {finalResult}");
            
            ConsoleHelper.WriteInfo("\nThe continuation task can access the result of the previous task");
            ConsoleHelper.WriteInfo("through the antecedent parameter's Result property.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates conditional continuations
        /// </summary>
        private void DemonstrateConditionalContinuations()
        {
            ConsoleHelper.WriteSubheader("Conditional Continuations");
            
            Console.WriteLine("Creating a task with different continuations based on outcome...");
            
            Task<bool> operationTask = Task.Run(() =>
            {
                Console.WriteLine("Performing an operation that might succeed or fail...");
                Thread.Sleep(1000);
                
                // Randomly succeed or fail
                Random random = new Random();
                bool success = random.Next(2) == 1;
                
                if (success)
                {
                    Console.WriteLine("Operation succeeded");
                }
                else
                {
                    Console.WriteLine("Operation failed");
                    throw new InvalidOperationException("Simulated failure");
                }
                
                return success;
            });
            
            // Continuation that runs only if the antecedent task completed successfully
            Task successContinuation = operationTask.ContinueWith(antecedent =>
            {
                Console.WriteLine("Success continuation is running...");
                Thread.Sleep(1000);
                Console.WriteLine("Success continuation completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            
            // Continuation that runs only if the antecedent task faulted
            Task failureContinuation = operationTask.ContinueWith(antecedent =>
            {
                Console.WriteLine("Failure continuation is running...");
                Console.WriteLine($"Error: {antecedent.Exception.InnerException.Message}");
                Thread.Sleep(1000);
                Console.WriteLine("Failure continuation completed");
            }, TaskContinuationOptions.OnlyOnFaulted);
            
            Console.WriteLine("Waiting for the task and its continuations to complete...");
            
            try
            {
                Task.WaitAll(successContinuation, failureContinuation);
            }
            catch (AggregateException)
            {
                // Ignore exceptions from the tasks
            }
            
            ConsoleHelper.WriteInfo("\nConditional continuations allow you to specify different actions");
            ConsoleHelper.WriteInfo("based on how the antecedent task completed (success, failure, cancellation).");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates multiple continuations in a chain
        /// </summary>
        private void DemonstrateMultipleContinuations()
        {
            ConsoleHelper.WriteSubheader("Multiple Continuations");
            
            Console.WriteLine("Creating a chain of multiple continuations...");
            
            Task<int> initialTask = Task.Run(() =>
            {
                Console.WriteLine("Initial task: Starting");
                Thread.Sleep(1000);
                Console.WriteLine("Initial task: Completed");
                return 10;
            });
            
            Task<int> step1Task = initialTask.ContinueWith(antecedent =>
            {
                int value = antecedent.Result;
                Console.WriteLine($"Step 1: Received {value}, doubling it");
                Thread.Sleep(1000);
                return value * 2;
            });
            
            Task<int> step2Task = step1Task.ContinueWith(antecedent =>
            {
                int value = antecedent.Result;
                Console.WriteLine($"Step 2: Received {value}, adding 5");
                Thread.Sleep(1000);
                return value + 5;
            });
            
            Task<string> finalTask = step2Task.ContinueWith(antecedent =>
            {
                int value = antecedent.Result;
                Console.WriteLine($"Final step: Received {value}, converting to string");
                Thread.Sleep(1000);
                return $"The final result is {value}";
            });
            
            Console.WriteLine("Waiting for the entire chain to complete...");
            string result = finalTask.Result;
            
            Console.WriteLine($"\nFinal result: {result}");
            
            ConsoleHelper.WriteInfo("\nTask continuations can be chained together to create a pipeline");
            ConsoleHelper.WriteInfo("of operations, where each step depends on the result of the previous step.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates various continuation options
        /// </summary>
        private void DemonstrateContinuationOptions()
        {
            ConsoleHelper.WriteSubheader("Continuation Options");
            
            Console.WriteLine("TaskContinuationOptions provide control over when and how continuations run:");
            
            // Demonstrate NotOnCanceled option
            Console.WriteLine("\n1. NotOnCanceled - Continuation runs only if the task wasn't canceled");
            
            CancellationTokenSource cts = new CancellationTokenSource();
            Task cancelableTask = Task.Run(() =>
            {
                Console.WriteLine("Cancelable task running...");
                
                // Check for cancellation
                for (int i = 0; i < 5; i++)
                {
                    Thread.Sleep(300);
                    cts.Token.ThrowIfCancellationRequested();
                }
                
                Console.WriteLine("Cancelable task completed without cancellation");
            }, cts.Token);
            
            Task notOnCanceledContinuation = cancelableTask.ContinueWith(antecedent =>
            {
                Console.WriteLine("This continuation runs only if the task wasn't canceled");
            }, TaskContinuationOptions.NotOnCanceled);
            
            // Cancel the task
            cts.Cancel();
            
            try
            {
                cancelableTask.Wait();
            }
            catch (AggregateException)
            {
                Console.WriteLine("Task was canceled");
            }
            
            Thread.Sleep(1000); // Give time for continuations to run or not run
            
            // Demonstrate ExecuteSynchronously option
            Console.WriteLine("\n2. ExecuteSynchronously - Run continuation on the same thread if possible");
            
            Task syncTask = Task.Run(() =>
            {
                Console.WriteLine($"Sync task running on thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(500);
            });
            
            Task syncContinuation = syncTask.ContinueWith(antecedent =>
            {
                Console.WriteLine($"Sync continuation running on thread {Thread.CurrentThread.ManagedThreadId}");
            }, TaskContinuationOptions.ExecuteSynchronously);
            
            syncContinuation.Wait();
            
            // Demonstrate LongRunning option
            Console.WriteLine("\n3. LongRunning - Hint that the continuation will be long-running");
            
            Task longRunningContinuation = Task.Run(() =>
            {
                Console.WriteLine("Task running...");
                Thread.Sleep(500);
            }).ContinueWith(antecedent =>
            {
                Console.WriteLine("Long-running continuation started");
                Thread.Sleep(1000);
                Console.WriteLine("Long-running continuation completed");
            }, TaskContinuationOptions.LongRunning);
            
            longRunningContinuation.Wait();
            
            Console.WriteLine("\nOther useful TaskContinuationOptions:");
            Console.WriteLine("- OnlyOnRanToCompletion: Run only if the task completed successfully");
            Console.WriteLine("- OnlyOnFaulted: Run only if the task threw an exception");
            Console.WriteLine("- OnlyOnCanceled: Run only if the task was canceled");
            Console.WriteLine("- AttachedToParent: Attach the continuation to the parent task");
            
            ConsoleHelper.WaitForKey();
        }
    }
}
