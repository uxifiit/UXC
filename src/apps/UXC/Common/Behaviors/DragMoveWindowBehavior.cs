using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace UXC.Common.Behaviors
{
    public class DragMoveWindowBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                AssociatedObject.MouseDown += Window_MouseDown;
            }
        }


        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.MouseDown -= Window_MouseDown;
            }

            base.OnDetaching();
        }


        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var window = (Window)sender;
            if (e.ChangedButton == MouseButton.Left)
            {
                window.DragMove();
            }
        }
    }
}
