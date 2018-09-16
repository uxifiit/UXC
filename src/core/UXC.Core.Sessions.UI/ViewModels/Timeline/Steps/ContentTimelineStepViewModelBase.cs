using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using UXC.Sessions.Timeline.Actions;
using UXC.Sessions.Timeline.Results;
using UXI.Common.UI;

namespace UXC.Sessions.ViewModels.Timeline
{
    public abstract class ContentTimelineStepViewModelBase : BindableBase, ITimelineStepViewModel
    {
        private const int DEFAULT_FONT_SIZE = 24;

        public ContentTimelineStepViewModelBase(ContentActionSettingsBase settings)
        {
            Foreground = ResolveColor(settings.Foreground, Colors.White);
            Background = ResolveColor(settings.Background, Colors.Black);

            if (settings.FontSize.HasValue)
            {
                FontSize = settings.FontSize.Value;
            }

            if (settings.ShowCursor.HasValue)
            {
                Cursor = settings.ShowCursor.Value ? Cursors.Arrow : Cursors.None;
            }
        }

        public bool IsContent => true;

        public event EventHandler<bool> IsContentChanged { add { } remove { } }


        public Brush Background { get; protected set; }


        public Brush Foreground { get; protected set; }


        public double FontSize { get; protected set; } = DEFAULT_FONT_SIZE;


        public Cursor Cursor { get; protected set; } = Cursors.Arrow;


        protected Brush ResolveColor(string colorString, Color defaultColor)
        {
            if (String.IsNullOrWhiteSpace(colorString) == false)
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(colorString);
                    return new SolidColorBrush(color);
                }
                catch
                {
                    // TODO log
                }
            }
            return new SolidColorBrush(defaultColor);
        }


        public event EventHandler<SessionStepResult> Completed;

        protected virtual void OnCompleted(SessionStepResult result)
        {
            Completed?.Invoke(this, result);
        }

        public abstract SessionStepResult Complete();

        public abstract void Execute(SessionRecordingViewModel recording);
    }
}
