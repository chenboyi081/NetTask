using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C203_用Wait_Handle还检测Task是否被取消
{
    class Program
    {
        static void Main(string[] args)
        {
            //第三种方法检测task是否被cancel就是调用CancellationToken.WaitHandle属性。对于这个属性的详细使用，在后续的文章中会深入的讲述，在这里主要知道一点就行了：CancellationToken的WaitOne()方法会阻止task的运行，只有CancellationToken的cancel()方法被调用后，这种阻止才会释放。
            //在下面的例子中，创建了两个task，其中task2调用了WaitOne()方法，所以task2一直不会运行，除非调用了CancellationToken的Cancel()方法，所以WaitOne()方法也算是检测task是否被cancel的一种方法了。

            // create the cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            // create the cancellation token
            CancellationToken token = tokenSource.Token;

            // create the task
            Task task1 = new Task(() =>
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

            // create a second task that will use the wait handle
            Task task2 = new Task(() =>
            {
                // wait on the handle
                token.WaitHandle.WaitOne();
                // write out a message
                Console.WriteLine(">>>>> Wait handle released");
            });

            // wait for input before we start the task
            Console.WriteLine("Press enter to start task");
            Console.WriteLine("Press enter again to cancel task");
            Console.ReadLine();
            // start the tasks
            task1.Start();
            task2.Start();

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
