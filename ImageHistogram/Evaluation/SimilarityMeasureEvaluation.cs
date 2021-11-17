using System;

namespace ImageHistogram.Evaluation
{
    public class SimilarityMeasureEvaluation
    {
        public SimilarityMeasureEvaluation(SimilarityHit hit, TimeSpan duration)
        {
            Hit = hit ?? throw new ArgumentNullException(nameof(hit));
            Duration = duration;
        }

        public SimilarityHit Hit { get; }
        public TimeSpan Duration { get; }
    }
}
