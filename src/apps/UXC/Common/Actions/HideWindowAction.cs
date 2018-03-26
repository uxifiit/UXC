using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using MahApps.Metro.Controls;

namespace UXC.Common.Actions
{
    public class HideWindowAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            try
            {
                AssociatedObject?.Invoke(() => Window.GetWindow(AssociatedObject)?.Hide());
            }
            catch { }
        }
    }
}
