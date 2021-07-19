using Numpy;
using System;

namespace Pde
{
    class Program
    {
        record Parameters(double BarLength = 1, double BarDiameter = 0.025, double BarDensitiy = 8000, double CP = 400, double K = 50, double H = 20, double InfinitTempeture = 25, double deltaT = 0.01, double deltaX = 0.1);

        static void Main(string[] args)
        {
            var datasetPath = args[0];
            var x = Convert.ToUInt16(args[1]);
            var t = Convert.ToUInt16(args[2]);

            var JN = InitJNData();
            var data = new Parameters(); //LoadData(datasetPath);
            var tempExplicit = CalculatePdeExplicit(data, x, t);
            var tempImplicit = CalculatePdeImplicit(data, x, t);
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine($"Tempture in requested point and time is {tempExplicit}");
            Console.WriteLine($"Tempture in requested point and time is {tempImplicit}");
            Console.WriteLine("------------------------------------------------");

        }

        private static double CalculatePdeExplicit(Parameters parmeters, ushort x, ushort t)
        {
            var JN = InitJNData();
            var alpha = parmeters.K / (parmeters.BarDensitiy * parmeters.CP);
            var m = alpha * parmeters.deltaT / Math.Pow(parmeters.deltaX, 2);
            var mm = 4 * parmeters.H * alpha * parmeters.deltaT / (parmeters.K * parmeters.BarDiameter);
            for (int n = 0; n <= t; n++)
            {
                for (int j = 1; j < 10; j++)
                {
                    JN[j][n + 1] = JN[j][n] + (m * (JN[j + 1][n] - (2 * JN[j][n]) + JN[j - 1][n])) - (mm * (JN[j][n] - parmeters.InfinitTempeture));
                }
            }
            return JN[x][t];
        }

        private static double CalculatePdeImplicit(Parameters parmeters, ushort x, ushort t)
        {
            var JN = InitJNData();
            var alpha = parmeters.K / (parmeters.BarDensitiy * parmeters.CP);
            double a = (Math.Pow(parmeters.deltaX, 2) / alpha * parmeters.deltaT) + 2;
            double m = 4 * parmeters.H * Math.Pow(parmeters.deltaX, 2) / (parmeters.K * parmeters.BarDiameter);
            double mm = Math.Pow(parmeters.deltaX, 2) / alpha * parmeters.deltaT;
            for (int nn = 0; nn <= t; nn++)
            {
                double mmm = (m * (JN[1][nn] - parmeters.InfinitTempeture)) - (mm * JN[1][nn]);
                double d1 = mmm - 20;
                double d2 = mmm;
                double d3 = mmm - 100;
                var MatrxiA = np.array(new double[] {
            a, 1, 0, 0, 0, 0, 0, 0, 0,
            1, a, 1, 0, 0, 0, 0, 0, 0,
            0, 1, a, 1, 0, 0, 0, 0, 0,
            0, 0, 1, a, 1, 0, 0, 0, 0,
            0, 0, 0, 1, a, 1, 0, 0, 0,
            0, 0, 0, 0, 1, a, 1, 0, 0,
            0, 0, 0, 0, 0, 1, a, 1, 0,
            0, 0, 0, 0, 0, 0, 1, a, 0,
            0, 0, 0, 0, 0, 0, 0, 1, a,
            }).reshape(9, 9);
                var InvertedMatrixA = np.linalg.inv(MatrxiA);
                var MatrixB = np.array(new double[] { d1, d2, d2, d2, d2, d2, d2, d2, d3 }).reshape(9, 1);
                var res = np.matmul(InvertedMatrixA, MatrixB);
                for (int jj = 1; jj < 9; jj++)
                {
                    
                    JN[jj][nn+1] = res[jj].asscalar<double>();
                }
            }
            DebugJN(JN);
            return JN[x][t];
        }

        private static Double[][] InitJNData()
        {
            var data = new double[11][];
            data[0] = new double[101];
            for (int j = 1; j <= 10; j++)
            {
                data[j] = new double[101];
                data[j][0] = 200;
            }
            for (int n = 1; n <= 100; n++)
            {
                data[0][n] = 20;
            }
            for (int n = 1; n <= 100; n++)
            {
                data[10][n] = 100;
            }
            data[0][0] = 20;
            data[10][0] = 100;
            DebugJN(data);
            return data;
        }

        private static void DebugJN(double[][] data)
        {
            Console.WriteLine("############################################");
            for (int j = 0; j < data.Length; j++)
            {
                Console.WriteLine();
                for (int n = 0; n <= 100; n++)
                {
                    Console.Write($"[{data[j][n]}]");
                }
                Console.WriteLine("############################################");
            }
        }

        private static object LoadData(string path)
        {
            throw new NotImplementedException();
        }
    }
}
