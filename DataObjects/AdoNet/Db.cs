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
		static readonly DbProviderFactory factory /* = DbProviderFactories.GetFactory("System.Data.SQLite.EF6") */;
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

		public async Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<IDataReader> readers, Func<IDataReader, Task<T>> make)
		{
			return await Task.WhenAll(from reader in readers select make(reader));
		}

		/// <summary>
		/// Returns a scalar object
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
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

		public async Task<object> ScalarAsync(string sql, params object[] parms)
		{

			using (var connection = await CreateConnectionAsync())
			{
				using (var command = CreateCommand(sql, connection, parms))
				{
					return await command.ExecuteScalarAsync();
				}
			}

		}

		/// <summary>
		/// Inserts a new record
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
		/// Inserts a new record asynchronously
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		public async Task<int> InsertAsync(string sql, params object[] parms)
		{

			using (var connection = await CreateConnectionAsync())
			{
				using (var command = CreateCommand(sql, connection, parms))
				{
					return await command.ExecuteNonQueryAsync();
				}
			}

		}

		/// <summary>
		/// Updates an existing record
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

		/// <summary>
		/// Updates an existing record asynchronously
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		public async Task<int> UpdateAsync(string sql, params object[] parms)
		{
			using (var connection = await CreateConnectionAsync())
			{
				using (var command = CreateCommand(sql, connection, parms))
				{
					return await command.ExecuteNonQueryAsync();
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

		public async Task<int> UpdateOrInsertAsync(string sqlUpdate, string sqlInsert, params object[] parms)
		{
			int updatedOrInserted = 0;
			using (var connection = await CreateConnectionAsync())
			{
				using (var updateCommand = CreateCommand(sqlUpdate, connection, parms))
				{
					var updated = await updateCommand.ExecuteNonQueryAsync();
					if (updated > 0)
					{
						updatedOrInserted = updated;
					}
					else
					{
						using (var insertCommand = CreateCommand(sqlInsert, connection, parms))
						{
							updatedOrInserted = await insertCommand.ExecuteNonQueryAsync();
						}
					}
				}
			}
			return updatedOrInserted;
		}

		/// <summary>
		/// Deletes a record
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		public int Delete(string sql, params object[] parms)
		{
			return Update(sql, parms);
		}

		/// <summary>
		/// Deletes a record asynchronously
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		public async Task<int> DeleteAsync(string sql, params object[] parms)
		{
			return await UpdateAsync(sql, parms);
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

		public async Task<int> CreateStructureAsync(string sql)
		{
			using (var connection = await CreateConnectionAsync())
			{
				using (var command = CreateCommand(sql, connection, null))
				{
					return await command.ExecuteNonQueryAsync();
				}
			}
		}

		/// <summary>
		/// Creates a connection object
		/// </summary>
		/// <returns></returns>
		DbConnection CreateConnection()
		{
			var connection = factory.CreateConnection();
			connection.ConnectionString = _connectionString;
			connection.Open();
			return connection;
		}

		/// <summary>
		/// Creates an async connection object
		/// </summary>
		/// <returns></returns>
		async Task<DbConnection> CreateConnectionAsync()
		{
			var connection = factory.CreateConnection();
			connection.ConnectionString = _connectionString;
			await connection.OpenAsync();
			return connection;
		}

		/// <summary>
		/// Creates a command object
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="conn"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		DbCommand CreateCommand(string sql, DbConnection conn, params object[] parms)
		{
			var command = factory.CreateCommand();
			command.Connection = conn;
			command.CommandText = sql;
			command.AddParameters(parms);
			return command;
		}

		/// <summary>
		/// Creates an adapter object
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		DbDataAdapter CreateAdapter(DbCommand command)
		{
			var adapter = factory.CreateDataAdapter();
			adapter.SelectCommand = command;
			return adapter;
		}
	}
}
