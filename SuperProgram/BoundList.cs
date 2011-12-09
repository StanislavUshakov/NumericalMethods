using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace Triangulation
{
    class BoundList : List<BPoint>
    {
        // puts the point into the list of BadPoints if it is bad
        public void marksPointBad(BPoint t)
        {
            for (int i = 0; i < this.Count(); i++)
            {
                BPoint it = this[i];
                Triangle tr = new Triangle(it.point, it.left.point, it.right.point);
                if (t.insideTriangle(tr))
                    this[i].badPoints.Add(t);
            }
        }

        // sets the list of BadPoints for the triangle
        public void marksTriangleBad(BPoint it)
        {
            Triangle tr = new Triangle(it.point, it.left.point, it.right.point);
            for (int i = 0; i < this.Count(); i++)
            {
                BPoint t = this[i];
                if (t.insideTriangle(tr))
                    it.badPoints.Add(t);
            }
        }

        // deletes the point from the list of BadPoints
        public void deleteBad(BPoint bp)
        {
            for (int i = 0; i < this.Count(); i++)
            {
                this[i].badPoints.Remove(bp);
            }

            // became neighbours
            bp.left.badPoints.Clear();
            bp.right.badPoints.Clear();
            marksTriangleBad(bp.left);
            marksTriangleBad(bp.right);
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

        // cut point from the list
        public void Cut(BPoint bp)
        {
            bp.left.right = bp.right;
            bp.right.left = bp.left;
            Update(bp.left);
            Update(bp.right);
            deleteBad(bp);
            Remove(bp);
        }

        // add new point
        public BPoint Excavation(BPoint bp)
        {
            double ang = (bp.rAngle + bp.lAngle) / 2;
            double r = (BPoint.dist(bp.point, bp.right.point) + BPoint.dist(bp.point, bp.left.point)) / 2;
            int x = bp.point.X + (int)(r * Math.Cos(ang));
            int y = bp.point.Y + (int)(r * Math.Sin(ang));
            Point p = new Point(x, y);
            Triangulator.pointCount++;
            p.Index = Triangulator.pointCount;
            BPoint newBP = new BPoint(p);

            bp.left.right = newBP;
            bp.right.left = newBP;
            newBP.left = bp.left;
            newBP.right = bp.right;
            Add(newBP);
            marksPointBad(newBP);

            Update(bp.left);
            Update(bp.right);
            deleteBad(bp);
            Remove(bp);

            return newBP;
        }
    }
}