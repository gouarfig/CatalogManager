using DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionService
{
    public class Service : IService
    {
		static readonly string provider = ConfigurationManager.AppSettings.Get("DataProvider");
		static readonly IDaoFactory factory = DaoFactories.GetFactory(provider);
	    private static readonly IConfigurationDao configurationDao = factory.ConfigurationDao;
	    private static readonly ICategoryDao categoryDao = factory.CategoryDao;
	    private static readonly IVolumeDao volumeDao = factory.VolumeDao;

		public bool BuildDatabase()
		{
			throw new NotImplementedException();
		}
	}
}
