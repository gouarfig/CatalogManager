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
	}
}
