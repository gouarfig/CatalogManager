using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.BusinessRules
{
	public class IdentifierAttribute : BusinessRuleAttribute
	{
		private const string Message = "{0} {1} is a required identifier.";

		public override bool Validate(Object entity, PropertyInfo property)
		{
			bool valid = true;
			try
			{
				var value = property.GetValue(entity);
				valid = (value != null);
				var integerValue = 0L;
				valid = long.TryParse(value.ToString(), out integerValue);
				valid = (integerValue > 0);
			}
			catch (Exception exception)
			{
				throw new BusinessObjectValidationException(String.Format(Message, entity.GetType().Name, property.Name), exception);
			}
			if (!valid)
			{
				throw new BusinessObjectValidationException(String.Format(Message, entity.GetType().Name, property.Name));
			}
			return valid;
		}
	}
}
