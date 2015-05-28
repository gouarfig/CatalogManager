using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataObjects
{
	public interface IConfigurationDao
	{
		int CreateStructure();
		Configuration Get(string name);
		int Put(Configuration configuration);
	}
}
