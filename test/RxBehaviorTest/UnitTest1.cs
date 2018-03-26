using System;
using System.Reactive.Linq;
using System.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Subjects;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace RxBehaviorTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReplaySubjectOfCompletedObservables()
        {
            ReplaySubject<IObservable<string>> subject = new ReplaySubject<IObservable<string>>();

            subject.OnNext(CreateObservable("A"));
            subject.OnNext(CreateObservable("B"));
            subject.OnNext(CreateObservable("C"));


            Subscribe("first", subject);

            Task.Delay(400).Wait();
            
            Subscribe("second", subject);
        }


        public IObservable<string> CreateObservable(string prefix)
        {
            TimeSpan dueTime = TimeSpan.FromMilliseconds(100);
            return Observable.Generate(0, i => i < 5, i => i + 1, i => prefix + " " + i, i => dueTime).Publish().RefCount();
        }

        public void Subscribe(string name, IObservable<IObservable<string>> strings)
        {
            strings.Subscribe(s => s.Subscribe(v => Console.WriteLine(name + ": " + v)));
        }
    }
}
