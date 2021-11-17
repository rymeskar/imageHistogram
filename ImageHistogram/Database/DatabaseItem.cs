using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace ImageHistogram
{
    public class DatabaseItem
    {
        public DatabaseItem(double[] standardizeHistogram, int[] histogram, string pathName, string familiarName)
        {
            StandardizeHistogram = standardizeHistogram ?? throw new ArgumentNullException(nameof(standardizeHistogram));
            Histogram = histogram ?? throw new ArgumentNullException(nameof(histogram));
            PathName = pathName ?? throw new ArgumentNullException(nameof(pathName));
            FamiliarName = familiarName ?? throw new ArgumentNullException(nameof(familiarName));
        }

        public double[] StandardizeHistogram { get; }
        public int[] Histogram { get; }
        public string PathName { get; }
        public string FamiliarName { get; }
    }
}
