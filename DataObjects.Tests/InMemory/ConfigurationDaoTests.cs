using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Moq;
using NUnit.Framework;
using DataObjects.InMemory;

namespace DataObjects.Tests.InMemory
{
	[TestFixture]
	[Category("travisci")]
	public class ConfigurationDaoTests
	{
		[Test]
		public void Instantiate()
		{
			var configuration = new ConfigurationDao();
			Assert.IsNotNull(configuration);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetNullParameter()
		{
			var configuration = new ConfigurationDao();
			configuration.Get(null);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetEmptyParameter()
		{
			var configuration = new ConfigurationDao();
			configuration.Get("");
		}

		[Test]
		public void Get()
		{
			var configuration = new ConfigurationDao();
			configuration.Get("some_value");
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void PutNullParameter()
		{
			var configuration = new ConfigurationDao();
			configuration.Put(null);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void PutNullNameParameter()
		{
			var configuration = new ConfigurationDao();
			var item = new Configuration(
				name: null,
				value: "The Value"
				);
			configuration.Put(item);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void PutEmptyNameParameter()
		{
			var configuration = new ConfigurationDao();
			var item = new Configuration(
				name: "",
				value: "The Value"
				);
			configuration.Put(item);
		}

		[Test]
		public void Put()
		{
			var configuration = new ConfigurationDao();
			var item = new Configuration(
				name: "My Name",
				value: "The Value"
				);
			configuration.Put(item);
		}

		[Test]
		public void GetNonExistingItem()
		{
			var configuration = new ConfigurationDao();
			var item = configuration.Get("something");
			Assert.IsNull(item);
		}

		[Test]
		public void PutAndGetWrongItem()
		{
			var configuration = new ConfigurationDao();
			var item = new Configuration(
				name: "My Name",
				value: "The Value"
				);
			configuration.Put(item);
			var itemBack = configuration.Get("something");
			Assert.IsNull(itemBack);
		}

		[Test]
		public void PutAndGetItem()
		{
			var configuration = new ConfigurationDao();
			var item = new Configuration(
				name: "My Name",
				value: "The Value"
				);
			configuration.Put(item);
			var itemBack = configuration.Get("My Name");
			Assert.AreEqual(item, itemBack);
		}
	}
}
