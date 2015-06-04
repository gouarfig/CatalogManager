using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects.AdoNet;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace DataObjects.Tests
{
	[TestFixture]
	[Category("sqlite")]
    public class DbTests
	{
		private static readonly string _testsDbFileName = "CatalogMasterTests.sqlite";
		private static readonly string _testTableName = "TestTable";
		private static readonly string _createSql = String.Format("CREATE TABLE {0} (Id INTEGER PRIMARY KEY, Value STRING (10));", _testTableName);
		private static readonly string _insertSql = String.Format(@"INSERT INTO {0} (Id, Value) VALUES (@Id, @Value)", _testTableName);
		private static readonly string _updateSql = String.Format(@"UPDATE {0} SET Value = @Value WHERE Id = @Id", _testTableName);

		private void DeleteDatabase()
		{
			if (File.Exists(_testsDbFileName))
			{
				File.Delete(_testsDbFileName);
			}
		}

		private Db CreateTestDatabase()
		{
			DeleteDatabase();
			var db = new Db();
			return db;
		}

		private void CreateTestTable(Db db)
		{
			var created = db.CreateStructure(_createSql);
			Assert.AreEqual(0, created);
		}

		[Test]
		public void CreateSqLiteDb()
		{
			SQLiteConnection.CreateFile(_testsDbFileName);
			using (var sqlite = new SQLiteConnection(String.Format("Data Source={0};Version=3", _testsDbFileName)))
			{
				Assert.IsNotNull(sqlite);
				sqlite.Open();
				sqlite.Close();
			}
			DeleteDatabase();
		}

		[Test]
		public void SelectFromNoTable()
		{
			var db = new Db();
			Assert.Throws<System.Data.SQLite.SQLiteException>(delegate { db.Scalar("SELECT * FROM NotATable", null); });
			DeleteDatabase();
		}

		[Test]
		public void InsertNewRecord()
		{
			var db = CreateTestDatabase();
			CreateTestTable(db);

			var sql = _insertSql;
			var record = new object[] 
			{
				"@Id", 10, 
				"@Value", "Some value"
			};
			var inserted = db.Insert(sql, record);
			Assert.AreEqual(1, inserted);

			DeleteDatabase();
		}

		[Test]
		public void InsertDuplicate()
		{
			var db = CreateTestDatabase();
			CreateTestTable(db);

			var sql = _insertSql;
			var record1 = new object[] 
			{
				"@Id", 10, 
				"@Value", "Some value"
			};
			var inserted = db.Insert(sql, record1);
			Assert.AreEqual(1, inserted);

			var record2 = new object[] 
			{
				"@Id", 10, 
				"@Value", "Some other value"
			};
			Assert.Throws<System.Data.SQLite.SQLiteException>(delegate { db.Insert(sql, record2); });

			DeleteDatabase();
		}

		[Test]
		public void UpdateNoRecord()
		{
			var db = CreateTestDatabase();
			CreateTestTable(db);

			var sql = _updateSql;
			var record = new object[] 
			{
				"@Id", 10, 
				"@Value", "Some value"
			};
			var updated = db.Update(sql, record);
			Assert.AreEqual(0, updated);

			DeleteDatabase();
		}

		[Test]
		public void UpdateRecord()
		{
			var db = CreateTestDatabase();
			CreateTestTable(db);

			var sql_insert = _insertSql;
			var record1 = new object[] 
			{
				"@Id", 10, 
				"@Value", "Some value"
			};
			var inserted = db.Insert(sql_insert, record1);
			Assert.AreEqual(1, inserted);

			var sql_update = _updateSql;
			var record2 = new object[] 
			{
				"@Id", 10, 
				"@Value", "Some other value"
			};
			var updated = db.Update(sql_update, record2);
			Assert.AreEqual(1, updated);

			DeleteDatabase();
		}

		[Test]
		public void UpdateOrInsertNewRecord()
		{
			var db = CreateTestDatabase();
			CreateTestTable(db);

			var sql_insert = _insertSql;
			var sql_update = _updateSql;
			var record = new object[] 
			{
				"@Id", 10, 
				"@Value", "Some value"
			};
			var inserted = db.UpdateOrInsert(sql_update, sql_insert, record);
			Assert.AreEqual(1, inserted);

			DeleteDatabase();
		}

		[Test]
		public void UpdateOrInsertExistingRecord()
		{
			var db = CreateTestDatabase();
			CreateTestTable(db);

			var sql_insert = _insertSql;
			var sql_update = _updateSql;
			var record1 = new object[] 
			{
				"@Id", 10, 
				"@Value", "Some value"
			};
			var record2 = new object[] 
			{
				"@Id", 10, 
				"@Value", "Some other value"
			};
			var inserted = db.Insert(sql_insert, record1);
			Assert.AreEqual(1, inserted);

			var updated = db.UpdateOrInsert(sql_update, sql_insert, record2);
			Assert.AreEqual(1, updated);

			DeleteDatabase();
		}
	}
}
