//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UXI.Common.Extensions;

//namespace UXC.Sessions.Recording
//{
//    public class SessionRecorderTarget
//    {
//        private static readonly List<SessionRecorderTarget> targets = new List<SessionRecorderTarget>();
//        public static IEnumerable<SessionRecorderTarget> Targets => targets;

//        public static readonly SessionRecorderTarget Local = new SessionRecorderTarget("Local");
//        //public static readonly SessionRecorderTarget None = new SessionRecorderTarget("None"); // not needed yet

//        private SessionRecorderTarget(string target)
//        {
//            Target = target;

//            targets.Add(this);
//        }   

//        public string Target { get; }

//        public static bool Contains(string target)
//        {
//            return targets.Any(t => t.Target.Equals(target, StringComparison.InvariantCultureIgnoreCase));
//        }

//        public static SessionRecorderTarget GetOrCreate(string target)
//        {
//            return targets.FirstOrDefault(t => t.Target.Equals(target, StringComparison.InvariantCultureIgnoreCase))
//                ?? new SessionRecorderTarget(target);
//        }
//    }
//}
