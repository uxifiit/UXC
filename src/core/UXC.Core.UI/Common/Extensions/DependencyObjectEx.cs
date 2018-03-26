using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace UXC.Core.Common.Extensions
{
    public static class DependencyObjectEx
    {
        /// <summary>
        /// Gets the name of the descendant from.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="name">The name.</param>
        /// <returns>The FrameworkElement.</returns>
        public static IEnumerable<FrameworkElement> GetChildrenRecursive(this DependencyObject parent)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);

            if (count < 1)
            {
                yield break;
            }

            for (var i = 0; i < count; i++)
            {
                var frameworkElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (frameworkElement != null)
                {
                    yield return frameworkElement;

                    var children = GetChildrenRecursive(frameworkElement);
                    foreach (var child in children)
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}
