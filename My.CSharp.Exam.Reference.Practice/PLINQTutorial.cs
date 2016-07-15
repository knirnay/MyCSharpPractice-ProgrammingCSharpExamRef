using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace My.CSharp.Exam.Reference.Practice
{
    class PLINQTutorial
    {
        public static void TestAsParallelLinqMethod()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            IEnumerable<int> numbers = Enumerable.Range(0, 10);
            int[] parallelResult = numbers.AsParallel().Where(i => i % 2 == 0).ToArray();
            foreach (int i in parallelResult)
            {
                Console.WriteLine(i);
            }
        }

        public static void TestWithoutParallelismLinqMethod()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            IEnumerable<int> numbers = Enumerable.Range(0, 10);
            int[] result = numbers.Where(i => i % 2 == 0).ToArray();
            foreach (int i in result)
            {
                Console.WriteLine(i);
            }
        }

        public static void MeasurePerformanceOfAboveMethodsAgainstEachOther()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                IEnumerable<int> numbers = Enumerable.Range(0, 10000);
                int[] parallelResult = numbers.AsParallel().Where(j => j % 2 == 0).ToArray();
            }

            sw.Stop();
            Console.WriteLine("Elapsed time for parallel run: {0}", sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                IEnumerable<int> numbers = Enumerable.Range(0, 1000);
                int[] result = numbers.Where(j => j % 2 == 0).ToArray();
            }

            sw.Stop();
            Console.WriteLine("Elapsed time for serial run: {0}", sw.ElapsedMilliseconds);
        }

        public static void TestAsOrderdLinqMethod()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            IEnumerable<int> numbers = Enumerable.Range(0, 10);
            int[] result = numbers.AsParallel().AsOrdered().Where(i => i % 2 == 0).ToArray();
            foreach (int i in result)
            {
                Console.WriteLine(i);
            }
        }

        public static void TestAsSequentialMethod()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            IEnumerable<int> numbers = Enumerable.Range(0, 10);
            int[] result = numbers.AsParallel().AsOrdered().Where(i => i % 2 == 0).AsSequential().ToArray();
            foreach (int i in result)
            {
                Console.WriteLine(i);
            }
        }

        public static void TestForAllMethod()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            IEnumerable<int> numbers = Enumerable.Range(0, 20);
            ParallelQuery<int> result = numbers.AsParallel().Where(i => i % 2 == 0);
            result.ForAll(e => Console.WriteLine(e));
        }

        public static void TestAggregateException()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            IEnumerable<int> numbers = Enumerable.Range(0, 20);
            try
            {
                ParallelQuery<int> result = numbers.AsParallel().Where(i => IsEven(i));
                result.ForAll(e => Console.WriteLine(e));
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("There were {0} exceptions", ex.InnerExceptions.Count);
            }
        }

        private static bool IsEven(int i)
        {
            if (i % 10 == 0)
            {
                throw new ArgumentException("Mode by 10 is zero", "i");
            }

            return i % 2 == 0;
        }
    }
}
