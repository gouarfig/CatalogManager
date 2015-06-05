using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.InMemory
{
	public class CategoryDao : ICategoryDao
	{
		public int CreateStructure()
		{
			return 0;
		}

		public List<BusinessObjects.Category> GetCategories()
		{
			throw new NotImplementedException();
		}

		public BusinessObjects.Category GetCategoryById(int id)
		{
			throw new NotImplementedException();
		}

		public BusinessObjects.Category GetCategoryByGuid(Guid guid)
		{
			throw new NotImplementedException();
		}

		public BusinessObjects.Category GetCategoryByName(string name)
		{
			throw new NotImplementedException();
		}

		public bool InsertCategory(BusinessObjects.Category categoy)
		{
			throw new NotImplementedException();
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
