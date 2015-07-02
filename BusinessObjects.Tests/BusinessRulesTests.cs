using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using NUnit.Framework;
using BusinessObjects.BusinessRules;

namespace BusinessObjects.Tests
{
	public class MyObjectUnderTest : BusinessObject
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
	}


	[TestFixture]
	public class BusinessRulesTests
	{
		[Test]
		public void BuildEmptyObject()
		{
			var a = new MyObjectUnderTest();
			Assert.That(a.Validate(), Is.False);
		}

		[Test]
		public void BuildValidObject()
		{
			var a = new MyObjectUnderTest()
			{
				Id = 1,
				Name = "something"
			};
			Assert.That(a.Validate(), Is.True);
		}
	}
}
