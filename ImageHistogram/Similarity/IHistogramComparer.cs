namespace ImageHistogram
{
    public interface IHistogramComparer
    {
        public SimilarityResponse Compare(double[] histogram1, double[] histogram2);
    }
}
