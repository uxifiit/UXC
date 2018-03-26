using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using UXC.Core.Devices;
using UXC.Models;
using UXC.Core.Data;
using UXI.Common.Extensions;
using Gma.System.MouseKeyHook;

namespace UXC.Devices.KeyboardMouse
{
    public class KeyboardDevice : IDevice
    {
        private IKeyboardEvents _hook = null;

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


        public KeyboardDevice()
        {
            Code = DeviceCode.Create(this, DeviceType.Input.KEYBOARD)
                .RunsOnMainThread(true)
                .ConnectionType(DeviceConnectionType.SystemApi)
                .Build();
        }


        public DeviceCode Code { get; }
        public Type DataType { get; } = typeof(KeyboardEventData);


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


        private void RegisterHookHandlers(IKeyboardEvents hook)
        {
            //if (_configuration.LogKeyDown)
            hook.KeyDown += hook_KeyDown;

            hook.KeyPress += hook_KeyPress;

            //if (_configuration.LogKeyUp)
            hook.KeyUp += hook_KeyUp;
        }


        private void UnregisterHookHandlers(IKeyboardEvents hook)
        {
            hook.KeyDown -= hook_KeyDown;

            hook.KeyPress -= hook_KeyPress;

            hook.KeyUp -= hook_KeyUp;
        }


        private void hook_KeyDown(object _, KeyEventArgs args) => OnKeyUpDownEvent(KeyboardEventType.KeyDown, args);
        private void hook_KeyUp(object _, KeyEventArgs args) => OnKeyUpDownEvent(KeyboardEventType.KeyUp, args);
        private void hook_KeyPress(object _, KeyPressEventArgs args) => OnKeyPressEvent(args);



        private void OnKeyUpDownEvent(KeyboardEventType eventType, KeyEventArgs args)
        {
            DateTime timestamp;
            if (args is KeyEventArgsExt)
            {
                var argsExt = (KeyEventArgsExt)args;

                timestamp = _ticksConverter.Convert(argsExt.Timestamp);
            }
            else
            {
                timestamp = DateTime.Now;
            }

            var eventData = new KeyboardEventData(eventType, args.Alt, args.Control, (Key)args.KeyCode, (Key)args.KeyData, args.KeyValue, args.Shift, (char)0, timestamp);

            Data?.Invoke(this, eventData);
        }


        private void OnKeyPressEvent(KeyPressEventArgs args)
        {
            DateTime timestamp;
            if (args is KeyPressEventArgsExt)
            {
                var argsExt = (KeyPressEventArgsExt)args;

                timestamp = _ticksConverter.Convert(argsExt.Timestamp);
            }
            else
            {
                timestamp = DateTime.Now;
            }

            var eventData = new KeyboardEventData(KeyboardEventType.KeyPress, false, false, Key.None, Key.None, 0, false, args.KeyChar, timestamp);

            Data?.Invoke(this, eventData);
        }


        public void DumpInfo()
        {                                                                                                  
            Log?.Invoke(this, new Core.LogMessage(Core.LogLevel.Info, LogTags.Connection, $"Connected: {_hook != null}"));
        }
    }
}
