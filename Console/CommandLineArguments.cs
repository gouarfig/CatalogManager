using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
	public sealed class CommandLineArguments
	{
		private List<CommandLineArgument> _arguments;

		public List<CommandLineArgument> Arguments
		{
			get
			{
				return _arguments;
			}
		}

		public CommandLineArguments(string[] args)
		{
			_arguments = new List<CommandLineArgument>();

			if (args.Length > 0)
			{
				var currentOption = "";
				var currentParameters = new List<string>();

				for (int i = 0; i < args.Length; i++)
				{
					if (args[i].StartsWith("/") || args[i].StartsWith("-"))
					{
						// Begining of a new option
						if (!String.IsNullOrEmpty(currentOption))
						{
							// We record the previous option
							_arguments.Add(new CommandLineArgument(currentOption, currentParameters));
						}
						currentOption = args[i].Substring(1);
						currentParameters = new List<string>();
					}
					else
					{
						// Parameters inside an option
						currentParameters.Add(args[i]);
					}
				}
				// Last option?
				if (!String.IsNullOrEmpty(currentOption))
				{
					_arguments.Add(new CommandLineArgument(currentOption, currentParameters));
				}
			}
		}
	}
}
