/**
 * UXC.Devices.EyeTracker.Calibration.UI
 * Copyright (c) 2018 The UXC Authors
 * 
 * Licensed under GNU General Public License 3.0 only.
 * Some rights reserved. See COPYING, AUTHORS.
 *
 * SPDX-License-Identifier: GPL-3.0-only
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UXC.Core.Data;
using UXC.Core.ViewModels;
using UXC.Devices.EyeTracker.Calibration;
using UXC.Devices.EyeTracker.ViewModels;
using UXI.Common.Extensions;
using UXI.Common.Helpers;

namespace UXC.Devices.EyeTracker.Views
{
    /// <summary>
    /// Interaction logic for CalibrationControl.xaml
    /// </summary>
    public partial class CalibrationControl : UserControl
    {
        private CalibrationViewModel _calibration;
        private Window _window;

        private bool isRunning = false;

        public CalibrationControl()
        {
            this.DataContextChanged += Control_DataContextChanged;
            this.Loaded += Control_Loaded;
            InitializeComponent();
        }


        private void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _calibration = e.NewValue as CalibrationViewModel;
        }


        async void Control_Loaded(object sender, RoutedEventArgs e)
        {
            _window = Window.GetWindow(this);

            if (isRunning == false)
            {
                isRunning = true;

                await RunCalibrationAsync();
            }
            else
            {
                // TODO
            }
        }


        private static KeyEventHandler CreateKeyCancelEventHandler(CancellationTokenSource cts)
        {
            return (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Escape)
                {
                    e.Handled = true;
                    cts.Cancel();
                }
            };
        }


        private static CancelEventHandler CreateCancelEventHandler(CancellationTokenSource cts)
        {
            return (s, e) =>
            {
                cts.Cancel();
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>See Tobii docs p. 20.</remarks>
        private async Task RunCalibrationAsync()
        {
            bool cancelled = false;
            bool failed = false;
            var calibration = _calibration;

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                KeyEventHandler keyCancelEventHandler = CreateKeyCancelEventHandler(cts);
                CancelEventHandler cancelEventHandler = CreateCancelEventHandler(cts);

                try
                {
                    if (_window != null)
                    {
                        _window.KeyDown += keyCancelEventHandler;
                        _window.Closing += cancelEventHandler;
                    }

                    VisualStateManager.GoToElementState((FrameworkElement)this.Content, nameof(IntroductionState), true);
                    await Task.Delay(3000, cts.Token);

                    VisualStateManager.GoToElementState((FrameworkElement)this.Content, nameof(CalibrationProcessState), true);
                    await Task.Delay(300, cts.Token);

                    var failedTask = AsyncHelper.InvokeAsync<EventHandler<Exception>>
                    (
                        () => { },
                        h => calibration.Failed += h,
                        h => calibration.Failed -= h, 
                        tcs => (_, ex) => tcs.TrySetException(ex),
                        cts.Token
                    );

                    var completedTask = AsyncHelper.InvokeAsync<EventHandler<CalibrationExecutionReport>>
                    (
                        () => calibration.Start(),
                        h => calibration.Completed += h, 
                        h => calibration.Completed -= h,
                        tcs => (_, __) => tcs.TrySetResult(true),
                        cts.Token
                    );

                    var finishedTask = await Task.WhenAny(failedTask, completedTask);

                    if (finishedTask.IsCanceled)
                    {
                        cancelled = true;
                    }
                    else if (finishedTask == failedTask)
                    {
                        failed = true;
                        // TODO 10/09/2016 add error
                        // show message based on the exception
                    }

                    // TODO 12/10/2016 Show loading if the evaluation takes too long.
                    // await calibration.FinishAsync();
                }
                catch (OperationCanceledException)
                {
                    cancelled = true;
                }
                catch (Exception)
                {
                    failed = true;
                    // Show unknown error
                }
                finally
                {
                    if (_window != null)
                    {
                        _window.KeyDown -= keyCancelEventHandler;
                        _window.Closing -= cancelEventHandler;
                    }
                }
            }

            if (cancelled)
            {
                calibration?.Cancel();
            }
            else if (failed)
            {
                VisualStateManager.GoToElementState((FrameworkElement)this.Content, nameof(ErrorState), true);
            }
        }

        private void pointControl_TargetPointReached(object sender, Core.Controls.PointReachedEventArgs e)
        {
            var control = (FrameworkElement)sender;
            var animation = (PointAnimationViewModel)control.DataContext;
            if (animation != null)
            {
                animation.CompletePoint(e.Point);
            }
        }
    }
}
