namespace Core {
	public class HelperUtils {
		public static void Swap<T>(ref T val1, ref T val2){
			T temp = val2;
			val2 = val1;
			val1 = temp;
		}
	}
}
