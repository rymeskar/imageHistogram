using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ImageHistogram
{
    public class SimilarityAggregator
    {
        private readonly IEnumerable<IHistogramComparer> _comparers;

        public SimilarityAggregator(IEnumerable<IHistogramComparer> comparers)
        {
            _comparers = comparers ?? throw new ArgumentNullException(nameof(comparers));
        }

        public IEnumerable<SimilarityResponse> Compare(double[] histogram1, double[] histogram2)
        {
            return _comparers.Select(c => {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = c.Compare(histogram1, histogram2);
                response.Duration = stopwatch.Elapsed;
                return response;
                });
        }
    }
}
