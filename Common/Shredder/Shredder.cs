using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Diagnostics;

#region Author
/**************************************************************************************
*  NShred V1    Secure Document Shredder Class                                        *
*                                                                                     *
*  Created:     March 5, 2005 (vb6->SDS)                                              *
*  ReWritten:   October 17 2008                                                       *
*  Purpose:     Secure Document Destruction                                           *
*  Revision:    2.0                                                                   *
*  IDE:         C# 2005 SP1                                                           *
*  Referenced:  Member Class NS                                                       *
*  Author:      John Underhill (Steppenwolfe)                                         *
*                                                                                     *
**************************************************************************************/

// RtlZeroMemory http://msdn.microsoft.com/en-us/library/ms803012.aspx
// RtlFillMemory http://msdn.microsoft.com/en-us/library/ms804319.aspx
// RtlCompareMemory http://msdn.microsoft.com/en-us/library/ms802989.aspx
// RtlEqualMemory http://msdn.microsoft.com/en-us/library/ms804291.aspx
// WriteFile http://msdn.microsoft.com/en-us/library/aa910675.aspx
// SetFilePointer http://msdn.microsoft.com/en-us/library/aa911934.aspx
// ReadFile http://msdn.microsoft.com/en-us/library/aa365467(VS.85).aspx
// CryptAcquireContext http://msdn.microsoft.com/en-us/library/aa379886.aspx
// CryptGenRandom http://msdn.microsoft.com/en-us/library/aa379942.aspx
// CryptReleaseContext  http://msdn.microsoft.com/en-us/library/aa924624.aspx

// hidden switch start from cmd line with /p for paranoid mode

#endregion

namespace PaJaMa.Common
{
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	public class Shredder
	{
		#region Constants
		// crypto
		private const Int32 ALG_TYPE_ANY = 0x0;
		private const Int32 ALG_SID_MD5 = 0x3;
		private const Int32 ALG_CLASS_HASH = 0x32768;
		private const Int32 HP_HASHVAL = 0x2;
		private const Int32 HP_HASHSIZE = 0x4;
		private const UInt32 CRYPT_VERIFYCONTEXT = 0xF0000000;
		private const Int32 PROV_RSA_FULL = 0x1;
		private const string MS_ENHANCED_PROV = "Microsoft Enhanced Cryptographic Provider v1.0";
		// findfile
		private const Int32 MAX_PATH = 260;
		private const Int32 MAX_ALTERNATE = 14;
		private const Int32 SYNCHRONIZE = 0x100000;
		private const Int32 STANDARD_RIGHTS_REQUIRED = 0xF0000;
		private const Int32 FILE_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF);
		private const Int32 PROCESS_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF);
		// attributes
		private const Int32 FILE_ATTRIBUTE_READONLY = 0x00000001;
		private const Int32 FILE_ATTRIBUTE_HIDDEN = 0x00000002;
		private const Int32 FILE_ATTRIBUTE_SYSTEM = 0x00000004;
		private const Int32 FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
		private const Int32 FILE_ATTRIBUTE_ENCRYPTED = 0x00000040;
		private const Int32 FILE_ATTRIBUTE_NORMAL = 0x00000080;
		private const Int32 FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
		private const Int32 FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
		private const Int32 FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
		private const Int32 FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
		private const Int32 FILE_ATTRIBUTE_OFFLINE = 0x00001000;
		private const Int32 FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
		private const UInt32 WRITE_THROUGH = 0x80000000;
		// file flags
		private const Int32 GENERIC_ALL = 0x10000000;
		private const Int32 GENERIC_WRITE = 0x40000000;
		private const Int32 FILE_SHARE_NONE = 0x0;
		private const Int32 OPEN_EXISTING = 3;
		private const Int32 FILE_BEGIN = 0;
		private const Int32 FILE_CURRENT = 1;
		private const Int32 FILE_END = 2;
		private const UInt32 FILE_MOVE_FAILED = 0xFFFFFFFF;
		// mem flags
		private const UInt32 MEM_COMMIT = 0x1000;
		private const UInt32 PAGE_READWRITE = 0x04;
		private const UInt32 MEM_RELEASE = 0x8000;
		// movefileex
		private const Int32 MOVEFILE_REPLACE_EXISTING = 0x1;
		private const Int32 MOVEFILE_DELAY_UNTIL_REBOOT = 0x4;
		private const Int32 MOVEFILE_WRITE_THROUGH = 0x8;
		// local
		private const UInt32 BUFFER_SIZE = 65536;
		private const string UNICODE_PREFIX = @"\\?\";
		// deviveIO
		private const UInt32 FsctlDeleteObjectId = (0x00000009 << 16) | (40 << 2) | 0 | (0 << 14);
		// process
		private const Int32 PROCESS_VM_READ = 0x16;
		private const Int32 PROCESS_SET_INFORMATION = 0x200;
		private const Int32 PROCESS_QUERY_INFORMATION = 0x400;
		private const Int32 PROCESS_TERMINATE = 0x1;
		private const Int32 WM_CLOSE = 0x10;
		private const Int32 WAIT_OBJECT_0 = 0x00000000;
		private const Int32 WAIT_TIMEOUT = 258;
		private const Int32 STILL_ACTIVE = 259;
		private const Int32 QS_ALLINPUT = 0x04FF;
		// privilege
		private const Int32 SE_PRIVILEGE_ENABLED = 0x00000002;
		private const Int32 TOKEN_QUERY = 0x00000008;
		private const Int32 TOKEN_ADJUST_PRIVILEGES = 0x00000020;
		private const string SE_ASSIGNPRIMARYTOKEN_NAME = "SeAssignPrimaryTokenPrivilege";
		private const string SE_AUDIT_NAME = "SeAuditPrivilege";
		private const string SE_BACKUP_NAME = "SeBackupPrivilege";
		private const string SE_CHANGE_NOTIFY_NAME = "SeChangeNotifyPrivilege";
		private const string SE_CREATE_GLOBAL_NAME = "SeCreateGlobalPrivilege";
		private const string SE_CREATE_PAGEFILE_NAME = "SeCreatePagefilePrivilege";
		private const string SE_CREATE_PERMANENT_NAME = "SeCreatePermanentPrivilege";
		private const string SE_CREATE_SYMBOLIC_LINK_NAME = "SeCreateSymbolicLinkPrivilege";
		private const string SE_CREATE_TOKEN_NAME = "SeCreateTokenPrivilege";
		private const string SE_DEBUG_NAME = "SeDebugPrivilege";
		private const string SE_ENABLE_DELEGATION_NAME = "SeEnableDelegationPrivilege";
		private const string SE_IMPERSONATE_NAME = "SeImpersonatePrivilege";
		private const string SE_INC_BASE_PRIORITY_NAME = "SeIncreaseBasePriorityPrivilege";
		private const string SE_INCREASE_QUOTA_NAME = "SeIncreaseQuotaPrivilege";
		private const string SE_INC_WORKING_SET_NAME = "SeIncreaseWorkingSetPrivilege";
		private const string SE_LOAD_DRIVER_NAME = "SeLoadDriverPrivilege";
		private const string SE_LOCK_MEMORY_NAME = "SeLockMemoryPrivilege";
		private const string SE_MACHINE_ACCOUNT_NAME = "SeMachineAccountPrivilege";
		private const string SE_MANAGE_VOLUME_NAME = "SeManageVolumePrivilege";
		private const string SE_PROF_SINGLE_PROCESS_NAME = "SeProfileSingleProcessPrivilege";
		private const string SE_RELABEL_NAME = "SeRelabelPrivilege";
		private const string SE_REMOTE_SHUTDOWN_NAME = "SeRelabelPrivilege";
		private const string SE_RESTORE_NAME = "SeRestorePrivilege";
		private const string SE_SECURITY_NAME = "SeRestorePrivilege";
		private const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
		private const string SE_SYNC_AGENT_NAME = "SeSyncAgentPrivilege";
		private const string SE_SYSTEM_ENVIRONMENT_NAME = "SeSystemEnvironmentPrivilege";
		private const string SE_SYSTEM_PROFILE_NAME = "SeSystemProfilePrivilege";
		private const string SE_SYSTEMTIME_NAME = "SeSystemtimePrivilege";
		private const string SE_TAKE_OWNERSHIP_NAME = "SeTakeOwnershipPrivilege";
		private const string SE_TCB_NAME = "SeTcbPrivilege";
		private const string SE_TIME_ZONE_NAME = "SeTimeZonePrivilege";
		private const string SE_TRUSTED_CREDMAN_ACCESS_NAME = "SeTrustedCredManAccessPrivilege";
		private const string SE_UNDOCK_NAME = "SeUndockPrivilege";
		private const string SE_UNSOLICITED_INPUT_NAME = "SeUnsolicitedInputPrivilege";
		#endregion

		#region Enum
		public enum EMoveMethod : uint
		{
			Begin = 0,
			Current = 1,
			End = 2
		}

		private enum ETOKEN_PRIVILEGES : uint
		{
			ASSIGN_PRIMARY = 0x1,
			TOKEN_DUPLICATE = 0x2,
			TOKEN_IMPERSONATE = 0x4,
			TOKEN_QUERY = 0x8,
			TOKEN_QUERY_SOURCE = 0x10,
			TOKEN_ADJUST_PRIVILEGES = 0x20,
			TOKEN_ADJUST_GROUPS = 0x40,
			TOKEN_ADJUST_DEFAULT = 0x80,
			TOKEN_ADJUST_SESSIONID = 0x100
		}

		enum SYSTEM_INFORMATION_CLASS
		{
			SystemInformationClassMin = 0,
			SystemBasicInformation = 0,
			SystemProcessorInformation = 1,
			SystemPerformanceInformation = 2,
			SystemTimeOfDayInformation = 3,
			SystemPathInformation = 4,
			SystemNotImplemented1 = 4,
			SystemProcessInformation = 5,
			SystemProcessesAndThreadsInformation = 5,
			SystemCallCountInfoInformation = 6,
			SystemCallCounts = 6,
			SystemDeviceInformation = 7,
			SystemConfigurationInformation = 7,
			SystemProcessorPerformanceInformation = 8,
			SystemProcessorTimes = 8,
			SystemFlagsInformation = 9,
			SystemGlobalFlag = 9,
			SystemCallTimeInformation = 10,
			SystemNotImplemented2 = 10,
			SystemModuleInformation = 11,
			SystemLocksInformation = 12,
			SystemLockInformation = 12,
			SystemStackTraceInformation = 13,
			SystemNotImplemented3 = 13,
			SystemPagedPoolInformation = 14,
			SystemNotImplemented4 = 14,
			SystemNonPagedPoolInformation = 15,
			SystemNotImplemented5 = 15,
			SystemHandleInformation = 16,
			SystemObjectInformation = 17,
			SystemPageFileInformation = 18,
			SystemPagefileInformation = 18,
			SystemVdmInstemulInformation = 19,
			SystemInstructionEmulationCounts = 19,
			SystemVdmBopInformation = 20,
			SystemInvalidInfoClass1 = 20,
			SystemFileCacheInformation = 21,
			SystemCacheInformation = 21,
			SystemPoolTagInformation = 22,
			SystemInterruptInformation = 23,
			SystemProcessorStatistics = 23,
			SystemDpcBehaviourInformation = 24,
			SystemDpcInformation = 24,
			SystemFullMemoryInformation = 25,
			SystemNotImplemented6 = 25,
			SystemLoadImage = 26,
			SystemUnloadImage = 27,
			SystemTimeAdjustmentInformation = 28,
			SystemTimeAdjustment = 28,
			SystemSummaryMemoryInformation = 29,
			SystemNotImplemented7 = 29,
			SystemNextEventIdInformation = 30,
			SystemNotImplemented8 = 30,
			SystemEventIdsInformation = 31,
			SystemNotImplemented9 = 31,
			SystemCrashDumpInformation = 32,
			SystemExceptionInformation = 33,
			SystemCrashDumpStateInformation = 34,
			SystemKernelDebuggerInformation = 35,
			SystemContextSwitchInformation = 36,
			SystemRegistryQuotaInformation = 37,
			SystemLoadAndCallImage = 38,
			SystemPrioritySeparation = 39,
			SystemPlugPlayBusInformation = 40,
			SystemNotImplemented10 = 40,
			SystemDockInformation = 41,
			SystemNotImplemented11 = 41,
			SystemInvalidInfoClass2 = 42,
			SystemProcessorSpeedInformation = 43,
			SystemInvalidInfoClass3 = 43,
			SystemCurrentTimeZoneInformation = 44,
			SystemTimeZoneInformation = 44,
			SystemLookasideInformation = 45,
			SystemSetTimeSlipEvent = 46,
			SystemCreateSession = 47,
			SystemDeleteSession = 48,
			SystemInvalidInfoClass4 = 49,
			SystemRangeStartInformation = 50,
			SystemVerifierInformation = 51,
			SystemAddVerifier = 52,
			SystemSessionProcessesInformation = 53,
			SystemInformationClassMax
		}
		#endregion

		#region Struct
		[StructLayout(LayoutKind.Sequential)]
		public struct FILETIME
		{
			public UInt32 dwLowDateTime;
			public UInt32 dwHighDateTime;
		};

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct WIN32_FIND_DATAA
		{
			public Int32 dwFileAttributes;
			public FILETIME ftCreationTime;
			public FILETIME ftLastAccessTime;
			public FILETIME ftLastWriteTime;
			public Int32 nFileSizeHigh;
			public Int32 nFileSizeLow;
			public Int32 dwReserved0;
			public Int32 dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
			public string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ALTERNATE)]
			public string cAlternate;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct WIN32_FIND_DATAW
		{
			public Int32 dwFileAttributes;
			public FILETIME ftCreationTime;
			public FILETIME ftLastAccessTime;
			public FILETIME ftLastWriteTime;
			public Int32 nFileSizeHigh;
			public Int32 nFileSizeLow;
			public Int32 dwReserved0;
			public Int32 dwReserved1;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
			public string cFileName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ALTERNATE)]
			public string cAlternateFileName;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct LUID
		{
			public Int32 LowPart;
			public Int32 HighPart;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct LUID_AND_ATTRIBUTES
		{
			public LUID pLuid;
			public Int32 Attributes;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		private struct TOKEN_PRIVILEGES
		{
			public Int32 PrivilegeCount;
			public LUID_AND_ATTRIBUTES Privileges;
		}
		#endregion

		#region API
		// crypto
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean CryptAcquireContextW(ref IntPtr hProv, [MarshalAs(UnmanagedType.LPWStr)]string pszContainer, [MarshalAs(UnmanagedType.LPWStr)]string pszProvider, UInt32 dwProvType, UInt32 dwFlags);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern Boolean CryptGenRandom(IntPtr hProv, UInt32 dwLen, IntPtr pbBuffer);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern Boolean CryptReleaseContext(IntPtr hProv, UInt32 dwFlags);

		// file management
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr CreateFileW(IntPtr lpFileName, Int32 dwDesiredAccess, Int32 dwShareMode, IntPtr lpSecurityAttributes,
		Int32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean SetFilePointerEx(IntPtr hFile, long liDistanceToMove, [Out, Optional] IntPtr lpNewFilePointer, UInt32 dwMoveMethod);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern Int32 WriteFile(IntPtr hFile, IntPtr lpBuffer, UInt32 nNumberOfBytesToWrite, ref UInt32 lpNumberOfBytesWritten, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern Int32 ReadFile(IntPtr hFile, IntPtr lpBuffer, UInt32 nNumberOfBytesToRead, ref UInt32 lpNumberOfBytesRead, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern Int32 CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern Int32 FlushFileBuffers(IntPtr hFile);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern Int32 SetFileAttributesW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName, Int32 dwFileAttributes);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern Int32 GetShortPathNameW([MarshalAs(UnmanagedType.LPWStr)]string lLongPath, [MarshalAs(UnmanagedType.LPWStr)]string lShortPath, Int32 lBuffer);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean GetFileSizeEx(IntPtr hFile, out UInt32 lpFileSize);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern Int32 MoveFileExW([MarshalAs(UnmanagedType.LPWStr)]string lpExistingFileName, [MarshalAs(UnmanagedType.LPWStr)]string lpNewFileName, Int32 dwFlags);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern Int32 DeleteFileW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern Int32 RemoveDirectoryW([MarshalAs(UnmanagedType.LPWStr)]string lpPathName);

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean DeviceIoControl(IntPtr hDevice, UInt32 dwIoControlCode, IntPtr lpInBuffer, UInt32 nInBufferSize,
		IntPtr lpOutBuffer, [Optional] UInt32 nOutBufferSize, out UInt32 lpBytesReturned, IntPtr lpOverlapped);

		// findfile
		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr FindFirstFileW([MarshalAs(UnmanagedType.LPWStr)]string lpFileName, out WIN32_FIND_DATAW lpFindFileData);

		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean FindNextFileW(IntPtr hFindFile, out WIN32_FIND_DATAW lpFindFileData);

		[DllImport("kernel32", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern Boolean FindClose(IntPtr hFindFile);

		// memory allocation
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr VirtualAlloc(IntPtr lpAddress, UInt32 dwSize, UInt32 flAllocationType, UInt32 flProtect);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean VirtualFree(IntPtr lpAddress, UInt32 dwSize, UInt32 dwFreeType);

		[DllImport("kernel32.dll", SetLastError = false)]
		static extern void RtlZeroMemory(IntPtr dest, IntPtr size);

		// ntapi
		[DllImport("ntdll.dll", SetLastError = false)]
		private static extern Int32 RtlFillMemory([In] IntPtr Destination, UInt32 length, byte fill);

		[DllImport("ntdll.dll", SetLastError = true)]
		private static extern void RtlZeroMemory(IntPtr Destination, uint length);

		[DllImport("ntdll.dll", SetLastError = false)]
		private static extern UInt32 RtlCompareMemory(IntPtr Source1, IntPtr Source2, UInt32 length);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern Int32 RtlMoveMemory(ref byte Destination, ref byte Source, IntPtr Length);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern Int32 RtlMoveMemory(ref byte Destination, ref IntPtr Source, IntPtr Length);

		// process
		[DllImport("psapi.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean EnumProcesses([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] [In][Out] UInt32[] processIds,
		  UInt32 arraySizeBytes, [MarshalAs(UnmanagedType.U4)] out UInt32 bytesCopied);

		[DllImport("psapi.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean EnumProcessModules(IntPtr hProcess, [MarshalAs(UnmanagedType.LPArray,
		ArraySubType = UnmanagedType.U4)] [In][Out] UInt32[] lphModule, UInt32 cb, [MarshalAs(UnmanagedType.U4)] out UInt32 lpcbNeeded);

		[DllImport("psapi.dll", SetLastError = true)]
		private static extern UInt32 GetModuleFileNameExA(IntPtr hProcess, IntPtr hModule,
		[Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] UInt32 nSize);

		[DllImport("ntdll.dll", SetLastError = true)]
		private static extern UInt32 NtTerminateProcess(IntPtr ProcessHandle, UInt32 ExitStatus);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean GetExitCodeProcess(IntPtr hProcess, out UInt32 lpExitCode);

		[DllImport("kernel32.dll")]
		private static extern uint WaitForMultipleObjects(uint nCount, IntPtr[] pHandles,
		Boolean bWaitAll, uint dwMilliseconds);

		// privilege - needed changes
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState,
		UInt32 BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern UInt32 GetCurrentProcessId();

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern Int32 OpenProcessToken(IntPtr ProcessHandle, UInt32 DesiredAccess, ref IntPtr TokenHandle);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern Int32 LookupPrivilegeValueW(Int32 lpSystemName, [MarshalAs(UnmanagedType.LPWStr)]string lpName, ref LUID lpLuid);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr OpenProcess(Int32 dwDesiredAccess, Int32 blnheritHandle, UInt32 dwAppProcessId);
		#endregion

		#region Events
		public delegate void ProgressMaxDelegate(Int32 Max);
		public event ProgressMaxDelegate ProgressMax;

		public delegate void ProgressTickDelegate();
		public event ProgressTickDelegate ProgressTick;

		public delegate void CompleteDelegate();
		public event CompleteDelegate Complete;

		public delegate void ErrorDelegate(Int32 ErrType, String Err);
		public event ErrorDelegate InError;

		public delegate void FileCountDelegate(Int32 Count, ref Boolean Cancel);
		public event FileCountDelegate FileCount;

		public delegate void StatusDelegate(Int32 Status, string Message);
		public event StatusDelegate Status;

		public event System.ComponentModel.ProgressChangedEventHandler ShredProgress;
		#endregion

		#region Declarations
		private Boolean bAnyAttribute = false;
		private Boolean bCloseInstance = false;
		private Boolean bDeleteDirectory = false;
		private Boolean bDeleteFolders = false;
		private Boolean bDeleteSubDirectories = false;
		private Boolean bParanoid = false;
		private string sFilePath = String.Empty;
		private string[] tkRights = { SE_ASSIGNPRIMARYTOKEN_NAME, SE_BACKUP_NAME, SE_DEBUG_NAME, SE_INC_BASE_PRIORITY_NAME, SE_RESTORE_NAME, SE_SECURITY_NAME, SE_TCB_NAME };
		private WIN32_FIND_DATAW W32;
		private ArrayList FileList = new ArrayList();
		private ArrayList DirectoryList = new ArrayList();
		#endregion

		#region Properties
		/// <summary>
		/// Delete a file regardless of its attributes
		/// </summary>
		public Boolean AnyAttribute
		{
			get
			{
				return bAnyAttribute;
			}
			set
			{
				bAnyAttribute = value;
			}
		}

		/// <summary>
		/// Terminate any running instances of the file
		/// </summary>
		public Boolean CloseInstance
		{
			get
			{
				return bCloseInstance;
			}
			set
			{
				bCloseInstance = value;
			}
		}

		/// <summary>
		/// Delete all files within a folder
		/// </summary>
		public Boolean DeleteDirectory
		{
			get
			{
				return bDeleteDirectory;
			}
			set
			{
				bDeleteDirectory = value;
			}
		}

		/// <summary>
		/// Delete files and their parent folder
		/// </summary>
		public Boolean DeleteFolders
		{
			get
			{
				return bDeleteFolders;
			}
			set
			{
				bDeleteFolders = value;
			}
		}

		/// <summary>
		/// Delete files within subdirectories
		/// </summary>
		public Boolean DeleteSubDirectories
		{
			get
			{
				return bDeleteSubDirectories;
			}
			set
			{
				bDeleteSubDirectories = value;
			}
		}

		/// <summary>
		/// File name or start path
		/// </summary>
		public string FilePath
		{
			get
			{
				return sFilePath;
			}
			set
			{
				sFilePath = value;
			}
		}

		/// <summary>
		/// Enable paranoid mode
		/// </summary>
		public Boolean ParanoidMode
		{
			get
			{
				return bParanoid;
			}
			set
			{
				bParanoid = value;
			}
		}
		#endregion

		#region Core Methods
		/// <summary>
		/// Entry point - starts the shredder
		/// </summary>
		/// <returns>Boolean</returns>
		public Boolean StartShredder()
		{
			Boolean bC = false;
			// reset collections
			FileList = new ArrayList();
			DirectoryList = new ArrayList();

			try
			{
				//add rights back to token
				adjustToken(true, tkRights);
				if (DeleteDirectory)
				{
					if (FilePath.Length < 4)
					{
						// can't shred the drive
						if (InError != null)
							InError(001, "Warning! You are attempting to shred the drive! Operation Aborted!");
						return false;
					}
					if (!FilePath.EndsWith(@"\"))
						FilePath += @"\";
					preLoader(FilePath, DeleteSubDirectories);

					// last chance
					if (FileCount != null)
					{
						if (FileList.Count != 0)
						{
							bC = true;
							FileCount(FileList.Count, ref bC);
						}
					}
					if (bC)
						return false;
					if (ProgressMax != null)
						ProgressMax(FileList.Count);
					// go
					foreach (string file in FileList)
					{
						if (fileExists(UNICODE_PREFIX + file))
						{
							if (CloseInstance)
								closeProcess(file);
							if (Status != null)
								Status(0, "Processing file " + file);
							if (ShredFile(UNICODE_PREFIX + file))
							{
								if (Status != null)
									Status(0, "File " + file + " Deleted");
							}
							else
							{
								if (Status != null)
									Status(0, "Could not delete " + file);
							}
						}
						else
						{
							// throw file name for log
							if (InError != null)
								InError(002, file);
						}
						// progress count
						if (ProgressTick != null)
							ProgressTick();
					}
					if (DeleteFolders)
					{
						if (DirectoryList.Count != 0)
						{
							DirectoryList.Reverse();
							foreach (string dir in DirectoryList)
							{
								removeDirectory(UNICODE_PREFIX + dir);
							}
						}
						removeDirectory(UNICODE_PREFIX + FilePath);
					}
					if (Complete != null)
						Complete();
					// if you got this far
					return true;
				}
				else
				{
					if (!fileExists(UNICODE_PREFIX + FilePath))
					{
						if (InError != null)
							InError(003, "The file path is invalid!");
					}
					if (CloseInstance)
						closeProcess(FilePath);
					// relay processing status
					if (Status != null)
						Status(0, "Processing file " + FilePath);
					if (ShredFile(UNICODE_PREFIX + FilePath))
					{
						if (Status != null)
							Status(0, "File " + FilePath + " Deleted");
						return true;
					}
					else
					{
						if (Status != null) 
							Status(0, "Could not delete " + FilePath);
						adjustToken(false, tkRights);
						return false;
					}
				}
			}
			finally
			{
				// return token to normal
				adjustToken(false, tkRights);
				FilePath = String.Empty;
			}
		}

		private int _progress = 0;

		/// <summary>
		/// Primary worker
		/// </summary>
		/// <param name="sPath">string - file name</param>
		/// <returns>Boolean</returns>
		private Boolean ShredFile(string sPath)
		{
			IntPtr hFile = IntPtr.Zero;
			IntPtr pBuffer = IntPtr.Zero;
			IntPtr pName = Marshal.StringToHGlobalAuto(sPath);
			UInt32 nFileLen = 0;
			UInt32 dwSize = BUFFER_SIZE;
			byte bR = 0;

			try
			{
				_progress = 0;
				if (ShredProgress != null)
					ShredProgress(this, new System.ComponentModel.ProgressChangedEventArgs(0, null));
				// reset attributes
				if (AnyAttribute)
					stripAttributes(sPath);
				// progress count on single file
				if (!DeleteDirectory)
					if (ProgressMax != null)
						ProgressMax(5);
				// open the file
				hFile = CreateFileW(pName, GENERIC_ALL, FILE_SHARE_NONE, IntPtr.Zero, OPEN_EXISTING, WRITE_THROUGH, IntPtr.Zero);
				nFileLen = fileSize(hFile);
				if (nFileLen > BUFFER_SIZE)
					nFileLen = BUFFER_SIZE;
				if (hFile.ToInt32() == -1)
					return false;
				// set the table
				SetFilePointerEx(hFile, 0, IntPtr.Zero, FILE_BEGIN);
				pBuffer = VirtualAlloc(IntPtr.Zero, nFileLen, MEM_COMMIT, PAGE_READWRITE);
				if (pBuffer == IntPtr.Zero)
					return false;
				// first pass all zeros
				RtlZeroMemory(pBuffer, nFileLen);
				if (OverwriteFile(hFile, pBuffer) != true)
					return false;
				_progress = 25;
				if (ShredProgress != null)
					ShredProgress(this, new System.ComponentModel.ProgressChangedEventArgs(25, null));
				if (WriteVerify(hFile, pBuffer, nFileLen) != true)
					return false;
				if (!DeleteDirectory)
					if (ProgressTick != null)
						ProgressTick();
				// second pass all ones
				bR = 0xFF;
				RtlFillMemory(pBuffer, nFileLen, bR);
				if (OverwriteFile(hFile, pBuffer) != true)
					return false;
				_progress = 50;
				if (ShredProgress != null)
					ShredProgress(this, new System.ComponentModel.ProgressChangedEventArgs(50, null));
				if (WriteVerify(hFile, pBuffer, nFileLen) != true)
					return false;
				if (!DeleteDirectory)
					if (ProgressTick != null)
						ProgressTick();
				// third pass random
				randomData(pBuffer, nFileLen);
				if (OverwriteFile(hFile, pBuffer) != true)
					return false;
				_progress = 75;
				if (ShredProgress != null)
					ShredProgress(this, new System.ComponentModel.ProgressChangedEventArgs(75, null));
				if (WriteVerify(hFile, pBuffer, nFileLen) != true)
					return false;
				if (!DeleteDirectory)
					if (ProgressTick != null)
						ProgressTick();
				// fourth pass zeros
				RtlZeroMemory(pBuffer, nFileLen);
				OverwriteFile(hFile, pBuffer);
				bR = 0;
				if (WriteVerify(hFile, pBuffer, nFileLen) != true)
					return false;
				if (!DeleteDirectory)
					if (ProgressTick != null)
						ProgressTick();
				// close
				if (CloseHandle(hFile) != 0)
					hFile = IntPtr.Zero;
				// reduce to zero bytes
				if (zeroFile(pName) != true)
					if (InError != null)
						InError(005, "Emptying file contents failed.");
				// paranoid mode
				if (ParanoidMode)
					orphanFile(pName);
				// rename the file
				if (renameFile(sPath) != true)
					if (InError != null)
						InError(006, "File could not be renamed.");
				//complete
				if (!DeleteDirectory)
				{
					if (ProgressTick != null)
						ProgressTick();
					if (Complete != null)
						Complete();
				}
				if (ShredProgress != null)
					ShredProgress(this, new System.ComponentModel.ProgressChangedEventArgs(100, null));
				return true;
			}

			finally
			{
				if (hFile != IntPtr.Zero)
					CloseHandle(hFile);
				if (pBuffer != IntPtr.Zero)
					VirtualFree(pBuffer, dwSize, MEM_RELEASE);
			}
		}

		/// <summary>
		/// Overwrite the file
		/// </summary>
		/// <param name="hFile">IntPtr - file handle</param>
		/// <param name="pBuffer">IntPtr - buffer address</param>
		/// <returns>Boolean</returns>
		private Boolean OverwriteFile(IntPtr hFile, IntPtr pBuffer)
		{
			UInt32 nFileLen = fileSize(hFile);
			UInt32 dwSeek = 0;
			UInt32 btWritten = 0;

			try
			{
				if (nFileLen < BUFFER_SIZE)
				{
					SetFilePointerEx(hFile, dwSeek, IntPtr.Zero, FILE_BEGIN);
					WriteFile(hFile, pBuffer, nFileLen, ref btWritten, IntPtr.Zero);
				}
				else
				{
					do
					{
						SetFilePointerEx(hFile, dwSeek, IntPtr.Zero, FILE_BEGIN);
						WriteFile(hFile, pBuffer, BUFFER_SIZE, ref btWritten, IntPtr.Zero);
						if (ShredProgress != null)
							ShredProgress(this, new System.ComponentModel.ProgressChangedEventArgs(_progress +
								Convert.ToInt16(25 * (double)dwSeek/nFileLen)
								, null));
						dwSeek += btWritten;
					} while ((nFileLen - dwSeek) > BUFFER_SIZE);
					WriteFile(hFile, pBuffer, (nFileLen - dwSeek), ref btWritten, IntPtr.Zero);
				}
				// reset file pointer
				SetFilePointerEx(hFile, 0, IntPtr.Zero, FILE_BEGIN);
				// add it up
				if ((btWritten + dwSeek) == nFileLen)
					return true;
				return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Verify that the file has been overwritten
		/// </summary>
		/// <param name="hFile">IntPtr - file handle</param>
		/// <param name="pCompare">IntPtr - buffer address</param>
		/// <param name="pSize">UIntPtr - buffer size</param>
		/// <returns>Boolean</returns>
		private Boolean WriteVerify(IntPtr hFile, IntPtr pCompare, UInt32 pSize)
		{
			IntPtr pBuffer = IntPtr.Zero;
			UInt32 iRead = 0;

			try
			{
				pBuffer = VirtualAlloc(IntPtr.Zero, pSize, MEM_COMMIT, PAGE_READWRITE);
				SetFilePointerEx(hFile, 0, IntPtr.Zero, FILE_BEGIN);
				if (ReadFile(hFile, pBuffer, pSize, ref iRead, IntPtr.Zero) == 0)
				{
					if (InError != null)
						InError(004, "The file write failed verification test.");
					return false; // bad read
				}
				if (RtlCompareMemory(pCompare, pBuffer, pSize) == pSize)
					return true; // equal
				return false;
			}
			finally
			{
				if (pBuffer != IntPtr.Zero)
					VirtualFree(pBuffer, pSize, MEM_RELEASE);
			}
		}
		#endregion

		#region Process
		private void closeProcess(string sTarget)
		{
			UInt32 arraySize = 96;
			UInt32 arrayBytesSize = arraySize * sizeof(UInt32);
			UInt32[] processIds = new UInt32[arraySize];
			UInt32[] processMods = new UInt32[1024];
			UInt32 bytesCopied = 0;
			UInt32 dwSize = 0;
			Boolean success = false;
			UInt32 ret = 0;
			IntPtr hProcess = IntPtr.Zero;
			System.Text.StringBuilder pvName = new System.Text.StringBuilder(260);

			do
			{
				arrayBytesSize *= 2;
				processIds = new UInt32[arrayBytesSize / 4];
				success = EnumProcesses(processIds, arrayBytesSize, out bytesCopied);
			} while (arrayBytesSize <= bytesCopied);

			if (!success)
				return;

			UInt32 numIdsCopied = bytesCopied >> 2; ;
			for (UInt32 index = 0; index < numIdsCopied; index++)
			{
				try
				{
					if (processIds[index] != 0)
					{
						hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, 0, processIds[index]);
						if (hProcess != IntPtr.Zero)
						{
							if (EnumProcessModules(hProcess, processMods, 1024, out dwSize))
								success = (EnumProcessModules(hProcess, processMods, dwSize, out dwSize));
							if (success)
								ret = GetModuleFileNameExA(hProcess, (IntPtr)processMods[0], pvName, dwSize);
							if (pvName.ToString() == sTarget)
							{
								killProcess(processIds[index]);
								CloseHandle(hProcess);
								return;
							}
							CloseHandle(hProcess);
						}
					}
				}
				catch
				{
					if (hProcess != IntPtr.Zero)
						CloseHandle(hProcess);
				}
			}
			return;
		}

		/// <summary>
		/// Terminate process
		/// </summary>
		/// <param name="ProcessId">uint - process id</param>
		/// <returns>Boolean</returns>
		private Boolean killProcess(UInt32 ProcessId)
		{
			UInt32 lpExitCode = 0;
			UInt32 ret = 0;
			UInt32 usafe = 0;
			IntPtr[] hProcess = new IntPtr[1];
			hProcess[0] = IntPtr.Zero;

			hProcess[0] = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_TERMINATE | SYNCHRONIZE, 0, ProcessId);
			if (hProcess[0] == IntPtr.Zero)
				return false;
			// ask nice
			GetExitCodeProcess(hProcess[0], out lpExitCode);
			ret = (NtTerminateProcess(hProcess[0], lpExitCode));
			// wait for it
			do
			{
				ret = (WaitForMultipleObjects(1, hProcess, false, 100));
				usafe++;
			}
			while ((ret != (WAIT_OBJECT_0)) && (usafe < 100));
			return true;
		}
		#endregion

		#region Security
		/// <summary>
		/// Change process security token access rights
		/// </summary>
		/// <param name="Enable">Boolean - Ebnable or disable a privilege</param>
		/// <returns>Boolean</returns>
		private Boolean adjustToken(Boolean Enable, string[] rights)
		{
			IntPtr hToken = IntPtr.Zero;
			IntPtr hProcess = IntPtr.Zero;
			LUID tLuid = new LUID();
			TOKEN_PRIVILEGES NewState = new TOKEN_PRIVILEGES();
			UInt32 uPriv = (UInt32)(ETOKEN_PRIVILEGES.TOKEN_ADJUST_PRIVILEGES | ETOKEN_PRIVILEGES.TOKEN_QUERY | ETOKEN_PRIVILEGES.TOKEN_QUERY_SOURCE);

			try
			{
				hProcess = OpenProcess(PROCESS_ALL_ACCESS, 0, GetCurrentProcessId());
				if (hProcess == IntPtr.Zero)
					return false;
				if (OpenProcessToken(hProcess, uPriv, ref hToken) == 0)
					return false;
				for (Int32 i = 0; i < rights.Length; i++)
				{
					// Get the local unique id for the privilege.
					if (LookupPrivilegeValueW(0, rights[i], ref tLuid) == 0)
						return false;
				}
				// Assign values to the TOKEN_PRIVILEGE structure.
				NewState.PrivilegeCount = 1;
				NewState.Privileges.pLuid = tLuid;
				NewState.Privileges.Attributes = (Enable ? SE_PRIVILEGE_ENABLED : 0);
				// Adjust the token privilege
				//IntPtr pState = IntPtr.Zero;
				//Marshal.StructureToPtr(NewState, pState, true);
				return (AdjustTokenPrivileges(hToken, false, ref NewState, (uint)Marshal.SizeOf(NewState), IntPtr.Zero, IntPtr.Zero));
			}
			finally
			{
				if (hToken != IntPtr.Zero)
					CloseHandle(hToken);
				if (hProcess != IntPtr.Zero)
					CloseHandle(hProcess);
			}
		}

		public Boolean IsAdmin()
		{
			IntPtr hToken = IntPtr.Zero;
			IntPtr hProcess = IntPtr.Zero;
			LUID tLuid = new LUID();
			UInt32 uPriv = (UInt32)(ETOKEN_PRIVILEGES.TOKEN_ADJUST_PRIVILEGES | ETOKEN_PRIVILEGES.TOKEN_QUERY | ETOKEN_PRIVILEGES.TOKEN_QUERY_SOURCE);

			try
			{
				hProcess = OpenProcess(PROCESS_ALL_ACCESS, 0, GetCurrentProcessId());
				if (hProcess == IntPtr.Zero)
					return false;
				if (OpenProcessToken(hProcess, uPriv, ref hToken) == 0)
					return false;
				return (LookupPrivilegeValueW(0, SE_TCB_NAME, ref tLuid) != 0);
			}
			finally
			{
				if (hToken != IntPtr.Zero)
					CloseHandle(hToken);
				if (hProcess != IntPtr.Zero)
					CloseHandle(hProcess);
			}
		}
		#endregion

		#region Helpers
		/// <summary>
		/// Delete the directory
		/// </summary>
		/// <param name="sDir">unicode string - directory name</param>
		/// <returns>Boolean</returns>
		private Boolean removeDirectory(string sDir)
		{
			if (RemoveDirectoryW(sDir) == 0)
				return false;
			return true;
		}

		/// <summary>
		/// Delete a file
		/// </summary>
		/// <param name="hFile">unicode string - file handle</param>
		/// <returns>Boolean</returns>
		private Boolean deleteFile(string FileName)
		{
			if (DeleteFileW(FileName) == 0)
				return false;
			return true;
		}

		/// <summary>
		/// Destroy file when computer restarts
		/// </summary>
		/// <param name="sPath">void</param>
		private Boolean destroyOnRestart(string sPath)
		{
			string sSource = shortPath(sPath);
			if (sSource.Length == 0)
				sSource = sPath;
			if (MoveFileExW(sSource, String.Empty, MOVEFILE_DELAY_UNTIL_REBOOT) == 0)
				return false;
			return true;
		}

		/// <summary>
		/// Test file path
		/// </summary>
		/// <param name="sPath">string - unicode file path</param>
		/// <returns>Boolean</returns>
		private Boolean fileExists(string sPath)
		{
			WIN32_FIND_DATAW WFD;
			IntPtr hFile = IntPtr.Zero;

			try
			{
				hFile = FindFirstFileW(sPath, out WFD);
				if (hFile.ToInt32() == -1)
					return false;
				return true;
			}
			finally
			{
				if (hFile != IntPtr.Zero)
					FindClose(hFile);
			}
		}

		/// <summary>
		/// Get the file size in bytes
		/// </summary>
		/// <param name="hFile">IntPtr - file handle</param>
		/// <returns>Int32 - file length</returns>
		private UInt32 fileSize(IntPtr hFile)
		{
			UInt32 nFileLen = 0;
			GetFileSizeEx(hFile, out nFileLen);
			return nFileLen;
		}

		/// <summary>
		/// Fill a buffer with random data
		/// </summary>
		/// <param name="pBuffer">IntPtr - buffer address</param>
		/// <param name="nSize">UInt32 - length of buffer</param>
		/// <returns>Boolean</returns>
		private Boolean randomData(IntPtr pBuffer, UInt32 nSize)
		{
			IntPtr iProv = IntPtr.Zero;

			try
			{
				// acquire context
				if (CryptAcquireContextW(ref iProv, "", MS_ENHANCED_PROV, PROV_RSA_FULL, CRYPT_VERIFYCONTEXT) != true)
					return false;
				// generate random block
				if (CryptGenRandom(iProv, nSize, pBuffer) != true)
					return false;
				return true;
			}
			finally
			{
				// release crypto engine
				if (iProv != IntPtr.Zero)
					CryptReleaseContext(iProv, 0);
			}
		}

		/// <summary>
		/// Rename a file 30 times
		/// </summary>
		/// <param name="sPath">string - file name</param>
		/// <returns>Boolean</returns>
		private Boolean renameFile(string sPath)
		{
			string sNewName = String.Empty;
			string sPartial = sPath.Substring(0, sPath.LastIndexOf(@"\") + 1);
			Int32 nLen = 10;
			char[] cName = new char[nLen];
			for (Int32 i = 0; i < 30; i++)
			{
				for (Int32 j = 97; j < 123; j++)
				{
					for (Int32 k = 0; k < nLen; k++)
					{
						if (k == (nLen - 4))
							sNewName += ".";
						else
							sNewName += (char)j;
					}
					if (MoveFileExW(sPath, sPartial + sNewName, MOVEFILE_REPLACE_EXISTING | MOVEFILE_WRITE_THROUGH) != 0)
						sPath = sPartial + sNewName;
					sNewName = String.Empty;
				}
			}
			// last step: delete the file
			if (deleteFile(sPath) != true)
				return false;
			return true;
		}

		/// <summary>
		/// retrieve short path name
		/// </summary>
		/// <param name="sPath">string - path</param>
		/// <returns>string</returns>
		private string shortPath(string sPath)
		{
			Int32 iLen;
			string sBuffer = String.Empty;
			sBuffer.PadRight(255, char.MinValue);
			iLen = GetShortPathNameW(sPath, sBuffer, 255);
			return sBuffer.Trim();
		}

		/// <summary>
		/// Reset a files attributes
		/// </summary>
		/// <param name="sPath">void</param>
		private void stripAttributes(string sPath)
		{
			SetFileAttributesW(sPath, FILE_ATTRIBUTE_NORMAL);
		}

		/// <summary>
		/// Delete the files object Id
		/// </summary>
		/// <param name="sPath">IntPtr - unicode file path</param>
		/// <returns>Boolean</returns>
		private Boolean orphanFile(IntPtr pName)
		{
			UInt32 lpBytesReturned = 0;
			IntPtr hFile = CreateFileW(pName, GENERIC_WRITE, FILE_SHARE_NONE, IntPtr.Zero, OPEN_EXISTING, WRITE_THROUGH, IntPtr.Zero);
			if (DeviceIoControl(hFile, FsctlDeleteObjectId, IntPtr.Zero, 0, IntPtr.Zero, 0, out lpBytesReturned, IntPtr.Zero))
				return false;
			return true;
		}

		/// <summary>
		/// Reduce file to zero bytes and flush buffer
		/// </summary>
		/// <param name="hFile">file handle</param>
		/// <returns>Boolean</returns>
		private Boolean zeroFile(IntPtr pName)
		{
			for (Int32 i = 0; i < 10; i++)
			{
				IntPtr hFile = CreateFileW(pName, GENERIC_ALL, FILE_SHARE_NONE, IntPtr.Zero, OPEN_EXISTING, WRITE_THROUGH, IntPtr.Zero);
				if (hFile == IntPtr.Zero)
					return false;
				SetFilePointerEx(hFile, 0, IntPtr.Zero, FILE_BEGIN);
				// unnecessary but..
				FlushFileBuffers(hFile);
				CloseHandle(hFile);
			}
			return true;
		}
		#endregion

		#region Directory
		/// <summary>
		/// Create a list of files to be deleted
		/// </summary>
		/// <param name="sPath">string - directory path</param>
		/// <param name="bRecurse">Boolean recurse subfolders</param>
		private void preLoader(string sPath, Boolean bRecurse)
		{
			IntPtr hFile = IntPtr.Zero;

			try
			{
				hFile = FindFirstFileW(sPath + "*.*", out W32);
				if (hFile.ToInt32() != -1)
				{
					do
					{
						if ((W32.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) != 0)
						{
							if (W32.cFileName != "." && W32.cFileName != "..")
							{
								if (bRecurse)
								{
									// recurse directory
									if (DeleteFolders)
										DirectoryList.Add(sPath + W32.cFileName + @"\");
									preLoader(sPath + W32.cFileName + @"\", true);
								}
							}
						}
						else
						{
							//add files
							FileList.Add(sPath + W32.cFileName);
						}
					} while (FindNextFileW(hFile, out W32));
					FindClose(hFile);
				}
			}
			finally
			{
				if (hFile != IntPtr.Zero)
					FindClose(hFile);
			}
			return;
		}
		#endregion

	}
}
