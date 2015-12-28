using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C201_通过轮询的方式检测Task是否被取消
{
    class Program
    {
        static void Main(string[] args)
        {
            //http://www.cnblogs.com/yanyangtian/archive/2010/05/25/1743218.html
            //   在很多Task内部都包含了循环，用来处理数据。我们可以在循环中通过CancellationToken的IsCancellationRequest属性来检测task是否被取消了。如果这个属性为true，那么我们就得跳出循环，并且释放task所占用的资源(如数据库资源，文件资源等). 
            //我们也可以在task运行体中抛出System.Threading.OperationCanceledException来取消运行的task。

            // create the cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            // create the cancellation token
            CancellationToken token = tokenSource.Token;
            // create the task

            Task task = new Task(() =>
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task cancel detected");
                        throw new OperationCanceledException(token);
                    }
                    else
                    {
                        Console.WriteLine("Int value {0}", i);
                    }
                }
            }, token);

            // wait for input before we start the task
            Console.WriteLine("Press enter to start task");
            Console.WriteLine("Press enter again to cancel task");
            Console.ReadLine();

            // start the task
            task.Start();

            // read a line from the console.
            Console.ReadLine();

            // cancel the task
            Console.WriteLine("Cancelling task");
            tokenSource.Cancel();

            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
