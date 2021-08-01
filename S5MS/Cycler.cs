using S5MS.Helpers;
using System;
using System.Reflection;
using System.Threading;

namespace S5MS
{
	class Cycler
	{
		public enum Mode
		{
			None,
			BlockAlways,
			BlockPeriodically
		}

		public static Mode WorkMode = Mode.None;

		public static int TimeInMillisecondsToBlock = 60000;
		public static int TimeInMillisecondsToUnblock = 100;

		public static void Init()
		{
			Console.CursorVisible = false;
			ConsoleHelper.WriteNewLine();
			ConsoleHelper.WriteLine("	* Steam Five Minutes Sharing " + Assembly.GetExecutingAssembly().GetName().Version + " *", ConsoleColor.Magenta);

			ConsoleHelper.WriteLine("   - Designed to abuse Steam Family Sharing offline mode -", ConsoleColor.DarkMagenta);

			ConsoleHelper.WriteNewLine();

			if (string.IsNullOrWhiteSpace(SteamHelper.CurrentSteamExecutablePath))
			{
				ConsoleHelper.WriteLine("* Error: Could not find Steam executable.", ConsoleColor.Red);
				ConsoleHelper.CloseApplication();
			}

			if (FirewallHelper.CurrentFirewallPolicy == null)
			{
				ConsoleHelper.WriteLine("* Error: Failed to get Firewall policy.", ConsoleColor.Red);
				ConsoleHelper.CloseApplication();
			}

			if (FirewallHelper.CurrentFirewallOutRule == null || FirewallHelper.CurrentFirewallInRule == null)
			{
				ConsoleHelper.WriteLine("* Error: Failed to generate Firewall rules.", ConsoleColor.Red);
				ConsoleHelper.CloseApplication();
			}

			if (WorkMode == Mode.None)
			{
				ConsoleHelper.WriteLine("* Select work mode:", ConsoleColor.DarkYellow);

				ConsoleHelper.WriteNewLine();

				ConsoleHelper.Write("   Press 1 to ", ConsoleColor.DarkCyan);
				ConsoleHelper.Write("Block Always", ConsoleColor.Cyan);

				ConsoleHelper.WriteNewLine();

				ConsoleHelper.Write("   Press 2 to ", ConsoleColor.DarkCyan);
				ConsoleHelper.Write("Block Periodically", ConsoleColor.Cyan);

				ConsoleHelper.WriteNewLine();

				switch (Console.ReadKey(true).Key)
				{
					case ConsoleKey.D1:
						WorkMode = Mode.BlockAlways;
						break;
					case ConsoleKey.D2:
						WorkMode = Mode.BlockPeriodically;
						break;
					default:
						WorkMode = Mode.BlockAlways;
						break;
				}

				ConsoleHelper.WriteNewLine();
				ConsoleHelper.Write(" Selected mode: ", ConsoleColor.DarkYellow);
				ConsoleHelper.Write(GetWorkModeString(WorkMode), ConsoleColor.Cyan);
				ConsoleHelper.WriteNewLine(2);
			}

			switch (WorkMode)
			{
				case Mode.BlockAlways:
					ConsoleHelper.WriteLine("* The program will block Steam backend connection until termination.", ConsoleColor.DarkYellow);
					break;
				case Mode.BlockPeriodically:
					ConsoleHelper.WriteLine(string.Format("* The program will block Steam backend connection every {0} minute{1} for {2} second{3} until termination.",
						Math.Round((float)TimeInMillisecondsToBlock / 60000, 1).ToString().Replace(",", "."),
						((float)Math.Round((float)TimeInMillisecondsToBlock / 60000, 1)) != 1 ? "s" : string.Empty,
						Math.Round((float)TimeInMillisecondsToUnblock / 1000, 3).ToString().Replace(",", "."),
						((float)Math.Round((float)TimeInMillisecondsToUnblock / 1000, 1)) != 1 ? "s" : string.Empty), ConsoleColor.DarkYellow);
					break;
			}
			ConsoleHelper.Write("* It is recommended to launch Steam with ", ConsoleColor.DarkYellow);
			ConsoleHelper.Write("-tcp", ConsoleColor.Yellow);
			ConsoleHelper.Write(" parameter.", ConsoleColor.DarkYellow);
			ConsoleHelper.WriteNewLine(2);
			ConsoleHelper.WriteLine("* Warning: The program will overwrite your current Steam status and can put you out of being Invisible.", ConsoleColor.DarkYellow);
			ConsoleHelper.WriteNewLine();
			ConsoleHelper.WriteLine("  Press any key to start.", ConsoleColor.DarkCyan);

			Console.ReadKey(true);

			ConsoleHelper.WriteNewLine();
			ConsoleHelper.WriteLine("* Program started.", ConsoleColor.DarkYellow);
			ConsoleHelper.WriteLine("* Close application to stop blocking Steam.", ConsoleColor.DarkYellow);
			ConsoleHelper.WriteNewLine();
			ConsoleHelper.WriteLine(" Status: ", ConsoleColor.DarkYellow);

			switch (WorkMode)
			{
				case Mode.BlockAlways:
					DoBlock();
					break;
				case Mode.BlockPeriodically:
					DoBlockCycle();
					break;
			}
		}

		public static void DoBlock()
		{
			FirewallHelper.AddFirewallRule(FirewallHelper.CurrentFirewallOutRule);
			FirewallHelper.AddFirewallRule(FirewallHelper.CurrentFirewallInRule);
			ConsoleHelper.WriteLine("  Steam blocked!", ConsoleColor.DarkGreen);
			ConsoleHelper.StartProcess("steam://friends/status/away");
			ConsoleHelper.StartProcess("steam://friends/status/online");

			Thread.Sleep(Timeout.Infinite);
		}

		public static void DoBlockCycle()
		{
			FirewallHelper.AddFirewallRule(FirewallHelper.CurrentFirewallOutRule);
			FirewallHelper.AddFirewallRule(FirewallHelper.CurrentFirewallInRule);
			ConsoleHelper.Write("\r  Steam blocked!  ", ConsoleColor.DarkGreen);
			ConsoleHelper.StartProcess("steam://friends/status/away");
			ConsoleHelper.StartProcess("steam://friends/status/online");
			Thread.Sleep(TimeInMillisecondsToUnblock);
			ConsoleHelper.StartProcess("steam://friends/status/away");
			ConsoleHelper.StartProcess("steam://friends/status/online");
			FirewallHelper.RemoveFirewallRule(FirewallHelper.CurrentFirewallOutRule);
			FirewallHelper.RemoveFirewallRule(FirewallHelper.CurrentFirewallInRule);
			ConsoleHelper.Write("\r  Steam unblocked!", ConsoleColor.DarkRed);
			Thread.Sleep(TimeInMillisecondsToBlock);

			DoBlockCycle();
		}

		public static string GetWorkModeString(Mode workMode)
		{
			switch (workMode)
			{
				case Mode.BlockAlways:
					return "Block Always";
				case Mode.BlockPeriodically:
					return "Block Periodically";
				default:
					return "Unknown";
			}
		}
	}
}
