using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
	public sealed class FileProperty : BusinessObject
	{
		public FileCatalog File { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
}
