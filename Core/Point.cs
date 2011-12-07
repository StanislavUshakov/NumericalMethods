namespace Core {
	public class Point{
		public int Index { get; set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		public double T { get; set; }
		
		public Point(int x, int y){
			X = x;
			Y = y;
		}

		public Point Clone(){
			return new Point(X, Y){Index = Index, T = T};
		}
	}
}
