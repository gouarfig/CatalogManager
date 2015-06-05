using System;
using System.Data.Common;

namespace DataObjects.AdoNet
{
	public static class DbExtentions
	{
		// adds parameters to a command object

		public static void AddParameters(this DbCommand command, object[] parms)
		{
			if (parms != null && parms.Length > 0)
			{

				// ** Iterator pattern

				// NOTE: processes a name/value pair at each iteration

				for (int i = 0; i < parms.Length; i += 2)
				{
					string name = parms[i].ToString();

					// no empty strings to the database

					if (parms[i + 1] is string && (string)parms[i + 1] == "")
						parms[i + 1] = null;

					// if null, set to DbNull

					object value = parms[i + 1] ?? DBNull.Value;

					// ** Factory pattern

					var dbParameter = command.CreateParameter();
					dbParameter.ParameterName = name;
					dbParameter.Value = value;

					command.Parameters.Add(dbParameter);
				}
			}
		}
	}
}