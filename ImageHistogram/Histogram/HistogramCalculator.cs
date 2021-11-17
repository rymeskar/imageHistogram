using ImageHistogram.Configuration;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageHistogram
{
    public class HistogramCalculator
    {
        public int BinCount { get; }
        public int BinWidth { get; } 
        public int BinCount3D { get; } 
        public int BinCount2D { get; }
        public HistogramCalculator(IOptions<HistogramOptions> options)
        {
            BinCount = options.Value.BinCount;
            BinWidth = (1 << 8) / BinCount;
            BinCount3D = BinCount * BinCount * BinCount;
            BinCount2D = BinCount * BinCount;
        }

        public int[] Calculate(Image<Rgba32> image)
        {
            var histogram = new int[BinCount3D];
            for (var i = 0; i < BinCount3D; i++)
            {
                histogram[i] = 0;
            }

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y < image.Height; y++)
                {
                    var pixel = image[x, y];
                    var r = pixel.R / BinWidth;
                    var g = pixel.G / BinWidth;
                    var b = pixel.B / BinWidth;
                    histogram[r + BinCount * g + BinCount2D * b]++;
                }
            }

            return histogram;
        }

        public double[] CalculateStandardized(Image<Rgba32> image)
        {
            var intHistogram = Calculate(image);
            var histogram = new double[BinCount3D];

            var total = image.Width * image.Height;
            for (var i = 0; i < intHistogram.Length; ++i)
            {
                histogram[i] = intHistogram[i] / (double)total;
            }

            return histogram;
        }
    }
}
