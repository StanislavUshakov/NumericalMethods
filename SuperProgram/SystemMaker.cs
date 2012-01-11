using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace SuperProgram
{
    static class SystemMaker
    {
        //sets in matrix and vector the system, that must be solved by Halectiy
        //matrix and vector must be created and initialized 0 before this executing this method.
        public static void MakeSystem(ref List<Triangle> triangles, ref Contour contour, ref double[,] matrix, ref double[] vector)
        {

            double[,] K = new double[3, 3];
            double[] F = new double[3];
            int i, j;
            double denominator_K; //4 * square of triangle
            double koefficient_line_segment; //length of line segment, devided on something

            foreach (Triangle triangle in triangles)
            {
                //-------------------make matrix K-------------------------------------------------------------

                K[0, 0] = 0.0; K[0, 1] = 0.0; K[0, 2] = 0.0;
                K[1, 0] = 0.0; K[1, 1] = 0.0; K[1, 2] = 0.0;
                K[2, 0] = 0.0; K[2, 1] = 0.0; K[2, 2] = 0.0;

                denominator_K = 4.0 * triangle.Square();

                K[0, 0] = ((triangle.J.Y - triangle.K.Y) * (triangle.J.Y - triangle.K.Y) +
                           (triangle.K.X - triangle.J.X) * (triangle.K.X - triangle.J.X)) / denominator_K;

                K[1, 1] = ((triangle.K.Y - triangle.J.Y) * (triangle.K.Y - triangle.J.Y) +
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
                        koefficient_line_segment = triangle.IJ.Length / 3.0;

                        K[0, 0] += koefficient_line_segment;  // (length of IJ) / 3
                        K[1, 1] += koefficient_line_segment;  // (length of IJ) / 3

                        koefficient_line_segment = triangle.IJ.Length / 6.0;

                        K[0, 1] += koefficient_line_segment;  // (length of IJ) / 6
                        K[1, 0] += koefficient_line_segment;  // (length of IJ) / 6

                        koefficient_line_segment = triangle.IJ.Length / 2.0;

                        F[0] += koefficient_line_segment;  // (length of IJ) / 2
                        F[1] += koefficient_line_segment;  // (length of IJ) / 2
                    }

                    if (contour.Contains(triangle.K)) //if KI
                    {
                        koefficient_line_segment = triangle.KI.Length / 3.0;

                        K[0, 0] += koefficient_line_segment;  //(length of KI) / 3
                        K[2, 2] += koefficient_line_segment;  //(length of KI) / 3

                        koefficient_line_segment = triangle.KI.Length / 6.0;

                        K[0, 2] += koefficient_line_segment;  //(length of KI) / 6
                        K[2, 0] += koefficient_line_segment;  //(length of KI) / 6

                        koefficient_line_segment = triangle.KI.Length / 2.0;

                        F[0] += koefficient_line_segment;  //(length of KI) / 2
                        F[2] += koefficient_line_segment;  //(length of KI) / 2
                    }
                }

                if (contour.Contains(triangle.J) && contour.Contains(triangle.K))  //if JK
                {
                    koefficient_line_segment = triangle.JK.Length / 3.0;

                    K[1, 1] += koefficient_line_segment;  //(length of JK) / 3
                    K[2, 2] += koefficient_line_segment;  //(length of JK) / 3

                    koefficient_line_segment = triangle.JK.Length / 6.0;

                    K[1, 2] += koefficient_line_segment;  //(length of JK) / 6
                    K[2, 1] += koefficient_line_segment;  //(length of JK) / 6

                    koefficient_line_segment = triangle.JK.Length / 2.0;

                    F[1] += koefficient_line_segment;  //(length of JK) / 2
                    F[2] += koefficient_line_segment;  //(length of JK) / 2
                }

                //--------------------------------------------------------------------------------------------------


                //-------------------------------------add matrix K in global matrix K-------------------------------

                matrix[triangle.I.Index][triangle.I.Index] += K[0, 0];  //K[i,i]
                matrix[triangle.I.Index][triangle.J.Index] += K[0, 1];  //K[i,j]
                matrix[triangle.I.Index][triangle.K.Index] += K[0, 2];  //K[i,k]

                matrix[triangle.J.Index][triangle.I.Index] += K[1, 0];  //K[j,i]
                matrix[triangle.J.Index][triangle.J.Index] += K[1, 1];  //K[j,j]
                matrix[triangle.J.Index][triangle.K.Index] += K[1, 2];  //K[j,k]

                matrix[triangle.K.Index][triangle.I.Index] += K[2, 0];  //K[k,i]
                matrix[triangle.K.Index][triangle.J.Index] += K[2, 1];  //K[k,j]
                matrix[triangle.K.Index][triangle.K.Index] += K[2, 2];  //K[k,k]

                //---------------------------------------------------------------------------------------------------
  
                //----------------------------add vektor F in global vektor F----------------------------------------

                vector[triangle.I.Index] += F[0];  //F[i]
                vector[triangle.J.Index] += F[1];  //F[j]
                vector[triangle.K.Index] += F[2];  //F[k]

                //---------------------------------------------------------------------------------------------------


            }//end of foreach

        }
    }
}
