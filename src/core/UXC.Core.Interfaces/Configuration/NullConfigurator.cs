using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Configuration
{
    public class NullConfigurator : IConfigurator
    {
        private static readonly Lazy<NullConfigurator> instance = new Lazy<NullConfigurator>();
        public static NullConfigurator Instance => instance.Value;


        public bool CanConfigure() => false;


        public void Configure(IDictionary<string, object> values)
        {
        }

        public void Reset(string key)
        {
        }

        public void ResetAll()
        {
        }
    }
}
