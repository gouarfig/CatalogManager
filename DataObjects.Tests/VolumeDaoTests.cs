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

namespace DataObjects.Tests
{
	[TestFixture]
	public class VolumeDaoTests
	{
		[Test]
		public void InstanciateWithoutDb()
		{
			var configuration = new VolumeDao();
			Assert.IsNotNull(configuration);
		}

		[Test]
		public void InstanciateWithDb()
		{
			var db = new Mock<IDb>();
			var configuration = new VolumeDao(db.Object);
			Assert.IsNotNull(configuration);
		}

		[Test]
		public void MakeAndTakeConfigurationBack()
		{
			var db = new Mock<IDb>();
			var configuration = new VolumeDao(db.Object);
			var reader = new Mock<IDataReader>();
			reader.Setup(r => r["Id"]).Returns(11);
			var guid = Guid.NewGuid();
			reader.Setup(r => r["Guid"]).Returns(guid);
			reader.Setup(r => r["Name"]).Returns("Test Volume");
			reader.Setup(r => r["VolumeType"]).Returns(3);
			reader.Setup(r => r["VolumeId"]).Returns("Volume ID");
			var created = new DateTime(1990, 1, 12, 12, 20, 02);
			reader.Setup(r => r["Created"]).Returns(created);
			var cataloged = new DateTime(2001, 2, 13, 13, 25, 03);
			reader.Setup(r => r["Cataloged"]).Returns(cataloged);
			reader.Setup(r => r["TotalSize"]).Returns(446522158);
			reader.Setup(r => r["SpaceFree"]).Returns(12045638);
			reader.Setup(r => r["RegularFiles"]).Returns(556677);
			reader.Setup(r => r["HiddenFiles"]).Returns(223344);
			reader.Setup(r => r["Path"]).Returns(@"C:\temp");
			reader.Setup(r => r["ComputerName"]).Returns("My Computer");
			reader.Setup(r => r["IncludeInSearch"]).Returns(true);
			var take = configuration.TestMakeTake(reader.Object);
			Assert.AreEqual(new object[]{
				"@Id", 11,
				"@Guid", guid,
				"@Name", "Test Volume",
				"@VolumeType", VolumeType.NetworkDrive,
				"@VolumeId", "Volume ID",
				"@Created", created,
				"@Cataloged", cataloged,
				"@TotalSize", 446522158,
				"@SpaceFree", 12045638,
				"@RegularFiles", 556677,
				"@HiddenFiles", 223344,
				"@Path", @"C:\temp",
				"@ComputerName", "My Computer",
				"@IncludeInSearch", true,
				}, take);
		}

		[Test]
		public void CreateStructure()
		{
			var db = new Mock<IDb>();
			var configuration = new VolumeDao(db.Object);
			configuration.CreateStructure();
			db.Verify(a => a.CreateStructure(It.IsNotNull<string>()));
		}
	}
}
