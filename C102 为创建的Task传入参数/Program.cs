using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C102_为创建的Task传入参数
{
    class Program
    {
        //我们之前提过，在创建Task的时候，我们在构造函数中传入了一个System.Action的委托，如果我们想要把一些参数传入到Task中，那么我 们可以传入System.Action<object>的委托，其中的那个object就是我们传入的参数。
         //注意：我们在传入参数后，必须把参数转换为它们原来的类型，然后再去调用相应的方法。例子中，因为System.Action对应的方法是printMessage()方法，而这个方法的要求的参数类型是string，所以要转换为string。

        static void Main(string[] args)
        {
            string[] messages = { "First task", "Second task",
"Third task", "Fourth task" };
            foreach (string msg in messages)
            {
                Task myTask = new Task(obj => printMessage((string)obj), msg);
                myTask.Start();
            }
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }

        static void printMessage(string message)
        {
            Console.WriteLine("Message: {0}", message);
        }
    }
}
