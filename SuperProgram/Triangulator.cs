using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using System.Windows.Forms;

namespace Triangulation
{
    public class Triangulator
    {
        List<BPoint> initialBoundList = new List<BPoint>(); // unsorted
        BoundList boundList = new BoundList();
        public static int pointCount = 0;

        public IEnumerable<Triangle> Triangulate(IEnumerable<Point> pointList)
        {
            initialBoundList.Clear();
            boundList.Clear();
            List<Triangle> triangleList = new List<Triangle>();
            triangleList.Clear();
            pointCount = (pointList as List<Point>).Count();

            // init initialBoundList
            foreach (Point p in pointList)
            {
                BPoint bp = new BPoint(p);
                initialBoundList.Add(bp);
            }

            // clean the pointList
            (pointList as List<Point>).Clear();

            // set left and right
            initialBoundList[0].left = initialBoundList[pointCount - 1];
            initialBoundList[0].right = initialBoundList[1];
            initialBoundList[pointCount - 1].left = initialBoundList[pointCount - 2];
            initialBoundList[pointCount - 1].right = initialBoundList[0];
            for (int i = 1; i < pointCount - 1; i++)
            {
                initialBoundList[i].right = initialBoundList[i + 1];
                initialBoundList[i].left = initialBoundList[i - 1];
            }

            // init sortedList
            foreach (BPoint bp in initialBoundList)
            {
                boundList.Add(bp);
            }

            // foreach BPoint find and set list of BadPoints
            // we take a point and check for all triangles if it is inside
            for (int t = 0; t < pointCount; t++)
            {
                boundList.marksTriangleBad(boundList[t]);
            }

            // while there is more than 1 triangle
            while (boundList.Count() > 3)
            {
                int i = 0;
                while ((i < boundList.Count) && ((boundList[i].eBad) || (boundList[i].badPoints.Count() > 0)))
                    i++;

                if (i == boundList.Count)
                {
                    MessageBox.Show("Ошибка триангуляции. Невозможно выбрать точку для выемки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (triangleList);
                }

                // work with the i node
                double ang = boundList[i].angle;
                if ((ang < Math.PI * 5 / 12) ||
                    ((ang < Math.PI / 2 - 0.001) && (boundList[i].jAngle > Math.PI / 6) && (boundList[i].kAngle > Math.PI / 6)))
                {
                    // cut
                    BPoint it = boundList[i];
                    BPoint jt = boundList[i].right;
                    BPoint kt = boundList[i].left;
                    (pointList as List<Point>).Add(it.point);

                    Triangle newTr = new Triangle(it.point, jt.point, kt.point);
                    triangleList.Add(newTr);
                    boundList.Cut(it);
                }
                else
                {
                    // new point
                    BPoint it = boundList[i];
                    BPoint jt = boundList[i].right;
                    BPoint kt = boundList[i].left;
                    (pointList as List<Point>).Add(it.point);

                    BPoint nt = boundList.Excavation(it);
                    Triangle newTrJ = new Triangle(it.point, jt.point, nt.point);
                    Triangle newTrK = new Triangle(it.point, nt.point, kt.point);
                    triangleList.Add(newTrJ);
                    triangleList.Add(newTrK);
                }
            }
            // set the last triangle and the points
            BPoint i0 = boundList[0];
            BPoint j0 = i0.right;
            BPoint k0 = i0.left;

            (pointList as List<Point>).Add(i0.point);
            (pointList as List<Point>).Add(j0.point);
            (pointList as List<Point>).Add(k0.point);
            Triangle newTr0 = new Triangle(i0.point, j0.point, k0.point);
            triangleList.Add(newTr0);

            return triangleList;
        }
    }
}
