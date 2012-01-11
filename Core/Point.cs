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

        //Evgenij
        public override bool Equals(object obj) {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;
            Point p = (Point)obj;
            return (Index == p.Index) && (X == p.X) && (Y == p.Y);
        }
        //end Evgenij

	}
}
