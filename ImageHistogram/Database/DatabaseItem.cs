using ImageHistogram.Histogram;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace ImageHistogram
{
    public class DatabaseItem
    {
        public DatabaseItem(HistogramsRepresentation histogram, string path, string familiarName)
        {
            Histograms = histogram ?? throw new ArgumentNullException(nameof(histogram));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            FamiliarName = familiarName ?? throw new ArgumentNullException(nameof(familiarName));
        }

        public HistogramsRepresentation Histograms { get; }
        public string Path { get; }
        public string FamiliarName { get; }
    }
}
