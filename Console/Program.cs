using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var commandLineArguments = new CommandLineArguments(args);
			var commandLineOptions = new CommandLineOptions();
			commandLineOptions.RunFromArguments(commandLineArguments);
		}
	}
}
