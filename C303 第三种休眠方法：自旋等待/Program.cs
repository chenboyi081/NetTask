using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C303_第三种休眠方法_自旋等待
{
    class Program
    {
        static void Main(string[] args)
        {
            //这种方法也是值得推荐的。之前的两种方法，当他们使得task休眠的时候，这些task已经从Scheduler的管理中退出来了，不被再内部的Scheduler管理(Scheduler，这里只是简单的提下，因为后面的文章会详细讲述，这里只要知道Scheduler是负责管理线程的)，因为休眠的task已经不被Scheduler管理了，所以Scheduler必须做一些工作去决定下一步是哪个线程要运行，并且启动它。为了避免Scheduler做那些工作，我们可以采用自旋等待：此时这个休眠的task所对应的线程不会从Scheduler中退出，这个task会把自己和CPU的轮转关联起来，我们还是用代码示例讲解吧。
            //代码中我们在Thread.SpinWait()方法中传入一个整数，这个整数就表示CPU时间片轮转的次数，至于要等待多长的时间，这个就和计算机有关了，不同的计算机，CPU的轮转时间不一样。自旋等待的方法常常于获得同步锁，后续会讲解。使用自旋等待会一直占用CPU，而且也会消耗CPU的资源，更大的问题就是这个方法会影响Scheduler的运作。


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
                    Thread.SpinWait(10000);
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
