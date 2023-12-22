using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
	internal class Day10
	{
		const char Vertical = '|';
		const char Horizontal = '-';
		const char NE = 'L';
		const char NW = 'J';
		const char SW = '7';
		const char SE = 'F';
		const char S = 'S';
		const char Ground = '.';

		record Coord(int Row, int Col);

		internal static int Solve()
		{
			var lines = Shared.GetFileLines("2023/day10-input.txt");

			Coord startingCoord = null;
			for (int i = 0; i < lines.Length; i++)
			{
				for (int j = 0; j < lines[i].Length; j++)
				{
					if (lines[i][j] == 'S')
					{
						startingCoord = new Coord(i, j);
					}
				}
			}

			Coord tmp = startingCoord;
			var visitedTiles = new List<Coord>() { startingCoord };
			var count = 0;
			while (true)
			{
				var neighbours = FindNeighbouringTiles(tmp, lines);

				var first = neighbours.FirstOrDefault();
				if (first == null) { break; }

				if (visitedTiles.Contains(first))
				{
					var second = neighbours.LastOrDefault();
					if(second == null) { break;  }
					if (visitedTiles.Contains(second)) { break; }
					tmp = second;
				}
				else { tmp = first; }

				visitedTiles.Add(tmp);
				count++;
			}
			return (int)Math.Ceiling((double)count / 2);
		}

		private static List<Coord> FindNeighbouringTiles(Coord coord, string[] lines)
		{
			var curr = lines[coord.Row][coord.Col];

			var neighbours = new List<Coord>();

			// up
			if ("S|LJ".Contains(curr) && coord.Row > 0)
			{
				if ("|F7".Contains(lines[coord.Row - 1][coord.Col]))
				{
					neighbours.Add(new Coord(coord.Row - 1, coord.Col));
				}
			}

			// down
			if ("S|F7".Contains(curr) && coord.Row < lines.Length - 1)
			{
				if ("|LJ".Contains(lines[coord.Row + 1][coord.Col]))
				{
					neighbours.Add(new Coord(coord.Row + 1, coord.Col));
				}
			}

			// right
			if ("S-FL".Contains(curr) && coord.Col < lines[0].Length - 1)
			{
				if ("-7J".Contains(lines[coord.Row][coord.Col + 1]))
				{
					neighbours.Add(new Coord(coord.Row, coord.Col + 1));
				}
			}

			// left
			if ("S-7J".Contains(curr) && coord.Col > 0)
			{
				if ("-FL".Contains(lines[coord.Row][coord.Col - 1]))
				{
					neighbours.Add(new Coord(coord.Row, coord.Col - 1));
				}
			}

			return neighbours;
		}
	}
}
