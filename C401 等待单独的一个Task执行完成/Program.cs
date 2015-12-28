using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C401_等待单独的一个Task执行完成
{
    class Program
    {
        static void Main(string[] args)
        {
  //           首先注意一点：这里提到的"等待"和之前文章提到的"休眠"意思是不一样的：
  //等待：在等待一个task的时候，这个task还是在运行之中的，"等待"相当于在监听运行的task的执行情况。
  //休眠：让tasku不运行
             //在上篇文章中介绍了如果从Task中获取执行后的结果：在Task执行完成之后调用Task.Result获取。其实也可以用其他的方法等待Task执行完成而不获取结果，这是很有用的：如果你想等待一个task完成之后再去做其他的事情。而且我们还可以等待一个task执行完成，或者等待所有的task执行完成，或者等待很多task中的一个执行完成。因为Task是由内部的Scheduler管理的，调用wait方法，其实就是我们在监控task的执行，看看这个task是否执行完了，如果完成，那么wanit方法就返回true，反之。
  //          从下面的例子可以看出，wait方法子task执行完成之后会返回true。
  //注意：当在执行的task内部抛出了异常之后，这个异常在调用wait方法时会被再次抛出。后面再"异常处理篇"会讲述。


            // create the cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            // create the cancellation token
            CancellationToken token = tokenSource.Token;
            // create and start the first task, which we will let run fully
            Task task = createTask(token);
            task.Start();

            // wait for the task
            Console.WriteLine("Waiting for task to complete.");
            task.Wait();
            Console.WriteLine("Task Completed.");

            // create and start another task
            task = createTask(token);
            task.Start();
            Console.WriteLine("Waiting 2 secs for task to complete.");
            bool completed = task.Wait(2000);
            Console.WriteLine("Wait ended - task completed: {0}", completed);

            // create and start another task
            task = createTask(token);
            task.Start();
            Console.WriteLine("Waiting 2 secs for task to complete.");
            completed = task.Wait(2000, token);
            Console.WriteLine("Wait ended - task completed: {0} task cancelled {1}",
            completed, task.IsCanceled);

            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }

        static Task createTask(CancellationToken token)
        {
            return new Task(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    // check for task cancellation
                    token.ThrowIfCancellationRequested();
                    // print out a message
                    Console.WriteLine("Task - Int value {0}", i);
                    // put the task to sleep for 1 second
                    token.WaitHandle.WaitOne(1000);
                }
            }, token);
        }
    }
}
