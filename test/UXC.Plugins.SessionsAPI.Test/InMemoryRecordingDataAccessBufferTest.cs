using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UXC.Core.Devices;
using UXC.Core.Data;
using UXC.Plugins.SessionsAPI.Recording;

namespace UXC.Plugins.SessionsAPI.Test
{
    [TestClass]
    public class InMemoryRecordingDataAccessBufferTest
    {
        private static readonly DeviceType deviceType = DeviceType.Physiological.EYETRACKER;
        private static ICollection<DeviceType> devices = new List<DeviceType>()
        {
            deviceType
        };

        private static readonly DateTime startDateTime = new DateTime(2017, 10, 10, 00, 37, 15);

        private static GazeData GenerateGazeData(int i)
        {
            return new GazeData
            (
                GazeDataValidity.Both,
                new EyeGazeData(EyeGazeDataValidity.Valid, new Point2(), new Point3(), new Point3(), new Point3(), 0.5d),
                new EyeGazeData(EyeGazeDataValidity.Valid, new Point2(), new Point3(), new Point3(), new Point3(), 0.5d),
                636431926355445200 + i * 100,
                startDateTime.AddMilliseconds(i)
            );
        }

        private static GazeData GenerateGazeData(int i, List<GazeData> container)
        {
            var data = GenerateGazeData(i);
            container.Add(data);
            return data;
        }


        [TestMethod]
        public void Test_TryGetNextPart_EmptyBuffer()
        {
            var buffer = new InMemoryRecordingDataAccessBuffer(devices);

            DeviceData data;
            bool received = buffer.TryGetNextPart(deviceType, DateTime.MinValue, out data);

            Assert.IsFalse(received);
            Assert.IsNull(data);
        }


        [TestMethod]
        public void Test_TryGetNextPart_FirstData()
        {
            var buffer = new InMemoryRecordingDataAccessBuffer(devices);

            var items = Enumerable.Range(0, 10)
                                  .Select(i => GenerateGazeData(i))
                                  .Execute(d => buffer.Add(deviceType, d))
                                  .ToList();

            DeviceData data;
            bool received = buffer.TryGetNextPart(deviceType, DateTime.MinValue, out data);

            Assert.IsTrue(received);
            Assert.AreEqual(items.First(), data);
        }


        [TestMethod]
        public void Test_TryGetNextPart_RepeatFirstData()
        {
            var buffer = new InMemoryRecordingDataAccessBuffer(devices);

            var items = Enumerable.Range(0, 10)
                                  .Select(i => GenerateGazeData(i))
                                  .Execute(d => buffer.Add(deviceType, d))
                                  .ToList();

            DeviceData data = items.First();

            var lastData = Enumerable.Repeat(DateTime.MinValue, 10)
                                     .Aggregate(data, (a, b) =>
                                     {
                                         Assert.IsTrue(buffer.TryGetNextPart(deviceType, b, out data));
                                         Assert.AreEqual(a, data);
                                         return data;
                                     });

            Assert.AreEqual(items.First(), data);
        }


        [TestMethod]
        public void Test_TryGetNextPart_WriteAllInReadAllOut()
        {
            var buffer = new InMemoryRecordingDataAccessBuffer(devices);

            var items = Enumerable.Range(0, 10)
                                  .Select(i => GenerateGazeData(i))
                                  .Execute(d => buffer.Add(deviceType, d))
                                  .ToList();

            int index = 0;

            DateTime fromTime = DateTime.MinValue;
            DeviceData previous = null;
            DeviceData data;

            while (buffer.TryGetNextPart(deviceType, fromTime, out data))
            {
                Assert.IsTrue(index < items.Count);
                Assert.AreEqual(items[index], data);
                Assert.AreNotEqual(previous, data);

                fromTime = data.Timestamp;
                index += 1;
                previous = data;
            }

            Assert.AreEqual(index, items.Count);
        }


        [TestMethod]
        public void Test_TryGetNextPart_SequentialWriteRead()
        {
            var buffer = new InMemoryRecordingDataAccessBuffer(devices);

            var originalData = Enumerable.Range(0, 100)
                                         .Select(i => GenerateGazeData(i))
                                         .ToList();

            DeviceData lastData = SequentialWriteReadData(buffer, originalData);

            Assert.AreEqual(originalData.Last(), lastData);
        }


        private static DeviceData SequentialWriteReadData(InMemoryRecordingDataAccessBuffer buffer, IEnumerable<DeviceData> dataCollection)
        {
            int counter = 0;
            int dataCount = 0;
            DeviceData data = null;
            var lastData = dataCollection.Execute(_ => dataCount += 1)
                                         .Execute(d => buffer.Add(deviceType, d))
                                         .Aggregate(data, (a, b) =>
                                         {
                                             Assert.IsTrue(buffer.TryGetNextPart(deviceType, a?.Timestamp ?? DateTime.MinValue, out data));
                                             Assert.AreEqual(b, data);
                                             counter += 1;
                                             return b;
                                         });

            Assert.AreEqual(dataCount, counter);
            return lastData;
        }


        [TestMethod]
        public void Test_TryGetNextPart_SequentialWriteRead_RecordingDataPart()
        {
            var buffer = new InMemoryRecordingDataAccessBuffer(devices);

            int dataCount = 10000;
            var originalData = Enumerable.Range(0, dataCount)
                                         .Select(i => GenerateGazeData(i))
                                         .ToList();

            int partSize = 100;
            var parts = Enumerable.Range(0, dataCount / partSize)
                                  .Select(i => originalData.Skip(i * partSize).Take(partSize))
                                  .Select(d => new DeviceDataPart(d))
                                  .ToList();

            DeviceData lastPart = SequentialWriteReadData(buffer, parts);

            Assert.AreEqual(parts.Last(), lastPart);
        }


        [TestMethod]
        public void Test_TryGetNextPart_ConcurrentWriteRead_RecordingDataPart()
        {
            var buffer = new InMemoryRecordingDataAccessBuffer(devices);
            Random random = new Random(0);

            using (var cts = new System.Threading.CancellationTokenSource())
            {
                bool isWriting = true;

                Task<List<DeviceData>> writeTask = Task.Run(() =>
                {
                    using (var ctsWrite = System.Threading.CancellationTokenSource.CreateLinkedTokenSource(cts.Token))
                    {
                        isWriting = true;

                        int dataCount = 10000;
                        var originalData = Enumerable.Range(0, dataCount)
                                                     .Select(i => GenerateGazeData(i))
                                                     .ToList();

                        int partSize = 100;
                        var parts = Enumerable.Range(0, dataCount / partSize)
                                              .Select(i => originalData.Skip(i * partSize).Take(partSize))
                                              .Select(d => new DeviceDataPart(d))
                                              .Cast<DeviceData>()
                                              .ToList();

                        parts.TakeUntil(_ => cts.Token.IsCancellationRequested)
                             .Execute(d => Task.Delay(random.Next(10)).Wait())
                             .ForEach(part => buffer.Add(deviceType, part));

                        isWriting = false;

                        return parts;
                    }
                }, cts.Token);


                Task<List<DeviceData>> readTask = Task.Run(() =>
                {
                    using (var ctsRead = System.Threading.CancellationTokenSource.CreateLinkedTokenSource(cts.Token))
                    {
                        DeviceData data = null;

                        return InfiniteCounter().TakeUntil(_ => cts.Token.IsCancellationRequested)
                                                .TakeWhile(_ => isWriting || buffer.HasData(deviceType))
                                                .Aggregate(new List<DeviceData>(), (list, _) =>
                        {
                            if (buffer.TryGetNextPart(deviceType, list.LastOrDefault()?.Timestamp ?? DateTime.MinValue, out data))
                            {
                                list.Add(data);
                            }
                            return list;
                        });
                    }
                }, cts.Token);


                // create 1 seconds timeout task that will trigger cancel of the cancellation token
                var timeoutTask = Task.Delay(TimeSpan.FromMinutes(1), cts.Token)
                                      .ContinueWith(_ => cts.Cancel(), TaskContinuationOptions.OnlyOnRanToCompletion);
                
                // execute tasks, wait for the first finished - timeout or both write and read tasks
                var completedTask = Task.WhenAny(timeoutTask, Task.WhenAll(writeTask, readTask)).GetAwaiter().GetResult();

                if (completedTask == timeoutTask && timeoutTask.Status == TaskStatus.RanToCompletion)
                {
                    Assert.Fail("Operations timed out.");
                }
                else
                {
                    // cancel timeoutTask
                    cts.Cancel();

                    var writtenData = writeTask.Result;
                    var readData = readTask.Result;

                    Assert.AreEqual(writtenData.Count, readData.Count);
                    CollectionAssert.AreEqual(writtenData, readData);
                }
            }
        }

        private static IEnumerable<int> InfiniteCounter()
        {
            int i = 0;
            while (true)
            {
                yield return i++;
            }
        }
    }
}
