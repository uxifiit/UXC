//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using UXC.Core.Views;

//namespace UXC.Core.ViewServices
//{
//    public class WindowsService : IWindowsService
//    {
//        public TWindow Open<TWindow>(object dataContext)
//            where TWindow : Window, new()
//        {
//            var window = new TWindow();
//            window.DataContext = dataContext;
//            window.Show();
//            return window;
//        }


//        private DisplayWindow _displayWindow;

//        public Window GetDisplayWindow()
//        {
//            var window = _displayWindow ?? (_displayWindow = new DisplayWindow());

//            return window;
//        }


//        public Window Open(object dataContext)
//        {
//            var window = new Window();
//            window.DataContext = dataContext;
//            window.Show();
//            return window;
//        }
//    }
//}
