using Microsoft.Win32;
using System;
using System.IO;

namespace S5MS.Helpers
{
	class SteamHelper
	{
		public const string SteamRegistryPath = @"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam";
		public static string CurrentSteamExecutablePath;

		public static void Init()
		{
			CurrentSteamExecutablePath = GetSteamExecutablePath();
		}

		private static string GetSteamExecutablePath()
		{
			try
			{
				return Path.GetFullPath((string)Registry.GetValue(SteamRegistryPath, "SteamExe", string.Empty));
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
	}
}
