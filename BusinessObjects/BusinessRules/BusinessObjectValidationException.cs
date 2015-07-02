using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.BusinessRules
{
	public class BusinessObjectValidationException : ApplicationException
	{
		public BusinessObjectValidationException() : base()
		{
		}

		public BusinessObjectValidationException(string message) : base(message)
		{
		}

		public BusinessObjectValidationException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
