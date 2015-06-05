using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.AdoNet
{
	/// <summary>
	/// ADO.NET data access class. 
	/// </summary>
	public sealed class Db : IDb
	{
		static readonly DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SQLite.EF6");
		private string _connectionName = "CatalogMaster.SQLite";
		private string _connectionString;

		public Db() : this(null)
		{
		}

		public Db(string conn)
		{
			if (!String.IsNullOrEmpty(conn))
			{
				_connectionName = conn;
			}
			_connectionString = ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString;
		}


		/// <summary>
		/// Fast read and instantiate (i.e. make) a collection of objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sql"></param>
		/// <param name="make"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		public IEnumerable<T> Read<T>(string sql, Func<IDataReader, T> make, params object[] parms)
		{
			using (var connection = CreateConnection())
			{
				using (var command = CreateCommand(sql, connection, parms))
				{
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							yield return make(reader);
						}
					}
				}
			}
		}

		// return a scalar object

		public object Scalar(string sql, params object[] parms)
		{

			using (var connection = CreateConnection())
			{
				using (var command = CreateCommand(sql, connection, parms))
				{
					return command.ExecuteScalar();
				}
			}

		}

		/// <summary>
		/// Insert a new record
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		public int Insert(string sql, params object[] parms)
		{

			using (var connection = CreateConnection())
			{
				using (var command = CreateCommand(sql, connection, parms))
				{
					return command.ExecuteNonQuery();
				}
			}

		}

		/// <summary>
		/// Update an existing record
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		public int Update(string sql, params object[] parms)
		{

			using (var connection = CreateConnection())
			{
				using (var command = CreateCommand(sql, connection, parms))
				{
					return command.ExecuteNonQuery();
				}
			}

		}

		public int UpdateOrInsert(string sqlUpdate, string sqlInsert, params object[] parms)
		{
			int updatedOrInserted = 0;
			using (var connection = CreateConnection())
			{
				using (var updateCommand = CreateCommand(sqlUpdate, connection, parms))
				{
					var updated = updateCommand.ExecuteNonQuery();
					if (updated > 0)
					{
						updatedOrInserted = updated;
					}
					else
					{
						using (var insertCommand = CreateCommand(sqlInsert, connection, parms))
						{
							updatedOrInserted = insertCommand.ExecuteNonQuery();
						}
					}
				}
			}
			return updatedOrInserted;
		}

		/// <summary>
		/// Delete a record
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		public int Delete(string sql, params object[] parms)
		{
			return Update(sql, parms);
		}

		public int CreateStructure(string sql)
		{
			using (var connection = CreateConnection())
			{
				using (var command = CreateCommand(sql, connection, null))
				{
					return command.ExecuteNonQuery();
				}
			}
		}

		// creates a connection object

		DbConnection CreateConnection()
		{
			// ** Factory pattern in action

			var connection = factory.CreateConnection();
			connection.ConnectionString = _connectionString;
			connection.Open();
			return connection;
		}

		// creates a command object

		DbCommand CreateCommand(string sql, DbConnection conn, params object[] parms)
		{
			// ** Factory pattern in action

			var command = factory.CreateCommand();
			command.Connection = conn;
			command.CommandText = sql;
			command.AddParameters(parms);
			return command;
		}

		// creates an adapter object

		DbDataAdapter CreateAdapter(DbCommand command)
		{
			// ** Factory pattern in action

			var adapter = factory.CreateDataAdapter();
			adapter.SelectCommand = command;
			return adapter;
		}
	}
}
