using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
	internal class Day9
	{
		//struct History
		//{
		//	public List<int> PrimarySequence { get; set; }
		//	public Dictionary<int, List<int>> BaseSequences { get; set; }
		//}
		internal static long Solve()
		{
			var lines = Shared.GetFileLines("2023/day9-input.txt");

			var sum = 0;
			foreach (var line in lines)
			{
				var primarySequence = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
				var baseSequences = new List<List<int>>();
				bool nonZero = true;
				var currSequence = primarySequence;
				while (nonZero)
				{
					var baseSequence = new List<int>();
					for (int i = 0; i < currSequence.Count - 1; i++)
					{
						var diff = currSequence[i + 1] - currSequence[i];
						baseSequence.Add(diff);
					}
					nonZero = baseSequence.Any(x => x != 0);
					baseSequences.Add(baseSequence);
					currSequence = baseSequence;
				}
				// find extrapolated num (next)
				var extrapolated = 0;
				for (int i = baseSequences.Count - 1; i > 0; i--)
				{
					extrapolated += baseSequences[i - 1][baseSequences[i - 1].Count - 1];
				}
				var finalExtrapolated = extrapolated + primarySequence[primarySequence.Count - 1];
				sum+= finalExtrapolated;
			}
			return sum;
		}

		internal static long SolvePart2()
		{
			var lines = Shared.GetFileLines("2023/day9-input.txt");

			var sum = 0;
			foreach (var line in lines)
			{
				var primarySequence = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToList();
				var baseSequences = new List<List<int>>();
				bool nonZero = true;
				var currSequence = primarySequence;
				while (nonZero)
				{
					var baseSequence = new List<int>();
					for (int i = 0; i < currSequence.Count - 1; i++)
					{
						var diff = currSequence[i + 1] - currSequence[i];
						baseSequence.Add(diff);
					}
					nonZero = baseSequence.Any(x => x != 0);
					baseSequences.Add(baseSequence);
					currSequence = baseSequence;
				}
				// find extrapolated num (prev)
				var extrapolated = 0;
				for (int i = baseSequences.Count - 1; i > 0; i--)
				{
					extrapolated = baseSequences[i - 1][0] - extrapolated;
				}
				var finalExtrapolated = primarySequence[0] - extrapolated;
				sum += finalExtrapolated;
			}
			return sum;
		}
	}
}
