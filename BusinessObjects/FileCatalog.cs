using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObjects
{
	public class FileCatalog
	{
		public int Id { get; set; }
		public Guid Guid { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }
		public FileType Type { get; set; }
		public string Hash { get; set; }
		public List<FileProperty> Properties { get; set; }

		public Volume Volume { get; set; }
	}
}
