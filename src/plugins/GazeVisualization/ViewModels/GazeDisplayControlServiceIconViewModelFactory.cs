using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using GazeVisualization.Services;
using UXC.Core.ViewModels;
using UXC.Core.ViewModels.Services;

namespace GazeVisualization.ViewModels
{
    public class GazeDisplayControlServiceIconViewModelFactory : IViewModelFactory
    {
        private readonly Dispatcher _dispatcher;

        public GazeDisplayControlServiceIconViewModelFactory(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }


        public Type SourceType { get; } = typeof(GazeDisplayControlService);

        public Type ViewModelType { get; } = typeof(ControlServiceViewModel);


        public object Create(object source)
        {
            return new ControlServiceViewModel((GazeDisplayControlService)source, _dispatcher, "Gaze Visualization");
        }
    }
}
