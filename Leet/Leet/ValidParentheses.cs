using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leet
{
	public class ValidParentheses
	{
		private static readonly Dictionary<char, char> map = new(){ {'(', ')' },{'[', ']' },{'{', '}' }};
		public static bool IsValid(string s)
		{
			var stack = new Stack<char>();
			foreach (char c in s)
			{
				if (map.ContainsKey(c))
				{
					stack.Push(c);
					continue;
				}

				if (IsMatch(stack, c))
				{
					stack.Pop();
					continue;
				}

				return false;
			}
			return stack.Count == 0;
		}

		private static bool IsMatch(Stack<char> stack, char input)
		{	
			if(stack.Count== 0) return false;

			var peek = stack.Peek();

			return map[peek] == input;
		}
	}
}
