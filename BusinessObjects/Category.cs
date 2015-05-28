using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
	public sealed class Category
	{
		public int Id { get; set; }
		public Guid Guid { get; set; }
		public string Name { get; set; }
		public DateTime Created { get; set; }

		public bool IncludeInSearch { get; set; }
	}
}
