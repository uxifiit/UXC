//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Threading;

//namespace UXC.Common.Helpers
//{
//    public static class DispatcherTimerEx
//    {

//        public static bool ClearTimer(ref DispatcherTimer timer)
//        {
//            if (timer != null && timer.IsEnabled)
//            {
//                timer.IsEnabled = false;
//                timer = null;
//                return true;
//            }
//            return false;
//        }


        

//        public static bool TrySchedule(ref DispatcherTimer timer, DateTime time, Action action)
//        {
//            ClearTimer(ref timer);

//            TimeSpan interval = time - DateTime.Now;
            
//            return TrySchedule(ref timer, interval, action);
//        }

//        public static bool TrySchedule<T>(ref DispatcherTimer timer, DateTime time, Action<T> action, T tag)
//        {
//            ClearTimer(ref timer);

//            TimeSpan interval = time - DateTime.Now;

//            return TrySchedule<T>(ref timer, interval, action, tag);
//        }

//        public static bool TrySchedule(ref DispatcherTimer timer, TimeSpan interval, Action action)
//        {
//            //logger.Info(LogHelper.Prepare("time: " + interval.ToString()));

//            if (interval >= TimeSpan.Zero)
//            {
//                Schedule(ref timer, interval, action);
//                return true;
//            }
//            return false;
//        }

//        public static bool TrySchedule<T>(ref DispatcherTimer timer, TimeSpan interval, Action<T> action, T tag)
//        {
//            //logger.Info(LogHelper.Prepare("time: " + interval.ToString()));

//            if (interval >= TimeSpan.Zero)
//            {
//                Schedule<T>(ref timer, interval, action, tag);
//                return true;
//            }
//            return false;
//        }


//        public static void Schedule(ref DispatcherTimer timer, TimeSpan interval, Action action)
//        {
//            ClearTimer(ref timer);

//            //logger.Info(LogHelper.Prepare("time: " + interval.ToString()));
           
//            timer = new DispatcherTimer()
//            {
//                Interval = interval
//            };
//            timer.Tick += (o, e) =>
//            {
//                //logger.Info(LogHelper.Prepare("executing timer"));

//                DispatcherTimer dt = (DispatcherTimer)o;
//                dt.IsEnabled = false;

//                action.Invoke();
//            };
//            timer.IsEnabled = true;
//            //logger.Info(LogHelper.Prepare("enabled"));
//        }

//        public static void Schedule<T>(ref DispatcherTimer timer, TimeSpan interval, Action<T> action, T tag)
//        {
//            ClearTimer(ref timer);

//            //logger.Info(LogHelper.Prepare("time: " + interval.ToString()));

//            timer = new DispatcherTimer()
//            {
//                Interval = interval,
//                Tag = tag
//            };
//            timer.Tick += (o, e) =>
//            {
//                //logger.Info(LogHelper.Prepare("executing timer"));

//                DispatcherTimer dt = (DispatcherTimer)o;
//                dt.IsEnabled = false;

//                T data = (T)tag;

//                action.Invoke(data);
//            };
//            timer.IsEnabled = true;
//            //logger.Info(LogHelper.Prepare("enabled"));
//        }

//        public static void ScheduleOrInvoke(ref DispatcherTimer timer, TimeSpan interval, Action action)
//        {
//            bool scheduled = TrySchedule(ref timer, interval, action);
//            if (scheduled == false)
//            {
//                action.Invoke();
//            }
//        }

//        public static void ScheduleOrInvoke(ref DispatcherTimer timer, DateTime time, Action action)
//        {
//            bool scheduled = TrySchedule(ref timer, time, action);
//            if (scheduled == false)
//            {
//                action.Invoke();
//            }
//        }

//        public static void ScheduleOrInvoke<T>(ref DispatcherTimer timer, TimeSpan interval, Action<T> action, T tag)
//        {
//            bool scheduled = TrySchedule<T>(ref timer, interval, action, tag);
//            if (scheduled == false)
//            {
//                action.Invoke(tag);
//            }
//        }

//        public static void ScheduleOrInvoke<T>(ref DispatcherTimer timer, DateTime time, Action<T> action, T tag)
//        {
//            bool scheduled = TrySchedule<T>(ref timer, time, action, tag);
//            if (scheduled == false)
//            {
//                action.Invoke(tag);
//            }
//        }
//    }
//}
