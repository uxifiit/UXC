using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjectBehaviorTestLibrary
{
    public interface Interface
    {
        void Method();
        int Property { get; }
    }

    internal interface InternalInterface
    {
        int InternalProperty { get; }
    }
    internal class InterfaceImplementation : Interface, InternalInterface
    {
        public int Property
        {
            get
            {
                return 5;
            }
        }

        public void Method()
        {
        }

        public int InternalProperty
        {
            get { return 20; }

        }
    }

    public interface IConsumer
    {
        int GetValue();
    }

    public class ImplementationConsumer : IConsumer
    {
        private readonly InternalInterface _impl;
        internal ImplementationConsumer(InterfaceImplementation impl)
        {
            _impl = impl;
        }

        public int GetValue()
        {
            return _impl.InternalProperty;
        }
    }

    public class Module : NinjectModule
    {
        public override void Load()
        {
            Bind<Interface, InternalInterface, InterfaceImplementation>().To<InterfaceImplementation>().InSingletonScope();
            Bind<IConsumer, ImplementationConsumer>().To<ImplementationConsumer>().InSingletonScope();
        }
    }
}
