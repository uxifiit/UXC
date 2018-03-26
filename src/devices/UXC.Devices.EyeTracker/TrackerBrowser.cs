using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UXC.Core.Modules;
using UXC.Devices.EyeTracker.Configuration;
using UXI.Common.Helpers;

namespace UXC.Devices.EyeTracker
{
    public class TrackerBrowser
    {
        private readonly List<ITrackerFinder> _finders = new List<ITrackerFinder>();
        private readonly IEyeTrackerConfiguration _configuration;

        public TrackerBrowser(IEnumerable<ITrackerFinder> finders, IEyeTrackerConfiguration configuration, IModulesService modules)
        {
            _configuration = configuration;

            AddTrackerFinders(finders);
            modules.Register<ITrackerFinder>(this, AddTrackerFinders);
        }


        private void AddTrackerFinders(IEnumerable<ITrackerFinder> finders)
        {
            if (finders != null && finders.Any())
            {
                finders.Where(f => _finders.Contains(f) == false)
                       .ForEach(f => _finders.Add(f));
            }
        }


        private IEnumerable<ITrackerFinder> FilterFinders(IEnumerable<ITrackerFinder> finders, string selectedDriver)
        {
            if (finders.Any(f => f.Name.Equals(selectedDriver, StringComparison.InvariantCultureIgnoreCase)))
            {
                return finders.Where(f => f.Name.Equals(selectedDriver, StringComparison.InvariantCultureIgnoreCase));
            }

            return finders;
        }


        public async Task<IEyeTrackerDriver> SearchAsync(CancellationToken token) 
        {
            string selectedDriver = _configuration.SelectedDriver?.Trim();

            var finders = String.IsNullOrWhiteSpace(selectedDriver)
                        ? _finders.ToList()
                        : FilterFinders(_finders.ToList(), selectedDriver);

            if (finders.Any())
            {
                using (CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(token))
                {
                    var firstTracker = await Task.WhenAny(finders.Select(tf => tf.SearchAsync(cts.Token)));
                    return firstTracker.Result;
                }
            }
            return null;
        }
    }
}
