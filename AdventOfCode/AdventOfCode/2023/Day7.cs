using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
	internal class Day7
	{
		struct HandStruct
		{
			public string Hand { get; set; }
			public int Bid { get; set; }
		}
		enum HandTypes : short
		{
			FiveOfKind, FourOfKind, FullHouse, ThreeOfKind, TwoPair, OnePair, HighCard
		}
		internal static long Solve()
		{
			var lines = Shared.GetFileLines("2023/day7-input.txt");

			var hands = LoadHands(lines);
			var handsSorted = hands.OrderBy(x => x.Hand, new HandsComparer()).ToList(); // sorted by highest rank
			int totalWinnings = CountWinnings(hands, handsSorted);
			return totalWinnings;
		}

		private static int CountWinnings(List<HandStruct> hands, List<HandStruct> handsSorted)
		{
			var totalWinnings = 0;
			for (int i = 0; i < hands.Count; i++)
			{
				var rank = hands.Count - i;
				totalWinnings += handsSorted[i].Bid * rank;
			}

			return totalWinnings;
		}

		private static List<HandStruct> LoadHands(string[] lines)
		{
			var hands = new List<HandStruct>();
			foreach (var line in lines)
			{
				var substrings = line.Split(' ');
				hands.Add(new HandStruct() { Hand = substrings[0], Bid = short.Parse(substrings[1]) });
			}
			return hands;
		}

		internal static long SolvePart2()
		{
			var lines = Shared.GetFileLines("2023/day7-input.txt");

			var hands = LoadHands(lines);

			var handsSorted = hands.OrderBy(x => x.Hand, new HandsComparerWithJokers()).ToList(); // sorted by highest rank
			int totalWinnings = CountWinnings(hands, handsSorted);
			return totalWinnings;
		}

		class HandsComparerWithJokers : HandsComparer
		{
			public HandsComparerWithJokers()
			{
				IncludeJockers = true;
			}
			internal override int CompareByStartingChar(string x, string y)
			{
				var i = 0;
				var result = 0;
				while (result == 0)
				{
					if (x[i] != y[i])
					{
						return AssignWeightToLetter(x[i]) > AssignWeightToLetter(y[i]) ? -1 : 1;
					}

					i++;
				}
				return result;
			}

			internal override int AssignWeightToLetter(char c)
			{
				if (char.IsDigit(c)) { return int.Parse(c.ToString()); }

				switch (c)
				{
					case 'A': return 20;
					case 'K': return 19;
					case 'Q': return 18;
					case 'J': return 0;
					case 'T': return 16;
					default:
						throw new NotSupportedException("Not supported character");
				}
			}
		}

		internal class HandsComparer : IComparer<string>
		{
			internal virtual bool IncludeJockers { get; set; }
			public int Compare(string x, string y)
			{
				if (x == y) return 0;
				// is five of kind
				HandTypes xType;
				if (IsFiveOfKind(x, IncludeJockers)) { xType = HandTypes.FiveOfKind; }
				else
				if (IsFourOfKind(x, IncludeJockers)) { xType = HandTypes.FourOfKind; }
				else
				if (IsFullHouse(x, IncludeJockers)) { xType = HandTypes.FullHouse; }
				else
				if (IsThreeOfKind(x, IncludeJockers)) { xType = HandTypes.ThreeOfKind; }
				else
				if (IsTwoPair(x, IncludeJockers)) { xType = HandTypes.TwoPair; }
				else
				if (IsOnePair(x, IncludeJockers)) { xType = HandTypes.OnePair; }
				else xType = HandTypes.HighCard;

				HandTypes yType;
				if (IsFiveOfKind(y, IncludeJockers)) { yType = HandTypes.FiveOfKind; }
				else
				if (IsFourOfKind(y, IncludeJockers)) { yType = HandTypes.FourOfKind; }
				else
				if (IsFullHouse(y, IncludeJockers)) { yType = HandTypes.FullHouse; }
				else
				if (IsThreeOfKind(y, IncludeJockers)) { yType = HandTypes.ThreeOfKind; }
				else
				if (IsTwoPair(y, IncludeJockers)) { yType = HandTypes.TwoPair; }
				else
				if (IsOnePair(y, IncludeJockers)) { yType = HandTypes.OnePair; }
				else yType = HandTypes.HighCard;

				if (xType == yType)
				{
					return CompareByStartingChar(x, y);
				}

				return (short)xType < (short)yType ? -1 : 1;
			}

			internal virtual int CompareByStartingChar(string x, string y)
			{
				var i = 0;
				var result = 0;
				while (result == 0)
				{
					if (char.IsLetter(x[i]) && char.IsDigit(y[i])) { return -1; }
					if (char.IsDigit(x[i]) && char.IsLetter(y[i])) { return 1; }
					if (char.IsDigit(x[i]) && char.IsDigit(y[i])) { result = -1 * Convert.ToInt16(x[i]).CompareTo(Convert.ToInt16(y[i])); }
					if (char.IsLetter(x[i]) && char.IsLetter(y[i]))
					{
						if (x[i] != y[i])
						{
							return AssignWeightToLetter(x[i]) > AssignWeightToLetter(y[i]) ? -1 : 1;
						}
					}

					i++;
				}
				return result;
			}

			internal virtual int AssignWeightToLetter(char c)
			{
				switch (c)
				{
					case 'A': return 10;
					case 'K': return 9;
					case 'Q': return 8;
					case 'J': return 7;
					case 'T': return 6;
					default:
						throw new NotSupportedException("Not supported character");
				}
			}
		}

		internal static bool IsFiveOfKind(string hand, bool includeJokers = false)
		{
			if (hand.GroupBy(a => a).Count() == 1)
			{
				return true;
			}

			if (includeJokers)
			{
				var groups = hand.GroupBy(a => a);
				if (groups.Count() > 2) { return false; }
				var jokerGroup = groups.FirstOrDefault(x => x.Key == 'J');
				if (jokerGroup == null) { return false; }
				return groups.Count() > 0;
			}
			return false;
		}

		internal static bool IsFourOfKind(string hand, bool includeJokers = false)
		{
			var groups = hand.GroupBy(a => a).OrderByDescending(c => c.Count()).ToList();
			var res = groups.Count() == 2 && groups[0].Count() == 4;
			if (res) { return true; }

			if (includeJokers)
			{
				if (groups.Count() > 3) { return false; }
				var jokerGroup = groups.FirstOrDefault(x => x.Key == 'J');
				if (jokerGroup == null ) { return false; }
				// there must be a smarter way...
				return (groups[0].Count() + jokerGroup.Count() == 4) || (groups[1].Count() + jokerGroup.Count() == 4);
			}
			return false;
		}

		internal static bool IsFullHouse(string hand, bool includeJokers = false)
		{
			var groups = hand.GroupBy(a => a).OrderByDescending(c => c.Count()).ToList();
			var res = groups.Count() == 2 && groups[0].Count() == 3 && groups[1].Count() == 2;
			if (res) { return true; }

			if (includeJokers)
			{
				if (groups.Count() > 3) { return false; }
				var jokerGroup = groups.FirstOrDefault(x => x.Key == 'J');
				if (jokerGroup == null) { return false; }
				return ((groups[0].Count() + jokerGroup.Count() == 3) || (groups[1].Count() + jokerGroup.Count()) == 3) && groups.Count() == 3;
			}

			return false;
		}

		internal static bool IsThreeOfKind(string hand, bool includeJokers = false)
		{
			var groups = hand.GroupBy(a => a).OrderByDescending(c => c.Count()).ToList();
			var res = groups.Count() == 3 && groups[0].Count() == 3;
			if (res) { return true; }

			if (includeJokers)
			{
				if (groups.Count() > 4) { return false; }
				var jokerGroup = groups.FirstOrDefault(x => x.Key == 'J');
				if (jokerGroup != null) { return true; }
			}

			return false;
		}

		internal static bool IsTwoPair(string hand, bool includeJokers = false)
		{
			var groups = hand.GroupBy(a => a).OrderByDescending(c => c.Count()).ToList();
			var res = groups.Count() == 3 && groups[0].Count() == 2 && groups[1].Count() == 2;
			if (res) { return true; }

			if (includeJokers)
			{
				if (groups.Count() > 4) { return false; }
				var jokerGroup = groups.FirstOrDefault(x => x.Key == 'J');
				if (jokerGroup != null) { return true; }
			}
			return false;
		}

		internal static bool IsOnePair(string hand, bool includeJokers = false)
		{
			var groups = hand.GroupBy(a => a).OrderByDescending(c => c.Count()).ToList();
			var res = groups.Count() == 4 && groups[0].Count() == 2;
			if (res) { return true; }

			if (includeJokers)
			{
				var jokerGroup = groups.FirstOrDefault(x => x.Key == 'J');
				if (jokerGroup != null) { return true; }
			}
			return false;
		}
	}
}
