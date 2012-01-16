using System;

namespace Core {
	public class LineSegment {
		public Point Start { get; private set; }
		public Point End { get; private set; }
		public double Length { get; private set; }

		public LineSegment(Point start, Point end){
			Start = start;
			End = end;
			Length = Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Y - end.Y, 2));
		}

		public bool Intersects(LineSegment other, IntersectionCheckOptions option){
            return projectionIntersectionCheck(other, option) && signedTriangleSquareCheck(other, option);
		}

		private static int signedTriangleSquare(Point a, Point b, Point c){
			return (b.X - a.X)*(c.Y - a.Y) - (b.Y - a.Y)*(c.X - a.X);
		}

		private bool signedTriangleSquareCheck(LineSegment other, IntersectionCheckOptions option){
			int call1 = signedTriangleSquare(Start, End, other.Start);
			int call2 = signedTriangleSquare(Start, End, other.End);
			int call3 = signedTriangleSquare(other.Start, other.End, Start);
			int call4 = signedTriangleSquare(other.Start, other.End, End);
			return option == IntersectionCheckOptions.WithoutEdgePoints ? call1*call2 < 0 && call3*call4 < 0 : call1*call2 <= 0 && call3*call4 <= 0;
		}

		private static bool projectionIntersection(int a, int b, int c, int d, IntersectionCheckOptions option){
			if (a > b){
				HelperUtils.Swap(ref a, ref b);
			}
			if (c > d){
				HelperUtils.Swap(ref c, ref d);
			}
			return option == IntersectionCheckOptions.WithoutEdgePoints ? Math.Max(a, c) < Math.Min(b, d) : Math.Max(a, c) <= Math.Min(b, d);
		}

		private bool projectionIntersectionCheck(LineSegment other, IntersectionCheckOptions option){
			bool call1 = projectionIntersection(Start.X, End.X, other.Start.X, other.End.X, option);
			bool call2 = projectionIntersection(Start.Y, End.Y, other.Start.Y, other.End.Y, option);
			return call1 && call2;            
		}
	}
}
