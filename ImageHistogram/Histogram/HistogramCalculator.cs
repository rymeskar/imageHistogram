using ImageHistogram.Configuration;
using ImageHistogram.Histogram;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace ImageHistogram
{
    public class HistogramCalculator
    {
        public int BinCount { get; }
        private int byteBinWidth;
        public int BinCount3D { get; } 
        public int BinCount2D { get; }

        private readonly double hBinWidth;
        private readonly double sBinWidth;
        private readonly double vBinWidth;
        private readonly ColorSpaceConverter convertor;

        public HistogramCalculator(IOptions<HistogramOptions> options)
        {
            BinCount = options.Value.BinCount;
            byteBinWidth = (1 << 8) / BinCount;
            BinCount3D = BinCount * BinCount * BinCount;
            BinCount2D = BinCount * BinCount;
            hBinWidth = 360.00000000001 / BinCount;
            sBinWidth = 1.000000000001 / BinCount;
            vBinWidth = 1.000000000001 / BinCount;
            convertor = new ColorSpaceConverter();
        }

        public HistogramsRepresentation CalculateHistograms(Image<Rgba32> image)
        {
            var rgb = CalculateStandardized(image,
                (pixel) => pixel, 
                (pixel) => pixel.R / byteBinWidth, 
                (pixel) => pixel.G / byteBinWidth, 
                (pixel) => pixel.B / byteBinWidth);
            var hsv = CalculateStandardized(image,
                (pixel) => convertor.ToHsv(pixel),
                (pixel) => (int)(pixel.H / hBinWidth),
                (pixel) => (int)(pixel.S / sBinWidth),
                (pixel) => (int)(pixel.V / vBinWidth));

            return new HistogramsRepresentation(rgb, hsv);
        }

        private int[] CalculateHistogram<T>(Image<Rgba32> image
            , Func<Rgba32, T> colorRepresentation
            , Func<T, int> first
            , Func<T, int> second
            , Func<T, int> third)
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
                    var colorPixel = colorRepresentation(pixel);
                    var r = first(colorPixel);
                    var g = second(colorPixel);
                    var b = third(colorPixel);
                    histogram[r + BinCount * g + BinCount2D * b]++;
                }
            }

            return histogram;
        }

        private double[] CalculateStandardized<T>(Image<Rgba32> image
            , Func<Rgba32, T> colorRepresentation
            , Func<T, int> first
            , Func<T, int> second
            , Func<T, int> third)
        {
            var intHistogram = CalculateHistogram(image, colorRepresentation, first, second, third);
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
