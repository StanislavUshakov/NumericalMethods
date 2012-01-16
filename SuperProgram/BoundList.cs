using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace Triangulation
{
    class BoundList : List<BPoint>
    {
        // marks bad (it - triangle, t - Point)
        void markBad(BPoint it, BPoint t)
        {
            Triangle tr = new Triangle(it.point, it.left.point, it.right.point);
            double ang = it.angle;
            if ((ang < Math.PI * 5 / 12) ||
                ((ang < Math.PI / 2) && (it.jAngle > Math.PI / 6) && (it.kAngle > Math.PI / 6)))
            {
                if (t.insideTriangle(tr))
                    it.badPoints.Add(t);
            }
            else
            {
                // check excavation for the intersection
                Point ep = ExcavationPoint(it);
                LineSegment excSegment = new LineSegment(it.point, ep);
                BPoint temp = it.right;
                bool flag = false;
                while ((!flag) && (temp != it.left))
                {
                    LineSegment tSegment = new LineSegment(temp.point, temp.right.point);
                    if (!((BPoint.equalPoints(excSegment.Start, tSegment.Start)) ||
                          (BPoint.equalPoints(excSegment.Start, tSegment.End)) ||
                          (BPoint.equalPoints(excSegment.End, tSegment.Start)) ||
                          (BPoint.equalPoints(excSegment.End, tSegment.End))))
                        flag = excSegment.Intersects(tSegment, IntersectionCheckOptions.WithoutEdgePoints);
                    temp = temp.right;
                }
                it.eBad = flag;
            }
        }

        // puts the point into the list of BadPoints if it is bad
        public void marksPointBad(BPoint t)
        {
            for (int i = 0; i < this.Count(); i++)
            {
                BPoint it = this[i];
                markBad(it, t);
            }
        }

        // sets the list of BadPoints for the triangle
        public void marksTriangleBad(BPoint it)
        {
            for (int i = 0; i < this.Count(); i++)
            {
                BPoint t = this[i];
                markBad(it, t);
            }
        }

        // deletes the point from the list of BadPoints
        public void deleteBad(BPoint bp)
        {
            for (int i = 0; i < this.Count(); i++)
            {
                this[i].badPoints.Remove(bp);
            }
        }

        // binary search
        public void Add(BPoint bp)
        {
            bp.updateAngles();
            int first = 0;
            int last = Count - 1;
            int ser;
            while (first < last)
            {
                ser = (first + last) / 2;
                if (this[ser].angle < bp.angle)
                    first = ser + 1;
                else
                    last = ser;
            }

            if ((last < 0) || (bp.angle > this[last].angle))
                base.Add(bp);
            else
                this.Insert(last, bp);
        }

        // update the place of bp according to the angle
        public void Update(BPoint bp)
        {
            Remove(bp);
            Add(bp);
        }

        // update the place of bp according to the angle
        public void UpdateBad(BPoint bp)
        {
            bp.eBad = false;
            bp.badPoints.Clear();
            marksTriangleBad(bp);
            marksPointBad(bp);
        }

        // cut point from the list
        public void Cut(BPoint bp)
        {
            bp.left.right = bp.right;
            bp.right.left = bp.left;
            Update(bp.left);
            Update(bp.right);
            UpdateBad(bp.left);
            UpdateBad(bp.right);
            deleteBad(bp);
            Remove(bp);
        }

        // finds the excavationPoint
        Point ExcavationPoint(BPoint bp)
        {
            double ang = (bp.rAngle + bp.lAngle) / 2;
            double r = (BPoint.dist(bp.point, bp.right.point) + BPoint.dist(bp.point, bp.left.point)) / 2;
            int x = bp.point.X + (int)(r * Math.Cos(ang));
            int y = bp.point.Y + (int)(r * Math.Sin(ang));
            Point p = new Point(x, y);
            return p;
        }

        // add new point
        public BPoint Excavation(BPoint bp)
        {
            Point p = ExcavationPoint(bp);
            Triangulator.pointCount++;
            p.Index = Triangulator.pointCount;
            BPoint newBP = new BPoint(p);

            bp.left.right = newBP;
            bp.right.left = newBP;
            newBP.left = bp.left;
            newBP.right = bp.right;
            deleteBad(bp);
            Remove(bp);

            Add(newBP);
            Update(newBP.left);
            Update(newBP.right);

            UpdateBad(newBP);
            UpdateBad(newBP.left);
            UpdateBad(newBP.right);

            return newBP;
        }
    }
}