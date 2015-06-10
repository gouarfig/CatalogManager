using System.Data;
using BusinessObjects;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionService
{
    public sealed class CatalogService : ICatalogService
    {
	    private const string _databaseVersion = "_DatabaseVersion";
	    private const int _currentDatabaseVersion = 1;
	    private IConfigurationDao _configurationDao = null;
	    private ICategoryDao _categoryDao = null;
	    private IVolumeDao _volumeDao = null;

	    public CatalogService(string provider = null)
	    {
		    if (String.IsNullOrEmpty(provider))
		    {
				provider = ConfigurationManager.AppSettings.Get("DataProvider");
			    if (provider == null)
			    {
				    provider = "";
			    }
		    }
			IDaoFactory factory = DaoFactories.GetFactory(provider);
		    _configurationDao = factory.ConfigurationDao;
		    _categoryDao = factory.CategoryDao;
		    _volumeDao = factory.VolumeDao;
	    }

		public bool InitializeDatabase()
		{
			_configurationDao.CreateStructure();
			_categoryDao.CreateStructure();
			_volumeDao.CreateStructure();
			_configurationDao.Put(new BusinessObjects.Configuration(_databaseVersion, _currentDatabaseVersion.ToString()));
			return true;
		}

	    public int GetDatabaseVersion()
	    {
		    int version = 0;
		    BusinessObjects.Configuration databaseVersion;
		    try
		    {
			    databaseVersion = _configurationDao.Get(_databaseVersion);
		    }
		    catch
		    {
			    databaseVersion = null;
		    }
		    if (databaseVersion == null)
		    {
				throw new DataException("The database doesn't exist, or is not properly initialized.");
		    }
		    if (!int.TryParse(databaseVersion.Value, out version))
		    {
				throw new DataException("Database version value is not a numeric");
		    }
		    return version;
	    }

	    public string DisplayBytesToHuman(string size)
	    {
		    long output;
		    if (long.TryParse(size, out output))
		    {
			    return DisplayBytesToHuman(output);
		    }
		    else
		    {
			    return "(Not A Number)";
		    }
	    }

	    public string DisplayBytesToHuman(long size)
	    {
		    var thousandPrefixes = new string[]
		    {
			    "", "k", "M", "G", "T", "P", "E", "Z", "Y"
		    };
		    int thousand = 0;
		    decimal number = size;
			while ((number > 1024) && (thousand < thousandPrefixes.Length))
			{
				thousand++;
				number = number/1024;
			}
		    return String.Format("{0:N} {1}", number, thousandPrefixes[thousand]);
	    }
    }
}
