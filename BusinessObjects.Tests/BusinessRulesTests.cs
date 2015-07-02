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
	[TestFixture]
	[Category("travisci")]
	public class BusinessRulesTests
	{
		[Test]
		public void BuildEmptyObject()
		{
			var a = new ObjectUnderTest();
			Assert.That(() => a.Validate(), Throws.TypeOf<BusinessObjectValidationException>().With.Message.Contains("is a required identifier"));
		}

		[Test]
		public void BuildNoNameObject()
		{
			var a = new ObjectUnderTest()
			{
				Id = 10
			};
			Assert.That(() => a.Validate(), Throws.TypeOf<BusinessObjectValidationException>().With.Message.Contains("is required"));
		}

		[Test]
		public void BuildNoStringLengthObject()
		{
			var a = new ObjectUnderTest()
			{
				Id = 1,
				Name = "something",
			};
			Assert.That(() => a.Validate(), Throws.TypeOf<BusinessObjectValidationException>().With.Message.Contains("must be between"));
		}

		[Test]
		public void BuildTooShortObject()
		{
			var a = new ObjectUnderTest()
			{
				Id = 1,
				Name = "something",
				Between10To20 = "123456789"
			};
			Assert.That(() => a.Validate(), Throws.TypeOf<BusinessObjectValidationException>().With.Message.Contains("must be between"));
		}

		[Test]
		public void BuildTooLongObject()
		{
			var a = new ObjectUnderTest()
			{
				Id = 1,
				Name = "something",
				Between10To20 = "123456789012345678901"
			};
			Assert.That(() => a.Validate(), Throws.TypeOf<BusinessObjectValidationException>().With.Message.Contains("must be between"));
		}

		[Test]
		public void BuildValidObject()
		{
			var a = new ObjectUnderTest()
			{
				Id = 1,
				Name = "something",
				Between10To20 = "1234567890"
			};
			Assert.That(a.Validate(), Is.True);
		}
	}
}
