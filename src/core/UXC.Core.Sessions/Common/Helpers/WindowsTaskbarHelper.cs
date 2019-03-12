/**
 * UXC.Core.Sessions
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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UXC.Sessions.Helpers
{
    public class WindowsTaskbarHelper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("shell32.dll")]
        static extern UInt32 SHAppBarMessage(UInt32 dwMessage, ref AppBarData pData);

        enum AppBarMessages
        {
            New = 0x00,
            Remove = 0x01,
            QueryPos = 0x02,
            SetPos = 0x03,
            GetState = 0x04,
            GetTaskBarPos = 0x05,
            Activate = 0x06,
            GetAutoHideBar = 0x07,
            SetAutoHideBar = 0x08,
            WindowPosChanged = 0x09,
            SetState = 0x0a
        }

        [StructLayout(LayoutKind.Sequential)]
        struct AppBarData
        {
            public int cbSize; // initialize this field using: Marshal.SizeOf(typeof(APPBARDATA));
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public Rectangle rc;
            public int lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Rectangle
        {
            public int Left, Top, Right, Bottom;

            public Rectangle(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public Rectangle(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return Left; }
                set { Right -= (Left - value); Left = value; }
            }

            public int Y
            {
                get { return Top; }
                set { Bottom -= (Top - value); Top = value; }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public Point Location
            {
                get { return new Point(Left, Top); }
                set { X = (int)value.X; Y = (int)value.Y; }
            }

            public Size Size
            {
                get { return new Size(Width, Height); }
                set { Width = (int)value.Width; Height = (int)value.Height; }
            }

            //public static implicit operator Rectangle(Rectangle r)
            //{
            //    return new Rectangle(r.Left, r.Top, r.Width, r.Height);
            //}

            //public static implicit operator Rectangle(Rectangle r)
            //{
            //    return new Rectangle(r);
            //}

            public static bool operator ==(Rectangle r1, Rectangle r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(Rectangle r1, Rectangle r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(Rectangle r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is Rectangle)
                    return Equals((Rectangle)obj);
                else if (obj is Rectangle)
                    return Equals(new Rectangle((Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
            }
        }


        enum AppBarStates
        {
            AlwaysOnTop = 0x00,
            AutoHide = 0x01
        }

        /// <summary>
        /// Set the Taskbar State option
        /// </summary>
        /// <param name="option">AppBarState to activate</param>
        static void SetTaskbarState(AppBarStates option)
        {
            AppBarData msgData = new AppBarData();
            msgData.cbSize = Marshal.SizeOf(msgData);
            msgData.hWnd = FindWindow("System_TrayWnd", null);
            msgData.lParam = (int)option;
            SHAppBarMessage((UInt32)AppBarMessages.SetState, ref msgData);
        }

        /// <summary>
        /// Gets the current Taskbar state
        /// </summary>
        /// <returns>current Taskbar state</returns>
        static AppBarStates GetTaskbarState()
        {
            AppBarData msgData = new AppBarData();
            msgData.cbSize = Marshal.SizeOf(msgData);
            msgData.hWnd = FindWindow("System_TrayWnd", null);
            return (AppBarStates)SHAppBarMessage((UInt32)AppBarMessages.GetState, ref msgData);
        }


        private void TryChangeTaskbarState(AppBarStates state)
        {
            try
            {
                if (_originalState.HasValue == false)
                {
                    _originalState = GetTaskbarState();
                }

                SetTaskbarState(state);
            }
            catch
            {

            }
        }
        

        private AppBarStates? _originalState;

        public void Show()
        {
            TryChangeTaskbarState(AppBarStates.AlwaysOnTop);
        }

        public void Hide()
        {
            TryChangeTaskbarState(AppBarStates.AutoHide);
        }

        public bool IsChanged => _originalState.HasValue;

        public void Reset()
        {
            if (_originalState.HasValue)
            {
                SetTaskbarState(_originalState.Value);
                _originalState = null;
            }
        }
    }
}
