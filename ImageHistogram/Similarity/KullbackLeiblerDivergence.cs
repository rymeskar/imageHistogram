using System;

namespace ImageHistogram
{
    public class KullbackLeiblerDivergence : IHistogramComparer
    {
        private readonly string _name = nameof(KullbackLeiblerDivergence);
        public SimilarityResponse Compare(double[] histogram1, double[] histogram2)
        {
            double total = 0.0;
            for (var i = 0; i < histogram1.Length; ++i)
            {
                // TODO: why?
                if (histogram2[i] ==  0)
                {
                    continue;
                }

                var val = histogram1[i]*Math.Log(histogram1[i]/histogram2[i]);
                total += val;
            }

            return new SimilarityResponse(_name, total);
        }
    }
}
