using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.ViewModels
{
    public interface ISettingsSectionViewModel
    {
        string Name { get; }

        void Save();

        void Reload();
    }
}
