using System;
using System.Threading;
using ThreadingConsoleDemo.Models;

namespace ThreadingConsoleDemo.Demos
{
    /// <summary>
    /// Demonstrates synchronization concepts including race conditions, locks, and deadlocks
    /// </summary>
    public class SynchronizationDemo
    {
        /// <summary>
        /// Demonstrates race conditions when multiple threads access shared data
        /// </summary>
        public void DemonstrateRaceCondition()
        {
            ConsoleHelper.WriteHeader("Race Condition", " ");
            
            ConsoleHelper.WriteInfo("This demo shows how race conditions occur when multiple threads access shared data.");
            ConsoleHelper.WriteInfo("We'll simulate multiple threads making deposits to a bank account without proper synchronization.\n");
            
            // Create a bank account with initial balance
            BankAccount account = new BankAccount("12345", "eman shehta", 1000);
            Console.WriteLine($"Initial account state: {account}");
            
            // Number of threads and operations
            int threadCount = 5;
            int operationsPerThread = 100;
            decimal depositAmount = 10;
            
            // Expected final balance calculation
            decimal expectedFinalBalance = account.Balance + (threadCount * operationsPerThread * depositAmount);
            Console.WriteLine($"Expected final balance: ${expectedFinalBalance:F2}\n");
            
            ConsoleHelper.WriteSubheader("Running with unsafe (non-synchronized) operations");
            
            // Create and start multiple threads using unsafe operations
            Thread[] threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < operationsPerThread; j++)
                    {
                        account.DepositUnsafe(depositAmount);
                    }
                });
                
                Console.WriteLine($"Starting thread {i + 1}...");
                threads[i].Start();
            }
            
            // Wait for all threads to complete
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            
            // Check final balance
            Console.WriteLine($"\nAll threads completed.");
            Console.WriteLine($"Final account state: {account}");
            
            if (account.Balance != expectedFinalBalance)
            {
                ConsoleHelper.WriteError($"Race condition detected! Balance is incorrect.");
                ConsoleHelper.WriteError($"Missing amount: ${expectedFinalBalance - account.Balance:F2}");
            }
            else
            {
                ConsoleHelper.WriteSuccess("No race condition occurred this time (which is rare).");
            }
            
            ConsoleHelper.WriteInfo("\nRace conditions occur because:");
            ConsoleHelper.WriteInfo("1. Thread 1 reads the balance (e.g., $1000)");
            ConsoleHelper.WriteInfo("2. Thread 2 reads the same balance ($1000) before Thread 1 updates it");
            ConsoleHelper.WriteInfo("3. Thread 1 adds $10 and writes $1010");
            ConsoleHelper.WriteInfo("4. Thread 2 adds $10 to its read value and writes $1010");
            ConsoleHelper.WriteInfo("5. Result: Two deposits but balance only increased once!");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates using locks to prevent race conditions
        /// </summary>
        public void DemonstrateLockPrevention()
        {
            ConsoleHelper.WriteHeader("Lock to prevent Race condition", "");
            
            ConsoleHelper.WriteInfo("This demo shows how to use locks to prevent race conditions.");
            ConsoleHelper.WriteInfo("We'll compare unsafe operations with properly synchronized operations.\n");
            
            // Create two bank accounts with the same initial balance
            BankAccount unsafeAccount = new BankAccount("12345", "eman shehta (Unsafe)", 1000);
            BankAccount safeAccount = new BankAccount("67890", "yousef sameh (Safe)", 1000);
            
            Console.WriteLine($"Initial unsafe account: {unsafeAccount}");
            Console.WriteLine($"Initial safe account: {safeAccount}");
            
            // Number of threads and operations
            int threadCount = 5;
            int operationsPerThread = 100;
            decimal depositAmount = 10;
            
            // Expected final balance calculation
            decimal expectedFinalBalance = unsafeAccount.Balance + (threadCount * operationsPerThread * depositAmount);
            Console.WriteLine($"Expected final balance: ${expectedFinalBalance:F2}\n");
            
            // Create and start threads for unsafe operations
            ConsoleHelper.WriteSubheader("Running with unsafe (non-synchronized) operations");
            Thread[] unsafeThreads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                unsafeThreads[i] = new Thread(() =>
                {
                    for (int j = 0; j < operationsPerThread; j++)
                    {
                        unsafeAccount.DepositUnsafe(depositAmount);
                    }
                });
                
                unsafeThreads[i].Start();
            }
            
            // Wait for all unsafe threads to complete
            foreach (Thread thread in unsafeThreads)
            {
                thread.Join();
            }
            
            // Create and start threads for safe operations
            ConsoleHelper.WriteSubheader("Running with safe (synchronized) operations");
            Thread[] safeThreads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                safeThreads[i] = new Thread(() =>
                {
                    for (int j = 0; j < operationsPerThread; j++)
                    {
                        safeAccount.Deposit(depositAmount);
                    }
                });
                
                safeThreads[i].Start();
            }
            
            // Wait for all safe threads to complete
            foreach (Thread thread in safeThreads)
            {
                thread.Join();
            }
            
            // Check final balances
            Console.WriteLine($"\nAll threads completed.");
            Console.WriteLine($"Final unsafe account: {unsafeAccount}");
            Console.WriteLine($"Final safe account: {safeAccount}");
            
            // Check for race conditions
            if (unsafeAccount.Balance != expectedFinalBalance)
            {
                ConsoleHelper.WriteError($"Race condition detected in unsafe account!");
                ConsoleHelper.WriteError($"Missing amount: ${expectedFinalBalance - unsafeAccount.Balance:F2}");
            }
            else
            {
                ConsoleHelper.WriteWarning("No race condition occurred in unsafe account (which is rare).");
            }
            
            if (safeAccount.Balance != expectedFinalBalance)
            {
                ConsoleHelper.WriteError($"Unexpected result in safe account!");
            }
            else
            {
                ConsoleHelper.WriteSuccess("Safe account has the correct balance as expected.");
            }
            
            ConsoleHelper.WriteInfo("\nLocking mechanisms in C#:");
            ConsoleHelper.WriteInfo("1. lock statement - simplest way to synchronize access to a block of code");
            ConsoleHelper.WriteInfo("2. Monitor class - provides more control over locking");
            ConsoleHelper.WriteInfo("3. Mutex - can be used for cross-process synchronization");
            ConsoleHelper.WriteInfo("4. Semaphore - controls access to a limited resource");
            ConsoleHelper.WriteInfo("5. ReaderWriterLock - allows multiple readers but exclusive writers");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Demonstrates deadlocks and how to prevent them
        /// </summary>
        public void DemonstrateDeadlock()
        {
            ConsoleHelper.WriteHeader("Deadlock", "");
            
            ConsoleHelper.WriteInfo("This demo shows how deadlocks occur and how to prevent them.");
            ConsoleHelper.WriteInfo("A deadlock happens when two or more threads are waiting for each other to release resources.\n");
            
            // Create two bank accounts
            BankAccount account1 = new BankAccount("12345", "Amona sh", 1000);
            BankAccount account2 = new BankAccount("67890", "yasoufa sa", 1000);
            
            Console.WriteLine($"Initial account 1: {account1}");
            Console.WriteLine($"Initial account 2: {account2}");
            
            bool runDeadlockDemo = ConsoleHelper.GetYesNoInput("\nWould you like to run the deadlock demonstration? (This may freeze the console temporarily)");
            
            if (runDeadlockDemo)
            {
                ConsoleHelper.WriteSubheader("Demonstrating potential deadlock");
                ConsoleHelper.WriteWarning("This may cause the application to hang for a few seconds...");
                ConsoleHelper.WriteInfo("We'll create two threads that transfer money in opposite directions.");
                ConsoleHelper.WriteInfo("Thread 1: Transfer from Account 1 to Account 2");
                ConsoleHelper.WriteInfo("Thread 2: Transfer from Account 2 to Account 1");
                
                // Create threads that may cause deadlock
                Thread thread1 = new Thread(() =>
                {
                    Console.WriteLine("Thread 1: Starting transfer from Account 1 to Account 2");
                    account1.TransferTo(account2, 500);
                    Console.WriteLine("Thread 1: Transfer completed");
                });
                
                Thread thread2 = new Thread(() =>
                {
                    Console.WriteLine("Thread 2: Starting transfer from Account 2 to Account 1");
                    account2.TransferTo(account1, 300);
                    Console.WriteLine("Thread 2: Transfer completed");
                });
                
                // Start both threads
                thread1.Start();
                thread2.Start();
                
                // Wait for threads with timeout to avoid hanging the demo completely
                bool thread1Completed = thread1.Join(5000); // 5 second timeout
                bool thread2Completed = thread2.Join(5000); // 5 second timeout
                
                if (!thread1Completed || !thread2Completed)
                {
                    ConsoleHelper.WriteError("\nDeadlock detected! Threads are waiting for each other.");
                    ConsoleHelper.WriteInfo("In a real application, this would cause the program to hang indefinitely.");
                    ConsoleHelper.WriteInfo("For this demo, we used timeouts to prevent hanging the console.");
                    
                    // Force threads to abort in .NET Core is not directly supported
                    // We'll just inform the user
                    ConsoleHelper.WriteWarning("Demo will continue, but threads may still be deadlocked in the background.");
                }
                else
                {
                    ConsoleHelper.WriteSuccess("\nNo deadlock occurred this time (which can happen due to thread scheduling).");
                }
            }
            
            // Reset accounts for the safe demo
            account1 = new BankAccount("12345", "John Doe", 1000);
            account2 = new BankAccount("67890", "Jane Doe", 1000);
            
            ConsoleHelper.WriteSubheader("\nDemonstrating deadlock prevention");
            ConsoleHelper.WriteInfo("We'll use a technique to prevent deadlocks by always acquiring locks in the same order.");
            
            // Create threads that use safe transfer method
            Thread safeThread1 = new Thread(() =>
            {
                Console.WriteLine("Safe Thread 1: Starting transfer from Account 1 to Account 2");
                account1.SafeTransferTo(account2, 500);
                Console.WriteLine("Safe Thread 1: Transfer completed");
            });
            
            Thread safeThread2 = new Thread(() =>
            {
                Console.WriteLine("Safe Thread 2: Starting transfer from Account 2 to Account 1");
                account2.SafeTransferTo(account1, 300);
                Console.WriteLine("Safe Thread 2: Transfer completed");
            });
            
            // Start both threads
            safeThread1.Start();
            safeThread2.Start();
            
            // Wait for threads to complete
            safeThread1.Join();
            safeThread2.Join();
            
            Console.WriteLine($"\nFinal account 1: {account1}");
            Console.WriteLine($"Final account 2: {account2}");
            
            ConsoleHelper.WriteSuccess("\nBoth safe transfers completed without deadlock.");
            
            ConsoleHelper.WriteInfo("\nDeadlock prevention techniques:");
            ConsoleHelper.WriteInfo("1. Lock ordering - Always acquire locks in the same order");
            ConsoleHelper.WriteInfo("2. Lock timeouts - Use timeouts when acquiring locks");
            ConsoleHelper.WriteInfo("3. Deadlock detection - Implement detection algorithms");
            ConsoleHelper.WriteInfo("4. Resource hierarchy - Establish a hierarchy for resource acquisition");
            
            ConsoleHelper.WaitForKey();
        }
    }
}
