using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C301_使用CancellationToken的Wait_Handle
{
    class Program
    {
        static void Main(string[] args)
        {
//            a)         在.NET 4并行编程中，让一个Task休眠的最好的方式就是使用CancellationToken的等待操作(Wait Handle)。而且操作起来也很简单：首先创建一个CancellationTokenSource的实例，然后通过这个实例的Token属性得到一个CancellationToken的实例，然后在用CancellationToken的WaitHandle属性，然后调用这个这个属性的WaitOne()方法。其实在之前讲述”Task的取消”一文中就已经使用过。

 

//        b)         WaitOne()方法有很多的重载方法来提供更多的功能，例如可以传入一个int的整数，表明要休眠多长的时间，单位是微秒，也可以传入一个TimeSpan的值。如果调用了CancellationToken的Cancel()方法，那么休眠就立刻结束。就是因为这个原因，我们之前的文章讲过，WaitOne()可以作为检测Task是否被取消的一个方案

//下面来看一段示例代码：

             //在下面的代码中，task在休眠了10秒钟之后就打印出一条信息。在例子中，在我们敲一下键盘之后，CancellationToken就会被Cancel，此时休眠就停止了，task重新唤醒，只不过是这个task将会被cancel掉。

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
                    bool cancelled = token.WaitHandle.WaitOne(10000);
                    // print out a message
                    Console.WriteLine("Task 1 - Int value {0}. Cancelled? {1}",
                    i, cancelled);
                    // check to see if we have been cancelled
                    if (cancelled)
                    {
                        throw new OperationCanceledException(token);
                    }
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
