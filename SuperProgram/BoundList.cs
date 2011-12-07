using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace Triangulation
{
    class BoundList: List<BPoint>
    {
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

        // update the place of из according to the angle
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
            bp.left.badPoints.Remove(bp);
            bp.right.badPoints.Remove(bp);
            Update(bp.left);
            Update(bp.right);
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

            // TO DO: check - do we need that
            bp.left.badPoints.Remove(bp);
            bp.right.badPoints.Remove(bp);
            // end TO DO

            Update(bp.left);
            Update(bp.right);
            Remove(bp);

            return newBP;
        }
    }
}