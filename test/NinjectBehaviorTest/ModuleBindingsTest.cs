using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Planning.Bindings;

namespace NinjectBehaviorTest
{

    [TestClass]
    public class ModuleBindingsTest
    {
        [TestMethod]
        public void SingleServicesBindingsTest()
        {
            StandardKernel kernel = new StandardKernel();
            var module = new MockModule(r =>
            {
                r.Bind<ClassA>().ToSelf().InSingletonScope();
                r.Bind<ClassB>().ToSelf().InSingletonScope();
            });

            kernel.Load(module);
            List<IBinding> bindings = module.Bindings.ToList();

            Assert.AreEqual(2, bindings.Count);

            foreach (var type in new Type[] { typeof(ClassA), typeof(ClassB) })
            {
                bool bound = bindings.Where(b => b.Service.Equals(type)).Any();
                Assert.IsTrue(bound);
            }
        }

        [TestMethod]
        public void SingleServiceMultipleBindingsTest()
        {
            StandardKernel kernel = new StandardKernel();
            var module = new MockModule(r =>
            {
                r.Bind<IBaseClass>().To<ClassA>().InSingletonScope();
                r.Bind<IBaseClass>().To<ClassB>().InSingletonScope();
            });

            kernel.Load(module);
            List<IBinding> bindings = module.Bindings.ToList();

            int count = bindings.Where(b => b.Service.Equals(typeof(IBaseClass))).Count();
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void MultipleServicesMultipleBindingsTest()
        {
            StandardKernel kernel = new StandardKernel();
            var module = new MockModule(r =>
            {
                r.Bind<IBaseClass, ClassA>().To<ClassA>().InSingletonScope();
                r.Bind<IBaseClass, ClassB>().To<ClassB>().InSingletonScope();
            });

            kernel.Load(module);
            List<IBinding> bindings = module.Bindings.ToList();

            foreach (var type in new Type[] { typeof(ClassA), typeof(ClassB) })
            {
                bool bound = bindings.Where(b => b.Service.Equals(type)).Any();
                Assert.IsTrue(bound);
            }

            int count = bindings.Where(b => b.Service.Equals(typeof(IBaseClass))).Count();
            Assert.AreEqual(2, count);
        }

    }
}
