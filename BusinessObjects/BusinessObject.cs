using System.Reflection;
using BusinessObjects.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
	public abstract class BusinessObject
	{
		private bool ValidateProperty(Object entity, PropertyInfo property)
		{
			var valid = true;
			foreach (BusinessRuleAttribute attribute in property.GetCustomAttributes(typeof(BusinessRuleAttribute), true))
			{
				if (!attribute.Validate(entity, property))
				{
					valid = false;
				}
			}
			return valid;
		}

		public bool Validate()
		{
			bool valid = true;
			PropertyInfo[] properties = this.GetType().GetProperties();
			foreach (var property in properties)
			{
				if (!ValidateProperty(this, property))
				{
					valid = false;
				}
			}
			return valid;
		}
	}
}
