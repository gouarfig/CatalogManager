using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
	public delegate bool ArgumentDelegate(IEnumerable<string> parameters);

	public sealed class CommandLineOption
	{
		public string Help { get; private set; }
		public string ShortOption { get; private set; }
		public string LongOption { get; private set; }
		public string Parameters { get; private set; }
		public ArgumentDelegate RunArgumentDelegate { get; private set; }

		public CommandLineOption(string help, string shortOption, string longOption, string parameters, ArgumentDelegate code)
		{
			Help = help;
			ShortOption = shortOption;
			LongOption = longOption;
			Parameters = parameters;
			RunArgumentDelegate = code;
		}
	}
}
