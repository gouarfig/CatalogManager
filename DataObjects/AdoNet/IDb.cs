using System;
using System.Collections.Generic;
using System.Data;

namespace DataObjects.AdoNet
{
	public interface IDb
	{
		/// <summary>
		/// Fast read and instantiate (i.e. make) a collection of objects
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sql"></param>
		/// <param name="make"></param>
		/// <param name="parms"></param>
		/// <returns></returns>
		IEnumerable<T> Read<T>(string sql, Func<IDataReader, T> make, params object[] parms);

		int CreateStructure(string sql);
		object Scalar(string sql, params object[] parms);
		int Insert(string sql, params object[] parms);
		int Update(string sql, params object[] parms);
		int Delete(string sql, params object[] parms);
		int UpdateOrInsert(string sqlUpdate, string sqlInsert, params object[] parms);
	}
}