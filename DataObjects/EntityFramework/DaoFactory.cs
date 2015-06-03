namespace DataObjects.EntityFramework
{
	/// <summary>
	/// Data Access Object Factory
	/// </summary>
	public class DaoFactory : IDaoFactory
	{
		public IConfigurationDao ConfigurationDao
		{
			get { throw new System.NotImplementedException(); }
		}

		public ICategoryDao CategoryDao
		{
			get { return new CategoryDao(); }
		}

		public IVolumeDao VolumeDao
		{
			get { throw new System.NotImplementedException(); }
		}
	}
}
