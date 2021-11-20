using System;

namespace ImageHistogram.Histogram
{
    public class HistogramsRepresentation
    {
        public HistogramsRepresentation(double[] rgbHistogram, double[] hsvHistogram, TimeSpan calcuationDuration)
        {
            RgbHistogram = rgbHistogram ?? throw new ArgumentNullException(nameof(rgbHistogram));
            HsvHistogram = hsvHistogram ?? throw new ArgumentNullException(nameof(hsvHistogram));
            CalcuationDuration = calcuationDuration;
        }

        public double[] RgbHistogram { get; }
        public double[] HsvHistogram { get; }
        public TimeSpan CalcuationDuration { get; }
    }
}
