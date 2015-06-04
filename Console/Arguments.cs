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
				new Argument("Shows the build and version information", "v", "version",
					new ArgumentDelegate(new Environment().Version))
			};
		}

		public Argument GetArgument(string arg)
		{
			var argument = _arguments.Find(a => a.ShortArgument == arg || a.LongArgument == arg);
			return argument;
		}

		public bool RunFromArguments(string[] args)
		{
			var result = false;
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
				}
			}
			return result;
		}
	}
}
