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
				new CommandLineOption("Shows the build and version information", "v", "Version", "",
					new ArgumentDelegate(new Environment().AssemblyVersion)),

				new CommandLineOption("Builds a blank database (does not delete an already existing one)", "b", "BuildDatabase", "",
					new ArgumentDelegate(new Environment().BuildDatabase)),

				new CommandLineOption("Displays available drives list", "a", "Drives", "",
					new ArgumentDelegate(new Environment().DisplayDriveList)),

				new CommandLineOption("Displays information about selected drive", "d", "Drive", "<drive>",
					new ArgumentDelegate(new Environment().DisplayDriveInfo)),
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
			System.Console.WriteLine("CatalogMasterCLI.exe [options]");
			System.Console.WriteLine("Options:");
			foreach (var argument in _commandLineOptions)
			{
				System.Console.WriteLine("  /{0} or /{1} {2}: {3}", argument.ShortOption, argument.LongOption, argument.Parameters, argument.Help);
			}
		}

		public bool RunFromArguments(CommandLineArguments commandLineArguments)
		{
			var results = true;

			foreach (var commandLineArgument in commandLineArguments.Arguments)
			{
				var result = RunFromArgument(commandLineArgument);
				if (result == false)
				{
					results = false;
				}
			}
			return results;
		}

		public bool RunFromArgument(CommandLineArgument commandLineArgument)
		{
			bool result = false;
			var commandLineOption = GetArgument(commandLineArgument.Option);
			if (commandLineOption != null)
			{
				result = commandLineOption.RunArgumentDelegate(commandLineArgument.Parameters);
			}
			return result;
		}
	}
}
