using System;

namespace Core {
	public class Triangle{
		private double? _square;

		public Point I { get; private set; }
		public Point J { get; private set; }
		public Point K { get; private set; }
		public LineSegment IJ { get; private set; }
		public LineSegment JK { get; private set; }
		public LineSegment KI { get; private set; }

		public Triangle(Point i, Point j, Point k){
			I = i;
			J = j;
			K = k;
			IJ = new LineSegment(i, j);
			JK = new LineSegment(j, k);
			KI = new LineSegment(k, i);
			_square = null;
		}

		public double Square(){
			if (_square == null){
				double p = (IJ.Length + JK.Length + KI.Length)/2;
				_square = Math.Sqrt(p*(p - IJ.Length)*(p - JK.Length)*(p - KI.Length));
			}
			return _square.Value;
		}
	}
}
