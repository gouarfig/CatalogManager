using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.InMemory
{
	public sealed class DaoFactory : IDaoFactory
	{
		public IConfigurationDao ConfigurationDao
		{
			get { return new ConfigurationDao(); }
		}

		public ICategoryDao CategoryDao
		{
			get { return new CategoryDao(); }
		}

		public IVolumeDao VolumeDao
		{
			get { return new VolumeDao(); }
		}
	}
}
