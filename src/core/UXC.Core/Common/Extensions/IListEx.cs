using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;

namespace UXC.Common.Extensions
{
    public static class IListEx
    {
        public static void TryAddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            list.ThrowIfNull(nameof(list));

            if (items != null)
            {
                foreach (var item in items)
                {
                    if (list.Contains(item) == false)
                    {
                        list.Add(item);
                    }
                }
            }
        }
    }
}
