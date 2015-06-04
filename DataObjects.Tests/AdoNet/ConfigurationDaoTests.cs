using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataObjects.AdoNet;
using Moq;
using NUnit.Framework;

namespace DataObjects.Tests.AdoNet
{
	[TestFixture]
	[Category("travisci")]
	public class ConfigurationDaoTests
	{
		[Test]
		public void InstanciateWithoutDb()
		{
			var configuration = new ConfigurationDao();
			Assert.IsNotNull(configuration);
		}

		[Test]
		public void InstanciateWithDb()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			Assert.IsNotNull(configuration);
		}

		[Test]
		public void MakeAndTakeConfigurationBack()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			var reader = new Mock<IDataReader>();
			reader.Setup(r => r["Name"]).Returns("NameTestData");
			reader.Setup(r => r["Value"]).Returns("ValueTestData");
			var take = configuration.TestMakeTake(reader.Object);
			Assert.AreEqual(new object[]{
				"@Name", "NameTestData",
				"@Value", "ValueTestData"
				}, take);
		}

		[Test]
		public void CreateStructure()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			configuration.CreateStructure();
			db.Verify(a => a.CreateStructure(It.IsNotNull<string>()));
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetNullParameter()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			configuration.Get(null);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetEmptyParameter()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			configuration.Get("");
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void GetInvalidParameter()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			var parameter = new string('a', 500);
			configuration.Get(parameter);
		}

		[Test]
		public void Get()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			configuration.Get("some_value");
			db.Verify(
				a => a.Read(
					"SELECT Name, Value FROM Configuration WHERE Name = @Name", 
					It.IsNotNull<Func<IDataReader,Configuration>>(), 
					It.IsNotNull<Object[]>()
				)
			);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentNullException))]
		public void PutNullParameter()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			configuration.Put(null);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void PutNullNameParameter()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			var item = new Configuration()
			{
				Name = null,
				Value = "The Value"
			};
			configuration.Put(item);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void PutEmptyNameParameter()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			var item = new Configuration()
			{
				Name = "",
				Value = "The Value"
			};
			configuration.Put(item);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void PutInvalidNameParameter()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			var item = new Configuration()
			{
				Name = new string('a', 500),
				Value = "The Value"
			};
			configuration.Put(item);
		}

		[Test]
		public void Put()
		{
			var db = new Mock<IDb>();
			var configuration = new ConfigurationDao(db.Object);
			var item = new Configuration()
			{
				Name = "My Name",
				Value = "The Value"
			};
			configuration.Put(item);
			db.Verify(
				a => a.UpdateOrInsert(
					"UPDATE Configuration SET Value = @Value WHERE Name = @Name",
					"INSERT INTO Configuration (Name, Value) VALUES (@Name, @Value)",
					It.IsNotNull<Object[]>()
				)
			);
		}
	}
}
