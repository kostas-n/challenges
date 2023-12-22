using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
	internal class Day4
	{
		internal static long Solve()
		{
			var fileLocation = Shared.GetFileLocation("2023/day4-input.txt");
			var lines = File.ReadAllLines(fileLocation);

			var regex = new Regex(@"Card.+\d+:");
			long sum = 0;
			foreach (var line in lines)
			{
				var game = line.Remove(0, regex.Match(line).Value.Length);
				var parts = game.Split('|');

				var winningNums = parts[0].Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x));

				var actualNums = parts[1].Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x));

				long pw = 0;
				foreach (var win in winningNums)
				{
					if (actualNums.Any(x => x == win))
					{
						if (pw == 0)
						{
							pw++;
						}
						else
						{
							pw *= 2;
						}
					};
				}
				sum += pw;
			}
			return sum;
		}

		// more efficient, no recursion
		internal static long SolvePart2_Vers2()
		{
			var fileLocation = Shared.GetFileLocation("2023/day4-input.txt");
			var lines = File.ReadAllLines(fileLocation);

			var copiesByGameNo = new Dictionary<int, int>();
			for (int i = 0; i < lines.Length; i++)
			{
				//init, add original
				copiesByGameNo.TryGetValue(i + 1, out var curr);
				copiesByGameNo[i + 1] = curr + 1;

				var factorOfCopies = copiesByGameNo[i + 1];
				var wonCopies = CalcWinsScratchCard(lines[i]);
				for (int j = 1; j <= wonCopies; j++)
				{
					var gameNo = i + 1 + j;
					copiesByGameNo.TryGetValue(gameNo, out var currVal);
					copiesByGameNo[gameNo] = currVal + factorOfCopies;
				}
			}

			return copiesByGameNo.Values.Aggregate((x, y) => x + y);
		}

		// with recursion..meh
		internal static long SolvePart2()
		{
			var fileLocation = Shared.GetFileLocation("2023/day4-input.txt");
			var lines = File.ReadAllLines(fileLocation);

			for (int i = 0; i < lines.Length; i++)
			{
				ProcessScratchCard(lines, i);
			}
			return _count;
		}
		private static int _count { get; set; } = 0;

		private static void ProcessScratchCard(string[] lines, int index)
		{
			var winsByCard = new Dictionary<int, int>();
			int wins;
			if (!winsByCard.TryGetValue(index, out wins))
			{
				wins = CalcWinsScratchCard(lines[index]);
				winsByCard.Add(index, wins);
			}
			_count++;
			for (int i = 0; i < wins; i++)
			{
				ProcessScratchCard(lines, index + i + 1);
			}
		}

		private static int CalcWinsScratchCard(string line)
		{
			var regex = new Regex(@"Card.+\d+:");
			var gameNum = int.Parse(Regex.Replace(regex.Match(line).Value, @"[^\d]", ""));

			var game = line.Remove(0, regex.Match(line).Value.Length);
			var parts = game.Split('|');

			var winningNums = parts[0].Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x));

			var actualNums = parts[1].Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x));

			return winningNums.Intersect(actualNums).Count();
		}
	}
}
