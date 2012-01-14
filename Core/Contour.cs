using System;
using System.Linq;
using System.Collections.Generic;

namespace Core
{
    public class Contour : List<Point>
    {
        private const int CopletionAccuracy = 5;

        private bool _orientedClockwise;
        private bool _orientedCounterclockwise;

        public Point Head { get; private set; }
        public Point Tail { get; private set; }
        public bool IsCompleted { get; private set; }
        public int Index { get; set; }

        public Contour(int index)
        {
            IsCompleted = false;
            Head = null;
            Tail = null;
            Index = index;
            _orientedClockwise = false;
            _orientedCounterclockwise = false;
        }

        public new void Add(Point point)
        {
            if (Head != null)
            {
                if (Math.Abs(point.X - Head.X) < CopletionAccuracy && Math.Abs(point.Y - Head.Y) < CopletionAccuracy)
                {
                    IsCompleted = true;
                    return;
                }
                Tail = point;
            }
            else
            {
                Head = point;
            }
            base.Add(point);
        }

        public bool IntersectsWithLineSegment(LineSegment segment, IntersectionCheckOptions option)
        {
            bool result = false;
            for (int i = 1; i < Count && !result; i++)
            {
                result = segment.Intersects(new LineSegment(this[i - 1], this[i]), option);
            }
            return result;
        }

        public BoundingBox GetBoundingBox()
        {
            int x1 = this.Min(x => x.X);
            int y1 = this.Min(x => x.Y);
            int x2 = this.Max(x => x.X);
            int y2 = this.Max(x => x.Y);
            return new BoundingBox(x1, y1, x2, y2);
        }

        public void RecalculateWithStep(int step)
        {
            double dist;
            for (int i = 1; i < Count; i++)
            {

                int x1 = this[i - 1].X;
                int y1 = this[i - 1].Y;
                double t1 = this[i - 1].T;
                int x2 = this[i].X;
                int y2 = this[i].Y;
                double t2 = this[i].T;

                dist = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                if (dist > step)
                {
                    Point pt;
                    if (dist > 2 * step)
                    {
                        pt = new Point((int)(x1 + step * (x2 - x1) / dist), (int)(y1 + step * (y2 - y1) / dist)) { Index = i + 1, T = t1 + (t2 - t1) * step / dist };
                    }
                    else
                    {
                        pt = new Point(x1 + (x2 - x1) / 2, y1 + (y2 - y1) / 2) { Index = i + 1, T = (t1 + t2) / 2 };
                    }
                    Insert(i, pt);
                    for (int j = i + 1; j < Count; j++)
                    {
                        this[j].Index = j + 1;
                    }
                }
            }

            do
            {
                int x1 = this[Count - 1].X;
                int y1 = this[Count - 1].Y;
                double t1 = this[Count - 1].T;
                int x2 = this[0].X;
                int y2 = this[0].Y;
                double t2 = this[0].T;
                dist = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                if (dist > step)
                {
                    Point pt;
                    if (dist > 2 * step)
                    {
                        pt = new Point((int)(x1 + step * (x2 - x1) / dist), (int)(y1 + step * (y2 - y1) / dist)) { Index = Count + 1, T = t1 + (t2 - t1) * step / dist };
                    }
                    else
                    {
                        pt = new Point(x1 + (x2 - x1) / 2, y1 + (y2 - y1) / 2) { Index = Count + 1, T = (t1 + t2) / 2 };
                    }
                    Insert(Count, pt);
                }
            } while (dist > step);
        }

        #region Setting Orientation Methods

        /// <summary>
        /// Calculate the angle between the line which is going through the points (Ax, Ay) and (Bx, By) and the Ox axis.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The angle - value between 0 and pi</returns>
        private static double GetAngleWithOX(double Ax, double Ay, double Bx, double By)
        {
            if (Ay > By)
            {
                //A should be lower than B. This is need for the "0-pi" condition.
                HelperUtils.Swap(ref Ax, ref Bx);
                HelperUtils.Swap(ref Ay, ref By);
            }

            return Math.Atan2(By - Ay, Bx - Ax);
        }

        private bool IsCurrentOrientationClockwise()
        {
            if (_orientedClockwise)
                return true;

            int minY = this.Min(x => x.Y);
			int contourBottomPointIndex = FindIndex(x => x.Y == minY);
			int rightNeighbour = contourBottomPointIndex < Count - 1 ? contourBottomPointIndex + 1 : 0;
            int leftNeighbour = contourBottomPointIndex > 0 ? contourBottomPointIndex - 1 : Count - 1;

            double angleLeftPoint = GetAngleWithOX(this[contourBottomPointIndex].X, this[contourBottomPointIndex].Y, this[leftNeighbour].X, this[leftNeighbour].Y);
            double angleRightPoint = GetAngleWithOX(this[contourBottomPointIndex].X, this[contourBottomPointIndex].Y, this[rightNeighbour].X, this[rightNeighbour].Y);

            return angleRightPoint < angleLeftPoint;
        }

        public void OrientCounterclockwise()
        {            
            if (!IsCompleted)
            {
                throw new Exception("Контур должен быть замкнутым");
            }
            if (!_orientedCounterclockwise)
            {
                int minY = this.Min(x => x.Y);
                int contourBottomPointIndex = FindIndex(x => x.Y == minY);                
                if (IsCurrentOrientationClockwise())
                {
                    renumerate(contourBottomPointIndex, 1, RenumerationDirection.Right);
                }
                else
                {
                    renumerate(contourBottomPointIndex, 1, RenumerationDirection.Left);
                }
                Sort(new PointIndexBasedComparer());
                Head = this[0];
                Tail = this[Count - 1];
                _orientedCounterclockwise = true;
                _orientedClockwise = false;
            }
        }

        public void OrientClockwise()
        {
            if (!IsCompleted)
            {
                throw new Exception("Контур должен быть замкнутым");
            }
            if (!_orientedClockwise)
            {
                int maxY = this.Min(x => x.Y);
                int contourBottomPointIndex = FindIndex(x => x.Y == maxY);                
                if (!IsCurrentOrientationClockwise())
                {
                    renumerate(contourBottomPointIndex, 1, RenumerationDirection.Left);
                }
                else
                {
                    renumerate(contourBottomPointIndex, 1, RenumerationDirection.Right);
                }
                Sort(new PointIndexBasedComparer());
                Head = this[0];
                Tail = this[Count - 1];
                _orientedClockwise = true;
                _orientedCounterclockwise = false;
            }
        }

        private void renumerate(int contourIndex, int index, RenumerationDirection direction)
        {
            this[contourIndex].Index = index;
            if (index != Count)
            {
                if (direction == RenumerationDirection.Right)
                {
                    if (contourIndex == Count - 1)
                    {
                        renumerate(0, index + 1, direction);
                    }
                    else
                    {
                        renumerate(contourIndex + 1, index + 1, direction);
                    }
                }
                else
                {
                    if (contourIndex == 0)
                    {
                        renumerate(Count - 1, index + 1, direction);
                    }
                    else
                    {
                        renumerate(contourIndex - 1, index + 1, direction);
                    }
                }
            }
        }

        private enum RenumerationDirection
        {
            Right, Left
        }

        private class PointIndexBasedComparer : IComparer<Point>
        {
            public int Compare(Point x, Point y)
            {
                int result = 0;
                if (x.Index > y.Index)
                {
                    result = 1;
                }
                if (x.Index < y.Index)
                {
                    result = -1;
                }
                return result;
            }
        }

        #endregion
    }
}
