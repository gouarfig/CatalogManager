﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
	class Program
	{
		static void Main(string[] args)
		{
			var arguments = new Arguments();
			arguments.RunFromArguments(args);
		}
	}
}
