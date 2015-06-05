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
	public class CategoryDaoTests
	{
		[Test]
		public void InstantiateWithoutDb()
		{
			var category = new CategoryDao();
			Assert.IsNotNull(category);
		}

		[Test]
		public void InstantiateWithDb()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			Assert.IsNotNull(category);
		}

		[Test]
		public void MakeAndTakeCategoryBack()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);

			var guid = Guid.NewGuid();
			DateTime created = DateTime.Today;

			var reader = new Mock<IDataReader>();
			reader.Setup(r => r["Id"]).Returns(123);
			reader.Setup(r => r["Guid"]).Returns(guid);
			reader.Setup(r => r["Name"]).Returns("NameTestData");
			reader.Setup(r => r["Created"]).Returns(created);
			reader.Setup(r => r["IncludeInSearch"]).Returns(true);
	
			var take = category.TestMakeTake(reader.Object);
			Assert.AreEqual(new object[]{
				"@Id", 123,
				"@Guid", guid,
				"@Name", "NameTestData",
				"@Created", created,
				"@IncludeInSearch", true,
				}, take);
		}

		[Test]
		public void CreateStructure()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			category.CreateStructure();
			db.Verify(a => a.CreateStructure(It.IsNotNull<string>()));
		}

		[Test]
		public void GetCategories()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			category.GetCategories();
			db.Verify(
				a => a.Read(
					"SELECT Id, Guid, Name, Created, IncludeInSearch FROM Category",
					It.IsNotNull<Func<IDataReader, Category>>(),
					It.IsNotNull<Object[]>()
				)
			);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetCategoryByZeroId()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			category.GetCategoryById(0);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetCategoryByNegativeId()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			category.GetCategoryById(-10);
		}

		[Test]
		public void GetCategoryById()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			category.GetCategoryById(123);
			db.Verify(
				a => a.Read(
					"SELECT Id, Guid, Name, Created, IncludeInSearch FROM Category WHERE Id = @Id",
					It.IsNotNull<Func<IDataReader, Category>>(),
					It.IsNotNull<Object[]>()
				)
			);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetCategoryByEmptyGuid()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			var guid = Guid.NewGuid();
			category.GetCategoryByGuid(Guid.Empty);
		}

		[Test]
		public void GetCategoryByGuid()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			var guid = Guid.NewGuid();
			category.GetCategoryByGuid(guid);
			db.Verify(
				a => a.Read(
					"SELECT Id, Guid, Name, Created, IncludeInSearch FROM Category WHERE Guid = @Guid",
					It.IsNotNull<Func<IDataReader, Category>>(),
					It.IsNotNull<Object[]>()
				)
			);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetCategoryByNullName()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			category.GetCategoryByName(null);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void GetCategoryByEmptyName()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			category.GetCategoryByName("");
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void GetCategoryByInvalidName()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			var name = new string('a', 500);
			category.GetCategoryByName(name);
		}

		[Test]
		public void GetCategoryByName()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			category.GetCategoryByName("MyName");
			db.Verify(
				a => a.Read(
					"SELECT Id, Guid, Name, Created, IncludeInSearch FROM Category WHERE Name = @Name",
					It.IsNotNull<Func<IDataReader, Category>>(),
					It.IsNotNull<Object[]>()
				)
			);
		}


		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void InsertCategoryWithInvalidName()
		{
			var db = new Mock<IDb>();
			var categoryDao = new CategoryDao(db.Object);
			var name = new string('a', 500);
			var category = new Category()
			{
				Guid = Guid.NewGuid(),
				Name = name,
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			categoryDao.InsertCategory(category);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void InsertCategoryWithEmptyName()
		{
			var db = new Mock<IDb>();
			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Guid = Guid.NewGuid(),
				Name = "",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			categoryDao.InsertCategory(category);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void InsertCategoryWithNullName()
		{
			var db = new Mock<IDb>();
			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Guid = Guid.NewGuid(),
				Name = null,
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			categoryDao.InsertCategory(category);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void InsertCategoryWithInvalidGuid()
		{
			var db = new Mock<IDb>();
			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Guid = Guid.Empty,
				Name = "Some cool name",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			categoryDao.InsertCategory(category);
		}

		[Test]
		public void InsertCategory()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Insert(
					"INSERT INTO Category (Guid, Name, Created, IncludeInSearch) VALUES (@Guid, @Name, @Created, @IncludeInSearch)",
					It.IsNotNull<Object[]>()
					)
				).Returns(1);

			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Guid = Guid.NewGuid(),
				Name = "Test Category",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			var inserted = categoryDao.InsertCategory(category);
			Assert.IsTrue(inserted);
		}

		[Test]
		public void InsertCategoryFailed()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Insert(
					It.IsAny<string>(),
					It.IsAny<Object[]>()
					)
				).Returns(0);

			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Guid = Guid.NewGuid(),
				Name = "Test Category",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			var inserted = categoryDao.InsertCategory(category);
			Assert.IsFalse(inserted);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void UpdateCategoryWithInvalidName()
		{
			var db = new Mock<IDb>();
			var categoryDao = new CategoryDao(db.Object);
			var name = new string('a', 500);
			var category = new Category()
			{
				Id = 10,
				Guid = Guid.NewGuid(),
				Name = name,
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			categoryDao.UpdateCategory(category);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void UpdateCategoryWithEmptyName()
		{
			var db = new Mock<IDb>();
			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Id = 10,
				Guid = Guid.NewGuid(),
				Name = "",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			categoryDao.UpdateCategory(category);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void UpdateCategoryWithNullName()
		{
			var db = new Mock<IDb>();
			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Id = 10,
				Guid = Guid.NewGuid(),
				Name = null,
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			categoryDao.UpdateCategory(category);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentException))]
		public void UpdateCategoryWithInvalidGuid()
		{
			var db = new Mock<IDb>();
			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Id = 10,
				Guid = Guid.Empty,
				Name = "Some cool name",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			categoryDao.UpdateCategory(category);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void UpdateCategoryWithNegativeId()
		{
			var db = new Mock<IDb>();
			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Id = -10,
				Guid = Guid.NewGuid(),
				Name = "Some cool name",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			categoryDao.UpdateCategory(category);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void UpdateCategoryWithZeroId()
		{
			var db = new Mock<IDb>();
			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Id = 0,
				Guid = Guid.NewGuid(),
				Name = "Some cool name",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			categoryDao.UpdateCategory(category);
		}

		[Test]
		public void UpdateCategory()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Update(
					"UPDATE Category SET Guid=@Guid, Name=@Name, Created=@Created, IncludeInSearch=@IncludeInSearch WHERE Id = @Id",
					It.IsNotNull<Object[]>()
					)
				).Returns(1);

			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Id = 10,
				Guid = Guid.NewGuid(),
				Name = "Test Category",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			var updated = categoryDao.UpdateCategory(category);
			Assert.IsTrue(updated);
		}

		[Test]
		public void UpdateCategoryFailed()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Update(
					It.IsAny<string>(),
					It.IsAny<Object[]>()
					)
				).Returns(0);

			var categoryDao = new CategoryDao(db.Object);
			var category = new Category()
			{
				Id = 10,
				Guid = Guid.NewGuid(),
				Name = "Test Category",
				IncludeInSearch = true,
				Created = DateTime.Now,
			};
			var updated = categoryDao.UpdateCategory(category);
			Assert.IsFalse(updated);
		}


		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void DeleteCategoryWithNegativeId()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			category.DeleteCategory(-10);
		}

		[Test]
		[ExpectedException(typeof(System.ArgumentOutOfRangeException))]
		public void DeleteCategoryWithZeroId()
		{
			var db = new Mock<IDb>();
			var category = new CategoryDao(db.Object);
			category.DeleteCategory(0);
		}

		[Test]
		public void DeleteCategory()
		{
			var db = new Mock<IDb>();
			db.Setup(
				a => a.Delete(
					"DELETE FROM Category WHERE Id = @Id",
					It.IsNotNull<Object[]>()
					)
				).Returns(1);

			var category = new CategoryDao(db.Object);
			var deleted = category.DeleteCategory(10);
			Assert.IsTrue(deleted);
		}
	}
}
