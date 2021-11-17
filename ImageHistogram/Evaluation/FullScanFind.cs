﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;

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

        public  EvaluationResponse FindMostSimilar(Image<Rgba32> image)
        {
            var histograms = histogram.CalculateHistograms(image);

            var similarityBag = new SimilarityBag();
            foreach (var item in database.Items)
            {
                var response = similarities.Compare(histograms, item.Histograms);

                similarityBag.AddRange(response, item);
            }

            return similarityBag.EvaluateBest();
        }
    }
}
