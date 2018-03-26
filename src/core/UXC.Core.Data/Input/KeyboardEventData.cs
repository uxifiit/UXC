using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXC.Core.Data
{
    public class KeyboardEventData : DeviceData
    {

        public KeyboardEventType EventType { get; }

        // Summary:
        //     Gets a value indicating whether the ALT key was pressed.
        //
        // Returns:
        //     true if the ALT key was pressed; otherwise, false.
        public bool Alt { get; }
        //
        // Summary:
        //     Gets a value indicating whether the CTRL key was pressed.
        //
        // Returns:
        //     true if the CTRL key was pressed; otherwise, false.
        public bool Control { get; }

        //
        // Summary:
        //     Gets the keyboard code for a System.Windows.Forms.Control.KeyDown or System.Windows.Forms.Control.KeyUp
        //     event.
        //
        // Returns:
        //     A System.Windows.Forms.Keys value that is the key code for the event.
        public Key KeyCode { get; }
        //
        // Summary:
        //     Gets the key data for a System.Windows.Forms.Control.KeyDown or System.Windows.Forms.Control.KeyUp
        //     event.
        //
        // Returns:
        //     A System.Windows.Forms.Keys representing the key code for the key that was
        //     pressed, combined with modifier flags that indicate which combination of
        //     CTRL, SHIFT, and ALT keys was pressed at the same time.
        public Key KeyData { get; }
        //
        // Summary:
        //     Gets the keyboard value for a System.Windows.Forms.Control.KeyDown or System.Windows.Forms.Control.KeyUp
        //     event.
        //
        // Returns:
        //     The integer representation of the System.Windows.Forms.KeyEventArgs.KeyCode
        //     property.
        public int KeyValue { get; }
        //
        // Summary:
        //     Gets the modifier flags for a System.Windows.Forms.Control.KeyDown or System.Windows.Forms.Control.KeyUp
        //     event. The flags indicate which combination of CTRL, SHIFT, and ALT keys
        //     was pressed.
        //
        // Returns:
        //     A System.Windows.Forms.Keys value representing one or more modifier flags.
        public Key Modifiers { get; }
        //
        // Summary:
        //     Gets a value indicating whether the SHIFT key was pressed.
        //
        // Returns:
        //     true if the SHIFT key was pressed; otherwise, false.
        public bool Shift { get; }


        public char KeyChar { get; }


        public KeyboardEventData(KeyboardEventType eventType, bool alt, bool control, Key keyCode, Key keyData, int keyValue, bool shift, char keyChar, DateTime timestamp)
            : base(timestamp)
        {
            EventType = eventType;
            Alt = alt;
            Control = control;
            KeyCode = keyCode;
            KeyData = keyData;
            KeyValue = keyValue;
            KeyChar = keyChar;
            Shift = shift;
        }
    }
}
