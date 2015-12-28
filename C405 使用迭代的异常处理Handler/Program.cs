using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C405_使用迭代的异常处理Handler
{
    class Program
    {
        static void Main(string[] args)
        {
//            一般情况下，我们需要区分哪些异常需要处理，而哪些异常需要继续往上传递。AggregateException类提供了一个Handle()方法，我们可以用这个方法来处理

//AggregateException中的每一个异常。在这个Handle()方法中，返回true就表明，这个异常我们已经处理了，不用抛出，反之。

//   在下面的例子中，抛出了一个OperationCancelException，在之前的task的取消一文中，已经提到过：当在task中抛出这个异常的时候，实际上就是这个task发送了取消的请求。下面的代码中，描述了如果在AggregateException.Handle()中处理不同的异常。

            // create the cancellation token source and the token
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            // create a task that waits on the cancellation token
            Task task1 = new Task(() =>
            {
                // wait forever or until the token is cancelled
                token.WaitHandle.WaitOne(-1);
                // throw an exception to acknowledge the cancellation
                throw new OperationCanceledException(token);
            }, token);
            // create a task that throws an exception
            Task task2 = new Task(() =>
            {
                throw new NullReferenceException();
            });
            // start the tasks
            task1.Start(); task2.Start();
            // cancel the token
            tokenSource.Cancel();
            // wait on the tasks and catch any exceptions
            try
            {
                Task.WaitAll(task1, task2);
            }
            catch (AggregateException ex)
            {
                // iterate through the inner exceptions using
                // the handle method
                ex.Handle((inner) =>
                {
                    if (inner is OperationCanceledException)
                    {

                        // ...handle task cancellation...
                        return true;
                    }
                    else
                    {
                        // this is an exception we don't know how
                        // to handle, so return false
                        return false;
                    }
                });
            }
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
