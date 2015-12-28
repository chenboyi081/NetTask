using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C402_等待多个task
{
    class Program
    {
        static void Main(string[] args)
        {
  //          我们也可以用WaitAll()方法来一直到等待多个task执行完成。只有当所有的task执行完成，或者被cancel，或者抛出异常，这个方法才会返回。WiatAll()方法和Wait()方法一样有一些重载。
  //注意：如果在等在的多个task之中，有一个task抛出了异常，那么调用WaitAll()方法时就会抛出异常。
            //在上面的例子中，首先创建了两个task，注意我们创建的是可以被cancel的task，因为使用CancellationToken。而且在第一个task中还是用waitOne()休眠方法，其实目的很简单：使得这个task的运行时间长一点而已。之后我们就调用了WaitAll()方法，这个方法一直到两个task执行完成之后才会返回的。

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
            Task.WaitAll(task1, task2);
            Console.WriteLine("Tasks Completed.");
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
