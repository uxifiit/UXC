//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UXC.Core.Devices;
//using UXC.Core.Data;

//namespace UXC.Plugins.SessionsAPI.Recording
//{
//    public class InMemoryRecordingDataAccessBuffer2
//    {
//        private readonly ConcurrentDictionary<DeviceType, BlockingCollection<RecordingData>> _data;

//        public InMemoryRecordingDataAccessBuffer2(IEnumerable<DeviceType> devices)
//        {
//            var collections = devices?.Select(d => new KeyValuePair<DeviceType, BlockingCollection<RecordingData>>(d, new BlockingCollection<RecordingData>()))
//                           ?? Enumerable.Empty<KeyValuePair<DeviceType, BlockingCollection<RecordingData>>>();

//            _data = new ConcurrentDictionary<DeviceType, BlockingCollection<RecordingData>>(collections);
//        }


//        public void Add(DeviceType device, RecordingData data)
//        {
//            BlockingCollection<RecordingData> coll;


//            if (_data.TryGetValue(device, out coll) == false
//                || coll.IsAddingCompleted)
//            {
//                coll = new BlockingCollection<RecordingData>();
//                coll.Add(data);

//                _data.AddOrUpdate(device, coll, (_, __) => coll);
//            }
//            else
//            {
//                coll.Add(data);
//            }


//            //while (_data.TryGetValue(device, out coll) 
//            //    && (coll.IsAddingCompleted || coll.TryAdd(data) == false))
//            //{
//            //}
//        }


//        public IList<RecordingData> GetLastData(DeviceType device)
//        {
//            var list = _data[device];

//            // alternatively use GetConsumingEnumerable - but that blocks the reader if the collection is empty.
//            //if (list.Any())
//            //{
//            //    List<RecordingData> items = new List<RecordingData>();
//            //    foreach (var item in list.GetConsumingEnumerable())
//            //    {
//            //        items.Add(item);
//            //        if (items.Any() == false)
//            //        {
//            //            break;
//            //        }
//            //    }
//            //}


//            BlockingCollection<RecordingData> buffer = _data.AddOrUpdate
//            (
//                device, 
//                (key) => new BlockingCollection<RecordingData>(), 
//                (key, previous) => 
//                {
//                    lock (previous)
//                    {
//                        previous.CompleteAdding();
//                        buffer = previous;
//                    }
//                    return previous;
//                }
//            );

//            if (buffer != null)
//            {
//                var data = buffer.ToList();

//                buffer.Dispose();

//                return data;
//            }

//            return null;
//        }


//        public bool TryGetNextPart(DeviceType device, DateTime fromTimeStamp, out RecordingData nextData)
//        {
//            var parts = _recordingDataParts[device];
//            RecordingData data;

//            if (parts.Any())
//            {
//                while (parts.TryPeek(out data) && data.TimeStamp <= fromTimeStamp)
//                {
//                    parts.TryDequeue(out data);
//                }

//                if (parts.TryPeek(out data))
//                {
//                    nextData = data;
//                    return true;
//                }
//            }

//            nextData = null;
//            return false;
//        }


//        public bool HasData(DeviceType device)
//        {
//            ConcurrentQueue<RecordingData> queue;
//            return _recordingDataParts.TryGetValue(device, out queue)
//                && queue.Any();
//        }


//        public void Close()
//        {
//            var queues = _recordingDataParts.Values.ToList();
//            _recordingDataParts.Clear();
//        }
//    }
//}
