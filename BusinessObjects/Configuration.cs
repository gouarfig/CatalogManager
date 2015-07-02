﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
	public sealed class Configuration
	{
		public string Name { get; private set; }
		public string Value { get; private set; }

		public Configuration(string name, string value)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Name can't be null or empty", "name");
			}
			Name = name;
			Value = value;
		}
	}
}
