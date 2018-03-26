using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Configuration;
using UXC.Core.Managers;
using UXC.Core.Modules;
using UXC.Sessions.Serialization;
using UXI.Common.Extensions;

namespace UXC.Sessions.Recording
{
    public class SessionRecorderFactoryLocator
    {
        private readonly Dictionary<string, ISessionRecorderFactory> _factories = new Dictionary<string, ISessionRecorderFactory>();

        public SessionRecorderFactoryLocator(IEnumerable<ISessionRecorderFactory> factories, IModulesService modules)
        {
            AddFactories(factories);

            modules.Register<ISessionRecorderFactory>(this, AddFactories); 
        }


        private void AddFactories(IEnumerable<ISessionRecorderFactory> factories)
        {
            factories?.ForEach(factory => _factories.TryAdd(factory.Target.ToLower(), factory));
        }


        public IEnumerable<ISessionRecorder> CreateForRecording(SessionRecording recording)
        {
            recording.ThrowIfNull(nameof(recording));

            return recording.RecorderConfigurations
                            .Where(r => _factories.ContainsKey(r.Key.ToLower()))
                            .Select(r => 
                            {
                                var recorder = _factories[r.Key.ToLower()].Create(recording);
                                Configurator.Configure(recorder, r.Value);
                                return recorder;
                            })
                   ?? Enumerable.Empty<ISessionRecorder>();
        }
    }
}
