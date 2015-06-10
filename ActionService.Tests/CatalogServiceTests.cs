using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ActionService.Tests
{
	[TestFixture]
	[Category("travisci")]
	public class CatalogServiceTests
    {
		[Test]
		public void Instantiate()
		{
			ICatalogService service = new CatalogService("inmemory");
			Assert.IsNotNull(service);
		}

		[Test]
		public void GetUninitializedDatabaseVersion()
		{
			ICatalogService service = new CatalogService("inmemory");
			Assert.Throws<DataException>(
				delegate {
					var version = service.GetDatabaseVersion();
				});
		}

		[Test]
		public void GetInitializedDatabaseVersion()
		{
			ICatalogService service = new CatalogService("inmemory");
			service.InitializeDatabase();
			var version = service.GetDatabaseVersion();
			Assert.IsTrue(version > 0);
		}

		[Test]
		public void DisplayBytes()
		{
			ICatalogService service = new CatalogService("inmemory");
			var size = service.DisplayBytesToHuman(1000);
			Assert.AreEqual("1,000.00 ", size);
		}

		[Test]
		public void DisplayKiloBytes()
		{
			ICatalogService service = new CatalogService("inmemory");
			var size = service.DisplayBytesToHuman(10 * 1024);
			Assert.AreEqual("10.00 k", size);
		}

		[Test]
		public void DisplayMegaBytes()
		{
			ICatalogService service = new CatalogService("inmemory");
			var size = service.DisplayBytesToHuman(2 * 1024 * 1024);
			Assert.AreEqual("2.00 M", size);
		}

		[Test]
		public void DisplayGigaBytes()
		{
			ICatalogService service = new CatalogService("inmemory");
			var size = service.DisplayBytesToHuman((long)(5.6 * 1024 * 1024 * 1024));
			Assert.AreEqual("5.60 G", size);
		}

		[Test]
		public void DisplayTeraBytes()
		{
			ICatalogService service = new CatalogService("inmemory");
			var size = service.DisplayBytesToHuman((long)(1.23 * 1024 * 1024 * 1024 * 1024));
			Assert.AreEqual("1.23 T", size);
		}

		[Test]
		public void DisplayPetaBytes()
		{
			ICatalogService service = new CatalogService("inmemory");
			var size = service.DisplayBytesToHuman((long)(1.23456 * 1024 * 1024 * 1024 * 1024 * 1024));
			Assert.AreEqual("1.23 P", size);
		}

		[Test]
		public void DisplayExaBytes()
		{
			ICatalogService service = new CatalogService("inmemory");
			var size = service.DisplayBytesToHuman((long)(1.236 * 1024 * 1024 * 1024 * 1024 * 1024 * 1024));
			Assert.AreEqual("1.24 E", size);
		}

		[Test]
		public void NotANumber()
		{
			ICatalogService service = new CatalogService("inmemory");
			var nan = service.DisplayBytesToHuman("ttt");
			Assert.AreEqual("(Not A Number)", nan);
		}
    }
}
