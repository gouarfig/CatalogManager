using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionService
{
	public interface ICatalogService
	{
		bool InitializeDatabase();
		int GetDatabaseVersion();
		string DisplayBytesToHuman(string size);
		string DisplayBytesToHuman(long size);
	}
}
