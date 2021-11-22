using ImageHistogram.Database;
using ImageHistogram.Evaluation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ImageHistogram
{
    public class UploadFileModel : PageModel
    {
        private IHostingEnvironment _environment;
        private readonly FullScanFind dbScan;
        private readonly ImageStorage imageStorage;

        public UploadFileModel(IHostingEnvironment environment, FullScanFind dbScan, ImageStorage imageStorage)
        {
            _environment = environment;
            this.dbScan = dbScan;
            this.imageStorage = imageStorage;
        }

        public string UploadedFilePath { get; private set; }
        public EvaluationResponse SimilarPictures { get; private set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        public async Task OnPostAsync()
        {
            ////var file = Path.Combine(_environment.ContentRootPath, "uploads", Upload.FileName);
            using var memoryStream = new MemoryStream();
            await Upload.CopyToAsync(memoryStream);
            var image = Image.Load(memoryStream.ToArray());
            UploadedFilePath = imageStorage.Store(image);

            SimilarPictures = await dbScan.FindMostSimilarAsync(image);
        }
    }
}