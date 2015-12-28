using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S404
{
    class Program
    {
        static void Main(string[] args)
        {
  //          在操作task的时候，只要出现了异常，.NET Framework就会把这些异常记录下来。例如在执行Task.Wait(),Task.WaitAll(),Task.WaitAny(),Task.Result.不管那里出现了异常，最后抛出的就是一个System.AggregateException.

  // System.AggregateException时用来包装一个或者多个异常的，这个类时很有用的，特别是在调用Task.WaitAll()方法时。因为在Task.WaitAll()是等待多个task执行完成，如果有任意task执行出了异常，那么这个异常就会被记录在System.AggregateException中，不同的task可能抛出的异常不同，但是这些异常都会被记录下来。


  //下面就是给出一个例子：在例子中，创建了两个task，它们都抛出异常。然后主线程开始运行task，并且调用WaitAll()方法，然后就捕获抛出的System.AggregateException,显示详细信息。

  //          从下面的例子可以看出，为了获得被包装起来的异常，需要调用System.AggregateException的InnerExceptions属性，这个属性返回一个异常的集合，然后就可以遍历这个集合。

  //而且从下面的例子可以看到：Exeception.Source属性被用来指明task1的异常时ArgumentOutRangeException.

            // create the tasks
            Task task1 = new Task(() =>
            {
                ArgumentOutOfRangeException exception = new ArgumentOutOfRangeException();
                exception.Source = "task1";
                throw exception;
            });
            Task task2 = new Task(() =>
            {
                throw new NullReferenceException();
            });
            Task task3 = new Task(() =>
            {
                Console.WriteLine("Hello from Task 3");
            });
            // start the tasks
            task1.Start(); task2.Start(); task3.Start();
            // wait for all of the tasks to complete
            // and wrap the method in a try...catch block
            try
            {
                Task.WaitAll(task1, task2, task3);
            }
            catch (AggregateException ex)
            {
                // enumerate the exceptions that have been aggregated
                foreach (Exception inner in ex.InnerExceptions)
                {
                    Console.WriteLine("Exception type {0} from {1}",
                    inner.GetType(), inner.Source);
                }
            }
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
