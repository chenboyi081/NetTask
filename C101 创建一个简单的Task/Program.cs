using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C101_创建一个简单的Task
{
    class Program
    {
        static void Main(string[] args)
        {
            //为了执行一个简单的Task，一般进行以下步骤：
            //首先，要创建一个Task类的实例，
            //然后，传入一个System.Action委托，这个委托中的方法就是这个Task运行时你要执行的方法，而且这个委托必须作为Task构造函数的一个参数传入。我们在传入委托作为参数的时候有多种方式：传入匿名委托，Lambda表达式或者一个显示什么方法的委托。
            //最后，调用Task实例的Start()方法来运行

            // use an Action delegate and a named method
            Task task1 = new Task(new Action(printMessage));
            // use a anonymous delegate
            Task task2 = new Task(delegate
            {
                printMessage();
            });

            // use a lambda expression and a named method
            Task task3 = new Task(() => printMessage());
            // use a lambda expression and an anonymous method
            Task task4 = new Task(() =>
            {
                printMessage();
            });

            task1.Start();
            task2.Start();
            task3.Start();
            task4.Start();
            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }

        static void printMessage()
        {
            Console.WriteLine("Hello World");
        }
    }
}
