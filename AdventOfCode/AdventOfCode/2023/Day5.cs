using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AdventOfCode._2023.Shared;

namespace AdventOfCode._2023
{
	internal class Day5
	{
		const string SEEDS = "seeds:";

		struct RangeStruct
		{
			public long DestinationRangeStart;
			public long SourceRangeStart;
			public long RangeLength;
		}

		internal static double Solve()
		{
			var fileLocation = Shared.GetFileLocation("2023/day5-input.txt");
			var lines = File.ReadAllLines(fileLocation);

			var seeds = new List<long>();
			string currentMap = "";

			var rangeByMap = new Dictionary<string, List<RangeStruct>>();
			foreach (var line in lines)
			{
				if (line.Trim().Length == 0) { continue; }
				if (line.StartsWith(SEEDS))
				{
					var seedsString = line.Remove(0, SEEDS.Length);
					seeds = seedsString.Split(' ').Where(x => x.Trim() != "").Select(s => long.Parse(s)).ToList();
				}
				if (!char.IsDigit(line[0]))
				{
					currentMap = line.Trim();
					rangeByMap.Add(currentMap, new List<RangeStruct>());
					continue;
				}
				long[] splitNums = line.Trim().Split(" ").Select(x => long.Parse(x)).ToArray();
				rangeByMap[currentMap].Add(new RangeStruct() { DestinationRangeStart = splitNums[0], SourceRangeStart = splitNums[1], RangeLength = splitNums[2] });
			}

			long minDestination = 0;
			foreach (var seed in seeds)
			{
				var tmpDestination = FindLocation(seed, rangeByMap);
				if (minDestination == 0) { minDestination = tmpDestination; continue; }
				if (minDestination > tmpDestination) { minDestination = tmpDestination; }
			}
			return minDestination;
		}

		internal static double SolvePart2()
		{
			var fileLocation = Shared.GetFileLocation("2023/day5-input.txt");
			var lines = File.ReadAllLines(fileLocation);

			var seedRanges = new Dictionary<long, long>();
			string currentMap = "";

			var rangeByMap = new Dictionary<string, List<RangeStruct>>();
			foreach (var line in lines)
			{
				if (line.Trim().Length == 0) { continue; }
				if (line.StartsWith(SEEDS))
				{
					var seedsString = line.Remove(0, SEEDS.Length);
					seedRanges = CalcSeedRanges(seedsString);
				}
				if (!char.IsDigit(line[0]))
				{
					currentMap = line.Trim();
					rangeByMap.Add(currentMap, new List<RangeStruct>());
					continue;
				}
				long[] splitNums = line.Trim().Split(" ").Select(x => long.Parse(x)).ToArray();
				rangeByMap[currentMap].Add(new RangeStruct() { DestinationRangeStart = splitNums[0], SourceRangeStart = splitNums[1], RangeLength = splitNums[2] });
			}

			long minDestination = 0;
			foreach (var seedRange in seedRanges)
			{
				var tmpDestination = FindLocation_Part2(seedRange, rangeByMap);
				if (minDestination == 0) { minDestination = tmpDestination; continue; }
				if (minDestination > tmpDestination) { minDestination = tmpDestination; }
			}

			return minDestination;
		}

		private static long FindLocation_Part2(KeyValuePair<long, long> seedRange, Dictionary<string, List<RangeStruct>> rangeByMap)
		{
			var currentSourceRange = seedRange;
			KeyValuePair<long, long> minDestinationRange = new KeyValuePair<long, long>();
			foreach (var key in rangeByMap.Keys) // for each map
			{
				foreach (var range in rangeByMap[key]) // for each map range
				{
					if (range.SourceRangeStart >= currentSourceRange.Key + currentSourceRange.Value) { continue; }
					if (range.SourceRangeStart + range.RangeLength < currentSourceRange.Key) { continue; }

					// -- 100 200 | 125 225  -> 125 200
					// -- 200, 440 | 180 600 -> 200 :440
					var mappedSourceRange = new KeyValuePair<long, long>(Math.Max(currentSourceRange.Key, range.SourceRangeStart), Math.Min(currentSourceRange.Value, range.SourceRangeStart + range.RangeLength));

					// 300 | 125 200 -> 325 400
					var startOffset = (currentSourceRange.Key > range.SourceRangeStart ? (currentSourceRange.Key - range.SourceRangeStart) : 0);
					var mappedDestinationRange = new KeyValuePair<long, long>(
						range.DestinationRangeStart + startOffset, 
						range.DestinationRangeStart + Math.Min(mappedSourceRange.Value - mappedSourceRange.Key, range.RangeLength)
					);
					if (minDestinationRange.Key < 1 || mappedDestinationRange.Key < minDestinationRange.Key) { minDestinationRange = mappedDestinationRange; }
				}

				currentSourceRange = minDestinationRange.Key > 0 ? minDestinationRange:  currentSourceRange;
				minDestinationRange = new KeyValuePair<long, long>();
			}
			return currentSourceRange.Key;
		}

		private static Dictionary<long, long> CalcSeedRanges(string seedsString)
		{
			var seedRanges = new Dictionary<long, long>();
			var splits = seedsString.Trim().Split(' ');
			long seedStart = 0;
			for (int i = 0; i < splits.Length; i++)
			{
				if (i % 2 == 0) { seedStart = long.Parse(splits[i].Trim()); }
				else
				{
					var length = long.Parse(splits[i].Trim());
					seedRanges.Add(seedStart, seedStart + length);
				}
			}
			return seedRanges;
		}

		private static long FindLocation(long seed, Dictionary<string, List<RangeStruct>> rangeByMap)
		{
			long? minDestination = null;
			var currentSource = seed;
			foreach (var key in rangeByMap.Keys) // for each map
			{
				foreach (var range in rangeByMap[key]) // for each map range
				{
					if (range.SourceRangeStart >= currentSource) { continue; }
					if (range.SourceRangeStart + range.RangeLength < currentSource) { continue; }
					var destination = (currentSource - range.SourceRangeStart) + range.DestinationRangeStart;
					if (!minDestination.HasValue || destination < minDestination) { minDestination = destination; }
				}

				currentSource = minDestination ?? currentSource;
				minDestination = null;
			}
			return currentSource;
		}
	}
}
