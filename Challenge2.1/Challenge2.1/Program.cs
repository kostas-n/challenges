using System;
using System.Collections.Generic;

namespace Challenge2._1
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			// int[] pegs = new int[] { 4, 30, 50, 72, 97 };
			//int[] pegs = new int[] { 4, 30, 50, 72 };
			// int[] pegs = new int[] { 4, 30, 50 };
			//int[] pegs = new int[] { 4, 30 };
			int[] pegs = new int[] { 4, 35, 63, 101, 134 };

			int[] result = Solution(pegs);
			Console.WriteLine($"{result[0]}/{result[1]}");
		}

		/// <summary>
		// We have to solve a N-equation system:
		// x[0] - 2*x[n-1] = 0, initial equation
		// x[i] + x[i+1] = pegs[i + 1] - pegs[i]; (N-1 equations) 
		// We can first produce a 2x2 equation system by using elimination. Solving that will give us the radius of the first gear.
		// We will then calculate the radii for the rest of the gears for asserting their value is >=1
		/// </summary>
		/// <param name="pegs"></param>
		/// <returns></returns>
		static int[] Solution(int[] pegs)
		{
			int[] impossibleRadius = new[] { -1, -1 };
			if (pegs.Length < 2) { return impossibleRadius; }

			int[] firstEquation = new int[3] { 1, -2, 0 }; // r1 -2*rn = 0, r1 radius is twice the size of rlast
			int[] secondEquation = Eliminate(pegs);

			int[] radiusOfFirstGear = CalculateFirstGearRadius(firstEquation, secondEquation);
			if (radiusOfFirstGear[0] < 1)
			{
				Console.WriteLine($"Impossible fraction: {radiusOfFirstGear[0]},{radiusOfFirstGear[1]}");
				return impossibleRadius;
			}
			
			radiusOfFirstGear = ReduceFraction(radiusOfFirstGear);

			Console.WriteLine($"Final fraction: {radiusOfFirstGear[0]},{radiusOfFirstGear[1]}");
			if (!AssertMinGearRadius(radiusOfFirstGear, pegs))
			{
				radiusOfFirstGear = impossibleRadius;
			}

			return radiusOfFirstGear;
		}

		static bool AssertMinGearRadius(int[] firstRadius, int[] pegs)
		{
			int[] leftGearRadius = firstRadius;
			// calculate rest of the gears radius
			for (int i = 0; i < pegs.Length - 1; i++)
			{
				int offset = pegs[i + 1] - pegs[i];
				int[] offsetAsFraction = new int[] { offset * leftGearRadius[1], leftGearRadius[1] };

				int[] rightGearAsFraction = new int[] { offsetAsFraction[0] - leftGearRadius[0], leftGearRadius[1] };

				Console.WriteLine($"radius {i + 1}: {rightGearAsFraction[0]}/{rightGearAsFraction[1]}");
				if (rightGearAsFraction[0] < rightGearAsFraction[1]) // smaller than 1
				{
					return false;
				}
				leftGearRadius = new int[] { rightGearAsFraction[0], rightGearAsFraction[1] };
			}
			// all good
			return true;
		}

		static int FindGreatestCommonDivisor(int a, int b)
		{
			if (b == 0)
			{
				return a;
			}
			int remainder = a % b;
			return FindGreatestCommonDivisor(b, remainder);
		}
		static int[] ReduceFraction(int[] fraction)
		{
			var gcd = FindGreatestCommonDivisor(fraction[0], fraction[1]);
			return new int[] { fraction[0] / gcd, fraction[1] / gcd };
		}

		static int[] CalculateFirstGearRadius(int[] firstEquation, int[] secondEquation)
		{
			// use cramer's rule
			int determinantX = firstEquation[2] * secondEquation[1] - firstEquation[1] * secondEquation[2];
			int determinant = firstEquation[0] * secondEquation[1] - firstEquation[1] * secondEquation[0];

			return new int[] { determinantX, determinant };
		}

		//static int[,] GenerateCoefficientsMatrix(int[] pegs, int[] initialEquation)
		//{
		//	int[,] matrix = new int[pegs.Length, pegs.Length];
		//	int[] offsets = new int[pegs.Length-1];

		//	//  r1 -2*rn = 0
		//	matrix[0, 0] = 1;
		//	matrix[0, pegs.Length] = -2;
		//	offsets[0] = 0;

		//	for (int i = 1; i < pegs.Length - 1; i++)
		//	{
		//		offsets[i] = pegs[i + 1] - pegs[i];
		//		matrix[i, i] = 1;
		//		matrix[i, i+1] = 1;
		//	}

		//	//find matrix inverse
		//}

		static int[] Eliminate(int[] pegs)
		{
			int result = pegs[1] - pegs[0]; // initial offset

			for (int i = 1; i < pegs.Length - 1; i++)
			{
				int offset = pegs[i + 1] - pegs[i];
				if (i % 2 == 0)
				{
					result += offset;
				}
				else
				{
					result -= offset;
				}
			}

			if (pegs.Length % 2 == 0)
			{
				return new int[] { 1, 1, result };
			}
			else
			{
				return new int[] { 1, -1, result };
			}
		}

	}
}
