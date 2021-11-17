using ImageHistogram.Histogram;
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

        public IEnumerable<SimilarityResponse> Compare(HistogramsRepresentation histogramsRepresentation1, HistogramsRepresentation histogramsRepresentation2)
        {
            var hsv = _comparers.Select(c => {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = c.Compare(histogramsRepresentation1.HsvHistogram, histogramsRepresentation2.HsvHistogram);
                response.Duration = stopwatch.Elapsed;
                response.Name = response.Name + ":HSV";
                return response;
                });

            var rgb = _comparers.Select(c => {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var response = c.Compare(histogramsRepresentation1.RgbHistogram, histogramsRepresentation2.RgbHistogram);
                response.Duration = stopwatch.Elapsed;
                response.Name = response.Name + ":RGB";
                return response;
            });

            return hsv.Concat(rgb);
        }
    }
}
