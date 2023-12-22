using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
	internal class Day8
	{
		internal struct MapRow
		{
			public string Start { get; set; }
			public string Left { get; set; }
			public string Right { get; set; }
		}
		internal static long Solve()
		{
			var lines = Shared.GetFileLines("2023/day8-input.txt");
			var map = new List<MapRow>();
			string instruction = "";
			var regex = new Regex(@"\w+");
			for (int i = 0; i < lines.Length; i++)
			{
				if (i == 0) { instruction = lines[i].Trim(); continue; }
				if (i == 1) { continue; }

				var matches = regex.Matches(lines[i]);
				var row = new MapRow() { Start = matches[0].Value, Left = matches[1].Value, Right = matches[2].Value };
				map.Add(row);
			}

			var endFound = false;
			int numOfSteps = 0;
			var instrIndex = 0;
			var lineIndex = 0;
			while (!endFound)
			{
				char inst = instruction[instrIndex];
				string? instructedNode = (inst == 'L') ? map[lineIndex].Left : map[lineIndex].Right;

				if (instructedNode == "ZZZ")
				{
					endFound = true;
				}
				lineIndex = FindNextNodeIndex(map, instructedNode, lineIndex + 1);

				if (instrIndex == instruction.Length - 1)
				{
					instrIndex = 0; /* reset */
				}
				else
				{
					instrIndex++;
				}
				numOfSteps++;
			}
			return numOfSteps;
		}

		private static int FindNextNodeIndex(List<MapRow> map, string nodeToLookup, int startIndex)
		{
			for (int i = startIndex; i < map.Count(); i++)
			{
				if (map[i].Start == nodeToLookup)
				{
					return i;
				}
			}
			// and look up...
			for (int i = 0; i <= startIndex; i++)
			{
				if (map[i].Start == nodeToLookup)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
