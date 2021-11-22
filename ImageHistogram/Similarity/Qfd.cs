using MathNet.Numerics.LinearAlgebra;
using System;

namespace ImageHistogram
{
    public class Qfd : IHistogramComparer
    {
        private readonly string _name = nameof(Qfd);

        private readonly Matrix<double> Metric;
        private readonly HistogramCalculator histogramCalculator;

        public Qfd(HistogramCalculator histogramCalculator)
        {
            this.histogramCalculator = histogramCalculator;
            Metric = CalculateMetric();
        }
        public SimilarityResponse Compare(double[] histogram1, double[] histogram2)
        {
            // TODO: number with x digits. Tranform to markdown table
            //var a = Metric.ToMatrixString(30, 30);
            var hist1 = Matrix<double>.Build.Dense(1, histogramCalculator.BinCount3D, histogram1);
            var hist2 = Matrix<double>.Build.Dense(1, histogramCalculator.BinCount3D, histogram2);

            var diff = hist1 - hist2;

            var temp = diff * Metric * diff.Transpose();

            var total = temp[0, 0];

            return new SimilarityResponse(_name, total);
        }

        /// <summary>
        /// Source: https://openproceedings.org/2011/conf/edbt/SkopalBL11.pdf
        /// </summary>
        /// <returns></returns>
        private Matrix<double> CalculateMetric()
        {
            var max = RepDistance(0, histogramCalculator.BinCount3D - 1);
            var matrix = Matrix<double>.Build.Dense(histogramCalculator.BinCount3D, histogramCalculator.BinCount3D);
            for (var x = 0; x < histogramCalculator.BinCount3D; ++x)
            {
                for (var y = 0; y < histogramCalculator.BinCount3D; ++y)
                {
                    matrix.At(x, y, 1 - RepDistance(x, y) / max);
                }
            }

            return matrix;
        }

        private double RepDistance(int i, int j)
        {
            var total = 0;
            for (var x = 0; x < 2; ++x)
            {
                var curI = i % histogramCalculator.BinCount;
                var curJ = j % histogramCalculator.BinCount;

                var dif = curI - curJ;
                total += dif * dif;

                i = i / histogramCalculator.BinCount;
                j = j / histogramCalculator.BinCount;
            }

            return Math.Sqrt(total);
        }
    }
}
