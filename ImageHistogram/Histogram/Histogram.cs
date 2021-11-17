using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageHistogram
{
    public class Histogram
    {
        public const int BinCount = 16;
        public static readonly int BinWidth = (1<<8) / BinCount;
        public static readonly int BinCount3D = BinCount * BinCount * BinCount;
        public static readonly int BinCount2D = BinCount * BinCount;
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
