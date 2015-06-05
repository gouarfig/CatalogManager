using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Console
{
	public sealed class CommandLineArgument
	{
		public string Option { get; private set; }
		public IEnumerable<string> Parameters { get; private set; }

		public CommandLineArgument(string option, IEnumerable<string> parameters)
		{
			Option = option;
			Parameters = parameters;
		}
	}
}
