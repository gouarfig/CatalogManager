﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.BusinessRules
{
	public class RequiredAttribute : BusinessRuleAttribute
	{
		private const string Message = "{0} {1} is required.";

		public override bool Validate(Object entity, PropertyInfo property)
		{
			bool valid = true;
			try
			{
				valid = (property.GetValue(entity) != null);
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
