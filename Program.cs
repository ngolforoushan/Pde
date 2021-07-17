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
            var temp = CalculatePde(data, JN, x, t);
        }

        private static double CalculatePde(Parameters data, double[][] t, ushort x, ushort y)
        {
            var alpha = data.K / (data.BarDensitiy * data.CP);
            var m = alpha * data.deltaT / Math.Pow(data.deltaX, 2);
            var mm = 4 * data.H * alpha * data.deltaT / (data.K * data.BarDiameter);
            for (int n = 0; n <= y; n++)
            {
                for (int j = 1; j < 10; j++)
                {
                    t[j][n + 1] = t[j][n] + (m * (t[j + 1][n] - (2 * t[j][n]) + t[j - 1][n]))-(mm*(t[j][n]-data.InfinitTempeture));
                }
            }
            return t[x][y];
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
            for (int j = 0; j < data.Length; j++)
            {
                Console.WriteLine();
                for (int n = 0; n <= 100; n++)
                {
                    Console.Write($"[{data[j][n]}]");
                }
            }
        }

        private static object LoadData(string path)
        {
            throw new NotImplementedException();
        }
    }
}
