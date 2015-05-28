using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataObjects
{
	/// <summary>
	/// Abstract factory interface.
	/// Creates data access objects.
	/// </summary>
	public interface IDaoFactory
	{
		IConfigurationDao ConfigurationDao { get; }
		ICategoryDao CategoryDao { get; }
	}
}
