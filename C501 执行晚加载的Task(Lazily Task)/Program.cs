using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C501_执行晚加载的Task_Lazily_Task_
{
    class Program
    {
        static void Main(string[] args)
        {
       //     晚加载，或者又名延迟初始化，主要的好处就是避免不必要的系统开销。在并行编程中，可以联合使用Lazy变量和Task<>.Factory.StartNew()做到这点。(Lazy变量时.NET 4中的一个新特性，这里大家不用知道Lazy的具体细节)

       //Lazy变量只有在用到的时候才会被初始化。所以我们可以把Lazy变量和task的创建结合：只有这个task要被执行的时候才去初始化。

       //下面还是通过例子来讲解： 


            //首先我们回想一下，在之前的系列文章中我们是怎么定义一个task的：直接new，或者通过task的factory来创建，因为创建task的代码是在main函数中的，所以只要new了一个task，那么这个task就被初始化。现在如果用了Lazy的task，那么现在我们初始化的就是那个Lazy变量了，而没有初始化task，(初始化Lazy变量的开销小于初始化task)，只有当调用了lazyData.Value时，Lazy变量中包含的那个task才会初始化。（这里欢迎大家提出自己的理解） 

            // define the function
            Func<string> taskBody = new Func<string>(() =>
            {
                Console.WriteLine("Task body working...");
                return "Task Result";
            });

            // create the lazy variable
            Lazy<Task<string>> lazyData = new Lazy<Task<string>>(() =>
            Task<string>.Factory.StartNew(taskBody));

            Console.WriteLine("Calling lazy variable");
            Console.WriteLine("Result from task: {0}", lazyData.Value.Result);

            // do the same thing in a single statement
            Lazy<Task<string>> lazyData2 = new Lazy<Task<string>>(
            () => Task<string>.Factory.StartNew(() =>
            {
                Console.WriteLine("Task body working...");
                return "Task Result";
            }));

            Console.WriteLine("Calling second lazy variable");
            Console.WriteLine("Result from task: {0}", lazyData2.Value.Result);

            // wait for input before exiting
            Console.WriteLine("Main method complete. Press enter to finish.");
            Console.ReadLine();
        }
    }
}
