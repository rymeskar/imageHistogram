using MathNet.Numerics.LinearAlgebra;
using System;

namespace ImageHistogram
{
    public class Qfd : IHistogramComparer
    {
        private readonly string _name = nameof(Qfd);

        private static readonly Matrix<double> Metric = CalculateMetric();

        public SimilarityResponse Compare(double[] histogram1, double[] histogram2)
        {
            var hist1 = Matrix<double>.Build.Dense(Histogram.BinCount, 1, histogram1);
            var hist2 = Matrix<double>.Build.Dense(Histogram.BinCount, 1, histogram1);

            var diff = hist1 - hist2;

            var temp = diff * Metric * diff.Transpose();

            var total = temp[0, 0];

            return new SimilarityResponse(_name, total);
        }

        /// <summary>
        /// Source: https://openproceedings.org/2011/conf/edbt/SkopalBL11.pdf
        /// </summary>
        /// <returns></returns>
        private static Matrix<double> CalculateMetric()
        {
            var max = RepDistance(0, Histogram.BinCount3D - 1);
            var matrix = Matrix<double>.Build.Dense(Histogram.BinCount3D, Histogram.BinCount3D);
            for (var x = 0; x < Histogram.BinCount3D; ++x)
            {
                for (var y = 0; y < Histogram.BinCount3D; ++y)
                {
                    matrix.At(x, y, 1 - RepDistance(x, y) / max);
                }
            }

            return matrix;
        }

        private static double RepDistance(int i, int j)
        {
            var total = 0;
            for (var x = 0; x < 2; ++x)
            {
                var curI = i % Histogram.BinCount;
                var curJ = j % Histogram.BinCount;

                var dif = curI - curJ;
                total += dif * dif;

                i = i / Histogram.BinCount;
                j = j / Histogram.BinCount;
            }

            return Math.Sqrt(total);
        }
    }
}
