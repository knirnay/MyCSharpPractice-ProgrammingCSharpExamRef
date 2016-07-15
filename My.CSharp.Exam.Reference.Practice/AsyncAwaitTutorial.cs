using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace My.CSharp.Exam.Reference.Practice
{
    class AsyncAwaitTutorial
    {
        public static async Task<string> DownloadContent()
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://microsoft.com");
                return result;
            }
        }

        public static Task SleepAsyncA(int millisecondTimeout)
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            return Task.Run(() => Thread.Sleep(millisecondTimeout));
        }

        public static Task SleepAsyncB(int millisecondTimeout)
        {
            Console.WriteLine("Method: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            TaskCompletionSource<bool> tcs = null;
            Timer t = new Timer(delegate { tcs.TrySetResult(true); }, null, -1, -1);
            tcs = new TaskCompletionSource<bool>(t);
            t.Change(millisecondTimeout, -1);
            return tcs.Task;
        }
    }
}
