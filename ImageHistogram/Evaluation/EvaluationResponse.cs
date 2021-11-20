using System;
using System.Collections.Generic;

namespace ImageHistogram.Evaluation
{
    public class EvaluationResponse
    {
        public EvaluationResponse(List<SimilarityMeasureEvaluation> evaluations)
        {
            Evaluations = evaluations ?? throw new ArgumentNullException(nameof(evaluations));
        }

        public List<SimilarityMeasureEvaluation> Evaluations { get; }

        public TimeSpan HistogramTime { get; set;  }
    }
}
