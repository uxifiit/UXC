using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;

namespace UXC.Sessions
{
    public class ProcessService : IProcessService
    {
        private readonly Dictionary<string, List<int>> _processes = new Dictionary<string, List<int>>();

        public void Reset()
        {
            CloseAll(true, null);
        }

        public void Add(int processId, string tag)
        {
            string key = GetKey(tag);

            _processes.AddOrUpdate(key, _ => new List<int>() { processId }, (_, list) => { list.Add(processId); return list; });
        }


        private string GetKey(string tag)
        {
            string key = tag?.Trim();

            return String.IsNullOrWhiteSpace(key)
                 ? String.Empty
                 : key;
        }


        public bool Close(string tag, bool killIfNotClosed, TimeSpan? killTimeout)
        {
            string key = GetKey(tag);

            List<int> processIds;

            if (_processes.TryGetValue(key, out processIds))
            {
                _processes.Remove(key);

                return CloseProcesses(processIds, killIfNotClosed, killTimeout);
            }

            return false;
        }


        public bool CloseAll(bool killIfNotClosed, TimeSpan? killTimeout)
        {
            var processIds = _processes.Values.SelectMany(l => l).ToList();
            _processes.Clear();

            if (processIds.Any())
            {
                return CloseProcesses(processIds, killIfNotClosed, killTimeout);
            }
            return false;
        }


        public bool CloseProcesses(IEnumerable<int> processIds, bool killIfNotClosed, TimeSpan? killTimeout)
        {
            if (processIds != null && processIds.Any())
            {
                processIds.Select(id => TryGetProcess(id))
                          .Where(process => process != null)
                          .Execute(process => CloseProcess(process, killIfNotClosed, killTimeout))
                          .ForEach(process => process.Dispose());

                return true;
            }

            return false;
        }


        private static Process TryGetProcess(int processId)
        {
            try
            {
                return Process.GetProcessById(processId);
            }
            catch
            {
                return null;
            }
        }


        public void CloseProcess(Process process, bool killIfNotClosed, TimeSpan? killTimeout = null)
        {
            if (process != null && process.HasExited == false)
            {
                bool closed = CloseProcessGracefully(process);

                if ((closed == false || process.HasExited == false) && killIfNotClosed)
                {
                    KillProcess(process);
                }
            }
        }

        public bool CloseProcessGracefully(Process process)
        {
            process.ThrowIfNull(nameof(process));

            if (process.HasExited == false)
            {
                return process.CloseMainWindow();
            }

            return true;
        }

        public void KillProcess(Process process)
        {
            process.ThrowIfNull(nameof(process));

            if (process.HasExited == false)
            {
                try
                {
                    process.Kill();
                }
                catch (Exception ex)
                {
                    // TODO log exception
                }
            }
        }
    }
}
