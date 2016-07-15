using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace My.CSharp.Exam.Reference.Practice
{
    class TaskTutorial
    {
        /// <summary>
        /// Queuing a work item to a thread pool can be useful, but it has its shortcomings. 
        /// There is no built in way to  now when the operation has  nished and what the return value is.
        /// This is why the .NET Framework introduces the concept of a Task, which is an object that represents some work that should be done. 
        /// The Task can tell you if the work is completed and if the operation returns a result, the Task gives you the result. 
        /// A task scheduler is responsible for starting the Task and managing it. By default, the Task scheduler uses threads from the thread pool to execute the Task. 
        /// Tasks can be used to make your application more responsive. If the thread that manages the user interface off loads work to another thread  from the thread pool, 
        /// it can  keep processing user events and ensure that the application can still be used. But it doesn’t help with scalability. 
        /// If a thread receives a web request and it would start a new Task, 
        /// it would just consume another thread from the thread pool while the original thread waits for results.
        /// Executing a Task on another thread makes sense only if you want to keep the user interface thread free for other work or if you want to parallelize your work on to multiple processors.
        /// </summary>
        public static void TestCreateTask()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Task t = Task.Run(() =>
            {
                for (int x = 0; x < 100; x++)
                {
                    Console.Write("*");
                }
            });

            //// Calling Wait is equivalent to calling join on a thread. It waits till the Task is finished
            //// before exiting the application.
            t.Wait();
        }

        public static void TestTaskReturningValue()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });

            Console.WriteLine(t.Result);
        }

        /// <summary>
        /// Beause of the object-oriented nature of the Task object, one thing you can do is add a continuation task.
        /// This means that you want another operation to execute as soon as the Task finishes.
        /// </summary>
        public static void TestTaskContinuation()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Task<int> t = Task.Run(() =>
            {
                return 42;
            }).ContinueWith((i) =>{
                return i.Result * 2;
            });

            Console.WriteLine(t.Result);
        }

        ///<summary>
        /// The ContinueWith menthod has a couple of overloads that you can use to configure when the continuation will run.
        /// This way you can add different continuation will run when an exception happens, the Task is canceled, or the Task
        /// completes successfully.
        /// </summary>
        public static void TestSchedulingDifferentContinuationTask()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });

            t.ContinueWith((i) =>
            {
                Console.WriteLine("Cancled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            t.ContinueWith((i) =>
            {
                Console.WriteLine("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task completedTask = t.ContinueWith((i) =>
            {
                Console.WriteLine("Completed");
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            completedTask.Wait();
        }

        /// <summary>
        /// Next to continuation Tasks, a Task can also have several child Tasks. The parent task finishes when all the child tasks are ready.
        /// The final Task runs only after the parent Task is finished, and the parent Task finishes when all three children are finished.
        /// You can use this to create quite complex Task hierarchies that will go through all the steps you specified.
        /// </summary>
        public static void TestAttachingChildTask()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Task<int[]> parent = Task.Run(() =>
            {
                int[] results = new int[3];
                new Task(() => results[0] = 0, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 1, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 2, TaskCreationOptions.AttachedToParent).Start();
                return results;
            });

            Task finalTask = parent.ContinueWith(
                parentTask =>
                {
                    foreach (int i in parent.Result)
                    {
                        Console.WriteLine(i);
                    }
                });

            finalTask.Wait();
        }

        /// <summary>
        /// In previous example, you had to create three Tasks with the same options. To make the process
        /// easier, you can use a TaskFactory. A TaskFactory is created with a certain configuration and can then
        /// be used to create tasks with that configuration.
        /// </summary>
        public static void TestTaskFactory()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Task<int[]> parent = Task.Run(() =>
            {
                int[] results = new int[3];
                TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent, TaskContinuationOptions.ExecuteSynchronously);
                tf.StartNew(() => results[0] = 0);
                tf.StartNew(() => results[1] = 1);
                tf.StartNew(() => results[2] = 2);
                return results;
            });

            Task finalTask = parent.ContinueWith(parentTask =>
            {
                foreach (int i in parent.Result)
                {
                    Console.WriteLine(i);
                }
            });

            finalTask.Wait();
        }

        /// <summary>
        /// Next to calling Wait on a single Task, you can also use the method WaitAll to wait for multiple Tasks
        /// to finish before continuing execution.
        /// 
        /// In this case, all three Tasks are executed simultaneously, and the whole run takes approximately 1000ms 
        /// instead of 3000ms.
        /// 
        /// Next to wait all is WhenAll method that you can use to schedule a continuation method after all Tasks have finished.
        /// </summary>
        public static void TestWaitAll()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("1");
                return 1;
            });

            tasks[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("2");
                return 2;
            });

            tasks[2] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("3");
                return 3;
            });

            Task.WaitAll(tasks);
        }

        /// <summary>
        /// Instead of watiting untill all tasks are finished, you can also wait until one of the tasks is finished.
        /// In this example, you process a completed Task as soon as it finishes. By keeping track of which Tasks are 
        /// finished, you don't have to wait until all Tasks have completed.
        /// 
        /// WaitAny function returns index from array of Task of the completed task.
        /// </summary>
        public static void TestWaitAny()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Task<int>[] tasks = new Task<int>[3];
            tasks[0] = Task.Run(() => { Thread.Sleep(2000); return 1; });
            tasks[1] = Task.Run(() => { Thread.Sleep(1000); return 2; });
            tasks[2] = Task.Run(() => { Thread.Sleep(3000); return 3; });

            while (tasks.Length > 0)
            {
                int i = Task.WaitAny(tasks);
                Task<int> completedTask = tasks[i];
                Console.WriteLine(completedTask.Result);
                List<Task<int>> temp = tasks.ToList();
                temp.RemoveAt(i);
                tasks = temp.ToArray();
            }
        }
    }
}
