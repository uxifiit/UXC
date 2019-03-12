/**
 * UXC.Devices.EyeTracker.Driver.Simulator
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXC.Core.Data;

namespace UXC.Devices.EyeTracker.Driver.Simulator
{
    class GazePointGenerator
    {
        private readonly SimulatorConfig _config;
        private readonly Point3 _height;
        private readonly Point3 _width;

        public GazePointGenerator(SimulatorConfig config)
        {
            _config = config;

            _width = (_config.DisplayArea.TopRight - _config.DisplayArea.TopLeft);
            _height = (_config.DisplayArea.BottomLeft - _config.DisplayArea.TopLeft);
        }


        public GazeData CreateValidData(Point2 gazePoint2d)
        {
            GazeDataValidity validity = GazeDataValidity.Both;
            var timestamp = DateTime.Now;

            var dx = _width * gazePoint2d.X;
            var dy = _height * gazePoint2d.Y;
            var gazePoint3d = _config.DisplayArea.TopLeft + (dx + dy);

            var gaze = new GazeData
            (
                validity,
                new EyeGazeData
                (
                    validity.GetLeftEyeValidity(),
                    gazePoint2d,
                    gazePoint3d,
                    _config.EyePosition,
                    _config.RelativeEyePosition,
                    _config.PupilDilation
                ),
                new EyeGazeData
                (
                    validity.GetRightEyeValidity(),
                    gazePoint2d,
                    gazePoint3d,
                    _config.EyePosition,
                    _config.RelativeEyePosition,
                    _config.PupilDilation
                ),
                timestamp.Ticks / 10,
                timestamp
            );

            return gaze;
        }


        public GazeData CreateInvalidData()
        {
            var timestamp = DateTime.Now;

            var gaze = new GazeData(
                GazeDataValidity.None,
                EyeGazeData.Empty, 
                EyeGazeData.Empty,
                timestamp.Ticks / 10,
                timestamp
            );

            return gaze;
        }
    }
}
