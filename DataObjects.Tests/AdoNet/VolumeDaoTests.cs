using System;
using System.Data;
using BusinessObjects;
using DataObjects.AdoNet;
using Moq;
using NUnit.Framework;

namespace DataObjects.Tests.AdoNet
{
	[TestFixture]
	public class VolumeDaoTests
	{
		private const string _selectString =
			"SELECT Id, Guid, Name, VolumeType, VolumeId, Created, Cataloged, TotalSize, SpaceFree, RegularFiles, HiddenFiles, Path, ComputerName, IncludeInSearch FROM Volume";
		private const string _insertString =
			"INSERT INTO Volume (Guid, Name, VolumeType, VolumeId, Created, Cataloged, TotalSize, SpaceFree, RegularFiles, HiddenFiles, Path, ComputerName, IncludeInSearch) " +
			"VALUES (@Guid, @Name, @VolumeType, @VolumeId, @Created, @Cataloged, @TotalSize, @SpaceFree, @RegularFiles, @HiddenFiles, @Path, @ComputerName, @IncludeInSearch)";

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
			var volume = new VolumeDao(db.Object);
			volume.CreateStructure();
			db.Verify(a => a.CreateStructure(It.IsNotNull<string>()));
		}

		[Test]
		public void GetVolumes()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.GetVolumes();
			db.Verify(
				a => a.Read(
					_selectString,
					It.IsNotNull<Func<IDataReader, Volume>>(),
					It.IsNotNull<Object[]>()
				)
			);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetVolumeByZeroId()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.GetVolumeById(0);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetVolumeByNegativeId()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.GetVolumeById(-10);
		}

		[Test]
		public void GetVolumeById()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.GetVolumeById(123);
			db.Verify(
				a => a.Read(
					String.Format("{0} WHERE Id = @Id", _selectString),
					It.IsNotNull<Func<IDataReader, Volume>>(),
					It.IsNotNull<Object[]>()
				)
			);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetVolumeByEmptyGuid()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			var guid = Guid.NewGuid();
			volume.GetVolumeByGuid(Guid.Empty);
		}

		[Test]
		public void GetVolumeByGuid()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			var guid = Guid.NewGuid();
			volume.GetVolumeByGuid(guid);
			db.Verify(
				a => a.Read(
					String.Format("{0} WHERE Guid = @Guid", _selectString),
					It.IsNotNull<Func<IDataReader, Volume>>(),
					It.IsNotNull<Object[]>()
				)
			);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetVolumeByNullName()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.GetVolumeByName(null);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetVolumeByEmptyName()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.GetVolumeByName("");
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void GetVolumeByInvalidName()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			var name = new string('a', 500);
			volume.GetVolumeByName(name);
		}

		[Test]
		public void GetVolumeByName()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.GetVolumeByName("MyName");
			db.Verify(
				a => a.Read(
					String.Format("{0} WHERE Name = @Name", _selectString),
					It.IsNotNull<Func<IDataReader, Volume>>(),
					It.IsNotNull<Object[]>()
				)
			);
		}


		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetVolumeByNullVolumeId()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.GetVolumeByVolumeId(null);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetVolumeByEmptyVolumeId()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.GetVolumeByVolumeId("");
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void GetVolumeByInvalidVolumeId()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			var name = new string('a', 500);
			volume.GetVolumeByVolumeId(name);
		}

		[Test]
		public void GetVolumeByVolumeId()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.GetVolumeByVolumeId("MyName");
			db.Verify(
				a => a.Read(
					String.Format("{0} WHERE VolumeId = @VolumeId", _selectString),
					It.IsNotNull<Func<IDataReader, Volume>>(),
					It.IsNotNull<Object[]>()
				)
			);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void InsertVolumeWithInvalidName()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var name = new string('a', 500);
			var volume = new Volume()
			{
				Guid = Guid.NewGuid(),
				Name = name,
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			volumeDao.InsertVolume(volume);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void InsertVolumeWithEmptyName()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Guid = Guid.NewGuid(),
				Name = "",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			volumeDao.InsertVolume(volume);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void InsertVolumeWithNullName()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Guid = Guid.NewGuid(),
				Name = null,
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			volumeDao.InsertVolume(volume);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void InsertVolumeWithInvalidGuid()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Guid = Guid.Empty,
				Name = "Some cool name",
				VolumeId = "Volume ID",
				Path = "C:\temp",
				ComputerName = "MyComputer",
				Created = DateTime.Now,
				IncludeInSearch = true,
			};
			volumeDao.InsertVolume(volume);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void InsertVolumeWithEmptyPath()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Guid = Guid.Empty,
				Name = "Some cool name",
				VolumeId = "",
				Path = "",
				ComputerName = "",
			};
			volumeDao.InsertVolume(volume);
		}

		[Test]
		public void InsertVolume()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Insert(
					_insertString,
					It.IsNotNull<Object[]>()
					)
				).Returns(1);

			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Guid = Guid.NewGuid(),
				Name = "Test Volume",
				VolumeId = "Volume ID",
				Path = "C:\temp",
				ComputerName = "MyComputer",
				Created = DateTime.Now,
				IncludeInSearch = true,
			};
			var inserted = volumeDao.InsertVolume(volume);
			Assert.IsTrue(inserted);
		}

		[Test]
		public void InsertVolumeWithMinimalInformation()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Insert(
					_insertString,
					It.IsNotNull<Object[]>()
					)
				).Returns(1);

			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Guid = Guid.NewGuid(),
				Name = "Test Volume",
				VolumeId = "",
				Path = "C:\temp",
				ComputerName = "",
			};
			var inserted = volumeDao.InsertVolume(volume);
			Assert.IsTrue(inserted);
		}

		[Test]
		public void InsertVolumeFailed()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Insert(
					It.IsAny<string>(),
					It.IsAny<Object[]>()
					)
				).Returns(0);

			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Guid = Guid.NewGuid(),
				Name = "Test Volume",
				VolumeId = "Volume ID",
				Path = "C:\temp",
				ComputerName = "MyComputer",
				IncludeInSearch = true,
			};
			var inserted = volumeDao.InsertVolume(volume);
			Assert.IsFalse(inserted);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void UpdateVolumeWithInvalidName()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var name = new string('a', 500);
			var volume = new Volume()
			{
				Id = 10,
				Guid = Guid.NewGuid(),
				Name = name,
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			volumeDao.UpdateVolume(volume);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void UpdateVolumeWithEmptyName()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Id = 10,
				Guid = Guid.NewGuid(),
				Name = "",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			volumeDao.UpdateVolume(volume);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void UpdateVolumeWithNullName()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Id = 10,
				Guid = Guid.NewGuid(),
				Name = null,
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			volumeDao.UpdateVolume(volume);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void UpdateVolumeWithInvalidGuid()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Id = 10,
				Guid = Guid.Empty,
				Name = "Some cool name",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			volumeDao.UpdateVolume(volume);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void UpdateVolumeWithNegativeId()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Id = -10,
				Guid = Guid.NewGuid(),
				Name = "Some cool name",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			volumeDao.UpdateVolume(volume);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void UpdateVolumeWithZeroId()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Id = 0,
				Guid = Guid.NewGuid(),
				Name = "Some cool name",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			volumeDao.UpdateVolume(volume);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void UpdateVolumeWithEmptyPath()
		{
			var db = new Mock<IDb>();
			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Id = 11,
				Guid = Guid.NewGuid(),
				Name = "Some cool name",
				VolumeId = "",
				Path = "",
				ComputerName = "",
			};
			volumeDao.UpdateVolume(volume);
		}

		[Test]
		public void UpdateVolume()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Update(
					"UPDATE Volume SET Guid=@Guid, Name=@Name, VolumeType=@VolumeType, VolumeId=@VolumeId, Created=@Created, Cataloged=@Cataloged, TotalSize=@TotalSize, SpaceFree=@SpaceFree, RegularFiles=@RegularFiles, HiddenFiles=@HiddenFiles, Path=@Path, ComputerName=@ComputerName, IncludeInSearch=@IncludeInSearch WHERE Id = @Id",
					It.IsNotNull<Object[]>()
					)
				).Returns(1);

			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Id = 10,
				Guid = Guid.NewGuid(),
				Name = "Test Volume",
				VolumeId = "",
				Path = "C:\temp",
				ComputerName = "",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			var updated = volumeDao.UpdateVolume(volume);
			Assert.IsTrue(updated);
		}

		[Test]
		public void UpdateVolumeFailed()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Update(
					It.IsAny<string>(),
					It.IsAny<Object[]>()
					)
				).Returns(0);

			var volumeDao = new VolumeDao(db.Object);
			var volume = new Volume()
			{
				Id = 10,
				Guid = Guid.NewGuid(),
				Name = "Test Volume",
				VolumeId = "",
				Path = "C:\temp",
				ComputerName = "",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			var updated = volumeDao.UpdateVolume(volume);
			Assert.IsFalse(updated);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void DeleteVolumeWithNegativeId()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.DeleteVolume(-10);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void DeleteVolumeWithZeroId()
		{
			var db = new Mock<IDb>();
			var volume = new VolumeDao(db.Object);
			volume.DeleteVolume(0);
		}

		[Test]
		public void DeleteVolume()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Delete(
					"DELETE FROM Volume WHERE Id = @Id",
					It.IsNotNull<Object[]>()
					)
				).Returns(1);

			var volume = new VolumeDao(db.Object);
			var deleted = volume.DeleteVolume(10);
			Assert.IsTrue(deleted);
		}
	}
}
