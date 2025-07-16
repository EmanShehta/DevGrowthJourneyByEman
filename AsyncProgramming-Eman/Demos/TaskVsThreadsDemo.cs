using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AsyncProgrammingDemo.Utils;

namespace AsyncProgrammingDemo.Demos
{
    /// <summary>
    /// Demonstrates the differences between Tasks and Threads
    /// </summary>
    public class TaskVsThreadsDemo
    {
        /// <summary>
        /// Runs the Task vs Threads demonstration
        /// </summary>
        public void Run()
        {
            ConsoleHelper.WriteHeader("Task Vs. Threads", "المهام مقابل مسار العمليات");
            
            ConsoleHelper.WriteInfo("This demo shows the key differences between Tasks and Threads in C#.");
            ConsoleHelper.WriteInfo("We'll compare their behavior, performance, and use cases.\n");
            
            // Compare creation and execution
            CompareCreation();
            
            // Compare resource usage
            CompareResourceUsage();
            
            // Compare features and capabilities
            CompareFeatures();
            
            // Show practical examples
            ShowPracticalExamples();
            
            // Summary
            ConsoleHelper.WriteSubheader("Summary: Tasks vs Threads");
            Console.WriteLine("Tasks:");
            Console.WriteLine("✓ Higher-level abstraction built on top of the thread pool");
            Console.WriteLine("✓ More efficient resource management");
            Console.WriteLine("✓ Support for return values, exceptions, and continuations");
            Console.WriteLine("✓ Designed for asynchronous operations");
            Console.WriteLine("✓ Integrate with async/await pattern");
            Console.WriteLine();
            Console.WriteLine("Threads:");
            Console.WriteLine("✓ Lower-level control over threading");
            Console.WriteLine("✓ Direct management of thread lifecycle");
            Console.WriteLine("✓ Useful for long-running operations");
            Console.WriteLine("✓ More control over thread priority and affinity");
            Console.WriteLine();
            ConsoleHelper.WriteInfo("In modern C# development, Tasks are generally preferred over direct Thread manipulation,");
            ConsoleHelper.WriteInfo("except for specific scenarios requiring low-level thread control.");
            
            ConsoleHelper.WaitForKey();
        }
        
        /// <summary>
        /// Compares the creation and execution of Tasks and Threads
        /// </summary>
        private void CompareCreation()
        {
            ConsoleHelper.WriteSubheader("Creation and Execution");
            
            Console.WriteLine("Creating and starting a Thread:");
            Thread thread = new Thread(() =>
            {
                Console.WriteLine($"Thread executing on thread ID: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Thread executing on thread IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
                Console.WriteLine($"Thread executing on thread IsBackground: {Thread.CurrentThread.IsBackground}");
                Thread.Sleep(1000);
                Console.WriteLine("Thread execution completed");
            });
            
            Console.WriteLine($"Thread created with ID: {thread.ManagedThreadId}, State: {thread.ThreadState}");
            thread.Start();
            Console.WriteLine("Thread.Start() called");
            thread.Join(); // Wait for thread to complete
            Console.WriteLine($"Thread state after completion: {thread.ThreadState}\n");
            
            Console.WriteLine("Creating and starting a Task:");
            Task task = Task.Run(() =>
            {
                Console.WriteLine($"Task executing on thread ID: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Task executing on thread  IsThreadPoolThread: {Thread.CurrentThread.IsThreadPoolThread}");
                Console.WriteLine($"Task executing on thread IsBackground : {Thread.CurrentThread.IsBackground}");
                Thread.Sleep(1000);
                Console.WriteLine("Task execution completed");
            });
            
            Console.WriteLine($"Task created with ID: {task.Id}, Status: {task.Status}");
            task.Wait(); // Wait for task to complete
            Console.WriteLine($"Task status after completion: {task.Status}\n");
            
            ConsoleHelper.WriteInfo("Key differences in creation:");
            ConsoleHelper.WriteInfo("1. Tasks are created and started in one step with Task.Run()");
            ConsoleHelper.WriteInfo("2. Threads require explicit Start() after creation");
            ConsoleHelper.WriteInfo("3. Tasks use the thread pool by default, while Threads create new OS threads");
            
            ConsoleHelper.WaitForKey();
        }

        /// <summary>
        /// Compares the resource usage of Tasks and Threads
        /// </summary>
        private void CompareResourceUsage()
        {
            ConsoleHelper.WriteSubheader("Resource Usage");

            int operationCount = 100;
            Console.WriteLine($"Creating {operationCount} Threads (CPU-bound workload)...");

            DateTime threadStart = DateTime.Now;
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < operationCount; i++)
            {
                var thread = new Thread(() =>
                {
                    //Thread.Sleep(10);
                    PerformHeavyCalculation(); // تنفيذ عملية حسابية تقيلة بدل Thread.Sleep
                });
                threads.Add(thread);
                thread.Start();
            }

            // ننتظر انتهاء كل الـ Threads عشان نحسب الوقت بدقة
            foreach (var thread in threads)
                thread.Join();

            TimeSpan threadTime = DateTime.Now - threadStart;
            Console.WriteLine($"Time to create and run {operationCount} threads: {threadTime.TotalMilliseconds:F2}ms\n");

            Console.WriteLine($"Creating {operationCount} Tasks (CPU-bound workload)...");
            DateTime taskStart = DateTime.Now;

            Task[] tasks = new Task[operationCount];
            for (int i = 0; i < operationCount; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    //Thread.Sleep(10);
                    PerformHeavyCalculation(); // نفس العملية الحسابية للتساوي في المقارنة
                });
            }

            // ننتظر انتهاء كل الـ Tasks (زي Task.WaitAll بدل Thread.Join)
            Task.WaitAll(tasks);

            TimeSpan taskTime = DateTime.Now - taskStart;
            Console.WriteLine($"Time to create and run {operationCount} tasks: {taskTime.TotalMilliseconds:F2}ms\n");

            // 📌 ملاحظات مهمة:
            // - استخدام Sleep مش دقيق لأنه ما بيستهلكش فعليًا من وقت المعالج
            // - العمليات الحسابية هنا بتقيس الأداء الحقيقي (CPU-bound)
            // - الـ Tasks بتستخدم ThreadPool، فبتكون أسرع وأكفأ من Threads في الأغلب
            // - الـ Threads بيتم إنشاؤهم من الصفر، فده بيكلف أكثر في الذاكرة والزمن
            // - Tasks مناسبة أكتر للعمليات المتوازية والتوسع Scalability
            #region
            /*
                📌 مقارنة الأداء بين Thread و Task:

                🔸 تخيلي إنك بتقيسي سرعة اتنين موظفين:
                    - بس بدل ما تديهم شغل، قولتي لهم: "استنوا 10 دقايق وبعدين ارجعولي".
                    - الاتنين هيستنوا نفس الوقت تقريبًا.
                    - ومش هتعرفي مين فيهم أسرع أو أكفأ فعلاً.
                    👉 ده نفس اللي بيحصل لما تستخدم Thread.Sleep.

                ✅ الحل: تديهم شغل فعلي – زي كتابة تقارير (أو عملية حسابية تقيلة)
                    double sum = 0;
                    for (int i = 0; i < 1000000; i++)
                        sum += Math.Sqrt(i);

                ⚠️ ليه Thread.Sleep مش مناسب لاختبار الأداء الحقيقي؟

                1. ❌ Sleep = وقت ميت (مش شغل فعلي)
                    - مفيش ضغط على المعالج.
                    - البرنامج بيقيس "انتظار" مش "تنفيذ".

                2. ❌ دقة Sleep مش مضمونة:
                    - Thread.Sleep(10) ممكن تكون 12ms أو أكتر.
                    - بتتأثر بعوامل زي:
                        - ضغط النظام
                        - دقة المؤقت Timer
                        - الجدولة Scheduling

                3. ❌ Tasks و Threads بيتعاملوا مع Sleep بنفس الشكل:
                    - الاتنين بيوقفوا وينتظروا.
                    - فمش هيبان الفرق الحقيقي بينهم.

                ✅ علشان كده بنستخدم عملية CPU-bound (شغل فعلي) عشان نعرف مين أكفأ وأسرع فعلاً.
                */
            #endregion
            ConsoleHelper.WriteInfo("Key differences in resource usage:");
            ConsoleHelper.WriteInfo("1. Tasks use a thread pool and are generally more efficient.");
            ConsoleHelper.WriteInfo("2. Threads are created from scratch and have higher overhead.");
            ConsoleHelper.WriteInfo("3. Tasks are better suited for high concurrency and scalability.");
            ConsoleHelper.WriteInfo("4. This test simulates CPU-bound workloads for better accuracy.");

            ConsoleHelper.WaitForKey();
        }

        // عملية حسابية تقيلة (CPU-bound) لاختبار فعلي لأداء المعالج
        private void PerformHeavyCalculation()
        {
            double result = 0;
            for (int i = 0; i < 1000000; i++)
            {
                result += Math.Sqrt(i);
            }
        }

        /// <summary>
        /// Compares the features and capabilities of Tasks and Threads
        /// </summary>
        private void CompareFeatures()
        {
            ConsoleHelper.WriteSubheader("Features and Capabilities");

            Console.WriteLine("Tasks provide several features not available with raw threads:");

            // ✅ 1. إمكانية إرجاع قيمة من داخل الـ Task (Task<TResult>)
            Console.WriteLine("\n1. Return Values:");
            Task<int> taskWithResult = Task.Run(() =>
            {
                Thread.Sleep(500); // محاكاة لعملية طويلة
                return 42; // بيرجع قيمة
            });

            Console.WriteLine("Task started with return value");
            Console.WriteLine($"Task result: {taskWithResult.Result}"); // .Result بتنتظر المهمة وتجيب الناتج

            // ✅ 2. التعامل مع الاستثناءات بسهولة (Exception Handling)
            Console.WriteLine("\n2. Exception Handling:");
            Task faultedTask = Task.Run(() =>
            {
                Thread.Sleep(500);

                // طباعة رقم الـ Thread قبل رمي الاستثناء
                Console.WriteLine($"Exception thrown from thread: {Thread.CurrentThread.ManagedThreadId}");

                throw new InvalidOperationException("Deliberate exception in task");
            });

            try
            {
                faultedTask.Wait();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine($"Exception caught from thread: {Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine($"Exception caught from task: {ae.InnerException.Message}");
            }


            // ✅ 3. تنفيذ عمليات متسلسلة (Continuations)
            Console.WriteLine("\n3. Continuations:");
            Task firstTask = Task.Run(() =>
            {
                Console.WriteLine("First task executing");
                Thread.Sleep(500);
                return "First task result";
            });

            // ContinueWith: بتنفيذ المهمة دي بعد انتهاء الأولى مباشرة
            Task continuationTask = firstTask.ContinueWith(t =>
            {
                // Console.WriteLine($"Continuation task received: {t.Result}");
                Thread.Sleep(500);
                Console.WriteLine("Continuation task completed");
            });

            continuationTask.Wait(); // ننتظر انتهاء المهمة الثانية

            // ✅ تلخيص سريع لأهم المزايا
            ConsoleHelper.WriteInfo("\nKey feature differences:");
            ConsoleHelper.WriteInfo("1. Tasks can return values (Task<T>)");
            ConsoleHelper.WriteInfo("2. Tasks propagate exceptions to the awaiting code");
            ConsoleHelper.WriteInfo("3. Tasks support continuations for chaining operations");
            ConsoleHelper.WriteInfo("4. Tasks integrate with async/await pattern");
            ConsoleHelper.WriteInfo("5. Tasks support cancellation and progress reporting");

            ConsoleHelper.WaitForKey();
        }


        /// <summary>
        /// Shows practical examples of when to use Tasks vs Threads
        /// </summary>
        private void ShowPracticalExamples()
        {
            ConsoleHelper.WriteSubheader("Practical Use Cases");
            
            Console.WriteLine("When to use Tasks:");
            Console.WriteLine("1. For most asynchronous operations");
            Console.WriteLine("2. When you need to return values from operations");
            Console.WriteLine("3. When working with async/await pattern");
            Console.WriteLine("4. For operations that benefit from thread pool management");
            Console.WriteLine("5. When you need to chain operations with continuations");
            Console.WriteLine("6. For parallel operations on collections (PLINQ, Parallel.ForEach)");
            
            
            Console.WriteLine("\nWhen to use Threads:");
            Console.WriteLine("1. For long-running operations that shouldn't block the thread pool");
            Console.WriteLine("2. When you need fine-grained control over thread priority");
            Console.WriteLine("3. When you need to set thread-specific properties (name, culture, etc.)");
            Console.WriteLine("4. For UI thread operations in older frameworks");
            Console.WriteLine("5. When implementing your own scheduling or thread management");
            
            Console.WriteLine("\nExample: Long-running operation with Thread vs Task");
            
            Console.WriteLine("\nUsing Thread for long-running operation:");
            Thread longRunningThread = new Thread(() => 
            {
                Console.WriteLine("Long-running thread started");
                // Simulate long-running work
                Thread.Sleep(1000);
                Console.WriteLine("Long-running thread completed");
            });
            longRunningThread.IsBackground = true;
            longRunningThread.Start();
            longRunningThread.Join();
            
            Console.WriteLine("\nUsing Task for long-running operation:");
            Task longRunningTask = Task.Factory.StartNew(() => 
            {
                Console.WriteLine("Long-running task started");
                // Simulate long-running work
                Thread.Sleep(1000);
                Console.WriteLine("Long-running task completed");
            }, TaskCreationOptions.LongRunning);
          //  TaskCreationOptions.LongRunning:
         //بيعطي تلميح للمجدول إن الـ Task دي طويلة، فـ ممكن يشغلها على Thread مستقل بدل ThreadPool.


            longRunningTask.Wait();
            
            ConsoleHelper.WriteInfo("\nNote: The TaskCreationOptions.LongRunning hint tells the task scheduler");
            ConsoleHelper.WriteInfo("that this task might be long-running and could benefit from its own thread");
            ConsoleHelper.WriteInfo("rather than using a thread pool thread.");
            
            ConsoleHelper.WaitForKey();
        }
    }
}
