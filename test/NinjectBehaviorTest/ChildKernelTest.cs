using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Extensions.ChildKernel;

namespace NinjectBehaviorTest
{
    [TestClass]
    public class ChildKernelTest
    {
        [TestMethod]
        public void TestOneChildGetParent()
        {
            StandardKernel kernel = new StandardKernel();
            ChildKernel child = new ChildKernel(kernel);

            kernel.Bind<IBaseClass>().To<ClassA>();

            child.Bind<IBaseClass>().To<ClassB>();

            var classesFromParent = kernel.GetAll<IBaseClass>();
            Assert.AreEqual(1, classesFromParent.Count());

            var classesFromChild = child.GetAll<IBaseClass>();
            Assert.AreEqual(1, classesFromChild.Count());

        }

        [TestMethod]
        public void TestReferenceFromParent()
        {
            StandardKernel kernel = new StandardKernel();
            ChildKernel child = new ChildKernel(kernel);

            kernel.Bind<IBaseClass, ClassA>().To<ClassA>();
            child.Bind<IBaseClass, ClassD_A>().To<ClassD_A>();

            var classD_A = child.Get<ClassD_A>();
            Assert.IsNotNull(classD_A);

            Assert.AreEqual("Class A: Class D", classD_A.Message);
        }

        [TestMethod]
        public void TestMultipleNestingChild()
        {
            StandardKernel kernel = new StandardKernel();
            ChildKernel child1 = new ChildKernel(kernel);
            ChildKernel child2 = new ChildKernel(child1);

            kernel.Bind<ClassA>().ToSelf();
            child2.Bind<ClassD_A>().ToSelf();

            var classD_A = child2.Get<ClassD_A>();
            Assert.IsNotNull(classD_A);

            Assert.AreEqual("Class A: Class D", classD_A.Message);
        }
    }
}
