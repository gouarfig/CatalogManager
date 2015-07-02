using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BusinessObjects.Tests
{
	[TestFixture]
	[Category("travisci")]
	public class GenericTests
	{
		private const int LoopCount = 60;

		private void LoopSomething(int i)
		{
			Thread.Sleep(1000);
		}

		private async Task LoopSomethingAsync(int i)
		{
			Debug.Print("LoopSomethingAsync({0})", i);
			await Task.Delay(1000);
		}

		[Test]
		public void NormalLoop()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			for (int i = 0; i < LoopCount; i++)
			{
				LoopSomething(i);
			}
			stopwatch.Stop();
			Debug.Print("NormalLoop: Time elapsed = {0}ms", stopwatch.ElapsedMilliseconds);
			Assert.That(stopwatch.ElapsedMilliseconds, Is.InRange(50000, 70000));
		}

		[Test]
		public void ParallelLoop()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			Parallel.For(0, LoopCount, i => LoopSomething(i));
			stopwatch.Stop();
			Debug.Print("ImprovedLoop: Time elapsed = {0}ms", stopwatch.ElapsedMilliseconds);
			Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(60000));
		}

		//TODO do some ReadAsync and await
    }
}
