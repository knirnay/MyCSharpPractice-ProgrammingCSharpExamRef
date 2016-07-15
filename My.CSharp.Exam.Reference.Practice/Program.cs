using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.CSharp.Exam.Reference.Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadTutorial.TestCreateThread();
            ThreadTutorial.TestBackgroundThreadMethod();
            ThreadTutorial.TestParameterizedThreadStart();
            ThreadTutorial.TestStoppingThread();
            ThreadTutorial.TestThreadStaticAttribute();
            ThreadTutorial.TestThreadLocalClass();
            ThreadTutorial.TestQueuingWorkToThreadPool();
            TaskTutorial.TestCreateTask();
            TaskTutorial.TestTaskReturningValue();
            TaskTutorial.TestTaskContinuation();
            TaskTutorial.TestSchedulingDifferentContinuationTask();
            TaskTutorial.TestAttachingChildTask();
            TaskTutorial.TestTaskFactory();
            TaskTutorial.TestWaitAll();
            TaskTutorial.TestWaitAny();
            ParallelClassTutorial.TestParalleFor();
            ParallelClassTutorial.TestParalleForeach();
            ParallelClassTutorial.TestParallelBreak();
            string result = AsyncAwaitTutorial.DownloadContent().Result;
            Task t1 = AsyncAwaitTutorial.SleepAsyncA(100000);
            Task t2 = AsyncAwaitTutorial.SleepAsyncB(100000);
            PLINQTutorial.TestAsParallelLinqMethod();
            PLINQTutorial.TestWithoutParallelismLinqMethod();
            PLINQTutorial.MeasurePerformanceOfAboveMethodsAgainstEachOther();
            PLINQTutorial.TestAsOrderdLinqMethod();
            PLINQTutorial.TestAsSequentialMethod();
            PLINQTutorial.TestForAllMethod();
            PLINQTutorial.TestAggregateException();
            Console.ReadLine();
        }
    }
}
