using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObjects
{
	public sealed class FileCatalog
	{
		public int Id { get; set; }
		public Guid Guid { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }
		public FileType Type { get; set; }
		public string Hash { get; set; }
		public DateTime FirstSeen { get; set; }
		public DateTime LastSeen { get; set; }
		public bool Deleted { get; set; }
		public List<FileProperty> Properties { get; set; }

		public Volume Volume { get; set; }
	}
}
