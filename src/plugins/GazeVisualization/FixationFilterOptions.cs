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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.GazeToolkit.Fixations;
using UXI.GazeToolkit.Fixations.VelocityThreshold;
using UXI.GazeToolkit.Interpolation;
using UXI.GazeToolkit.Selection;
using UXI.GazeToolkit.Smoothing;

namespace GazeVisualization
{
    public class BaseOptions
    {
        public BaseOptions Clone() { return (BaseOptions)this.MemberwiseClone(); }

        public event EventHandler OptionsChanged;
        protected void OnOptionsChanged() => OptionsChanged?.Invoke(this, EventArgs.Empty);
    }


    public class FillInGapsOptions : BaseOptions, IFillInGapsOptions
    {
        private TimeSpan maxGapDuration = TimeSpan.FromMilliseconds(75);
        public TimeSpan MaxGapDuration { get { return maxGapDuration; } set { maxGapDuration = value; OnOptionsChanged(); } }
    }


    public class EyeSelectionOptions : BaseOptions, IEyeSelectionOptions
    {
        private EyeSelectionStrategy eyeSelectionStrategy = EyeSelectionStrategy.Average;
        public EyeSelectionStrategy EyeSelectionStrategy { get { return eyeSelectionStrategy; } set { eyeSelectionStrategy = value; OnOptionsChanged(); } }
    }


    public class NoiseReductionOptions : BaseOptions, INoiseReductionOptions
    {
        private NoiseReductionStrategy noiseReductionStrategy = NoiseReductionStrategy.Exponential;
        public NoiseReductionStrategy NoiseReductionStrategy { get { return noiseReductionStrategy; } set { noiseReductionStrategy = value; OnOptionsChanged(); } }
    }


    public class FixationsClassificationOptions : BaseOptions, IVelocityCalculationOptions, IVelocityThresholdClassificationOptions
    {
        public const int DEFAULT_FREQUENCY = 60;

        private int? dataFrequency = DEFAULT_FREQUENCY;
        public int? DataFrequency { get { return dataFrequency; } set { dataFrequency = value; OnOptionsChanged(); } }

        private TimeSpan timeWindowHalfDuration = TimeSpan.FromMilliseconds(20);
        public TimeSpan TimeWindowHalfDuration { get { return timeWindowHalfDuration; } set { timeWindowHalfDuration = value; OnOptionsChanged(); } }

        private double velocityThreshold = 30d;
        public double VelocityThreshold { get { return velocityThreshold; } set { velocityThreshold = value; OnOptionsChanged(); } }
    }


    public class MergeAdjacentFixationsOptions : BaseOptions, IFixationsMergingOptions
    {
        private TimeSpan maxTimeBetweenFixations = TimeSpan.FromMilliseconds(75);
        public TimeSpan MaxTimeBetweenFixations { get { return maxTimeBetweenFixations; } set { maxTimeBetweenFixations = value; OnOptionsChanged(); } }


        private double maxAngleBetweenFixations = 0.5d;
        public double MaxAngleBetweenFixations { get { return maxAngleBetweenFixations; } set { maxAngleBetweenFixations = value; OnOptionsChanged(); } }
    }


    public class DiscardShortFixationsOptions : BaseOptions, IFixationsDiscardingOptions
    {
        private TimeSpan minimumFixationDuration = TimeSpan.FromMilliseconds(60);
        public TimeSpan MinimumFixationDuration { get { return minimumFixationDuration; } set { minimumFixationDuration = value; OnOptionsChanged(); } }
    }
}
