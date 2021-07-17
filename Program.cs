using System;

namespace Pde
{
    class Program
    {
        record Parameters(int BarLength = 1, float BarDiameter = 0.025f, int BarDensitiy = 8000, int CP = 400, int K = 50, int H = 20, int Tempeture = 25,float deltaT=0.01f,float deltaX=0.1f);

        static void Main(string[] args)
        {
            var datasetPath = args[0];
            var x = Convert.ToUInt16(args[1]);
            var t = Convert.ToUInt16(args[2]);

            var JN = InitJNData();
            var data = new Parameters(); //LoadData(datasetPath);
            var temp = CalculatePde(data, JN, x, t);
        }

        private static object CalculatePde(Parameters data, ushort[][] jN, ushort x, ushort t)
        {
            var T = jN;
            var alpha = data.K / (data.BarDensitiy * data.CP);
            var m = alpha * data.deltaT / Math.Pow(data.deltaX, 2);
            for (int n = 0; n <= t; n++)
            {
                for (int j = 1; j < 10; j++)
                {
                    T[j][n + 1] = T[j][n] + m * (T[j + 1][n] - 2 * T[j][n] + T[j - 1][n]);
                }
            }
        }

        private static UInt16[][] InitJNData()
        {
            var data = new UInt16[11][];
            data[0] = new UInt16[101];
            for (int j = 1; j <= 10; j++)
            {
                data[j] = new UInt16[101];
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

        private static void DebugJN(ushort[][] data)
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
