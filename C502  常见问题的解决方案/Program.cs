using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C502__常见问题的解决方案
{
    class Program
    {
        static void Main(string[] args)
        {
//            a.       Task 死锁
//描述：如果有两个或者多个task(简称TaskA)等待其他的task（TaskB）执行完成才开始执行，但是TaskB也在等待TaskA执行完成才开始执行，这样死锁就产生了。
 
//解决方案：避免这个问题最好的方法就是：不要使的task来依赖其他的task。也就是说，最好不要你定义的task的执行体内包含其他的task。
 
//例子：在下面的例子中，有两个task，他们相互依赖：他们都要使用对方的执行结果。当主程序开始运行之后，两个task也开始运行，但是因为两个task已经死锁了，所以主程序就一直等待。

            // define an array to hold the Tasks
            Task<int>[] tasks = new Task<int>[2];

            // create and start the first task
            tasks[0] = Task.Factory.StartNew(() =>
            {
                // get the result of the other task,
                // add 100 to it and return it as the result
                return tasks[1].Result + 100;
            });

            // create and start the second task
            tasks[1] = Task.Factory.StartNew(() =>
            {
                // get the result of the other task,
                // add 100 to it and return it as the result
                return tasks[1].Result + 100;
            });


            // wait for the tasks to complete
            Task.WaitAll(tasks);

            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
