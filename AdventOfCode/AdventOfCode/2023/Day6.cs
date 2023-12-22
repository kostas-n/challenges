using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
	internal class Day6
	{
		internal static long Solve()
		{
			var lines = Shared.GetFileLines("2023/day6-input.txt");

			var times = lines[0].Remove(0, "Time:".Length).Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
			var distances = lines[1].Remove(0, "Distance:".Length).Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();

			long numOfDifferentWaysToWin = 0;
			for (int i = 0; i < times.Length; i++)
			{
				long diffWaysToWin = CalcQuadratic((long)times[i], (long)distances[i]);

				numOfDifferentWaysToWin = numOfDifferentWaysToWin == 0 ? diffWaysToWin : numOfDifferentWaysToWin *= diffWaysToWin;
			}
			return numOfDifferentWaysToWin;
		}

		private static long CalcQuadratic(long time, long recordDistance)
		{
			// (time - t1) * (t1) = distance => time * t1 - t1*t1 = distance; => 
			// t1^2 -time*t1 + distance = 0;
			// x = -b +-sqrt(b^2 - 4ac)/2a
			// D = time^2 - 4*distance
			// t = -1 - sqrt(D)/2a

			var diakrinousa = Math.Pow(time, 2) - 4 * recordDistance;
			double t1 = 0.0, t2 = 0.0;
			if (diakrinousa >= 0)
			{
				t1 = (-time - Math.Sqrt(diakrinousa)) / -2;
				t2 = (-time + Math.Sqrt(diakrinousa)) / -2;
			}
			var min = Math.Min(t1, t2);
			var max = Math.Max(t1, t2);
			int start = min % 1 == 0 ? (int)min + 1 : (int)Math.Ceiling(min);
			int end = max % 1 == 0 ? (int)max - 1 : (int)Math.Floor(max);
			int diffWaysToWin = 0;
			if (end >= start)
			{
				diffWaysToWin = end - start + 1;
			}

			return diffWaysToWin;
		}

		internal static long SolvePart2()
		{
			var lines = Shared.GetFileLines("2023/day6-input.txt");

			var time = long.Parse(lines[0].Remove(0, "Time:".Length).Trim().Replace(" ", ""));
			var distance = long.Parse(lines[1].Remove(0, "Distance:".Length).Trim().Replace(" ", ""));

			long diffWaysToWin = CalcQuadratic(time, distance);

			return diffWaysToWin;
		}
	}
}
