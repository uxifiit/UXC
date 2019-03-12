/**
 * UXC
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
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using UXC.Core;
using UXC.Core.Devices;

namespace UXC.Common.Converters
{
    public class ControlServiceStateToBrushConverter : DependencyObject, IValueConverter
    {
        public Brush StoppedBrush
        {
            get { return (Brush)GetValue(StoppedBrushProperty); }
            set { SetValue(StoppedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisconnectedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StoppedBrushProperty =
            DependencyProperty.Register(nameof(StoppedBrush), typeof(Brush), typeof(ControlServiceStateToBrushConverter), new PropertyMetadata(new SolidColorBrush()));


        public Brush RunningBrush
        {
            get { return (Brush)GetValue(RunningBrushProperty); }
            set { SetValue(RunningBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadyBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RunningBrushProperty =
            DependencyProperty.Register(nameof(RunningBrush), typeof(Brush), typeof(ControlServiceStateToBrushConverter), new PropertyMetadata(new SolidColorBrush()));

        public Brush ErrorBrush
        {
            get { return (Brush)GetValue(ErrorBrushProperty); }
            set { SetValue(ErrorBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CalibrationBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorBrushProperty =
            DependencyProperty.Register(nameof(ErrorBrush), typeof(Brush), typeof(ControlServiceStateToBrushConverter), new PropertyMetadata(new SolidColorBrush()));


        public Brush DefaultBrush
        {
            get { return (Brush)GetValue(DefaultBrushProperty); }
            set { SetValue(DefaultBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultBrushProperty =
            DependencyProperty.Register(nameof(DefaultBrush), typeof(Brush), typeof(ControlServiceStateToBrushConverter), new PropertyMetadata(new SolidColorBrush()));


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ControlServiceState state = (ControlServiceState)value;
            switch (state)
            {
                case ControlServiceState.Stopped:
                    return StoppedBrush;
                case ControlServiceState.Error:
                    return ErrorBrush;
                case ControlServiceState.Running:
                    return RunningBrush;
            }

            return DefaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
