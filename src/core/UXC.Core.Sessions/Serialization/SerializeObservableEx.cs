using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UXC.Core;
using UXC.Core.Data;
using UXC.Core.Data.Serialization;

namespace UXC.Sessions.Serialization
{
    public static class SerializeObservableEx
    {
        public static IObservable<TData> TakeUntilOtherCompletes<TData, TOther>(this IObservable<TData> data, IObservable<TOther> other)
        {
            return Observable.Create<TData>(o =>
                new CompositeDisposable
                (
                    other.IgnoreElements().Finally(o.OnCompleted).Subscribe(),
                    data.Subscribe(o)
                )
            );
        }


        public static IObservable<T> AttachWriter<T>(this IObservable<T> data, string path, IDataSerializationFactory factory, Type targetDataType)
        {
            return Observable.Create<T>(o =>
            {
                var streamWriter = new StreamWriter(path, true, new UTF8Encoding(false));
                var dataWriter = factory.CreateWriterForType(streamWriter, targetDataType);

                return new CompositeDisposable
                (
                    data.Do(d => dataWriter.Write(d))
                        .Finally(dataWriter.Close)
                        .Subscribe(o),
                    dataWriter,
                    streamWriter
                );
            });

            // removed this implementation due to exceptions being raised that the streamWriter was disposed sooner than the dataWriter. 
            //return Observable.Using
            //(
            //    () => new StreamWriter(path, true, new UTF8Encoding(false)),
            //    streamWriter => Observable.Using
            //    (
            //        () => factory.CreateWriterForType(streamWriter, targetDataType),
            //        dataWriter => data.Do(d => dataWriter.Write(d))
            //                          .Finally(dataWriter.Close)
            //    )
            //);
        }


        public static IObservable<T> AttachWriter<T>(this IObservable<T> data, string path, IDataSerializationFactory factory, Type targetDataType, int bufferSize)
        {
            return Observable.Create<T>(o =>
            {
                var streamWriter = new StreamWriter(path, true, new UTF8Encoding(false));
                var dataWriter = factory.CreateWriterForType(streamWriter, targetDataType);

                var bufferedWriter = new BufferedDataWriter(dataWriter, bufferSize);

                return new CompositeDisposable
                (
                    data.Do(d => bufferedWriter.Write(d))
                        .Finally(bufferedWriter.Close)
                        .Subscribe(o),
                    dataWriter,
                    streamWriter
                );
            });

            // removed this implementation due to exceptions being raised that the streamWriter was disposed sooner than the dataWriter. 
            //return Observable.Using
            //(
            //    () => new StreamWriter(path, true, new UTF8Encoding(false)),
            //    streamWriter => Observable.Using
            //    (
            //        () => factory.CreateWriterForType(streamWriter, targetDataType),
            //        dataWriter => Observable.Using
            //        (
            //            () => new BufferedDataWriter(dataWriter, bufferSize),
            //            bufferedWriter => data.Do(d => bufferedWriter.Write(d))
            //                                  .Finally(bufferedWriter.Close)
            //        )
            //    )
            //);
        }


        public static IObservable<T> AttachWriter<T>(this IObservable<T> data, string path, IDataSerializationFactory factory, int bufferSize)
        {
            return AttachWriter(data, path, factory, typeof(T), bufferSize);
        }


        public static IObservable<T> AttachWriter<T>(this IObservable<T> data, string path, IDataSerializationFactory factory)
        {
            return AttachWriter(data, path, factory, typeof(T));
        }
    }
}
