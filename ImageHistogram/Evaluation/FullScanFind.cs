using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;

namespace ImageHistogram.Evaluation
{
    public class FullScanFind
    {
        private readonly ImageDatabase database;
        private readonly SimilarityAggregator similarities;
        private readonly Histogram histogram;

        public FullScanFind(ImageDatabase database, SimilarityAggregator similarities, Histogram histogram)
        {
            this.database = database;
            this.similarities = similarities;
            this.histogram = histogram;
        }

        public  EvaluationResponse FindMostSimilar(Image<Rgba32> image)
        {
            var histogramArray = histogram.CalculateStandardized(image);

            var similarityBag = new SimilarityBag();
            foreach (var item in database.Items)
            {
                var response = similarities.Compare(histogramArray, item.StandardizeHistogram);

                similarityBag.AddRange(response, item);
            }

            return similarityBag.EvaluateBest();
        }
    }
}
