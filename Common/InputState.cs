using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PaJaMa.Common
{
    public class InputState
    {
        public static bool GetKeyState(int key)
        {
			return (Win32Api.GetAsyncKeyState(key) & 0x8000) != 0;
        }

        public static bool LeftMouseButtonDown()
        {
            return GetKeyState(MouseKeyboard.VK_LBUTTON);
        }

        public static bool RightMouseButtonDown()
        {
            return GetKeyState(MouseKeyboard.VK_RBUTTON);
        }
    }
}
