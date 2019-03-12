/**
 * UXC.Core.UI
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
using UXC.Core.Common.Extensions;
using UXC.Core.ViewModels;
using UXI.Common.Converters;
using UXI.Common.Extensions;
using UXI.Common.Helpers;

namespace UXC.Core.Controls
{
    /// <summary>
    /// Interaction logic for PointAnimationControl.xaml
    /// </summary>
    public partial class PointAnimationControl : UserControl
    {
        private const string STORYBOARD_SHRINK_POINT = "StoryboardShrinkPoint";
        private const string STORYBOARD_ENLARGE_POINT = "StoryboardEnlargePoint";

        private const string POSITION_CONVERTER_LEFT = "LeftPositionConverter";
        private const string POSITION_CONVERTER_TOP = "TopPositionConverter";

        private const double DEFAULT_MOVEMENT_DURATION_MILLISECONDS = 1000;

        public PointAnimationControl()
        {
            InitializeComponent();
        }


        private RelativeToAbsolutePositionConverter leftPositionConverter;
        private RelativeToAbsolutePositionConverter LeftPositionConverter => leftPositionConverter
            ?? (leftPositionConverter = (RelativeToAbsolutePositionConverter)Resources[POSITION_CONVERTER_LEFT]);

        private RelativeToAbsolutePositionConverter topPositionConverter;
        private RelativeToAbsolutePositionConverter TopPositionConverter => topPositionConverter
            ?? (topPositionConverter = (RelativeToAbsolutePositionConverter)Resources[POSITION_CONVERTER_TOP]);

        private Storyboard enlargeStoryboard;
        private Storyboard EnlargeStoryboard => enlargeStoryboard
            ?? (enlargeStoryboard = (Storyboard)Resources[STORYBOARD_ENLARGE_POINT]);

        private Storyboard shrinkStoryboard;
        private Storyboard ShrinkStoryboard => shrinkStoryboard
            ?? (shrinkStoryboard = (Storyboard)Resources[STORYBOARD_SHRINK_POINT]);




        public Brush PointFill
        {
            get { return (Brush)GetValue(PointFillProperty); }
            set { SetValue(PointFillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PointFill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointFillProperty =
            DependencyProperty.Register(nameof(PointFill), typeof(Brush), typeof(PointAnimationControl), new PropertyMetadata(new SolidColorBrush(Colors.LimeGreen)));



        public double MovementDuration
        {
            get { return (double)GetValue(MovementDurationProperty); }
            set { SetValue(MovementDurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MovementDuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MovementDurationProperty =
            DependencyProperty.Register(nameof(MovementDuration), typeof(double), typeof(PointAnimationControl), new PropertyMetadata(DEFAULT_MOVEMENT_DURATION_MILLISECONDS));



        public bool LoopShrinkOnTargetPoint
        {
            get { return (bool)GetValue(LoopShrinkOnTargetPointProperty); }
            set { SetValue(LoopShrinkOnTargetPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoopShrink.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoopShrinkOnTargetPointProperty =
            DependencyProperty.Register(nameof(LoopShrinkOnTargetPoint), typeof(bool), typeof(PointAnimationControl), new PropertyMetadata(false, (s, e) =>
            {
                var control = (PointAnimationControl)s;
                var loop = (bool)e.NewValue;

                control.UpdateEllipseState(loop);
            }));


        private void UpdateEllipseState(bool loop)
        {
            if (loop)
            {
                VisualStateManager.GoToElementState((FrameworkElement)this.Content, PulsateState.Name, true);
            }
            else
            {
                VisualStateManager.GoToElementState((FrameworkElement)this.Content, StaticState.Name, true);
            }

            _isShrunk = false;
        }


        public bool ShrinkOnTargetPoint
        {
            get { return (bool)GetValue(ShrinkOnTargetPointProperty); }
            set { SetValue(ShrinkOnTargetPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShrinkOnTargetPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShrinkOnTargetPointProperty =
            DependencyProperty.Register(nameof(ShrinkOnTargetPoint), typeof(bool), typeof(PointAnimationControl), new PropertyMetadata(true));



        public bool AnimateToTargetPoint
        {
            get { return (bool)GetValue(AnimateToTargetPointProperty); }
            set { SetValue(AnimateToTargetPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnimateToTargetPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimateToTargetPointProperty =
            DependencyProperty.Register(nameof(AnimateToTargetPoint), typeof(bool), typeof(PointAnimationControl), new PropertyMetadata(true));



        public Point CurrentPoint
        {
            get { return (Point)GetValue(CurrentPointProperty); }
            set { SetValue(CurrentPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPointProperty =
            DependencyProperty.Register(nameof(CurrentPoint), typeof(Point), typeof(PointAnimationControl), new PropertyMetadata(new Point(), (s, e) =>
            {
                var control = (PointAnimationControl)s;

                if (e.NewValue != null)
                {
                    var point = (Point)e.NewValue;
                    control.UpdateEllipsePosition(point);
                }
                else
                {
                    // hide
                }
            }));


        private void UpdateEllipsePosition(Point point)
        {
            var left = LeftPositionConverter.Convert(point.X);
            var top = TopPositionConverter.Convert(point.Y);
            Canvas.SetLeft(ellipse, left);
            Canvas.SetTop(ellipse, top);
        }


        public Point TargetPoint
        {
            get { return (Point)GetValue(TargetPointProperty); }
            set { SetValue(TargetPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetPointProperty =
            DependencyProperty.Register(nameof(TargetPoint), typeof(Point), typeof(PointAnimationControl), new PropertyMetadata(new Point(), (s, e) =>
            {
                var control = (PointAnimationControl)s;
                if (e.NewValue != null)
                {
                    var point = (Point)e.NewValue;
                    control.AnimateToPointAsync(point).Forget();
                }
            }));

        private bool _isShrunk = false;

        private async Task AnimateToPointAsync(Point point)
        {
            var cancellationToken = CancellationToken.None;

            if (point != CurrentPoint)
            {
                // enlarge, if shrinking is enabled
                if (LoopShrinkOnTargetPoint)
                {
                    UpdateEllipseState(false);
                }
                else if (ShrinkOnTargetPoint)
                {
                    if (_isShrunk)
                    {
                        await EnlargeStoryboard.RunAsync(cancellationToken);
                        _isShrunk = false;
                    }
                }


                if (AnimateToTargetPoint && MovementDuration > 0d)
                {
                    // animate move
                    Storyboard moveStoryboard = CreateMoveEllipseStoryboard(point, TimeSpan.FromMilliseconds(MovementDuration));
                    await moveStoryboard.RunAsync(cancellationToken);
                }
                else
                {
                    UpdateEllipsePosition(point);
                }


                if (LoopShrinkOnTargetPoint)
                {
                    UpdateEllipseState(true);
                }
                else if (ShrinkOnTargetPoint)
                {
                    if (_isShrunk == false)
                    {
                        await ShrinkStoryboard.RunAsync(cancellationToken);
                        _isShrunk = true;
                    }
                }

                CurrentPoint = point;
                TargetPointReached?.Invoke(this, new PointReachedEventArgs(point, DateTime.Now));
            }
        }



        private static DoubleAnimationUsingKeyFrames CreateMoveAnimation(double targetValue, KeyTime keyTime)
        {
            return new DoubleAnimationUsingKeyFrames()
            {
                KeyFrames = new DoubleKeyFrameCollection()
                {
                    new EasingDoubleKeyFrame(targetValue, keyTime)
                    {
                        EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn }
                    }
                }
            };
        }


        private Storyboard CreateMoveEllipseStoryboard(Point point, TimeSpan duration)
        {
            Storyboard storyboard = new Storyboard();

            var animationTime = KeyTime.FromTimeSpan(duration);

            var animationLeft = CreateMoveAnimation(LeftPositionConverter.Convert(point.X), animationTime);

            Storyboard.SetTargetProperty(animationLeft, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTarget(animationLeft, ellipse);

            storyboard.Children.Add(animationLeft);


            var animationTop = CreateMoveAnimation(TopPositionConverter.Convert(point.Y), animationTime);
            
            Storyboard.SetTargetProperty(animationTop, new PropertyPath(Canvas.TopProperty));
            Storyboard.SetTarget(animationTop, ellipse);

            storyboard.Children.Add(animationTop);

            return storyboard;
        }


        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePositionConverter(e.NewSize.Width, e.NewSize.Height);
        }


        private void UpdatePositionConverter(double width, double height)
        {
            LeftPositionConverter.Maximum = width;
            TopPositionConverter.Maximum = height;
        }


        public event EventHandler<PointReachedEventArgs> TargetPointReached;
    }


    public class PointReachedEventArgs
    {
        public PointReachedEventArgs(Point point, DateTime timestamp)
        {
            Point = point;
            TimeStamp = timestamp;
        }

        public Point Point { get; }

        public DateTime TimeStamp { get; }
    }
}
