using System;
using System.Collections.Generic;
using BusinessObjects;

namespace DataObjects.EntityFramework
{
	/// <summary>
	/// Data Access Object for Category
	/// </summary>
	public class CategoryDao : ICategoryDao
	{
		private const string Structure = "CREATE TABLE Category (Id INTEGER PRIMARY KEY AUTOINCREMENT, Guid STRING (36), Name STRING, Created DATETIME, IncludeInSearch BOOLEAN);";

		public int CreateStructure()
		{
			throw new NotImplementedException();
		}

		public List<Category> GetCategories()
		{
			throw new NotImplementedException();
		}


		public Category GetCategoryById(int id)
		{
			throw new NotImplementedException();
		}

		public Category GetCategoryByGuid(Guid guid)
		{
			throw new NotImplementedException();
		}

		public Category GetCategoryByName(string name)
		{
			throw new NotImplementedException();
		}

		public bool InsertCategory(Category categoy)
		{
			throw new NotImplementedException();
		}

		public bool UpdateCategory(Category category)
		{
			throw new NotImplementedException();
		}

		public bool DeleteCategory(int categoryId)
		{
			throw new NotImplementedException();
		}
	}
}
