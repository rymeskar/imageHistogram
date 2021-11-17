using System;
using System.Linq;

namespace ImageHistogram
{
    public class MinkowskiDistance : IHistogramComparer
    {

        private readonly int _degree;

        private readonly string _name;

        public MinkowskiDistance(int degree)
        {
            _degree = degree;
            _name = nameof(MinkowskiDistance) + _degree;
        }

        public SimilarityResponse Compare(double[] histogram1, double[] histogram2)
        {
            double total = 0.0;
            for (var i = 0; i < histogram1.Length; ++i)
            {
                var abs = Math.Abs(histogram1[i] - histogram2[i]);
                var exped = Math.Pow(abs, _degree);
                total += exped;
            }

            var retVal = Math.Pow(total, 1.0 / _degree);
            return new SimilarityResponse(_name, retVal);
        }
    }
}
