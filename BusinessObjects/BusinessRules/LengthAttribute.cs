using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.BusinessRules
{
	public class LengthAttribute : BusinessRuleAttribute
	{
		private const string Message = "{0} {1} must be between {2} and {3} characters long.";
		private long _min = 0L;
		private long _max = 0L;

		public LengthAttribute(long min, long max)
		{
			_min = min;
			_max = max;
		}

		public override bool Validate(Object entity, PropertyInfo property)
		{
			bool valid = true;
			try
			{
				var value = property.GetValue(entity);
				valid = (value != null);
				var length = value.ToString().Length;
				valid = (_min <= length && length <= _max);
			}
			catch (Exception exception)
			{
				throw new BusinessObjectValidationException(String.Format(Message, entity.GetType().Name, property.Name, _min, _max), exception);
			}
			if (!valid)
			{
				throw new BusinessObjectValidationException(String.Format(Message, entity.GetType().Name, property.Name, _min, _max));
			}
			return valid;
		}
	}
}
