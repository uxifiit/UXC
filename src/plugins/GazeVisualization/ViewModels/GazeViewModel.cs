/**
 * GazeVisualization
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
using System.Reactive.Linq;
using System.Reactive;
using System.Threading.Tasks;
using GazeVisualization.Observers;
using UXC.Core.Data;
using UXI.Common.UI;
using UXC.Core.ViewModels;
using System.Diagnostics;
using System.Reactive.Subjects;
using UXI.GazeToolkit.Fixations;
using UXI.GazeToolkit.Fixations.VelocityThreshold;
using UXI.GazeToolkit.Interpolation;
using UXI.GazeToolkit.Selection;
using UXI.GazeToolkit.Smoothing;
using UXI.GazeToolkit;

namespace GazeVisualization.ViewModels
{
    class GazeViewModel : BindableBase
    {
        private readonly GazeObserver _observer;
               
        public GazeViewModel(GazeObserver observer)
        {
            _observer = observer;
            Setup();
        }


        public FixationFilterSettingsViewModel Settings { get; } = new FixationFilterSettingsViewModel();


        private void Setup()
        {
            var gaze = _observer.RawGaze.ObserveOnDispatcher();

            var eyeGaze = gaze.CombineLatest(Settings.FillInGapsOptions, (data, options) => options != null ? data.FillInGaps(options) : data)
                              .CombineLatest(Settings.EyeSelectionOptions, (data, options) => options != null ? data.SelectEye(options) : Observable.Empty<SingleEyeGazeData>())
                              .CombineLatest(Settings.NoiseReductionOptions, (data, options) => options != null ? data.ReduceNoise(options) : data);

            var velocities = eyeGaze.CombineLatest(Settings.FixationsClassificationOptions, (data, options) => options != null ? data.CalculateVelocities(options) : Observable.Empty<EyeVelocity>());

            var fixations = velocities.CombineLatest(Settings.FixationsClassificationOptions, (data, options) => options != null ? data.ClassifyByVelocity(options) : Observable.Empty<EyeMovement>())
                                      .CombineLatest(Settings.MergeAdjacentFixationsOptions, (data, options) => options != null ? data.MergeAdjacentFixations(options) : data)
                                      .CombineLatest(Settings.DiscardShortFixationsOptions, (data, options) => options != null ? data.DiscardShortFixations(options) : data);


            gaze.Switch().Subscribe(g =>
            {
                RawLeftEyePoint.UpdatePosition(g.LeftEye.GazePoint2D.X, g.LeftEye.GazePoint2D.Y);
                RawRightEyePoint.UpdatePosition(g.RightEye.GazePoint2D.X, g.RightEye.GazePoint2D.Y);
            }, () =>
            {
                RawLeftEyePoint.IsVisible = false;
                RawRightEyePoint.IsVisible = false;
            });

            eyeGaze.Switch().Subscribe(
                g => SingleEyePoint.UpdatePosition(g.GazePoint2D.X, g.GazePoint2D.Y), 
                () => SingleEyePoint.IsVisible = false
            );

            fixations.Switch().Subscribe(
                g => FixationPoint.UpdatePosition(g.Position.X, g.Position.Y), 
                () => FixationPoint.IsVisible = false
            );
        }


        public PointViewModel RawLeftEyePoint { get; } = new PointViewModel();


        public PointViewModel RawRightEyePoint { get; } = new PointViewModel();


        public PointViewModel SingleEyePoint { get; } = new PointViewModel();


        public PointViewModel FixationPoint { get; } = new PointViewModel();
    }
}
