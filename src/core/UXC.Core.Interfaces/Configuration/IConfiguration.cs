using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration;

namespace UXC.Core.Configuration
{
    /// <summary>
    /// Provides access the configuration of current object.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Gets the setting properties contained in the current <seealso cref="IConfiguration" />.
        /// </summary>
        IEnumerable<IConfigurationSettingProperty> Settings { get; }
    }
}
