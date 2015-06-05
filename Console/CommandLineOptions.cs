using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
	public sealed class CommandLineOptions
	{
		private List<CommandLineOption> _commandLineOptions; 

		public CommandLineOptions()
		{
			_commandLineOptions = new List<CommandLineOption>
			{
				new CommandLineOption("Shows the build and version information", "v", "Version",
					new ArgumentDelegate(new Environment().AssemblyVersion)),

				new CommandLineOption("Builds a blank database (does not delete an already existing one)", "b", "BuildDatabase",
					new ArgumentDelegate(new Environment().BuildDatabase)),

				new CommandLineOption("Displays available drives list", "d", "Drives",
					new ArgumentDelegate(new Environment().DisplayDriveList)),
			};
		}

		public CommandLineOption GetArgument(string arg)
		{
			arg = arg.ToLower();
			var argument = _commandLineOptions.Find(a => a.ShortOption.ToLower() == arg || a.LongOption.ToLower() == arg);
			return argument;
		}

		public void DisplayHelp()
		{
			System.Console.WriteLine("Catalog Master [options]");
			System.Console.WriteLine("Options:");
			foreach (var argument in _commandLineOptions)
			{
				System.Console.WriteLine("  /{0} (/{1}): {2}", argument.ShortOption, argument.LongOption, argument.Help);
			}
		}

		public bool RunFromArguments(string[] args)
		{
			var result = false;
			if (args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i].StartsWith("/") || args[i].StartsWith("-"))
					{
						var arg = args[i].Substring(1);
						var argument = GetArgument(arg);
						if (argument != null)
						{
							result = argument.RunArgumentDelegate(null);
						}
						else
						{
							DisplayHelp();
						}
					}
					else
					{
						DisplayHelp();
					}
				}
			}
			else
			{
				DisplayHelp();
			}
			return result;
		}
	}
}
