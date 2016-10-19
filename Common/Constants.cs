using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaJaMa.Common
{
    public class MouseKeyboard
    {
        //Left mouse button
        public const int VK_LBUTTON = 0x01;

        //Right mouse button
        public const int VK_RBUTTON = 0x02;

        //Control
        public const int VK_CANCEL = 0x03;

        //Middle mouse button 
        public const int VK_MBUTTON = 0x04;

        //Windows 2000/XP: X1 mouse button
        public const int VK_XBUTTON1 = 0x05;

        //Windows 2000/XP: X2 mouse button
        public const int VK_XBUTTON2 = 0x06;

        //BACKSPACE key
        public const int VK_BACK = 0x08;

        //TAB key
        public const int VK_TAB = 0x09;

        //CLEAR key
        public const int VK_CLEAR = 0x0C;

        //ENTER key
        public const int VK_RETURN = 0x0D;

        //SHIFT key
        public const int VK_SHIFT = 0x10;

        //CTRL key
        public const int VK_CONTROL = 0x11;

        //ALT key
        public const int VK_MENU = 0x12;

        //PAUSE key
        public const int VK_PAUSE = 0x13;

        //CAPS LOCK key
        public const int VK_CAPITAL = 0x14;

        //Input Method Editor 
        public const int VK_KANA = 0x15;

        //IME Hanguel mode 
        public const int VK_HANGUEL = 0x15;

        //IME Hangul mode
        public const int VK_HANGUL = 0x15;

        //IME Junja mode
        public const int VK_JUNJA = 0x17;

        //IME final mode
        public const int VK_FINAL = 0x18;

        //IME Hanja mode
        public const int VK_HANJA = 0x19;

        //IME Kanji mode
        public const int VK_KANJI = 0x19;

        //ESC key
        public const int VK_ESCAPE = 0x1B;

        //IME convert
        public const int VK_CONVERT = 0x1C;

        //IME nonconvert
        public const int VK_NONCONVERT = 0x1D;

        //IME accept
        public const int VK_ACCEPT = 0x1E;

        //IME mode change request
        public const int VK_MODECHANGE = 0x1F;

        //SPACEBAR
        public const int VK_SPACE = 0x20;

        //PAGE UP key
        public const int VK_PRIOR = 0x21;

        //PAGE DOWN key
        public const int VK_NEXT = 0x22;

        //END key
        public const int VK_END = 0x23;

        //HOME key
        public const int VK_HOME = 0x24;

        //LEFT ARROW key
        public const int VK_LEFT = 0x25;

        //UP ARROW key
        public const int VK_UP = 0x26;

        //RIGHT ARROW key
        public const int VK_RIGHT = 0x27;

        //DOWN ARROW key
        public const int VK_DOWN = 0x28;

        //SELECT key
        public const int VK_SELECT = 0x29;

        //PRpublic const int key
        public const int VK_PRint = 0x2A;

        //EXECUTE key
        public const int VK_EXECUTE = 0x2B;

        //PRpublic const int SCREEN key
        public const int VK_SNAPSHOT = 0x2C;

        //INS key
        public const int VK_INSERT = 0x2D;

        //DEL key
        public const int VK_DELETE = 0x2E;

        //HELP key
        public const int VK_HELP = 0x2F;

        //0 key
        public const int ZERO = 0x30;

        //1 key
        public const int ONE = 0x31;

        //2 key
        public const int TWO = 0x32;

        //3 key
        public const int THREE = 0x33;

        //4 key
        public const int FOUR = 0x34;

        //5 key
        public const int FIVE = 0x35;

        //6 key
        public const int SIX = 0x36;

        //7 key
        public const int SEVEN = 0x37;

        //8 key
        public const int EIGHT = 0x38;

        //9 key
        public const int NINE = 0x39;

        //A key
        public const int A = 0x41;

        //B key
        public const int B = 0x42;

        //C key
        public const int C = 0x43;

        //D key
        public const int D = 0x44;

        //E key
        public const int E = 0x45;

        //F key
        public const int F = 0x46;

        //G key
        public const int G = 0x47;

        //H key
        public const int H = 0x48;

        //I key
        public const int I = 0x49;

        //J key
        public const int J = 0x4A;

        //K key
        public const int K = 0x4B;

        //L key
        public const int L = 0x4C;

        //M key
        public const int M = 0x4D;

        //N key
        public const int N = 0x4E;

        //O key
        public const int O = 0x4F;

        //P key
        public const int P = 0x50;

        //Q key
        public const int Q = 0x51;

        //R key
        public const int R = 0x52;

        //S key
        public const int S = 0x53;

        //T key
        public const int T = 0x54;

        //U key
        public const int U = 0x55;

        //V key
        public const int V = 0x56;

        //W key
        public const int W = 0x57;

        //X key
        public const int X = 0x58;

        //Y key
        public const int Y = 0x59;

        //Z key
        public const int Z = 0x5A;

        //Left Windows key 
        public const int VK_LWIN = 0x5B;

        //Right Windows key 
        public const int VK_RWIN = 0x5C;

        //Applications key 
        public const int VK_APPS = 0x5D;

        //Computer Sleep key
        public const int VK_SLEEP = 0x5F;

        //Numeric keypad 0 key
        public const int VK_NUMPAD0 = 0x60;

        //Numeric keypad 1 key
        public const int VK_NUMPAD1 = 0x61;

        //Numeric keypad 2 key
        public const int VK_NUMPAD2 = 0x62;

        //Numeric keypad 3 key
        public const int VK_NUMPAD3 = 0x63;

        //Numeric keypad 4 key
        public const int VK_NUMPAD4 = 0x64;

        //Numeric keypad 5 key
        public const int VK_NUMPAD5 = 0x65;

        //Numeric keypad 6 key
        public const int VK_NUMPAD6 = 0x66;

        //Numeric keypad 7 key
        public const int VK_NUMPAD7 = 0x67;

        //Numeric keypad 8 key
        public const int VK_NUMPAD8 = 0x68;

        //Numeric keypad 9 key
        public const int VK_NUMPAD9 = 0x69;

        //Multiply key
        public const int VK_MULTIPLY = 0x6A;

        //Add key
        public const int VK_ADD = 0x6B;

        //Separator key
        public const int VK_SEPARATOR = 0x6C;

        //Subtract key
        public const int VK_SUBTRACT = 0x6D;

        //Decimal key
        public const int VK_DECIMAL = 0x6E;

        //Divide key
        public const int VK_DIVIDE = 0x6F;

        //F1 key
        public const int VK_F1 = 0x70;

        //F2 key
        public const int VK_F2 = 0x71;

        //F3 key
        public const int VK_F3 = 0x72;

        //F4 key
        public const int VK_F4 = 0x73;

        //F5 key
        public const int VK_F5 = 0x74;

        //F6 key
        public const int VK_F6 = 0x75;

        //F7 key
        public const int VK_F7 = 0x76;

        //F8 key
        public const int VK_F8 = 0x77;

        //F9 key
        public const int VK_F9 = 0x78;

        //F10 key
        public const int VK_F10 = 0x79;

        //F11 key
        public const int VK_F11 = 0x7A;

        //F12 key
        public const int VK_F12 = 0x7B;

        //F13 key
        public const int VK_F13 = 0x7C;

        //F14 key
        public const int VK_F14 = 0x7D;

        //F15 key
        public const int VK_F15 = 0x7E;

        //F16 key
        public const int VK_F16 = 0x7F;

        //NUM LOCK key
        public const int VK_NUMLOCK = 0x90;

        //SCROLL LOCK key
        public const int VK_SCROLL = 0x91;

        //Left SHIFT key
        public const int VK_LSHIFT = 0xA0;

        //Right SHIFT key
        public const int VK_RSHIFT = 0xA1;

        //Left CONTROL key
        public const int VK_LCONTROL = 0xA2;

        //Right CONTROL key
        public const int VK_RCONTROL = 0xA3;

        //Left MENU key
        public const int VK_LMENU = 0xA4;

        //Right MENU key
        public const int VK_RMENU = 0xA5;

        //Windows 2000/XP: Browser Back key
        public const int VK_BROWSER_BACK = 0xA6;

        //Windows 2000/XP: Browser Forward key
        public const int VK_BROWSER_FORWARD = 0xA7;

        //Windows 2000/XP: Browser Refresh key
        public const int VK_BROWSER_REFRESH = 0xA8;

        //Windows 2000/XP: Browser Stop key
        public const int VK_BROWSER_STOP = 0xA9;

        //Windows 2000/XP: Browser Search key 
        public const int VK_BROWSER_SEARCH = 0xAA;

        //Windows 2000/XP: Browser Favorites key
        public const int VK_BROWSER_FAVORITES = 0xAB;

        //Windows 2000/XP: Browser Start and Home key
        public const int VK_BROWSER_HOME = 0xAC;

        //Windows 2000/XP: Volume Mute key
        public const int VK_VOLUME_MUTE = 0xAD;

        //Windows 2000/XP: Volume Down key
        public const int VK_VOLUME_DOWN = 0xAE;

        //Windows 2000/XP: Volume Up key
        public const int VK_VOLUME_UP = 0xAF;

        //Windows 2000/XP: Next Track key
        public const int VK_MEDIA_NEXT_TRACK = 0xB0;

        //Windows 2000/XP: Previous Track key
        public const int VK_MEDIA_PREV_TRACK = 0xB1;

        //Windows 2000/XP: Stop Media key
        public const int VK_MEDIA_STOP = 0xB2;

        //Windows 2000/XP: Play/Pause Media key
        public const int VK_MEDIA_PLAY_PAUSE = 0xB3;

        //Windows 2000/XP: Start Mail key
        public const int VK_LAUNCH_MAIL = 0xB4;

        //Windows 2000/XP: Select Media key
        public const int VK_LAUNCH_MEDIA_SELECT = 0xB5;

        //Windows 2000/XP: Start Application 1 key
        public const int VK_LAUNCH_APP1 = 0xB6;

        //Windows 2000/XP: Start Application 2 key
        public const int VK_LAUNCH_APP2 = 0xB7;

        //Used for miscellaneous characters; it can vary by keyboard.
        // Windows 2000/XP: For the US standard keyboard, the ';:' key 
        public const int VK_OEM_1 = 0xBA;

        //Windows 2000/XP: For any country/region, the '+' key
        public const int VK_OEM_PLUS = 0xBB;

        //Windows 2000/XP: For any country/region, the ',' key
        public const int VK_OEM_COMMA = 0xBC;

        //Windows 2000/XP: For any country/region, the '-' key
        public const int VK_OEM_MINUS = 0xBD;

        //Windows 2000/XP: For any country/region, the '.' key
        public const int VK_OEM_PERIOD = 0xBE;

        //Used for miscellaneous characters; it can vary by keyboard.
        //Windows 2000/XP: For the US standard keyboard, the '/?' key 
        public const int VK_OEM_2 = 0xBF;

        //Used for miscellaneous characters; it can vary by keyboard. 
        //Windows 2000/XP: For the US standard keyboard, the '`~' key 
        public const int VK_OEM_3 = 0xC0;

        //Used for miscellaneous characters; it can vary by keyboard. 
        //Windows 2000/XP: For the US standard keyboard, the '[{' key
        public const int VK_OEM_4 = 0xDB;

        //Used for miscellaneous characters; it can vary by keyboard. 
        //Windows 2000/XP: For the US standard keyboard, the '\|' key
        public const int VK_OEM_5 = 0xDC;


        //Used for miscellaneous characters; it can vary by keyboard. 
        //Windows 2000/XP: For the US standard keyboard, the ']}' key
        public const int VK_OEM_6 = 0xDD;

        //Used for miscellaneous characters; it can vary by keyboard. 
        //Windows 2000/XP: For the US standard keyboard, the 'single-quote/double-quote' key
        public const int VK_OEM_7 = 0xDE;

        //Used for miscellaneous characters; it can vary by keyboard.
        public const int VK_OEM_8 = 0xDF;

        //Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
        public const int VK_OEM_102 = 0xE2;

        public const int VK_PROCESSKEY = 0xE5;//98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key

        //Windows 2000
        public const int VK_PACKET = 0xE7;//XP: Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP

        //Attn key
        public const int VK_ATTN = 0xF6;

        //CrSel key
        public const int VK_CRSEL = 0xF7;

        //ExSel key
        public const int VK_EXSEL = 0xF8;

        //Erase EOF key
        public const int VK_EREOF = 0xF9;

        //Play key
        public const int VK_PLAY = 0xFA;

        //Zoom key
        public const int VK_ZOOM = 0xFB;

        //Reserved 
        public const int VK_NONAME = 0xFC;

        //PA1 key

        public const int VK_PA1 = 0xFD;

        //    }
        public const int VK_OEM_CLEAR = 0xFE;
    }
}
