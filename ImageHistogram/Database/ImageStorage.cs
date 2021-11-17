using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;

namespace ImageHistogram.Database
{
    public class ImageStorage
    {
        private readonly IWebHostEnvironment env;

        public ImageStorage(IWebHostEnvironment env)
        {
            this.env = env;
        }

        public ImageStorage()
        {
            // TODO: better
        }

        public string Store(Image<Rgba32> image) => Store(image, Guid.NewGuid().ToString() + ".jpg"); 

        public string Store(Image<Rgba32> image, string name)
        {
            var filePath = Path.Combine(env.WebRootPath, "images", name);
            image.SaveAsJpeg(filePath);
            return name;
        }
    }
}
