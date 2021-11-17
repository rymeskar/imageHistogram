using ImageHistogram.Configuration;
using ImageHistogram.Database;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageHistogram
{
    public class DatabaseInitializer
    {
        private readonly PictureDbOptions _config;
        private readonly HistogramCalculator histogram;
        private readonly ImageStorage imageStorage;

        public DatabaseInitializer(IOptions<PictureDbOptions> config, HistogramCalculator histogram, ImageStorage imageStorage)
        {
            _config = config?.Value ?? throw new ArgumentNullException(nameof(config));
            this.histogram = histogram;
            this.imageStorage = imageStorage;
        }

        public ImageDatabase Create()
        {
            var items = new List<DatabaseItem>();
            foreach (var file in Directory.GetFiles(_config.DirectoryPath).Take(_config.ImageCount))
            {
                var image = Image.Load<Rgba32>(file);
                var dbItem = new DatabaseItem(histogram.CalculateHistograms(image), file, Path.GetFileName(file));
                items.Add(dbItem);
                if (_config.ResavePictures)
                {
                    imageStorage.Store(image, dbItem.FamiliarName);
                }
            }

            return new ImageDatabase(items);
        }
    }
}
