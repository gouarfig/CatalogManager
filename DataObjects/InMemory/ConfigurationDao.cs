using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataObjects.InMemory
{
	public sealed class ConfigurationDao : IConfigurationDao
	{
		private List<Configuration> _data;

		public ConfigurationDao()
		{
			_data = new List<Configuration>();
		}

		public int CreateStructure()
		{
			return 0;
		}

		public BusinessObjects.Configuration Get(string name)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentException("name can't be null or empty", "name");
			}
			return _data.Find(c => c.Name == name);
		}

		public int Put(BusinessObjects.Configuration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			if (configuration.Validate())
			{
				var previous = Get(configuration.Name);
				if (previous != null)
				{
					_data.Remove(previous);
				}
				_data.Add(configuration);
				return 1;
			}
			else
			{
				return 0;
			}
		}
	}
}
