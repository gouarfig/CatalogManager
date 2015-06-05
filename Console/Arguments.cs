using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
	public sealed class Arguments
	{
		private List<Argument> _arguments; 

		public Arguments()
		{
			_arguments = new List<Argument>
			{
				new Argument("Shows the build and version information", "v", "Version",
					new ArgumentDelegate(new Environment().AssemblyVersion)),

				new Argument("Builds a blank database (does not delete an already existing one)", "b", "BuildDatabase",
					new ArgumentDelegate(new Environment().BuildDatabase)),

				new Argument("Displays available drives list", "d", "Drives",
					new ArgumentDelegate(new Environment().DisplayDriveList)),
			};
		}

		public Argument GetArgument(string arg)
		{
			arg = arg.ToLower();
			var argument = _arguments.Find(a => a.ShortArgument.ToLower() == arg || a.LongArgument.ToLower() == arg);
			return argument;
		}

		public void DisplayHelp()
		{
			System.Console.WriteLine("Catalog Master [options]");
			System.Console.WriteLine("Options:");
			foreach (var argument in _arguments)
			{
				System.Console.WriteLine("  /{0} (/{1}): {2}", argument.ShortArgument, argument.LongArgument, argument.Help);
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
