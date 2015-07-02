using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.BusinessRules
{
	public class RequiredAttribute : BusinessRuleAttribute
	{
		public override bool Validate(Object entity, PropertyInfo property)
		{
			return (property.GetValue(entity) != null);
		}
	}
}
