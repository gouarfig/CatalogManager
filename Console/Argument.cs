using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
	public delegate bool ArgumentDelegate(IEnumerable<string> parameters);

	public sealed class Argument
	{
		public string Help { get; private set; }
		public string ShortArgument { get; private set; }
		public string LongArgument { get; private set; }
		public ArgumentDelegate RunArgumentDelegate { get; private set; }

		public Argument(string help, string shortArgument, string longArgument, ArgumentDelegate code)
		{
			Help = help;
			ShortArgument = shortArgument;
			LongArgument = longArgument;
			RunArgumentDelegate = code;
		}
	}
}
