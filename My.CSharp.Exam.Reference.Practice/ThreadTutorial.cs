namespace My.CSharp.Exam.Reference.Practice
{
    using System;
    using System.Threading;

    public static class ThreadTutorial
    {
        private static void CreateThread()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Thread proc:{0}", i);
                //// Why the Thread.Sleep(0)? It is used to signal to Windows that this thread is  nished. 
                //// In- stead of waiting for the whole time-slice of the thread to finish, 
                //// it will immediately switch to another thread.
                Thread.Sleep(0);
            }
        }

        /// <summary>
        /// The Thread class can be found in the System.Threading namespace. This class enables you to 
        /// create new treads, manage their priority, and get their status. 
        /// The Thread class isn’t something that you should use in your applications, except when you have special needs. However, when using the Thread class you have control over all con gura- tion options. You can, for example, specify the priority of your thread, tell Windows that your thread is long running, or con gure other advanced options.
        /// Listing 1-1 shows an example of using the Thread class to run a method on another thread. The Console class synchronizes the use of the output stream for you so you can write to it from multiple threads. Synchronization is the mechanism of ensuring that two threads don’t execute a speci c portion of your program at the same time. In the case of a console appli- cation, this means that no two threads can write data to the screen at the exact same time. 
        /// If one thread is working with the output stream, other threads will have to wait before it’s finished. 
        /// Both your process and your thread have a priority. Assigning a low priority is useful for applications such as a screen saver. Such an application shouldn’t compete with other applica- tions for CPU time. A higher-priority thread should be used only when it’s absolutely neces- sary. A new thread is assigned a priority of Normal, which is okay for almost all scenarios. 
        /// Another thing that’s important to know about threads is the difference between fore- ground and background threads. Foreground threads can be used to keep an application alive. Only when all foreground threads end does the common language runtime (CLR) shut down your application. Background threads are then terminated.
        /// </summary>
        public static void TestCreateThread()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Thread t = new Thread(new ThreadStart(CreateThread));
            t.Start();
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Main thread: Do some work");
                Thread.Sleep(0);
            }

            //// The Thread.Join method is called on the main thread to let it wait until the other thread  finishes
            t.Join();
            Console.ReadLine();
        }

        private static void BackgroundThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Thread proc: {0}", i);
                Thread.Sleep(1000);
            }
        }

        public static void TestBackgroundThreadMethod()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Thread t = new Thread(new ThreadStart(CreateThread));
            /*
             * If you run this application with the IsBackground property set to true, 
             * the application exits immediately. If you set it to false (creating a foreground thread), 
             * the application prints the ThreadProc message ten times.
             */
            t.IsBackground = true;
            t.Start();
        }

        private static void ParameterizedThreadStart(object o)
        {
            for (int i = 0; i < (int)o; i++)
            {
                Console.WriteLine("Thread proc: {0}", i);

                Thread.Sleep(0);
            }
        }

        public static void TestParameterizedThreadStart()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Thread t = new Thread(new ParameterizedThreadStart(ParameterizedThreadStart));
            //// In this case, the value 5 is passed to the ThreadMethod as an object. 
            //// You can cast it to the expected type to use it in your method.
            t.Start(5);
            t.Join();
        }

        /// <summary>
        /// To stop a thread, you can use the Thread.Abort method. 
        /// However, because this method is executed by another thread, 
        /// it can happen at any time. When it happens, a ThreadAbort- Exception is thrown on the target thread. 
        /// This can potentially leave a corrupt state and make your application unusable.
        /// A better way to stop a thread is by using a shared variable that both your target and your calling thread can access.
        /// 
        /// In this case, the thread is initialized with a lambda expression (which in turn is just a short- hand version of a delegate). 
        /// The thread keeps running until stopped becomes true. After that, 
        /// the t.Join method causes the console application to wait till the thread finishes execution.
        /// </summary>
        public static void TestStoppingThread()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            bool stopped = false;

            Thread t = new Thread(new ThreadStart(() =>
            {
                while (!stopped)
                {
                    Console.WriteLine("Running...");
                    Thread.Sleep(1000);
                }
            }));

            t.Start();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
            stopped = true;
            t.Join();
        }

        [ThreadStatic]
        private static int field;

        /// <summary>
        /// A thread has its own call stack that stores all the methods that are executed. 
        /// Local vari- ables are stored on the call stack and are private to the thread.
        /// A thread can also have its own data that’s not a local variable. 
        /// By marking a  eld with the ThreadStatic attribute, each thread gets its own copy of a field 
        /// 
        /// With the ThreadStaticAttribute applied, the maximum value of _ eld becomes 10. If you remove it, 
        /// you can see that both threads access the same value and it becomes 20
        /// </summary>
        public static void TestThreadStaticAttribute()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    field++;
                    Console.WriteLine("Thread A: {0}", field);
                }
            }).Start();

            new Thread(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    field++;
                    Console.WriteLine("Thread B: {0}", field);
                }
            }).Start();

            Console.ReadKey();
        }

        private static ThreadLocal<int> localField = new ThreadLocal<int>(() =>
        {
            return Thread.CurrentThread.ManagedThreadId;
        });

        /// <summary>
        /// If you want to use local data in a thread and initialize it for each thread, you can use the ThreadLocal<T>
        /// class. This class takes a delegate to a method that initializes the value.
        /// </summary>
        public static void TestThreadLocalClass()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            new Thread(() =>
            {
                for (int x = 0; x < localField.Value; x++)
                {
                    Console.WriteLine("Thread A: {0}", localField.Value);
                }
            }).Start();

            new Thread(() =>
            {
                for (int x = 0; x < localField.Value; x++)
                {
                    Console.WriteLine("Thread B: {0}", localField.Value);
                }
            }).Start();

            Console.ReadKey();
        }

        /// <summary>
        ///  When working directly with the Thread class, you create a new thread each time, and the thread dies when
        ///  you are finished with it. The creation of a thread, however, is something that costs some time and resources.
        ///  
        /// A thread pool is created to reuse those threads, similar to the way a database connection pooling works.
        /// Instead of letting a thred die, you send it back to the pool where it can be reused whenever a request comes in.
        /// </summary>
        public static void TestQueuingWorkToThreadPool()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            ThreadPool.QueueUserWorkItem((s) =>
            {
                Console.WriteLine("Working on a thread from thread pool.");
            });

            Console.ReadLine();
        }
    }
}
