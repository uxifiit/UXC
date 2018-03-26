using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjectBehaviorTest
{
    public class ClassA : IBaseClass
    {
        public string Message
        {
            get
            {
                return "Class A";
            }
        }
    }

    public class ClassB : IBaseClass
    {
        public string Message
        {
            get
            {
                return "Class B";
            }
        }
    }

    public class ClassC : IBaseClass
    {
        public string Message
        {
            get
            {
                return "Class C";
            }
        }
    }

    public class ClassD_A : IBaseClass
    {
        private readonly ClassA child;

        public ClassD_A(ClassA classA)
        {
            child = classA;
        }

        public string Message
        {
            get
            {
                return child.Message + ": Class D";
            }
        }
    }
}
