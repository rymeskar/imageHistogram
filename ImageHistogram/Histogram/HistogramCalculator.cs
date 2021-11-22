using ImageHistogram.Configuration;
using ImageHistogram.Histogram;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

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
            var pixelValueCount = 1 << 8;
            BinCount = options.Value.BinCount;

            if (BinCount >= pixelValueCount || pixelValueCount % BinCount != 0)
            {
                throw new ArgumentException($"{nameof(BinCount)} must divide {pixelValueCount}");
            }
            byteBinWidth = pixelValueCount / BinCount;
            BinCount3D = BinCount * BinCount * BinCount;
            BinCount2D = BinCount * BinCount;
            hBinWidth = 360.00000000001 / BinCount;
            sBinWidth = 1.000000000001 / BinCount;
            vBinWidth = 1.000000000001 / BinCount;
            convertor = new ColorSpaceConverter();
        }

        public async Task<HistogramsRepresentation> CalculateHistogramsAsync(Image<Rgba32> image)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var rgbTask = new Task<double[]>(() => CalculateStandardized(image,
                (pixel) => pixel, 
                (pixel) => pixel.R / byteBinWidth, 
                (pixel) => pixel.G / byteBinWidth, 
                (pixel) => pixel.B / byteBinWidth));
            var hsvTask = new Task<double[]>(() => CalculateStandardized(image,
                (pixel) => convertor.ToHsv(pixel),
                (pixel) => (int)(pixel.H / hBinWidth),
                (pixel) => (int)(pixel.S / sBinWidth),
                (pixel) => (int)(pixel.V / vBinWidth)));
            rgbTask.Start();
            hsvTask.Start();
            await Task.WhenAll(hsvTask, rgbTask);
            stopwatch.Stop();
            return new HistogramsRepresentation(rgbTask.Result, hsvTask.Result, stopwatch.Elapsed);
        }

        private int[] CalculateHistogram<T>(Image<Rgba32> image
            , Func<Rgba32, T> colorRepresentation
            , Func<T, int> first
            , Func<T, int> second
            , Func<T, int> third)
        {
            var histogram = new int[BinCount3D];


            for (var row = 0; row < image.Height; ++row)
            {
                foreach (var pixel in image.GetPixelRowSpan(row))
                {
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

            double total = image.Width * image.Height;
            for (var i = 0; i < intHistogram.Length; ++i)
            {
                histogram[i] = intHistogram[i] / total;
            }

            return histogram;
        }
    }
}
