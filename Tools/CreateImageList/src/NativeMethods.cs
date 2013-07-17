using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CreateImageList
{
	internal static class NativeMethods
	{
		public enum RT
		{
			NONE = 0,
			CURSOR = 1,
			BITMAP = 2,
			ICON = 3,
			MENU = 4,
			DIALOG = 5,
			STRING = 6,
			FONTDIR = 7,
			FONT = 8,
			ACCELERATOR = 9,
			RCDATA = 10,
			MESSAGETABLE = 11
		}

		public enum IMAGE
		{
			IMAGE_BITMAP = 0,
			IMAGE_ICON = 1,
			IMAGE_CURSOR = 2,
			IMAGE_ENHMETAFILE = 3
		}

		public enum LR
		{
			LR_DEFAULTCOLOR = 0x0000,
			LR_MONOCHROME = 0x0001,
			LR_COLOR = 0x0002,
			LR_COPYRETURNORG = 0x0004,
			LR_COPYDELETEORG = 0x0008,
			LR_LOADFROMFILE = 0x0010,
			LR_LOADTRANSPARENT = 0x0020,
			LR_DEFAULTSIZE = 0x0040,
			LR_VGACOLOR = 0x0080,
			LR_LOADMAP3DCOLORS = 0x1000,
			LR_CREATEDIBSECTION = 0x2000,
			LR_COPYFROMRESOURCE = 0x4000,
			LR_SHARED = 0x8000
		}

		public const int TRUE = 1;
		public const int FALSE = 0;

		[DllImport("kernel32.dll",SetLastError=true)]
		public static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int FreeLibrary(IntPtr hModule);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr FindResource(IntPtr hModule,int lpName,RT rtType);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr LoadIcon(IntPtr hInstance,IntPtr lpIconName);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr LoadImage(IntPtr hInstance, IntPtr lpszName,IMAGE uType,int cxDesired, int cyDesired,LR fuLoad);

		public delegate int ENUMRESTYPEPROC(IntPtr hModule, IntPtr lpName, IntPtr lParam);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int EnumResourceTypes(IntPtr hModule, ENUMRESTYPEPROC lpEnumFunc, IntPtr lParam);

		public delegate int ENUMRESNAMEPROC(IntPtr hModule,RT rtType,IntPtr lpName,IntPtr lParam);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int EnumResourceNames(IntPtr hModule,RT rtType,ENUMRESNAMEPROC lpEnumFunc,IntPtr lParam);

		public static bool IS_INTRESOURCE(IntPtr value)
		{
			if (((uint)value) > ushort.MaxValue)
				return false;
			return true;
		}

		public static uint GET_RESOURCE_ID(IntPtr value)
		{
			if (IS_INTRESOURCE(value) == true)
				return (uint)value;
			throw new System.NotSupportedException("value is not an ID!");
		}

		public static string GET_RESOURCE_NAME(IntPtr value)
		{
			if (IS_INTRESOURCE(value) == true)
				return value.ToString();
			return Marshal.PtrToStringUni((IntPtr)value);
		}

	}
}
