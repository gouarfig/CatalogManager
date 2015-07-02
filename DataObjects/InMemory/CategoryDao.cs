using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataObjects.InMemory
{
	public class CategoryDao : ICategoryDao
	{
		private List<Category> _data;

		public CategoryDao()
		{
			_data = new List<Category>();
		}

		public int CreateStructure()
		{
			return 0;
		}

		public List<BusinessObjects.Category> GetCategories()
		{
			return _data;
		}

		public BusinessObjects.Category GetCategoryById(int id)
		{
			return _data.Find(c => c.Id == id);
		}

		public BusinessObjects.Category GetCategoryByGuid(Guid guid)
		{
			return _data.Find(c => c.Guid == guid);
		}

		public BusinessObjects.Category GetCategoryByName(string name)
		{
			return _data.Find(c => c.Name == name);
		}

		public bool InsertCategory(BusinessObjects.Category category)
		{
			if (_data.Contains(category)) throw new ArgumentException("This category already exists");
			_data.Add(category);
			return true;
		}

		public bool UpdateCategory(BusinessObjects.Category category)
		{
			throw new NotImplementedException();
		}

		public bool DeleteCategory(int categoryId)
		{
			throw new NotImplementedException();
		}
	}
}
