using S5MS.Helpers;
using System;
using System.Reflection;

namespace S5MS
{
	class Program
	{
		public static void Main()
		{
			Console.Title = "Steam Five Minutes Sharing " + Assembly.GetExecutingAssembly().GetName().Version;

			ConsoleHelper.Init();
			SteamHelper.Init();
			FirewallHelper.Init();
			Cycler.Init();
		}
	}
}
