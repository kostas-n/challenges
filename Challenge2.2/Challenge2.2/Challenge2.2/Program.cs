using System;

namespace Challenge2._2
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			//var id = GetIdOfProjectionOnXAxis(4);
			var id = Solution(4,2);
			Console.WriteLine(id);
			Console.ReadKey();
		}

		static long Solution(long x, long y)
		{
			long cellIdOnXProjection = GetCellIdOnXProjection(x); // id of (x,1)

			// id(x,2) = id(x,1) + x
			// id(x,3) = id(x,2) + x + 1
			// id(x,4) = id(x,3) + x + 2

			// id(x,y) = ID(x,1) + x*(y-1) + (y-1)(y-2)/2 
			var result = cellIdOnXProjection + x * (y - 1) + (y - 1) * (y - 2) / 2;
			return result;
		}

		static long GetCellIdOnXProjection(long x)
		{
			// F(n) = n*(n+1)/2
			return x * (x + 1) / 2;
		}
	}
}
