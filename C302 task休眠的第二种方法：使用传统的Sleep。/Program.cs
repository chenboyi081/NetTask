using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C302_task休眠的第二种方法_使用传统的Sleep_
{
    class Program
    {
        static void Main(string[] args)
        {
            //我们现在已经知道了：其实TPL(并行编程)的底层还是基于.NET的线程机制的。所以还是可以用传统的线程技术来使得一个task休眠：调用静态方法—Thread.Sleep(),并且可以传入一个int类型的参数，表示要休眠多长时间。
            //这种方法和之前第一种方法最大的区别就是：使用Thread.Sleep()之后，然后再调用token的cancel方法，task不会立即就被cancel，这主要是因为Thread.Sleep()将会一直阻塞线程，直到达到了设定的时间，这之后，再去check task时候被cancel了。举个例子，假设再task方法体内调用Thread.Sleep(100000)方法来休眠task，然后再后面的代码中调用token.Cancel()方法，此时处于并行编程内部机制不会去检测task是否已经发出了cancel请求，而是一直休眠，直到时间超过了100000微秒。如果采用的是之前的第一种休眠方法，那么不管WaitOne（）中设置了多长的时间，只要token.Cancel()被调用，那么task就像内部的Scheduler发出了cancel的请求，而且task会被cancel。

            // create the cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            // create the cancellation token
            CancellationToken token = tokenSource.Token;

            // create the first task, which we will let run fully
            Task task1 = new Task(() =>
            {
                for (int i = 0; i < Int32.MaxValue; i++)
                {
                    // put the task to sleep for 10 seconds
                    Thread.Sleep(10000);

                    // print out a message
                    Console.WriteLine("Task 1 - Int value {0}", i);
                    // check for task cancellation
                    token.ThrowIfCancellationRequested();
                }
            }, token);
            // start task
            task1.Start();

            // wait for input before exiting
            Console.WriteLine("Press enter to cancel token.");
            Console.ReadLine();

            // cancel the token
            tokenSource.Cancel();

            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
