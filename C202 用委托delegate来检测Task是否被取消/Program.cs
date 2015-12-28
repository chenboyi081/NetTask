using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C202_用委托delegate来检测Task是否被取消
{
    class Program
    {
        static void Main(string[] args)
        {
  //          我们可以在注册一个委托到CancellationToken中，这个委托的方法在CancellationToken.Cancel()调用之前被调用。

  //我们可以用这个委托中的方法来作为一个检测task是否被取消的另外一个可选的方法，因为这个方法是在Cancel()方法被调用之前就调用的，所以这个委托中的方法可以检测task是否被cancel了，也就是说，只要这个委托的方法被调用，那么就说这个CancellationToken.Cancel()方法被调用了，而且在这个委托的方法中我们可以做很多的事情，如通知用户取消操作发生了。

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

            // register a cancellation delegate
            token.Register(() =>
            {
                Console.WriteLine(">>>>>> Delegate Invoked\n");
            });

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
