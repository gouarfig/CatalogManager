using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.AdoNet
{
	/// <summary>
	/// Data Access Object Factory
	/// </summary>
	public sealed class DaoFactory : IDaoFactory
	{
		public IConfigurationDao ConfigurationDao
		{
			get { return new ConfigurationDao(new Db()); }
		}

		public ICategoryDao CategoryDao
		{
			get { return new CategoryDao(new Db()); }
		}
	}
}
