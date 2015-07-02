using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.BusinessRules;

namespace BusinessObjects
{
	public sealed class Category : BusinessObject
	{
		[Identifier]
		public int Id { get; set; }
		public Guid Guid { get; set; }
		[Required]
		public string Name { get; set; }
		public DateTime Created { get; set; }

		public bool IncludeInSearch { get; set; }
	}
}
