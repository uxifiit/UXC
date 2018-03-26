using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//using MouseKeyboardActivityMonitor;
//using MouseKeyboardActivityMonitor.WinApi;

using System.Threading;
using UXI.Common.Helpers;
using UXC.Core.Devices;
using UXC.Core.Data;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;
using UXI.Common.Extensions;
using System.Diagnostics;
using System.Windows.Threading;

namespace UXC.Devices.KeyboardMouse
{
    public class MouseDevice : IDevice
    {
        private IMouseEvents _hook = null;

        // used for pairing Environment.TickCount value from events to DateTime timestamp.
        private readonly TicksToTimestampConverter _ticksConverter = new TicksToTimestampConverter();

        private static class LogTags
        {
            public const string Recording = "Recording";
            public static string Connection = "Connection";
        }

        public event DeviceDataReceivedEventHandler Data;
        public event ErrorOccurredEventHandler ConnectionError;
        public event DeviceLogEventHandler Log;

        public MouseDevice()
        {
            Code = DeviceCode.Create(this, DeviceType.Input.MOUSE)
                .RunsOnMainThread(true)
                .ConnectionType(DeviceConnectionType.SystemApi)
                .Build();            
        }


        public DeviceCode Code { get; }
        public Type DataType { get; } = typeof(MouseEventData);


        public bool ConnectToDevice()
        {
            try
            {
                _hook = Hook.GlobalEvents();
                return true;
            }
            catch (Exception ex)
            {
                Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Error, LogTags.Connection, "Failed to connect to device", ex));
                ConnectionError?.Invoke(this, ex);
            }
            return false;
        }


        public bool DisconnectDevice()
        {
            var hook = ObjectEx.GetAndReplace(ref _hook, null) as IDisposable;
            if (hook != null)
            {
                hook.Dispose();
            }
            return true;
        }


        public bool StartRecording()
        {
            var hook = _hook;
            if (hook != null)
            {
                try
                {
                    _ticksConverter.Reset();
                    RegisterHookHandlers(hook);
                    return true;
                }
                catch (Exception ex)
                {
                    Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Error, LogTags.Recording, "Failed to start recording", ex));
                    ConnectionError?.Invoke(this, ex);

                    UnregisterHookHandlers(hook);
                }
            }
            return false;
        }


        public bool StopRecording()
        {
            var hook = _hook;
            if (hook != null)
            {
                try
                {
                    UnregisterHookHandlers(hook);
                    return true;
                }
                catch (Exception ex)
                {
                    Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Error, LogTags.Recording, "Failed to stop recording", ex));
                    ConnectionError?.Invoke(this, ex);
                }
            }
            return false;
        }


        private void RegisterHookHandlers(IMouseEvents hook)
        {
            hook.MouseMoveExt += hook_MouseMove;
            hook.MouseClick += hook_MouseClick;
            hook.MouseDoubleClick += hook_MouseDoubleClick;
            hook.MouseDownExt += hook_MouseDown;
            hook.MouseUpExt += hook_MouseUp;
            hook.MouseWheelExt += hook_MouseWheel;
            hook.MouseDragStartedExt += hook_MouseDragStarted;
            hook.MouseDragFinishedExt += hook_MouseDragFinished;
        }


        private void UnregisterHookHandlers(IMouseEvents hook)
        {
            hook.MouseMoveExt -= hook_MouseMove;
            hook.MouseClick -= hook_MouseClick;
            hook.MouseDoubleClick -= hook_MouseDoubleClick;
            hook.MouseDownExt -= hook_MouseDown;
            hook.MouseUpExt -= hook_MouseUp;
            hook.MouseWheelExt -= hook_MouseWheel;
            hook.MouseDragStartedExt -= hook_MouseDragStarted;
            hook.MouseDragFinishedExt -= hook_MouseDragFinished;
        }


        private void hook_MouseMove(object _, MouseEventExtArgs e) => OnMouseEvent(MouseEventType.Move, e);
        private void hook_MouseClick(object _, MouseEventArgs e) => OnMouseEvent(MouseEventType.Click, e);
        private void hook_MouseDoubleClick(object _, MouseEventArgs e) => OnMouseEvent(MouseEventType.DoubleClick, e);
        private void hook_MouseDown(object _, MouseEventExtArgs e) => OnMouseEvent(MouseEventType.ButtonDown, e);
        private void hook_MouseUp(object _, MouseEventExtArgs e) => OnMouseEvent(MouseEventType.ButtonUp, e);
        private void hook_MouseWheel(object _, MouseEventExtArgs e) => OnMouseEvent(MouseEventType.Wheel, e);
        private void hook_MouseDragStarted(object _, MouseEventExtArgs e) => OnMouseEvent(MouseEventType.DragStart, e);
        private void hook_MouseDragFinished(object _, MouseEventExtArgs e) => OnMouseEvent(MouseEventType.DragFinish, e);


        private void OnMouseEvent(MouseEventType eventType, MouseEventArgs args)
        {
            if (args is MouseEventExtArgs)
            {
                OnMouseEvent(eventType, (MouseEventExtArgs)args);
            }
            else
            {
                OnMouseEvent(eventType, args, _ticksConverter.Recent);
            }
        }


        private void OnMouseEvent(MouseEventType eventType, MouseEventExtArgs args)
        {
            DateTime timestamp = _ticksConverter.Convert(args.Timestamp);

            OnMouseEvent(eventType, args, timestamp);
        }


        private void OnMouseEvent(MouseEventType eventType, MouseEventArgs args, DateTime timestamp)
        {
            var eventData = new MouseEventData(eventType, args.X, args.Y, (MouseButton)args.Button, args.Delta, timestamp);

            Data?.Invoke(this, eventData);
        }


        public void DumpInfo()
        {
            Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"Connected: {_hook != null}"));
        }
    }
}
