using System;

namespace ImageHistogram.Evaluation
{
    public class SimilarityHit
    {
        public SimilarityHit(SimilarityResponse response, DatabaseItem item)
        {
            this.response = response;
            this.item = item ?? throw new ArgumentNullException(nameof(item));
        }

        public SimilarityResponse response { get; }
        public DatabaseItem item { get; }
    }
}
