using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C205_创建组合的取消Task的Token
{
    class Program
    {
        static void Main(string[] args)
        {
             //我们可以用CancellationTokenSource.CreateLinkedTokenSource()方法来创建一个组合的token，这个组合的token有很多的CancellationToken组成。主要组合token中的任意一个token调用了Cancel()方法，那么使用这个组合token的所有task就会被取消。代码如下：

            // create the cancellation token sources
            CancellationTokenSource tokenSource1 = new CancellationTokenSource();
            CancellationTokenSource tokenSource2 = new CancellationTokenSource();
            CancellationTokenSource tokenSource3 = new CancellationTokenSource();

            // create a composite token source using multiple tokens
            CancellationTokenSource compositeSource =
                CancellationTokenSource.CreateLinkedTokenSource(
            tokenSource1.Token, tokenSource2.Token, tokenSource3.Token);

            // create a cancellable task using the composite token
            Task task = new Task(() =>
            {
                // wait until the token has been cancelled
                compositeSource.Token.WaitHandle.WaitOne();
                // throw a cancellation exception
                throw new OperationCanceledException(compositeSource.Token);
            }, compositeSource.Token);

            // start the task
            task.Start();

            // cancel one of the original tokens
            tokenSource2.Cancel();

            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
