using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
	public class Environment
	{
		public bool Version(IEnumerable<string> parameters)
		{
			System.Console.WriteLine("CatalogMaster version 0.0.0");
			return true;
		}
	}
}
