using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace SuperProgram
{
    static class SystemMaker
    {
        //sets in matrix and vector the system, that must be solved by Cholesky
        //matrix and vector must be created and initialized 0 before this executing this method.
        public static void MakeSystem(ref List<Triangle> triangles, ref Contour contour, ref double[,] matrix, ref double[] vector)
        {

            double[,] K = new double[3, 3];
            double[] F = new double[3];

            double denominator_K; //4 * square of triangle
            double koefficient_line_segment; //length of line segment, devided on something
            int points_index_difference; //the difference of 2 points indexes: used to check are 2 point neighbors on the contour
            
            int last_point_index_in_contour = 0;
            foreach (Point point in contour)
                if (point.Index > last_point_index_in_contour)
                    last_point_index_in_contour = point.Index;

            foreach (Triangle triangle in triangles)
            {
                //-------------------make matrix K-------------------------------------------------------------

                K[0, 0] = 0.0; K[0, 1] = 0.0; K[0, 2] = 0.0;
                K[1, 0] = 0.0; K[1, 1] = 0.0; K[1, 2] = 0.0;
                K[2, 0] = 0.0; K[2, 1] = 0.0; K[2, 2] = 0.0;

                denominator_K = 4.0 * triangle.Square();

                K[0, 0] = ((triangle.J.Y - triangle.K.Y) * (triangle.J.Y - triangle.K.Y) +
                           (triangle.K.X - triangle.J.X) * (triangle.K.X - triangle.J.X)) / denominator_K;

                K[1, 1] = ((triangle.K.Y - triangle.I.Y) * (triangle.K.Y - triangle.I.Y) +
                           (triangle.I.X - triangle.K.X) * (triangle.I.X - triangle.K.X)) / denominator_K;

                K[2, 2] = ((triangle.I.Y - triangle.J.Y) * (triangle.I.Y - triangle.J.Y) +
                           (triangle.J.X - triangle.I.X) * (triangle.J.X - triangle.I.X)) / denominator_K;

                K[0, 1] = ((triangle.J.Y - triangle.K.Y) * (triangle.K.Y - triangle.I.Y) +
                           (triangle.K.X - triangle.J.X) * (triangle.I.X - triangle.K.X)) / denominator_K;

                K[0, 2] = ((triangle.J.Y - triangle.K.Y) * (triangle.I.Y - triangle.J.Y) +
                           (triangle.K.X - triangle.J.X) * (triangle.J.X - triangle.I.X)) / denominator_K;

                K[1, 2] = ((triangle.K.Y - triangle.I.Y) * (triangle.I.Y - triangle.J.Y) +
                           (triangle.I.X - triangle.K.X) * (triangle.J.X - triangle.I.X)) / denominator_K;

                K[1, 0] = K[0, 1];

                K[2, 0] = K[0, 2];

                K[2, 1] = K[1, 2];

                //--------------------------------------------------------------------------------------------------



                //------------------------------make matrix K_with_wave and vektor F---------------------------------

                F[0] = 0.0;
                F[1] = 0.0;
                F[2] = 0.0;

                if (contour.Contains(triangle.I))
                {
                    if (contour.Contains(triangle.J)) //if IJ
                    {
                        points_index_difference = contour.FindIndex( p => p.Index == triangle.I.Index)
                            - contour.FindIndex( p => p.Index == triangle.J.Index);
                        if ((points_index_difference == 1) || (points_index_difference == -1) 
                            || (points_index_difference == contour.Count - 1))
                        {// if I and J are neighbors
                            koefficient_line_segment = triangle.IJ.Length / 3.0;

                            K[0, 0] += koefficient_line_segment;  // (length of IJ) / 3
                            K[1, 1] += koefficient_line_segment;  // (length of IJ) / 3

                            koefficient_line_segment = triangle.IJ.Length / 6.0;

                            K[0, 1] += koefficient_line_segment;  // (length of IJ) / 6
                            K[1, 0] += koefficient_line_segment;  // (length of IJ) / 6

                            koefficient_line_segment = (triangle.I.T + triangle.J.T) * triangle.IJ.Length / 4.0;
                            double f0 = FindKoefF(triangle.I, triangle.I, triangle.J, triangle);
                            double f1 = FindKoefF(triangle.J, triangle.I, triangle.J, triangle);
                            F[0] += f0;  
                            F[1] += f1;  
                        }
                    }

                    if (contour.Contains(triangle.K)) //if KI
                    {
                        points_index_difference = contour.FindIndex(p => p.Index == triangle.I.Index)
                            - contour.FindIndex(p => p.Index == triangle.K.Index);
                        if ((points_index_difference == 1) || (points_index_difference == -1)
                            || (points_index_difference == contour.Count - 1))
                        {// if I and K are neighbors
                            koefficient_line_segment = triangle.KI.Length / 3.0;

                            K[0, 0] += koefficient_line_segment;  //(length of KI) / 3
                            K[2, 2] += koefficient_line_segment;  //(length of KI) / 3

                            koefficient_line_segment = triangle.KI.Length / 6.0;

                            K[0, 2] += koefficient_line_segment;  //(length of KI) / 6
                            K[2, 0] += koefficient_line_segment;  //(length of KI) / 6

                            koefficient_line_segment = (triangle.I.T + triangle.K.T) * triangle.KI.Length / 4.0;
                            double f0 = FindKoefF(triangle.I, triangle.K, triangle.I, triangle);
                            double f2 = FindKoefF(triangle.K, triangle.K, triangle.I, triangle);
                            F[0] += f0;  
                            F[2] += f2;  
                        }
                    }
                }

                if (contour.Contains(triangle.J) && contour.Contains(triangle.K))  //if JK
                {
                    points_index_difference = contour.FindIndex(p => p.Index == triangle.K.Index)
                            - contour.FindIndex(p => p.Index == triangle.J.Index);
                    if ((points_index_difference == 1) || (points_index_difference == -1)
                        || (points_index_difference == contour.Count - 1))                    
                    {// if K and J are neighbors
                        koefficient_line_segment = triangle.JK.Length / 3.0;

                        K[1, 1] += koefficient_line_segment;  //(length of JK) / 3
                        K[2, 2] += koefficient_line_segment;  //(length of JK) / 3

                        koefficient_line_segment = triangle.JK.Length / 6.0;

                        K[1, 2] += koefficient_line_segment;  //(length of JK) / 6
                        K[2, 1] += koefficient_line_segment;  //(length of JK) / 6

                        koefficient_line_segment = (triangle.J.T + triangle.K.T) * triangle.JK.Length / 4.0;
                        double f1 = FindKoefF(triangle.J, triangle.J, triangle.K, triangle);
                        double f2 = FindKoefF(triangle.K, triangle.J, triangle.K, triangle);
                        F[1] += f1;  
                        F[2] += f2;  
                    }
                }

                //--------------------------------------------------------------------------------------------------


                //-------------------------------------add matrix K in global matrix K-------------------------------

                matrix[triangle.I.Index - 1, triangle.I.Index - 1] += K[0, 0];  //K[i,i]
                matrix[triangle.I.Index - 1, triangle.J.Index - 1] += K[0, 1];  //K[i,j]
                matrix[triangle.I.Index - 1, triangle.K.Index - 1] += K[0, 2];  //K[i,k]

                matrix[triangle.J.Index - 1, triangle.I.Index - 1] += K[1, 0];  //K[j,i]
                matrix[triangle.J.Index - 1, triangle.J.Index - 1] += K[1, 1];  //K[j,j]
                matrix[triangle.J.Index - 1, triangle.K.Index - 1] += K[1, 2];  //K[j,k]

                matrix[triangle.K.Index - 1, triangle.I.Index - 1] += K[2, 0];  //K[k,i]
                matrix[triangle.K.Index - 1, triangle.J.Index - 1] += K[2, 1];  //K[k,j]
                matrix[triangle.K.Index - 1, triangle.K.Index - 1] += K[2, 2];  //K[k,k]

                //---------------------------------------------------------------------------------------------------
  
                //----------------------------add vektor F in global vektor F----------------------------------------

                vector[triangle.I.Index - 1] += F[0];  //F[i]
                vector[triangle.J.Index - 1] += F[1];  //F[j]
                vector[triangle.K.Index - 1] += F[2];  //F[k]

                //---------------------------------------------------------------------------------------------------
                
            }//end of foreach
        }

        static private void FindABC(Point j, Point k, ref double a, ref double b, ref double c)
        {
            a = j.X * k.Y - k.X * j.Y;
            b = j.Y - k.Y;
            c = -j.X + k.X;
        }

        static private double FindKoefF(Point inPoint, Point i, Point j, Triangle triangle)
        {
            double deltaX = j.X - i.X;
            double deltaY = j.Y - i.Y;
            double deltaT = j.T - i.T;
            double a = 0, b = 0, c = 0;
            double length = Math.Sqrt( (i.X - j.X) * (i.X - j.X) + (i.Y - j.Y) * (i.Y - j.Y) );
            
            Point jToFindL, kToFindL;
            if (inPoint.Index == triangle.I.Index)
            {
                jToFindL = triangle.J;
                kToFindL = triangle.K;
            }
            else if (inPoint.Index == triangle.J.Index)
            {
                jToFindL = triangle.K;
                kToFindL = triangle.I;
            }
            else
            {
                jToFindL = triangle.I;
                kToFindL = triangle.J;
            }

            FindABC(jToFindL, kToFindL, ref a, ref b, ref c);

            double Ni = a + b * i.X + c * i.Y;
            double koeftBeforeT = b * deltaX + c * deltaY;

            double res = length / (2.0 * triangle.Square()) *
                ( Ni * i.T + (i.T * koeftBeforeT + Ni * deltaT) / 2.0 + (deltaT * koeftBeforeT / 3.0) );
            return res;
        }
    }
}
