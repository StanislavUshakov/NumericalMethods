using System;
using System.Drawing;
using System.Windows.Forms;
using Core;
using Point = Core.Point;

namespace SuperProgram.CodeBehind {
	public class GeometryDrawer{
		private readonly PictureBox _pictureBox;

		public Image LastDrawnFrame { get; private set; }
		public int CanvasWidth { get; private set; }
		public int CanvasHeight { get; private set; }

		public GeometryDrawer(PictureBox pictureBox){
			if (pictureBox == null){
				throw new ArgumentNullException("pictureBox");
			}
			_pictureBox = pictureBox;
			LastDrawnFrame = pictureBox.Image;
			CanvasHeight = pictureBox.Height;
			CanvasWidth = pictureBox.Width;
		}

		public void DrawPoints(Pen pen, params Point[] points){
			Graphics gr = Graphics.FromImage(_pictureBox.Image);
			foreach (var point in points){
				gr.DrawEllipse(pen, point.X - 1, _pictureBox.Height - point.Y + 1, 3, 3);
				int xOffset = getXOffset(point);
				if (point.Index > 0){
					gr.DrawString(point.Index.ToString(), SystemFonts.CaptionFont, Brushes.Black, point.X + xOffset, _pictureBox.Height - point.Y);
				}
			}
			_pictureBox.Refresh();
		}

		public void DrawLineSegments(Pen pen, params LineSegment[] lineSegments) {
			foreach (var lineSegment in lineSegments){
				DrawPoints(pen, lineSegment.Start, lineSegment.End);
				drawLine(pen, lineSegment.Start, lineSegment.End);
			}
		}

		public void DrawTriangles(Pen pen, params Triangle[] triangles) {
			foreach (var triangle in triangles){
				DrawPoints(pen, triangle.I, triangle.J, triangle.K);
				drawLine(pen, triangle.I, triangle.J);
				drawLine(pen, triangle.J, triangle.K);
				drawLine(pen, triangle.K, triangle.I);
			}
		}

		public void DrawContours(Pen pen, params Contour[] contours){
			foreach (var contour in contours){
				DrawPoints(pen, contour.Head);
				for (int i = 1; i < contour.Count; i++){
					DrawPoints(pen, contour[i]);
					drawLine(pen, contour[i-1], contour[i]);
				}
				drawLine(pen, contour.Head, contour.Tail);
			}
		}

		private void drawLine(Pen pen, Point pt1, Point pt2) {
			Graphics gr = Graphics.FromImage(_pictureBox.Image);
			gr.DrawLine(pen, pt1.X, _pictureBox.Height - pt1.Y, pt2.X, _pictureBox.Height - pt2.Y);
			_pictureBox.Refresh();
		}

		private int getXOffset(Point pt){
			return pt.X < _pictureBox.Width / 2 ? 2 : -10;
		}
	}
}
