using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BusinessObjects;

namespace DataObjects.AdoNet
{
	/// <summary>
	/// Data Access Object for Category
	/// </summary>
	public sealed class CategoryDao : ICategoryDao
	{
		private static readonly string _tableName = "Category";
		private static readonly int _nameFieldMaxLength = 200;
		private static readonly string _createStructure = String.Format(
			"CREATE TABLE {0} (" +
			"Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
			"Guid STRING (36), " +
			"Name STRING ({1}), " +
			"Created DATETIME, " +
			"IncludeInSearch BOOLEAN);",
			_tableName,
			_nameFieldMaxLength);

		private static readonly string _selectAll = String.Format("SELECT Id, Guid, Name, Created, IncludeInSearch FROM {0}", _tableName);

		private readonly IDb _db;

		public CategoryDao()
		{
			_db = new Db();
		}

		public CategoryDao(IDb db)
		{
			_db = db;
		}

		public int CreateStructure()
		{
			return _db.CreateStructure(_createStructure);
		}

		public List<Category> GetCategories()
		{
			return _db.Read(_selectAll, Make).ToList();
		}


		public Category GetCategoryById(int id)
		{
			if (id <= 0) throw new ArgumentException("id");

			var sql = String.Format("{0} WHERE Id = @Id", _selectAll);
			object[] parms = { "@Id", id };
			return _db.Read(sql, Make, parms).FirstOrDefault();
		}

		public Category GetCategoryByGuid(Guid guid)
		{
			if (guid == Guid.Empty) throw new ArgumentException("guid");

			var sql = String.Format("{0} WHERE Guid = @Guid", _selectAll);
			object[] parms = { "@Guid", guid };
			return _db.Read(sql, Make, parms).FirstOrDefault();
		}

		public Category GetCategoryByName(string name)
		{
			if (String.IsNullOrEmpty(name)) throw new ArgumentException("name");
			if (name.Length > _nameFieldMaxLength) throw new ArgumentOutOfRangeException(String.Format("name can't be more than {0} characters length", _nameFieldMaxLength), "name");

			var sql = String.Format("{0} WHERE Name = @Name", _selectAll);
			object[] parms = { "@Name", name };
			return _db.Read(sql, Make, parms).FirstOrDefault();
		}

		public bool InsertCategory(Category category)
		{
			if (category == null) throw new ArgumentNullException("category");
			if (category.Guid == Guid.Empty) throw new ArgumentException("category.Guid can't be empty", "category");
			if (String.IsNullOrEmpty(category.Name)) throw new ArgumentException("category.Name can't be empty", "category");
			if (category.Name.Length > _nameFieldMaxLength) throw new ArgumentOutOfRangeException(String.Format("category.name can't be more than {0} characters length", _nameFieldMaxLength), "category");

			var sql = String.Format("INSERT INTO {0} (Guid, Name, Created, IncludeInSearch) VALUES (@Guid, @Name, @Created, @IncludeInSearch)", _tableName);
			return (_db.Insert(sql, Take(category)) == 1);
		}

		public bool UpdateCategory(Category category)
		{
			if (category.Id <= 0) throw new ArgumentOutOfRangeException("category", "category.Id needs to be > 0");
			if (category == null) throw new ArgumentNullException("category");
			if (category.Guid == Guid.Empty) throw new ArgumentException("category.Guid can't be empty", "category");
			if (String.IsNullOrEmpty(category.Name)) throw new ArgumentException("category.Name can't be empty", "category");
			if (category.Name.Length > _nameFieldMaxLength) throw new ArgumentOutOfRangeException(String.Format("category.name can't be more than {0} characters length", _nameFieldMaxLength), "category");

			var sql = string.Format("UPDATE {0} SET Guid=@Guid, Name=@Name, Created=@Created, IncludeInSearch=@IncludeInSearch WHERE Id = @Id", _tableName);
			return (_db.Update(sql, Take(category)) == 1);
		}

		public bool DeleteCategory(int categoryId)
		{
			if (categoryId <= 0) throw new ArgumentOutOfRangeException("categoryId", "categoryId needs to be > 0");

			var sql = string.Format("DELETE FROM {0} WHERE Id = @Id", _tableName);
			object[] parms = { "@Id", categoryId };
			return (_db.Delete(sql, parms) == 1);
		}

		private static readonly Func<IDataReader, Category> Make = reader =>
			new Category
			{
				Id = reader["Id"].AsInt(),
				Guid = reader["Guid"].AsGuid(),
				Name = reader["Name"].AsString(),
				Created = reader["Created"].AsDateTime(),
				IncludeInSearch = reader["IncludeInSearch"].AsBool(),
			};

		private object[] Take(Category category)
		{
			return new object[]
			{
				"@Id", category.Id,
				"@Guid", category.Guid,
				"@Name", category.Name,
				"@Created", category.Created,
				"@IncludeInSearch", category.IncludeInSearch,
			};
		}

		/// <summary>
		/// This function is only used for unit test of private methods
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public object[] TestMakeTake(IDataReader reader)
		{
			var temp = Make(reader);
			return Take(temp);
		}
	}
}
