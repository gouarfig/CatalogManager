using BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
	/// <summary>
	/// Defines methods to access categories.
	/// This is a database-independant interface.
	/// </summary>
	public interface ICategoryDao
	{
		int CreateStructure();

		/// <summary>
		/// Gets a list of volume categories
		/// </summary>
		/// <returns></returns>
		List<Category> GetCategories();

		Category GetCategoryById(int id);
		Category GetCategoryByGuid(Guid guid);
		Category GetCategoryByName(string name);
		bool InsertCategory(Category categoy);
		bool UpdateCategory(Category category);
		bool DeleteCategory(int categoryId);
	}
}
