using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace S5MS.Helpers
{
	class ConsoleHelper
	{
		public delegate void EventHandler();
		public static event EventHandler OnExit = delegate { };

		private delegate bool ConsoleEventDelegate(int eventType);
		private static ConsoleEventDelegate ConsoleHandler;

		public static void Init()
		{
			ConsoleHandler = new ConsoleEventDelegate(ConsoleEventCallback);
			SetConsoleCtrlHandler(ConsoleHandler, true);
		}

		public static void WriteNewLine()
		{
			WriteNewLine(1);
		}

		public static void WriteNewLine(int count)
		{
			for (int i = count; i > 0; i--)
				WriteLine(string.Empty, Console.BackgroundColor, Console.ForegroundColor);
		}

		[Obsolete("Use WriteNewLine() instead")]
		public static void WriteLine()
		{
			WriteNewLine();
		}

		public static void WriteLine(string value)
		{
			WriteLine(value, Console.BackgroundColor, Console.ForegroundColor);
		}

		public static void WriteLine(string value, ConsoleColor foregroundColor)
		{
			WriteLine(value, Console.BackgroundColor, foregroundColor);
		}

		public static void WriteLine(string value, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
		{
			ConsoleColor oldBackgroundColor = Console.BackgroundColor;
			ConsoleColor oldForegroundColor = Console.ForegroundColor;

			Console.BackgroundColor = backgroundColor;
			Console.ForegroundColor = foregroundColor;
			Console.WriteLine(value);
			Console.BackgroundColor = oldBackgroundColor;
			Console.ForegroundColor = oldForegroundColor;
		}

		public static void Write()
		{
			Write(string.Empty, Console.BackgroundColor, Console.ForegroundColor);
		}

		public static void Write(string value)
		{
			Write(value, Console.BackgroundColor, Console.ForegroundColor);
		}

		public static void Write(string value, ConsoleColor foregroundColor)
		{
			Write(value, Console.BackgroundColor, foregroundColor);
		}

		public static void Write(string value, ConsoleColor backgroundColor, ConsoleColor foregroundColor)
		{
			ConsoleColor oldBackgroundColor = Console.BackgroundColor;
			ConsoleColor oldForegroundColor = Console.ForegroundColor;

			Console.BackgroundColor = backgroundColor;
			Console.ForegroundColor = foregroundColor;
			Console.Write(value);
			Console.BackgroundColor = oldBackgroundColor;
			Console.ForegroundColor = oldForegroundColor;
		}

		public static void StartProcess(string processName)
		{
			Process.Start(new ProcessStartInfo(processName) { UseShellExecute = true });
		}

		public static void CloseApplication()
		{
			WriteNewLine();
			WriteLine("Press any key to exit...");
			Flash();
			Console.ReadKey(true);
			Environment.Exit(0);
		}

		public static void Flash()
		{
			FLASHWINFO fInfo = new FLASHWINFO();

			fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
			fInfo.hwnd = Process.GetCurrentProcess().MainWindowHandle;
			fInfo.dwFlags = FLASHW_TRAY | FLASHW_TIMERNOFG;
			fInfo.uCount = uint.MaxValue;
			fInfo.dwTimeout = 0;

			FlashWindowEx(ref fInfo);
		}

		private static bool ConsoleEventCallback(int eventType)
		{
			if (eventType == 2)
			{
				OnExit();
			}
			return false;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

		[StructLayout(LayoutKind.Sequential)]
		private struct FLASHWINFO
		{
			public uint cbSize;
			public IntPtr hwnd;
			public uint dwFlags;
			public uint uCount;
			public int dwTimeout;
		}

		private const uint FLASHW_STOP = 0;
		private const uint FLASHW_CAPTION = 1;
		private const uint FLASHW_TRAY = 2;
		private const uint FLASHW_ALL = 3;
		private const uint FLASHW_TIMER = 4;
		private const uint FLASHW_TIMERNOFG = 12;
	}
}
