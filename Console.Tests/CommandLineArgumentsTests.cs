using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console.Tests
{
	[TestFixture]
	[Category("travisci")]
    public class CommandLineArgumentsTests
    {
		[Test]
		public void Instantiate()
		{
			string[] args = new string[0];

			var arguments = new CommandLineArguments(args);
			Assert.IsNotNull(arguments);
		}
    }
}
