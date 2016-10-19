using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PaJaMa.Common
{
	public class Win32Api
	{
		#region Constants
		public const int INVALID_HANDLE_VALUE = (-1);
		#endregion

		#region user32.dll
		[DllImport("user32.dll")]
		public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int width, int height, bool repaint);

		[DllImport("user32.dll")]
		public static extern short GetAsyncKeyState(int key);

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SetFocus(IntPtr hwnd);

		[DllImport("user32.dll")]
		public static extern int GetForegroundWindow();
		#endregion

		[DllImport("User32.dll")]
		public static extern Int32 SetForegroundWindow(IntPtr hwnd);

		#region kernel32.dll
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern bool ActivateActCtx(IntPtr hActCtx, out uint lpCookie);

		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateActCtx(ref ACTCTX actctx);

		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern bool DeactivateActCtx(uint dwFlags, uint lpCookie);

		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern void ReleaseActCtx(IntPtr hActCtx);

		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern int GetShortPathName(string longPath, StringBuilder buffer, int bufferSize);
		#endregion

		#region shell32.dll
		public class InteropSHFileOperation
		{
			public enum FO_Func : uint
			{
				FO_MOVE = 0x0001,
				FO_COPY = 0x0002,
				FO_DELETE = 0x0003,
				FO_RENAME = 0x0004,
			}

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			struct SHFILEOPSTRUCT
			{
				public IntPtr hwnd;
				public FO_Func wFunc;
				[MarshalAs(UnmanagedType.LPWStr)]
				public string pFrom;
				[MarshalAs(UnmanagedType.LPWStr)]
				public string pTo;
				public ushort fFlags;
				[MarshalAs(UnmanagedType.Bool)]
				public bool fAnyOperationsAborted;
				public IntPtr hNameMappings;
				[MarshalAs(UnmanagedType.LPWStr)]
				public string lpszProgressTitle;

			}

			[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
			static extern int SHFileOperation([In] ref SHFILEOPSTRUCT lpFileOp);

			private SHFILEOPSTRUCT _ShFile;
			public FILEOP_FLAGS fFlags;

			public IntPtr hwnd
			{
				set
				{
					this._ShFile.hwnd = value;
				}
			}
			public FO_Func wFunc
			{
				set
				{
					this._ShFile.wFunc = value;
				}
			}

			public string pFrom
			{
				set
				{
					this._ShFile.pFrom = value + '\0' + '\0';
				}
			}
			public string pTo
			{
				set
				{
					this._ShFile.pTo = value + '\0' + '\0';
				}
			}

			public bool fAnyOperationsAborted
			{
				set
				{
					this._ShFile.fAnyOperationsAborted = value;
				}
			}
			public IntPtr hNameMappings
			{
				set
				{
					this._ShFile.hNameMappings = value;
				}
			}
			public string lpszProgressTitle
			{
				set
				{
					this._ShFile.lpszProgressTitle = value + '\0';
				}
			}

			public InteropSHFileOperation()
			{

				this.fFlags = new FILEOP_FLAGS();
				this._ShFile = new SHFILEOPSTRUCT();
				this._ShFile.hwnd = IntPtr.Zero;
				this._ShFile.wFunc = FO_Func.FO_COPY;
				this._ShFile.pFrom = "";
				this._ShFile.pTo = "";
				this._ShFile.fAnyOperationsAborted = false;
				this._ShFile.hNameMappings = IntPtr.Zero;
				this._ShFile.lpszProgressTitle = "";

			}

			public int Execute()
			{
				this._ShFile.fFlags = this.fFlags.Flag;
				return SHFileOperation(ref this._ShFile);
			}

			public class FILEOP_FLAGS
			{
				[Flags]
				private enum FILEOP_FLAGS_ENUM : ushort
				{
					FOF_MULTIDESTFILES = 0x0001,
					FOF_CONFIRMMOUSE = 0x0002,
					FOF_SILENT = 0x0004,  // don't create progress/report
					FOF_RENAMEONCOLLISION = 0x0008,
					FOF_NOCONFIRMATION = 0x0010,  // Don't prompt the user.
					FOF_WANTMAPPINGHANDLE = 0x0020,  // Fill in SHFILEOPSTRUCT.hNameMappings
					// Must be freed using SHFreeNameMappings
					FOF_ALLOWUNDO = 0x0040,
					FOF_FILESONLY = 0x0080,  // on *.*, do only files
					FOF_SIMPLEPROGRESS = 0x0100,  // means don't show names of files
					FOF_NOCONFIRMMKDIR = 0x0200,  // don't confirm making any needed dirs
					FOF_NOERRORUI = 0x0400,  // don't put up error UI
					FOF_NOCOPYSECURITYATTRIBS = 0x0800,  // dont copy NT file Security Attributes
					FOF_NORECURSION = 0x1000,  // don't recurse into directories.
					FOF_NO_CONNECTED_ELEMENTS = 0x2000,  // don't operate on connected elements.
					FOF_WANTNUKEWARNING = 0x4000,  // during delete operation, warn if nuking instead of recycling (partially overrides FOF_NOCONFIRMATION)
					FOF_NORECURSEREPARSE = 0x8000,  // treat reparse points as objects, not containers
				}

				public bool FOF_MULTIDESTFILES = false;
				public bool FOF_CONFIRMMOUSE = false;
				public bool FOF_SILENT = false;
				public bool FOF_RENAMEONCOLLISION = false;
				public bool FOF_NOCONFIRMATION = false;
				public bool FOF_WANTMAPPINGHANDLE = false;
				public bool FOF_ALLOWUNDO = false;
				public bool FOF_FILESONLY = false;
				public bool FOF_SIMPLEPROGRESS = false;
				public bool FOF_NOCONFIRMMKDIR = false;
				public bool FOF_NOERRORUI = false;
				public bool FOF_NOCOPYSECURITYATTRIBS = false;
				public bool FOF_NORECURSION = false;
				public bool FOF_NO_CONNECTED_ELEMENTS = false;
				public bool FOF_WANTNUKEWARNING = false;
				public bool FOF_NORECURSEREPARSE = false;
				public ushort Flag
				{
					get
					{
						ushort ReturnValue = 0;

						if (this.FOF_MULTIDESTFILES == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_MULTIDESTFILES;
						if (this.FOF_CONFIRMMOUSE == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_CONFIRMMOUSE;
						if (this.FOF_SILENT == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_SILENT;
						if (this.FOF_RENAMEONCOLLISION == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_RENAMEONCOLLISION;
						if (this.FOF_NOCONFIRMATION == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOCONFIRMATION;
						if (this.FOF_WANTMAPPINGHANDLE == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_WANTMAPPINGHANDLE;
						if (this.FOF_ALLOWUNDO == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_ALLOWUNDO;
						if (this.FOF_FILESONLY == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_FILESONLY;
						if (this.FOF_SIMPLEPROGRESS == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_SIMPLEPROGRESS;
						if (this.FOF_NOCONFIRMMKDIR == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOCONFIRMMKDIR;
						if (this.FOF_NOERRORUI == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOERRORUI;
						if (this.FOF_NOCOPYSECURITYATTRIBS == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NOCOPYSECURITYATTRIBS;
						if (this.FOF_NORECURSION == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NORECURSION;
						if (this.FOF_NO_CONNECTED_ELEMENTS == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NO_CONNECTED_ELEMENTS;
						if (this.FOF_WANTNUKEWARNING == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_WANTNUKEWARNING;
						if (this.FOF_NORECURSEREPARSE == true)
							ReturnValue |= (ushort)FILEOP_FLAGS_ENUM.FOF_NORECURSEREPARSE;

						return ReturnValue;
					}
				}
			}

		}

		#endregion

		#region Misc Objects
		[StructLayout(LayoutKind.Sequential)]
		public struct ACTCTX
		{
			public int cbSize;
			public uint dwFlags;
			public string lpSource;
			public ushort wProcessorArchitecture;
			public ushort wLangId;
			public string lpAssemblyDirectory;
			public string lpResourceName;
			public string lpApplicationName;
			public IntPtr hModule;
		}
		#endregion


		[DllImport("user32.dll")]
		static extern short VkKeyScan(char c);

		[DllImport("user32.dll", SetLastError = true)]
		static extern int ToAscii(
			uint uVirtKey,
			uint uScanCode,
			byte[] lpKeyState,
			out uint lpChar,
			uint flags
			);

		public static char GetModifiedKey(char c)
		{
			short vkKeyScanResult = VkKeyScan(c);

			// a result of -1 indicates no key translates to input character
			if (vkKeyScanResult == -1)
				return c;

			// vkKeyScanResult & 0xff is the base key, without any modifiers
			uint code = (uint)vkKeyScanResult & 0xff;

			// set shift key pressed
			byte[] b = new byte[256];
			b[0x10] = 0x80;

			uint r;
			// return value of 1 expected (1 character copied to r)
			if (1 != ToAscii(code, code, b, out r, 0))
				return c;

			return (char)r;
		}
	}
}
