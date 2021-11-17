using System;

namespace ImageHistogram.Histogram
{
    public class HistogramsRepresentation
    {
        public HistogramsRepresentation(double[] rgbHistogram, double[] hsvHistogram)
        {
            RgbHistogram = rgbHistogram ?? throw new ArgumentNullException(nameof(rgbHistogram));
            HsvHistogram = hsvHistogram ?? throw new ArgumentNullException(nameof(hsvHistogram));
        }

        public double[] RgbHistogram { get; }
        public double[] HsvHistogram { get; }
    }
}
