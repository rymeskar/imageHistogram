using ImageHistogram;
using ImageHistogram.Configuration;
using ImageHistogram.Evaluation;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;

namespace ExperimentRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            Startup.RegisterBusinessServices(services);
            services.Configure<HistogramOptions>(o => o.BinCount = 16);
            services.Configure<PictureDbOptions>(o =>
            {
                o.ImageCount = 10;
                o.DirectoryPath = @"C:\Users\karymes\source\repos\ImageHistogram\photos\train";
                o.ResavePictures = false;
            });

            var provider = services.BuildServiceProvider();
            var scan = provider.GetRequiredService<FullScanFind>();
            var db = provider.GetRequiredService<ImageDatabase>();

            var timeDict = new Dictionary<string, TimeSpan>();
            foreach (var item in db.Items)
            {
                var image = Image.Load<Rgba32>(item.Path);
                var scanned = scan.FindMostSimilar(image);
                Console.WriteLine($"Image: {item.FamiliarName},");

                foreach (var evaluation in scanned.Evaluations)
                {
                    if (evaluation.Hit.item.FamiliarName != item.FamiliarName)
                    {
                        Console.Error.WriteLine("We have a problem with not finding identity.");
                    }
                    Console.WriteLine($"\tFound: {evaluation.Hit.item.FamiliarName};Measure: {evaluation.Hit.response.Measure};Measure: {evaluation.Hit.response.Name}");
                    if (!timeDict.TryGetValue(evaluation.Hit.response.Name, out var soFar))
                    {
                        soFar = TimeSpan.Zero;
                    }

                    timeDict[evaluation.Hit.response.Name] = soFar + evaluation.Hit.response.Duration;
                }
            }

            Console.WriteLine("-----------Durations-----");
            Console.WriteLine(JsonConvert.SerializeObject(timeDict));
        }
    }
}
