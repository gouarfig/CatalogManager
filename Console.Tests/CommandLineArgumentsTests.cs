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

		[Test]
		public void Empty()
		{
			string[] args = new string[0];

			var cla = new CommandLineArguments(args);
			var arguments = cla.Arguments;
			Assert.IsNotNull(arguments);
			Assert.AreEqual(0, arguments.Count);
		}

		[Test]
		public void Simple()
		{
			string[] args = new string[]
			{
				"/Version"
			};

			var cla = new CommandLineArguments(args);
			var arguments = cla.Arguments;
			Assert.IsNotNull(arguments);
			// 1 option
			Assert.AreEqual(1, arguments.Count);
			// No parameter
			Assert.AreEqual(0, arguments[0].Parameters.Count());
		}

		[Test]
		public void None()
		{
			string[] args = new string[]
			{
				"Something"
			};

			var cla = new CommandLineArguments(args);
			var arguments = cla.Arguments;
			Assert.IsNotNull(arguments);
			Assert.AreEqual(0, arguments.Count);
		}

		[Test]
		public void TwoNonOption()
		{
			string[] args = new string[]
			{
				"Something", "else"
			};

			var cla = new CommandLineArguments(args);
			var arguments = cla.Arguments;
			Assert.IsNotNull(arguments);
			Assert.AreEqual(0, arguments.Count);
		}

		[Test]
		public void ThreeOptionsAndParameters()
		{
			string[] args = new string[]
			{
				"/v", "version",
				"/b", "build", "something",
				"/d", "and", "even", "more",
			};

			var cla = new CommandLineArguments(args);
			var arguments = cla.Arguments;
			Assert.IsNotNull(arguments);
			Assert.AreEqual(3, arguments.Count);

			Assert.AreEqual(1, arguments[0].Parameters.Count());
			Assert.AreEqual(2, arguments[1].Parameters.Count());
			Assert.AreEqual(3, arguments[2].Parameters.Count());
		}
	}
}
