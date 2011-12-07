namespace Core {
	public class BoundingBox {
		public int X1 { get; private set; }
		public int Y1 { get; private set; }
		public int X2 { get; private set; }
		public int Y2 { get; private set; }

		public BoundingBox(int x1, int y1, int x2, int y2){
			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
		}

		public bool Contains(BoundingBox other){
			return other.X1 >= X1 && other.Y1 >= Y1 && other.X2 <= X2 && other.Y2 <= Y2;
		}
	}
}
