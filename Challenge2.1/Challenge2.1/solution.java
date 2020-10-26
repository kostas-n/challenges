public class HelloWorld{

     public static void main(String []args){
        System.out.println("Hello World");
        int[] result = solution(new int[]{4,30,50,72});
        System.out.println(result[0] + "/" + result[1] );
     }
     
    public static int[] solution(int[] pegs){
	    int[] impossibleRadius = new int[] { -1, -1 };
		if (pegs.length < 2) { return impossibleRadius; }

		int[] firstEquation = new int[] { 1, -2, 0 }; // r1 -2*rn = 0
		int[] secondEquation = BuildSecondEquation(pegs);

		int[] radiusOfFirstGear = CalculateFirstGearRadius(firstEquation, secondEquation);
		if (radiusOfFirstGear[0] < 1)
		{
			return impossibleRadius;
		}
			
		radiusOfFirstGear = ReduceFraction(radiusOfFirstGear);

		if (!AssertRestOfGearRadii(radiusOfFirstGear, pegs))
		{
			radiusOfFirstGear = impossibleRadius;
		}

		return radiusOfFirstGear;
    }
    
	static boolean AssertRestOfGearRadii(int[] firstRadius, int[] pegs)
	{
		int[] leftGearRadius = firstRadius;
		// calculate the rest of the radii
		for (int i = 0; i < pegs.length - 1; i++)
		{
			int offset = pegs[i + 1] - pegs[i];
			int[] offsetAsFraction = new int[] { offset * leftGearRadius[1], leftGearRadius[1] };

			int[] rightGearAsFraction = new int[] { offsetAsFraction[0] - leftGearRadius[0], leftGearRadius[1] };

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
		int gcd = FindGreatestCommonDivisor(fraction[0], fraction[1]);
		return new int[] { fraction[0] / gcd, fraction[1] / gcd };
	}
	
	static int[] CalculateFirstGearRadius(int[] firstEquation, int[] secondEquation)
	{
		// use cramer's rule
		int determinantX = firstEquation[2] * secondEquation[1] - firstEquation[1] * secondEquation[2];
		int determinant = firstEquation[0] * secondEquation[1] - firstEquation[1] * secondEquation[0];

		return new int[] { determinantX, determinant };
	}
		
	static int[] BuildSecondEquation(int[] pegs)
	{
		int result = pegs[1] - pegs[0]; // initial offset

		for (int i = 1; i < pegs.length - 1; i++)
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

		if (pegs.length % 2 == 0)
		{
			return new int[] { 1, 1, result };
		}
		else
		{
			return new int[] { 1, -1, result };
		}
	}
	
}