using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Configuration
{
    /// <summary>
    /// Defines an interface for configuring an instance of <see cref="IConfigurable"/>.
    /// </summary>
    public interface IConfigurator
    {
        bool CanConfigure();

        /// <summary>
        /// Rewrites configuration settings in the associated configurable target, e.g., <see cref="IConfigurable"/> instance,  with values specified by the keys.
        /// Keys should match keys of the target's settings.
        /// </summary>
        /// <param name="values"></param>
        void Configure(IDictionary<string, object> values);

        /// <summary>
        /// Resets all configuration settings of the configurated target.
        /// </summary>
        /// <param name="target"></param>
        void ResetAll();

        /// <summary>
        /// Resets a single configuration setting of the configurated target depicted by the key. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        void Reset(string key);
    }
}
