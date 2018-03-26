using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace NinjectBehaviorTest
{
    [TestClass]
    public class BindingsTest
    {

        [TestMethod]
        public void GetAllAfterGetTwoInstancesDefaultTest()
        {
            StandardKernel kernel = new StandardKernel();
            kernel.Bind<ClassA>().ToSelf();

            var instance1 = kernel.Get<ClassA>();
            var instance2 = kernel.Get<ClassA>();

            Assert.AreNotSame(instance1, instance2);

            var instances = kernel.GetAll<ClassA>();

            Assert.AreEqual(1, instances.Count()); 
        }

        [TestMethod]
        public void GetAllAfterGetTwoInstancesTransientTest()
        {
            StandardKernel kernel = new StandardKernel();
            kernel.Bind<ClassA>().ToSelf().InTransientScope();

            var instance1 = kernel.Get<ClassA>();
            var instance2 = kernel.Get<ClassA>();

            Assert.AreNotSame(instance1, instance2);
        }

        [TestMethod]
        public void GetAllAfterGetTwoInstancesSingletonTest()
        {
            StandardKernel kernel = new StandardKernel();
            kernel.Bind<ClassA>().ToSelf().InSingletonScope();

            var instance1 = kernel.Get<ClassA>();
            var instance2 = kernel.Get<ClassA>();

            Assert.AreSame(instance1, instance2);
            Assert.AreEqual(1, kernel.GetAll<ClassA>().Count());
        }

        [TestMethod]
        public void GetAllAfterGetTwoInstancesTest()
        {
            StandardKernel kernel = new StandardKernel();
            kernel.Bind<IBaseClass>().To<ClassA>().InTransientScope();
            kernel.Bind<IBaseClass>().To<ClassB>().InTransientScope();

            var instances1 = kernel.GetAll<IBaseClass>();
            var instances2 = kernel.GetAll<IBaseClass>();


            foreach (var instance1 in instances1)
            {
                foreach (var instance2 in instances2)
                {
                    Assert.AreNotSame(instance1, instance2);
                }
            }

            Assert.AreEqual(2, kernel.GetAll<IBaseClass>().Count());
        }

    }
}
