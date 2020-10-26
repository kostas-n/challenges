public class HelloWorld{

     public static void main(String []args){
        String id = solution(30000,40000);
        System.out.println(id);
     }
     
    public static String solution(long x, long y){
        long cellIdOnXProjection = GetCellIdOnXProjection(x); // id of (x,1)

		// id(x,2) = id(x,1) + x
		// id(x,3) = id(x,2) + x + 1
		// id(x,4) = id(x,3) + x + 2
        // ...
		// id(x,y) = ID(x,1) + x*(y-1) + (y-1)(y-2)/2 
		long result = cellIdOnXProjection + x * (y - 1) + (y - 1) * (y - 2) / 2;
		return String.valueOf(result);
    }
    
    static long GetCellIdOnXProjection(long x){
		// F(n) = n*(n+1)/2
		return x * (x + 1) / 2;
	}
}