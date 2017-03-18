using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SixteenBox.Windows.Native
{
    public static class WinInput
    {
        // We have to use this extern to catch caps locks state
        // If we use keyPressed for caps lock, we can't for sure know when is activated
        [DllImport("user32.dll")]
        public static extern short GetKeyState(Key keyCode);

        // Keys codes
        public enum Key : int
        {
            CapsLock = 0x14,
            NumLock = 0x90,
            ScrollLock = 0x91
        }
    }
}
