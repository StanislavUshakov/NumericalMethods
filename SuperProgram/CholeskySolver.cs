using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cholesky
{
    class CholeskySolver
    {
        public static double[] Solve(double[,] A, double[] f)
        {
            int N = A.GetLength(0);
            double[,] B = new double[N, N];
            double[,] C = new double[N, N];
            int i, j, k;
            for (i = 0; i < N; ++i) //заполняем первый столбец В
                B[i, 0] = A[i, 0];
            for (j = 1; j < N; ++j) //заполняем первую строку С
                C[0, j] = A[0, j] / B[0, 0];
            C[0, 0] = 1;
            double sum;

            for (j = 1; j < N; ++j) //j=1, т.к. первый столбец заполнен
            {
                C[j, j] = 1; //главная диагональ
                //заполняем верхнюю часть С и нижнюю часть B
                for (i = 1; i < j; ++i) //i=1, т.к. первая строка заполнена
                {
                    sum = 0;
                    for (k = 0; k <= i - 1; ++k)
                        sum += B[i, k] * C[k, j];
                    C[i, j] = (A[i, j] - sum) / B[i, i];
                    B[i, j] = 0;
                }

                i = j; //этот случай считаем отдельно, чтобы не менять главную диагональ C
                sum = 0;
                for (k = 0; k <= i - 1; ++k)
                    sum += B[i, k] * C[k, j];
                B[i, i] = A[i, i] - sum;

                //заполняем верхнюю часть B и нижнюю часть C
                for (i = j + 1; i < N; ++i)
                {
                    sum = 0;
                    for (k = 0; k <= i - 1; ++k)
                        sum += B[i, k] * C[k, j];
                    B[i, j] = A[i, j] - sum;
                    C[i, j] = 0;
                }
            }

            // A*x == f
            // B*C*x == f
            // C*x == y     (1)
            // B*y == f     (2)
            double[] x = new double[N];
            double[] y = new double[N];
            // решаем (2)
            y[0] = f[0] / B[0, 0];
            for (i = 1; i < N; ++i)
            {
                sum = 0;
                for (k = 0; k <= i - 1; ++k)
                    sum += B[i, k] * y[k];
                y[i] = (f[i] - sum) / B[i, i];
            }

            // решаем (1)
            x[N - 1] = y[N - 1];
            for (i = N - 2; i >= 0; --i)
            {
                sum = 0;
                for (k = i + 1; k <= N - 1; ++k)
                    sum += C[i, k] * x[k];
                x[i] = y[i] - sum;
            }

            return x;
        }
    }
}
