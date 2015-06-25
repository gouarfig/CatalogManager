using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BusinessObjects.Tests
{
	[TestFixture]
    public class GenericTests
    {
		private void DoSomethingInALoop(int i)
		{
		}

		[Test]
		public void NormalLoop()
		{
			for (int i = 0; i < 1000; i++)
			{
				DoSomethingInALoop(i);
			}
		}

		[Test]
		public void ImprovedLoop()
		{
			Parallel.For(0, 1000, i => DoSomethingInALoop(i));
		}

		//TODO do some ReadAsync and await
    }
}
