using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using UXI.Common.Extensions;
using UXI.Common.UI;

namespace UXC.Core.ViewModels
{
    public class PointAnimationViewModel : BindableBase
    {
        private bool _isCancelled;

        private PointsSequence _sequence;
        private IEnumerator<Point> _sequenceEnumerator;

        public PointAnimationViewModel(PointsSequence sequence)
        {
            ResetCurrentPoint();

            _sequence = sequence;

            _sequenceEnumerator = sequence?.GetEnumerator();
        }


        private bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
            private set
            {
                if (Set(ref isRunning, value))
                {
                    cancelCommand?.RaiseCanExecuteChanged();
                }
            }
        }


        private bool isCompleted = false;
        public bool IsCompleted
        {
            get { return isCompleted; }
            private set { Set(ref isCompleted, value); }
        }


        private Point targetPoint = default(Point);
        public Point TargetPoint { get { return targetPoint; } private set { Set(ref targetPoint, value); } }


        public bool Continue()
        {
            if (IsCompleted == false && IsRunning)
            {
                if (_sequenceEnumerator != null && _sequenceEnumerator.MoveNext())
                {
                    var nextPoint = _sequenceEnumerator.Current;
                    TargetPoint = new Point(nextPoint.X, nextPoint.Y);

                    return true;
                }
                else
                {
                    Complete();
                }
            }
            
            return false;
        }


        private RelayCommand cancelCommand = null;
        public RelayCommand CancelCommand => cancelCommand
            ?? (cancelCommand = new RelayCommand(() => Cancel(), () => IsRunning));

        public void Cancel()
        {
            if (IsCompleted == false && _isCancelled == false)
            {
                _isCancelled = true;
                IsRunning = false;

                ClearEnumerator();

                OnCancelled();
            }
        }


        protected virtual void OnCancelled()
        {
            Cancelled?.Invoke(this, EventArgs.Empty);
        }


        public void CompletePoint(Point point)
        {
            if (IsRunning)
            {
                if (_sequenceEnumerator != null && _sequenceEnumerator.Current == point)
                {
                    PointCompleted?.Invoke(this, point);
                }
            }
        }

        public event EventHandler<Point> PointCompleted;


        public void Start()
        {
            IsRunning = true;
            Continue();
        }

        
        public void Stop()
        {
            IsRunning = false;
        }


        protected void Complete()
        {
            if (IsRunning)
            {
                IsRunning = false;

                ClearEnumerator();

                OnCompleted();
            }
        }


        protected virtual void OnCompleted()
        {
            IsCompleted = true;
            Completed?.Invoke(this, EventArgs.Empty);
        }


        public void ResetCurrentPoint()
        {
            TargetPoint = new Point(0.5d, 0.5d);
        }


        private void ClearEnumerator()
        {
            var enumerator = ObjectEx.GetAndReplace(ref _sequenceEnumerator, null);
            enumerator?.Dispose();
        }

        public event EventHandler Completed;
        public event EventHandler Cancelled;
    }
}
