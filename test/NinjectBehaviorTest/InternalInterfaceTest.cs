using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using NinjectBehaviorTestLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjectBehaviorTest
{
    [TestClass]
    public class InternalInterfaceTest
    {
        [TestMethod]
        public void RetrievePublicTypes()
        {
            StandardKernel kernel = new StandardKernel(new NinjectSettings() { InjectNonPublic = true });
            kernel.Load(new NinjectBehaviorTestLibrary.Module());

            IConsumer consumer = kernel.Get<IConsumer>();
              
            Assert.IsNotNull(consumer);

            Assert.AreEqual(20, consumer.GetValue());
        }
    }
}
