using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FlueFlame.AspNetCore.Helpers;

internal static class WaitHelper
{
	public static bool WaitUntil(Func<bool> condition, TimeSpan? timeout = null,
		TimeSpan? frequency = null, bool throwOnTimeout = true)
	{
		frequency ??= TimeSpan.FromMilliseconds(100);
		timeout ??= TimeSpan.FromSeconds(5);
		
		bool hasBeenExecuted = false;
		bool hasTimedOut = false;
  
		var stopwatch = new Stopwatch();
		stopwatch.Start();
  
		while (!hasBeenExecuted && !hasTimedOut)
		{
			if (stopwatch.ElapsedMilliseconds > timeout.Value.TotalMilliseconds) 
			{
				if (throwOnTimeout)
					throw new InvalidOperationException();
				hasTimedOut = true;
			}

			hasBeenExecuted = condition();

			Task.Delay(frequency.Value).Wait();
		}

		return true;
	}
}