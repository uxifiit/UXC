using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RxBehaviorTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new UnitTest1().ReplaySubjectOfCompletedObservables();

            Console.ReadLine();
        }
    }
}
