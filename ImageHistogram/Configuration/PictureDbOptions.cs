namespace ImageHistogram.Configuration
{
    public class PictureDbOptions
    {
        public string DirectoryPath { get; set; }
        public int ImageCount { get; set; }
        public bool ResavePictures { get; set; } = true;
    }
}
