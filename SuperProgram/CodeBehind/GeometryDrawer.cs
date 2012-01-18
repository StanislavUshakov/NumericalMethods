using System;
using System.Drawing;
using System.Windows.Forms;
using Core;
using Point = Core.Point;
using System.Collections.Generic;

namespace SuperProgram.CodeBehind {
	public class GeometryDrawer{
		private readonly PictureBox _pictureBox;

        //Ekaterina
        private BufferedGraphicsContext _currentBufferContext;
        private System.Drawing.BufferedGraphics _imageBuffer;
        //end Ekaterina

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

            //Ekaterina
            _currentBufferContext = BufferedGraphicsManager.Current;
            //end Ekaterina
		}

		public void DrawPoints(Pen pen, params Point[] points){
			Graphics gr = Graphics.FromImage(_pictureBox.Image);
			foreach (var point in points){
                //Ekaterina
                _imageBuffer.Graphics.DrawEllipse(pen, point.X - 1, _pictureBox.Height - point.Y + 1, 3, 3);
                //end Ekaterina
				int xOffset = getXOffset(point);
				if (point.Index > 0){
                    //Ekaterina
                    _imageBuffer.Graphics.DrawString(point.Index.ToString(), SystemFonts.CaptionFont, Brushes.Black, point.X + xOffset, _pictureBox.Height - point.Y);
                    //end Ekaterina
				}
			}
            _imageBuffer.Render();
		}

		public void DrawLineSegments(Pen pen, params LineSegment[] lineSegments) {
			foreach (var lineSegment in lineSegments){
				DrawPoints(pen, lineSegment.Start, lineSegment.End);
				drawLine(pen, lineSegment.Start, lineSegment.End);
			}
            //Ekaterina
            _imageBuffer.Render();
            //end Ekaterina
		}

		public void DrawTriangles(Pen pen, params Triangle[] triangles) {
			foreach (var triangle in triangles){
				DrawPoints(pen, triangle.I, triangle.J, triangle.K);
				drawLine(pen, triangle.I, triangle.J);
				drawLine(pen, triangle.J, triangle.K);
				drawLine(pen, triangle.K, triangle.I);
			}
            //Ekaterina
            _imageBuffer.Render();
            //end Ekaterina
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
            //Ekaterina
            _imageBuffer.Render();
            //end Ekaterina
		}

		private void drawLine(Pen pen, Point pt1, Point pt2) {
            //Ekaterina
            _imageBuffer.Graphics.DrawLine(pen, pt1.X, _pictureBox.Height - pt1.Y, pt2.X, _pictureBox.Height - pt2.Y);
            //end Ekaterina
		}

		private int getXOffset(Point pt){
			return pt.X < _pictureBox.Width / 2 ? 2 : -10;
		}

        //Ekaterina
        public void BufferDispose()
        {
            _currentBufferContext.Dispose();
        }

        public void SetImageBuffer()
        {
            _imageBuffer = _currentBufferContext.Allocate(_pictureBox.CreateGraphics(), _pictureBox.DisplayRectangle);
        }

        public bool IsBufferNull()
        {
            return _imageBuffer == null ? true : false;
        }

        public void FillBufferRectangle(Brush brush, int x, int y, int width, int height)
        {
            _imageBuffer.Graphics.FillRectangle(brush, x, y, width, height);
        }

        public void RefreshImage()
        {
            _imageBuffer.Render();
        }

        public void RefreshImage(Graphics gr)
        {
            _imageBuffer.Render(gr);
        }

        public void FillTriangles(List<Triangle> triangleList, double[] T)
        {
            double maxT = 0;
            foreach (var temp in T)
                if (maxT < temp)
                    maxT = temp;
            double TStep = maxT > 0.0001 ? maxT / 255 : 1;

            foreach (var triangle in triangleList)
            {
                Point[] trianglePoints = { triangle.I, triangle.J, triangle.K };

                double fillT = (T[triangle.I.Index - 1] + T[triangle.J.Index - 1] + T[triangle.K.Index - 1]) / 3;
                int TColor = fillT > 0 ? 255 - (int)(fillT / TStep) : 255;
                SolidBrush brush = new SolidBrush(Color.FromArgb(255, TColor, TColor));
                fillTriangle(brush, trianglePoints);
            }
            _imageBuffer.Render();
        }

        private void fillTriangle(Brush brush, Point[] trianglePoints)
        {
            System.Drawing.Point[] drawingPoints = { new System.Drawing.Point(trianglePoints[0].X, _pictureBox.Height - trianglePoints[0].Y), 
                new System.Drawing.Point(trianglePoints[1].X, _pictureBox.Height - trianglePoints[1].Y), 
                new System.Drawing.Point(trianglePoints[2].X, _pictureBox.Height - trianglePoints[2].Y)};
            _imageBuffer.Graphics.FillPolygon(brush, drawingPoints);
        }
        //end Ekaterina
	}
}
