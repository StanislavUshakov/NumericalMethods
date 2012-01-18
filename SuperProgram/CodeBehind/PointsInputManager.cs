using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Core;
using Point = Core.Point;

namespace SuperProgram.CodeBehind {
	public class PointsInputManager{
		private const string ErrorMessageBoxTitle = "Ошибка";
		private const string ErrorMessageBoxMessage = "Пересечение";

		private readonly GeometryDrawer _drawer;
		private readonly TabControlHelper _tabControlHelper;
		private int _lastPointIndex;

		public List<Contour> Contours { get; private set; }

		public bool AllContoursAreCompleted{
			get { return Contours.All(x => x.IsCompleted); }
		}

		public PointsInputManager(GeometryDrawer drawer, TabControlHelper tabControlHelper){
			Contours = new List<Contour>();
			_drawer = drawer;
			_tabControlHelper = tabControlHelper;
			_lastPointIndex = 0;
		}

		public void Reset(){
			Contours.Clear();
			_lastPointIndex = 0;
		}

        public void SetUpTestCase()
        {
            Contour currentContour = new Contour(Contours.Count + 1);
            Contours.Add(currentContour);
            Point pt1 = new Point(0, 0);
            pt1.T = 0;
            currentContour.Add(pt1);
            Point pt2 = new Point(100, 0);
            pt2.T = 100;
            currentContour.Add(pt2);
            Point pt3 = new Point(0, 100);
            pt3.T = 0;
            currentContour.Add(pt3);
            //Point pt4 = new Point(0, 100);
            //pt4.T = 0;
            //currentContour.Add(pt4);
            //Point pt5 = new Point(0, 40);
            //pt5.T = 1;
            //currentContour.Add(pt5);
            currentContour.Add(pt1);
        }

		public void PointsInputHandler(object sender, MouseEventArgs e){
			Contour currentContour = Contours.FirstOrDefault(x => !x.IsCompleted);
			if (currentContour == null){
				currentContour = new Contour(Contours.Count + 1);
				Contours.Add(currentContour);
			}
			var newPoint = new Point(e.X, _drawer.CanvasHeight - e.Y){Index = _lastPointIndex + 1};
			if (currentContour.Head == null) {
				currentContour.Add(newPoint);
				_drawer.DrawPoints(Pens.Black, newPoint);
                //Ekaterina
                _drawer.RefreshImage();
                //end Ekaterina
				_lastPointIndex++;
			} else{
				if (currentContour.Tail == null){
					var newLineSegment = new LineSegment(currentContour.Head, newPoint);
					if (Contours.Where(x => x != currentContour).Any(x => x.IntersectsWithLineSegment(newLineSegment, IntersectionCheckOptions.WithEdgePoints)) || 
						currentContour.IntersectsWithLineSegment(newLineSegment, IntersectionCheckOptions.WithoutEdgePoints)){
						MessageBox.Show(ErrorMessageBoxMessage, ErrorMessageBoxTitle);
					} else {
						currentContour.Add(newPoint);
						_drawer.DrawLineSegments(Pens.Black, newLineSegment);
						_lastPointIndex++;
					}
				} else{
					var newLineSegment = new LineSegment(currentContour.Tail, newPoint);
					if (Contours.Where(x => x != currentContour).Any(x => x.IntersectsWithLineSegment(newLineSegment, IntersectionCheckOptions.WithEdgePoints)) ||
						currentContour.IntersectsWithLineSegment(newLineSegment, IntersectionCheckOptions.WithoutEdgePoints)) {
						MessageBox.Show(ErrorMessageBoxMessage, ErrorMessageBoxTitle);
					} else{
						currentContour.Add(newPoint);
						if (!currentContour.IsCompleted) {
							_drawer.DrawLineSegments(Pens.Black, newLineSegment);
							_lastPointIndex++;
						} else {
							_drawer.DrawLineSegments(Pens.Black, new LineSegment(currentContour.Tail, currentContour.Head));
						}
					}
				}
			}
			if (currentContour.IsCompleted){
				_tabControlHelper.CreatePageForContour(currentContour);
			}
		}

		public Contour GetSingleContour(){
			if (Contours.Count == 0){
				throw new Exception("Контуры отсутствуют");
			}
			if (!AllContoursAreCompleted){
				throw new Exception("Все контуры должны быть замкнуты!");
			}
			var boundingBoxes = new Dictionary<int, BoundingBox>();
			for (int i=0; i<Contours.Count; i++){
				boundingBoxes.Add(i, Contours[i].GetBoundingBox());
			}
			var largestBox = boundingBoxes.FirstOrDefault(x => boundingBoxes.Where(y => y.Key != x.Key).All(y => x.Value.Contains(y.Value)));
			var restBoxes = boundingBoxes.Where(x => x.Key != largestBox.Key).ToArray();
			if (largestBox.Value == null) {
				throw new Exception("Контуры не образуют единой области. Дальнейшие вычисления невозможны");
			}
			if (restBoxes.Any(x => restBoxes.Where(y => y.Key != x.Key).Any(y => x.Value.Contains(y.Value)))){
				throw new Exception("Вложенность дырок недопустима. Дальнейшие вычисления невозможны");
			}
			var largestContour = Contours[largestBox.Key];
			largestContour.OrientCounterclockwise();
			for (int i = 0; i < Contours.Count; i++ ){
				if (i != largestBox.Key){
					var contour = Contours[i];
					contour.OrientClockwise();
					var nearestPoints = largestContour.ToDictionary(x => x, 
						x => contour.ToDictionary(y => y, y => Math.Sqrt(Math.Pow(x.X - y.X, 2) + Math.Pow(x.Y - y.Y, 2))).OrderBy(r => r.Value).First()).
						OrderBy(r => r.Value.Value).First();
					int largeContourPointIndex = nearestPoints.Key.Index;
					int contourPointIndex = nearestPoints.Value.Key.Index;
					for (int j = 0; j < contour.Count - contourPointIndex + 1; j++){
						Point pt = contour[contourPointIndex - 1 + j].Clone();
						pt.Index = largeContourPointIndex + 1 + j;
						largestContour.Insert(pt.Index - 1, pt);
					}
					for (int j = 0; j < contourPointIndex; j++){
						Point pt = contour[j].Clone();
						pt.Index = largeContourPointIndex + contour.Count - contourPointIndex + j + 2;
						largestContour.Insert(pt.Index - 1, pt);
					}
					Point self = largestContour[largeContourPointIndex - 1].Clone();
					int offset = self.Index + contour.Count + 2;
					self.Index = offset;
					largestContour.Insert(self.Index - 1, self);
					for (int j = offset; j < largestContour.Count; j++){
						largestContour[j].Index = j + 1;
					}
				}
			}
			largestContour.Index = 1;
			Contours.Clear();
			Contours.Add(largestContour);
			return largestContour;
		}
	}
}
