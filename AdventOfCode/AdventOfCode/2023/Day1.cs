using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
	// https://adventofcode.com/2023/day/2
	internal class Day1
	{
		public static int Solve()
		{
			var fileLocation = Shared.GetFileLocation("2023/day1-Input.txt");
			var lines = File.ReadAllLines(fileLocation);

			var sum = 0;
			var nums = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
			foreach (var line in lines)
			{
				var op1 = '0';
				var op2 = '0';
				var firstIndex = line.IndexOfAny(nums);
				if (firstIndex > -1) { op1 = line[firstIndex]; }

				var lastIndex = line.LastIndexOfAny(nums);
				if (lastIndex > -1 && lastIndex != firstIndex) { op2 = line[lastIndex]; } else { op2 = op1; }

				sum += short.Parse(op1.ToString() + op2.ToString());
			}
			return sum;
		}

		public static int SolvePart2()
		{
			var fileLocation = Shared.GetFileLocation("2023/day1-Input.txt");
			var lines = File.ReadAllLines(fileLocation);

			var sum = 0;
			var nums = new Dictionary<string, string>(){
				{ "one", "1"},
				{ "two", "2"},
				{ "three", "3"},
				{ "four", "4"},
				{ "five", "5"},
				{ "six", "6"},
				{ "seven", "7"},
				{ "eight", "8"},
				{ "nine", "9"},
				{ "zero", "0"},
				{ "0", "0"},
				{ "1", "1"},
				{ "2", "2"},
				{ "3", "3"},
				{ "4", "4"},
				{ "5", "5"},
				{ "6", "6"},
				{ "7", "7"},
				{ "8", "8"},
				{ "9", "9"},
			};

			foreach (var line in lines)
			{
				var op1 = "0";
				var op2 = "0";

				var firstNum = FindFirstNum(line, nums);
				if (firstNum != string.Empty) { op1 = nums[firstNum]; }

				var lastNum = FindLastNum(line, nums);
				if (lastNum != string.Empty) { op2 = nums[lastNum]; } else { op2 = op1; }

				sum += short.Parse(op1.ToString() + op2.ToString());
			}
			return sum;
		}

		private static string FindFirstNum(string input, Dictionary<string, string> nums)
		{
			var minIndex = -1;
			var curr = "";
			foreach (var item in nums)
			{
				var ind = input.IndexOf(item.Key);
				if (ind > -1 && (ind < minIndex || curr == ""))
				{
					minIndex = ind;
					curr = item.Key;
				}
			}
			return curr;
		}

		private static string FindLastNum(string input, Dictionary<string, string> nums)
		{
			var maxIndex = -1;
			var curr = "";
			foreach (var item in nums)
			{
				var ind = input.LastIndexOf(item.Key);
				if (ind > -1 && (ind > maxIndex || curr == ""))
				{
					maxIndex = ind;
					curr = item.Key;
				}
			}
			return curr;
		}
	}
}
