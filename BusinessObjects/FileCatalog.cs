using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObjects.BusinessRules;

namespace BusinessObjects
{
	public sealed class FileCatalog : BusinessObject
	{
		[Identifier]
		public int Id { get; set; }
		public Guid Guid { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Path { get; set; }
		public FileType Type { get; set; }
		public string Hash { get; set; }
		public DateTime FirstSeen { get; set; }
		public DateTime LastSeen { get; set; }
		public bool Deleted { get; set; }
		public List<FileProperty> Properties { get; set; }

		[Required]
		public Volume Volume { get; set; }
	}
}
