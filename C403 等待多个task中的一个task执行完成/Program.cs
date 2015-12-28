using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C403_等待多个task中的一个task执行完成
{
    class Program
    {
        static void Main(string[] args)
        {
             //可以用WaitAny()方法来等待多个task中的一个task执行完成。通俗的讲就是：有很多的task在运行，调用了WaitAny()方法之后，只要那些运行的task其中有一个运行完成了，那么WaitAny()就返回了。

            // create the cancellation token source
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            // create the cancellation token
            CancellationToken token = tokenSource.Token;
            // create the tasks
            Task task1 = new Task(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    // check for task cancellation
                    token.ThrowIfCancellationRequested();
                    // print out a message
                    Console.WriteLine("Task 1 - Int value {0}", i);
                    // put the task to sleep for 1 second
                    token.WaitHandle.WaitOne(1000);
                }
                Console.WriteLine("Task 1 complete");
            }, token);
            Task task2 = new Task(() =>
            {
                Console.WriteLine("Task 2 complete");
            }, token);
            // start the tasks
            task1.Start();
            task2.Start();
            // wait for the tasks
            Console.WriteLine("Waiting for tasks to complete.");
            int taskIndex = Task.WaitAny(task1, task2);
            Console.WriteLine("Task Completed. Index: {0}", taskIndex);
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
