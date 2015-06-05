using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BusinessObjects;

namespace DataObjects.AdoNet
{
	public sealed class ConfigurationDao : IConfigurationDao
	{
		private static readonly string _tableName = "Configuration";
		private static readonly int _nameFieldMaxLength = 50;
		private static readonly string _createStructure = String.Format("CREATE TABLE {0} (Name TEXT ({1}) PRIMARY KEY, Value TEXT);", _tableName, _nameFieldMaxLength);

		private readonly IDb _db;

		public ConfigurationDao()
		{
			_db = new Db();
		}

		public ConfigurationDao(IDb db)
		{
			_db = db;
		}

		public int CreateStructure()
		{
			return _db.CreateStructure(_createStructure);
		}

		public Configuration Get(string name)
		{
			if (String.IsNullOrEmpty(name)) throw new ArgumentException("name");
			if (name.Length > _nameFieldMaxLength) throw new ArgumentOutOfRangeException(String.Format("name can't be more than {0} characters length", _nameFieldMaxLength), "name");

			string sql = String.Format(@"SELECT Name, Value FROM {0} WHERE Name = @Name", _tableName);
			object[] parameters = {"@Name", name};
			return _db.Read(sql, Make, parameters).FirstOrDefault();
		}

		public int Put(Configuration configuration)
		{
			if (configuration == null) throw new ArgumentNullException("configuration");
			if (String.IsNullOrEmpty(configuration.Name)) throw new ArgumentException("configuration");
			if (configuration.Name.Length > _nameFieldMaxLength) throw new ArgumentOutOfRangeException(String.Format("configuration.Name can't be more than {0} characters length", _nameFieldMaxLength), "configuration");

			string sqlUpdate = String.Format(@"UPDATE {0} SET Value = @Value WHERE Name = @Name", _tableName);
			string sqlInsert = String.Format(@"INSERT INTO {0} (Name, Value) VALUES (@Name, @Value)", _tableName);
			return _db.UpdateOrInsert(sqlUpdate, sqlInsert, Take(configuration));
		}

		private static readonly Func<IDataReader, Configuration> Make = reader =>
			new Configuration
			(
				name: reader["Name"].AsString(),
				value: reader["Value"].AsString()
			);

		private object[] Take(Configuration configuration)
		{
			return new object[]
			{
				"@Name", configuration.Name,
				"@Value", configuration.Value,
			};
		}

		/// <summary>
		/// This function is only used for unit test of private methods
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public object[] TestMakeTake(IDataReader reader)
		{
			var temp = Make(reader);
			return Take(temp);
		}
	}
}
