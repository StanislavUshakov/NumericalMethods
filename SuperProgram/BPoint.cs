using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace Triangulation
{
    class BPoint
    {
        public Point point { get; set; }
        public double angle;
        public double jAngle;
        public double kAngle;
        public double lAngle { get; private set; }
        public double rAngle { get; private set; }
        public BPoint left { get; set; }
        public BPoint right { get; set; }
        public List<BPoint> badPoints; // list of BPoint's indexes inside the angle
        private static double eps = 0.001;

        public BPoint(Point vpoint)
        {
            point = vpoint;
            left = null;
            right = null;
            angle = 0;
            jAngle = 0;
            kAngle = 0;
            lAngle = 0;
            rAngle = 0;
            badPoints = new List<BPoint>();
        }

        public static double dist(Point p1, Point p2)
        {
            double res = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            return res;
        }

        public double updateAngles()
        {
            // right
            Point i = point;
            Point j = new Point(i.X + 10, i.Y);
            Point k = right.point;
            double ij = dist(i, j);
            double ik = dist(i, k);
            double jk = dist(j, k);
            double cosI = (ij * ij + ik * ik - jk * jk) / (ij * ik * 2);
            if (k.Y >= i.Y)
                rAngle = Math.Acos(cosI);
            else
                rAngle = Math.PI * 2 - Math.Acos(cosI);

            // left
            k = left.point;
            ik = dist(i, k);
            jk = dist(j, k);
            cosI = (ij * ij + ik * ik - jk * jk) / (ij * ik * 2);
            if (k.Y >= i.Y)
                lAngle = Math.Acos(cosI);
            else
                lAngle = Math.PI * 2  - Math.Acos(cosI);

            if (lAngle < rAngle)
                lAngle += Math.PI * 2;
            angle = lAngle - rAngle;

            // set neighbour angles
            j = right.point;
            ij = dist(i, j);
            jk = dist(j, k);
            double cosJ = (ij * ij + jk * jk - ik * ik) / (ij * jk * 2);
            jAngle = Math.Acos(cosJ);
            kAngle = Math.PI - angle - jAngle;

            return angle;
        }

        public bool equalPoints(Point p1, Point p2)
        {
            return (p1.X == p2.X) && (p1.Y == p2.Y);
        }

        public bool insideTriangle(Triangle tr)
        {
            if (equalPoints(point, tr.I) || equalPoints(point, tr.J) || equalPoints(point, tr.K))
                return false;

            Triangle trij = new Triangle(tr.I, tr.J, point);
            Triangle trik = new Triangle(tr.I, tr.K, point);
            Triangle trkj = new Triangle(tr.K, tr.J, point);

            bool res = (Math.Abs(tr.Square() - (trij.Square() + trik.Square() + trkj.Square()))  <  eps);
            return res;
        }
    }
}