using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C103_获取Task的执行结果
{
    class Program
    {
        static void Main(string[] args)
        {
             //如果要获取Task的结果，那么在创建Task的时候，就要采用Task<T>来实例化一个Task，其中的那个T就是task执行完成之后返回结果的类型。之后采用Task实例的Result属性就可以获取结果。

            // create the task
            Task<int> task1 = new Task<int>(() =>
            {
                int sum = 0;
                for (int i = 0; i < 100; i++)
                {
                    sum += i;
                }
                return sum;
            });

            task1.Start();
            // write out the result
            Console.WriteLine("Result 1: {0}", task1.Result);

            Console.ReadLine();

            #region 下面的代码展示了如何通过Task.Factory.StartNew<T>()创建一个Task，并且获取结果
            //// create the task
            //Task<int> task1 = Task.Factory.StartNew<int>(() =>
            //{
            //    int sum = 0;
            //    for (int i = 0; i < 100; i++)
            //    {
            //        sum += i;
            //    }
            //    return sum;
            //});

            //// write out the result
            //Console.WriteLine("Result 1: {0}", task1.Result);

            //Console.ReadLine(); 
            #endregion
        }
    }
}
