using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.BusinessRules
{
	public abstract class BusinessRuleAttribute : Attribute
	{
		public abstract bool Validate(Object entity, PropertyInfo property);
	}
}
