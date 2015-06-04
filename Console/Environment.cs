using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
	public class Environment
	{
		public bool Version(IEnumerable<string> parameters)
		{
			var assembly = Assembly.GetExecutingAssembly().GetName();
			var name = "Catalog Master";
			var version = assembly.Version;
			System.Console.WriteLine(String.Format("{0} version {1}", name, version));
			return true;
		}
	}
}
