using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Plugins.UXR.Extensions
{
    public static class IObservableEx
    {
        public static IObservable<T> RepeatAfterDelay<T>(this IObservable<T> source, TimeSpan delay, IScheduler scheduler)
        {
            var repeatSignal = Observable.Empty<T>()
                                         .Delay(delay, scheduler);

            return source.Concat(repeatSignal).Repeat();
        }
    }
}
