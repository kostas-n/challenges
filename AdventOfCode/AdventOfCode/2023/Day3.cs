using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
	// https://adventofcode.com/2023/day/3
	internal static class Day3
	{
		struct Coord
		{
			internal int X;
			internal int Y;
		}
		struct NumberCoord
		{
			internal int XStart;
			internal int XEnd;
			internal int Y;
			internal int Number;
		}
		const char NOT_SYMBOL = '.';
		const char GEAR = '*';
		public static int Solve()
		{
			var fileLocation = Shared.GetFileLocation("2023/day3-input.txt");
			var lines = File.ReadAllLines(fileLocation);

			// structures for storing coordinates
			var symbolCoords = new List<Coord>();
			var numberCoords = new List<NumberCoord>();

			string currentNum = "";
			int currentXStart = -1, currentXEnd = -1, currentY = -1;
			for (int i = 0; i < lines.Length; i++)
			{
				// any nums from the end of the prev line
				if (currentNum.Length > 0)
				{
					numberCoords.Add(new NumberCoord() { Number = int.Parse(currentNum), XStart = currentXStart, XEnd = currentXEnd, Y = currentY });
				}
				currentNum = "";
				currentXStart = currentXEnd = currentY = -1;
				for (int j = 0; j < lines[i].Length; j++)
				{
					if (lines[i][j] != NOT_SYMBOL && !char.IsDigit(lines[i][j]))
					{
						symbolCoords.Add(new Coord() { X = j, Y = i });
					}
					if (char.IsDigit(lines[i][j]))
					{
						if (currentNum.Length == 0)
						{
							currentXStart = j;
						}
						currentNum += lines[i][j];
						currentXEnd = j;
						currentY = i;
					}
					else
					{
						if (currentNum.Length > 0)
						{
							numberCoords.Add(new NumberCoord() { Number = int.Parse(currentNum), XStart = currentXStart, XEnd = currentXEnd, Y = currentY });
						}
						currentNum = "";
						currentXStart = currentXEnd = currentY = -1;
					}
				}
			}

			var sum = 0;
			// finding adjacents 
			//var currLn = -1;
			foreach (var numberCoord in numberCoords)
			{
				//bool match = false;
				foreach (var symbolCoord in symbolCoords)
				{
					if (Math.Abs(numberCoord.Y - symbolCoord.Y) <= 1)
					{
						if (numberCoord.XStart - 1 <= symbolCoord.X && numberCoord.XEnd + 1 >= symbolCoord.X)
						{
							sum += numberCoord.Number;
							//if (currLn != numberCoord.Y)
							//{
							//	currLn = numberCoord.Y;
							//	Console.WriteLine();
							//	Console.Write("line" + currLn);
							//}
							//Console.Write(" " + numberCoord.Number);
							//match = true;
							break;
						}
					}
				}
				//if (match) continue;
				//if (currLn != numberCoord.Y)
				//{
				//	currLn = numberCoord.Y;
				//	Console.WriteLine();
				//	Console.Write("line" + currLn);
				//}
				//Console.ForegroundColor = ConsoleColor.Red;
				//Console.Write(" " + numberCoord.Number);
				//Console.ResetColor();
			}
			Console.WriteLine();
			return sum;
		}

		public static int SolvePart2()
		{
			// won't bother with DRY...
			var fileLocation = Shared.GetFileLocation("2023/day3-input.txt");
			var lines = File.ReadAllLines(fileLocation);

			// structures for storing coordinates
			var gearCoords = new List<Coord>();
			var numberCoords = new List<NumberCoord>();

			string currentNum = "";
			int currentXStart = -1, currentXEnd = -1, currentY = -1;
			for (int i = 0; i < lines.Length; i++)
			{
				// any nums from the end of the prev line
				if (currentNum.Length > 0)
				{
					numberCoords.Add(new NumberCoord() { Number = int.Parse(currentNum), XStart = currentXStart, XEnd = currentXEnd, Y = currentY });
				}
				currentNum = "";
				currentXStart = currentXEnd = currentY = -1;
				for (int j = 0; j < lines[i].Length; j++)
				{
					if (lines[i][j] == GEAR)
					{
						gearCoords.Add(new Coord() { X = j, Y = i });
					}
					if (char.IsDigit(lines[i][j]))
					{
						if (currentNum.Length == 0)
						{
							currentXStart = j;
						}
						currentNum += lines[i][j];
						currentXEnd = j;
						currentY = i;
					}
					else
					{
						if (currentNum.Length > 0)
						{
							numberCoords.Add(new NumberCoord() { Number = int.Parse(currentNum), XStart = currentXStart, XEnd = currentXEnd, Y = currentY });
						}
						currentNum = "";
						currentXStart = currentXEnd = currentY = -1;
					}
				}
			}

			var sum = 0;
			foreach (var gearCoord in gearCoords)
			{
				// find a pair of adjacent numbers
				var adjacentNums = new List<NumberCoord>();
				foreach (var numberCoord in numberCoords)
				{
					if (Math.Abs(gearCoord.Y - numberCoord.Y) <= 1)
					{
						if (gearCoord.X + 1 >= numberCoord.XStart && gearCoord.X - 1 <= numberCoord.XEnd)
						{
							adjacentNums.Add(numberCoord);
						}
					}
				}
				if (adjacentNums.Count == 2) { sum += adjacentNums.Select(a => a.Number).Aggregate(1, (x, y) => x * y); }
			}
			return sum;
		}
	}
}
