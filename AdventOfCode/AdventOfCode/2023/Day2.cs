using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
	// https://adventofcode.com/2023/day/2
	internal class Day2
	{
		enum Colour
		{
			Red = 0, Green = 1, Blue = 2, Empty = -1
		}
		private static Dictionary<Colour, int> Expected = new Dictionary<Colour, int>()
		{
			{ Colour.Red, 12 },
			{ Colour.Green, 13 },
			{ Colour.Blue, 14 },
		};

		public static int SolvePart2()
		{
			// todo: DRY, won't bother for now
			var fileLocation = Shared.GetFileLocation("2023/day2-input.txt");
			var lines = File.ReadAllLines(fileLocation);

			var regex = new Regex(@"Game \d+");

			int totalNum = 0;
			foreach (var game in lines)
			{
				var m = regex.Match(game);
				var gameNo = int.Parse(m.Value.Remove(0, "Game".Length));
				var tmp = game.Remove(0, m.Value.Length + 1);

				var gameSets = tmp.Split(";");
				int powerOfSet = 0;

				powerOfSet = FindPower(gameSets);

				totalNum += powerOfSet;
			}
			return totalNum;
		}

		private static int FindPower(string[] gameSets)
		{
			var maxValsByColour = new Dictionary<Colour, int>();
			foreach (var set in gameSets)
			{
				var cubes = set.Split(',');
				var regex1 = new Regex(@"\d+");
				var regex2 = new Regex(@"(red|green|blue)");
				foreach (var cube in cubes)
				{
					var count = int.Parse(regex1.Match(cube).Groups[0].Value);
					Colour colour = Colour.Empty;
					switch (regex2.Match(cube).Groups[0].Value)
					{
						case "red":
							colour = Colour.Red;
							break;
						case "green":
							colour = Colour.Green;
							break;
						case "blue":
							colour = Colour.Blue;
							break;
						default:
							break;
					}
					if (maxValsByColour.ContainsKey(colour))
					{
						if (maxValsByColour[colour] < count) { maxValsByColour[colour] = count; }
					}
					else { maxValsByColour.Add(colour, count); }
				}
			}

			return maxValsByColour.Values.Aggregate((a, b) => a * b);
		}

		public static int Solve()
		{
			var fileLocation = Shared.GetFileLocation("2023/day2-input.txt");
			var lines = File.ReadAllLines(fileLocation);

			var regex = new Regex(@"Game \d+");

			int totalNum = 0;
			foreach (var game in lines)
			{
				var m = regex.Match(game);
				var gameNo = int.Parse(m.Value.Remove(0, "Game".Length));
				var tmp = game.Remove(0, m.Value.Length + 1);

				var gameSets = tmp.Split(";");
				bool allSetsPossible = true;
				foreach (var set in gameSets)
				{
					var actuals = ParseSet(set);
					if (!AreActualsPossible(actuals))
					{
						allSetsPossible = false;
						break;
					}
				}
				if (allSetsPossible)
				{
					totalNum += gameNo;
				}
			}
			return totalNum;
		}

		private static bool AreActualsPossible(Dictionary<Colour, int> actuals)
		{
			foreach (var actual in actuals)
			{
				if (Expected.TryGetValue(actual.Key, out int expectedValue))
				{
					if (expectedValue < actual.Value)
					{
						return false;
					}
				}
			}
			return true;
		}

		private static Dictionary<Colour, int> ParseSet(string set)
		{
			var vals = new Dictionary<Colour, int>();
			var cubes = set.Split(',');
			var regex1 = new Regex(@"\d+");
			var regex2 = new Regex(@"(red|green|blue)");
			foreach (var item in cubes)
			{
				var count = regex1.Match(item).Groups[0].Value;
				Colour colour = Colour.Empty;
				switch (regex2.Match(item).Groups[0].Value)
				{
					case "red":
						colour = Colour.Red;
						break;
					case "green":
						colour = Colour.Green;
						break;
					case "blue":
						colour = Colour.Blue;
						break;
					default:
						break;
				}

				vals.Add(colour, int.Parse(count));
			}
			return vals;
		}
	}
}
