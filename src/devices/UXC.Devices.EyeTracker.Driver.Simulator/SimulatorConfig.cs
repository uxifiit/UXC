using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker.Driver.Simulator
{
    class SimulatorConfig
    {
        public TrackBoxCoords TrackBox { get; } = new TrackBoxCoords()
        {
             FrontUpperRight = new Point3(
                x: -230.80000305175781,
                y: -187.69999694824219,
                z: 750.0
            ),
            FrontUpperLeft = new Point3(
                x: 230.80000305175781,
                y: -187.69999694824219,
                z: 750.0
            ),
            FrontLowerLeft = new Point3(
                x: -230.80000305175781,
                y: 184.69999694824219,
                z: 750.0
            ),
            FrontLowerRight = new Point3(
                x: 230.80000305175781,
                y: 184.69999694824219,
                z: 750.0
            ),
            BackUpperRight = new Point3(
                x: -134.5,
                y: -112.69999694824219,
                z: 450.0
            ),
            BackUpperLeft = new Point3(
                x: 134.5,
                y: -112.69999694824219,
                z: 450.0
            ),
            BackLowerLeft = new Point3(
                x: -134.5,
                y: 109.69999694824219,
                z: 450.0
            ),
            BackLowerRight = new Point3(
                x: 134.5,
                y: 109.69999694824219,
                z: 450.0
            )
        };

        public DisplayArea DisplayArea { get; } = new DisplayArea(
            bottomLeft: new Point3(
                x: -259.0,
                y: 13.294699668884277,
                z: -12.019499778747559
            ),
            topLeft: new Point3(
                x: -259.0,
                y: 317.7550048828125,
                z: 98.795097351074219
            ),
            topRight: new Point3(
                x: 259.0,
                y: 317.7550048828125,
                z: 98.795097351074219
            ));


        public Point3 EyePosition { get; } = new Point3(0, 50, 634);
        public Point3 RelativeEyePosition { get; } = new Point3(0.427537739276886, 0.35594475269317627, 0.62007302045822144);
        //public double PupillaryDistance { get; } = 60d;
        //public double GazePointDistance { get; } = 10d;

        public double PupilDilation { get; } = 2.6d;
    }
}
