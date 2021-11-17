using System;

namespace ImageHistogram
{
    public class SimilarityResponse
    {
        public SimilarityResponse(string name, double meaure)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Measure = meaure;
        }

        public string Name { get; set; }
        public double Measure { get; }

        public TimeSpan Duration { get; set; }
    }
}
