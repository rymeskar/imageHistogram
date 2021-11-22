using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageHistogram.Evaluation
{
    public class FullScanFind
    {
        private readonly ImageDatabase database;
        private readonly SimilarityAggregator similarities;
        private readonly HistogramCalculator histogram;

        public FullScanFind(ImageDatabase database, SimilarityAggregator similarities, HistogramCalculator histogram)
        {
            this.database = database;
            this.similarities = similarities;
            this.histogram = histogram;
        }

        public async Task<EvaluationResponse> FindMostSimilarAsync(Image<Rgba32> image)
        {
            var histograms = await histogram.CalculateHistogramsAsync(image);

            var similarityBag = new SimilarityBag();

            var tasks = new List<Task>();
            foreach (var item in database.Items)
            {
                var task = new Task(() =>
                {
                    var response = similarities.Compare(histograms, item.Histograms);

                    similarityBag.AddRange(response, item);
                });
                task.Start();
                tasks.Add(task);
                
            }

            await Task.WhenAll(tasks);
            var retVal = similarityBag.EvaluateBest();

            retVal.HistogramTime = histograms.CalcuationDuration;
            return retVal;
        }
    }
}
