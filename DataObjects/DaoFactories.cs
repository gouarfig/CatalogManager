using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
	/// <summary>
	/// Factory of factories. This class is a factory class that creates data-base specific factories which in turn create data acces objects.
	/// </summary>
    public class DaoFactories
    {
		/// <summary>
		/// Gets a provider specific (i.e. database specific) factory
		/// </summary>
		/// <param name="dataProvider"></param>
		/// <returns></returns>
		public static IDaoFactory GetFactory(string dataProvider)
		{
			switch (dataProvider.ToLower())
			{
				case "ado.net":
					return new AdoNet.DaoFactory();
				case "entityframework":
					return new EntityFramework.DaoFactory();
				case "inmemory":
					return new InMemory.DaoFactory();

				default:
					return new AdoNet.DaoFactory();
			}
		}
    }
}
