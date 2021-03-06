﻿using System;
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
		private IDb _db = null;

		public DaoFactory()
		{
			_db = new Db();
		}

		public IConfigurationDao ConfigurationDao
		{
			get { return new ConfigurationDao(_db); }
		}

		public ICategoryDao CategoryDao
		{
			get { return new CategoryDao(_db); }
		}

		public IVolumeDao VolumeDao
		{
			get { return new VolumeDao(_db); }
		}
	}
}
