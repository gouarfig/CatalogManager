using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
	public class Volume
	{
		public int Id { get; set; }
		public Guid Guid { get; set; }
		public string Name { get; set; }
		public VolumeType Type { get; set; }

		public string VolumeId { get; set; }
		public DateTime Created { get; set; }
		public DateTime Cataloged { get; set; }
		public long TotalSize { get; set; }
		public long SpaceFree { get; set; }
		public long RegularFiles { get; set; }
		public long HiddenFiles { get; set; }
		public string Path { get; set; }
		public string ComputerName { get; set; }

		public bool IncludeInSearch { get; set; }

		public List<Category> Categories { get; set; }
		public List<FileCatalog> Files { get; set; }
	}
}
