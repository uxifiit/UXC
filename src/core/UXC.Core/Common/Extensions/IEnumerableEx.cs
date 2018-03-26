using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class IEnumerableEx
    {
        public static ObservableCollection<TSource> ToObservableCollection<TSource>(this IEnumerable<TSource> source)
        {
            return new ObservableCollection<TSource>(source ?? Enumerable.Empty<TSource>());
        }


        public static void Enumerate<TSource>(this IEnumerable<TSource> source)
        {
            foreach (var _ in source) { }
        }
    }
}
