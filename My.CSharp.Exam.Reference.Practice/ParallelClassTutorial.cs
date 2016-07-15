/* The System.Threading.Task namespace also contains another class that can be usded for parallel processing.
 * The Parallel class has a couple of static methods - For, ForEach and Invoke that you can use to parallelize work.
 * 
 * Parallelism involves taking a certain task and splitting it into a set of related tasks that can be executed concurrently.
 * This also means that you shouldn't go through your code to replace all your loops with parallel loops. 
 * You should use the Parallel class only when your code doesn't have to be executed sequentially.
 * 
 * Increasing performance with parallel processing happens only when you have a lot of work to be done that can be executed in
 * parallel. For smaller work sets or for work that has to synchronize access to resources, using the Parallel class can hurt
 * performance.
 */
namespace My.CSharp.Exam.Reference.Practice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    class ParallelClassTutorial
    {
        public static void TestParalleFor()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Parallel.For(0, 10, i =>
            {
                Thread.Sleep(1000);
            });
        }

        public static void TestParalleForeach()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            IEnumerable<int> numbers = Enumerable.Range(0, 10);
            Parallel.ForEach(numbers, i =>
            {
                Thread.Sleep(1000);
            });
        }

        /// <summary>
        /// You can cancel the loop by using the ParallelLoopState object. You have two options to do this
        /// Break or Stop. Break ensures that all iterations that are currently running will be finished. 
        /// Stop just terminates everything.
        /// 
        /// When breaking the parallel loop, the result variable has an IsCompleted value of false and a
        /// LowestBreakIteration of 500. When you use Stop method, the LowestBreakIteration is null.
        /// </summary>
        public static void TestParallelBreak()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            ParallelLoopResult result = Parallel.For(0, 1000, (int i, ParallelLoopState loopState) =>
            {
                if (i == 500)
                {
                    Console.WriteLine("Breaking loop");
                    loopState.Break();
                }

                return;
            });
        }
    }
}
