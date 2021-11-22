using ImageHistogram;
using ImageHistogram.Configuration;
using ImageHistogram.Evaluation;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ExperimentRunner
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            Startup.RegisterBusinessServices(services);
            services.Configure<HistogramOptions>(o => o.BinCount = 8);
            services.Configure<PictureDbOptions>(o =>
            {
                o.ImageCount = 80;
                o.DirectoryPath = @"C:\Users\karymes\source\repos\ImageHistogram\photos\train";
                o.ResavePictures = false;
            });

            var provider = services.BuildServiceProvider();
            var scan = provider.GetRequiredService<FullScanFind>();
            var db = provider.GetRequiredService<ImageDatabase>();

            var timeDict = new Dictionary<string, TimeSpan>();
            var scanningHistogramTimespan = TimeSpan.Zero;

            foreach (var item in db.Items)
            {
                var image = Image.Load<Rgba32>(item.Path);
                var scanned = await scan.FindMostSimilarAsync(image);
                scanningHistogramTimespan += scanned.HistogramTime;
                // Console.WriteLine($"Image: {item.FamiliarName},");

                foreach (var evaluation in scanned.Evaluations)
                {
                    if (evaluation.Hit.item.FamiliarName != item.FamiliarName)
                    {
                        Console.Error.WriteLine($"We have a problem with not finding identity for {item.FamiliarName}.");
                    }
                    //Console.WriteLine($"\tFound: {evaluation.Hit.item.FamiliarName};Measure: {evaluation.Hit.response.Measure};Measure: {evaluation.Hit.response.Name}");
                    if (!timeDict.TryGetValue(evaluation.Hit.response.Name, out var soFar))
                    {
                        soFar = TimeSpan.Zero;
                    }

                    timeDict[evaluation.Hit.response.Name] = soFar + evaluation.Hit.response.Duration;
                }
            }

            timeDict["InitHistogramTime"] = db.HistogramTime;
            timeDict["OverallInit"] = db.InitializationTime;
            timeDict["scanningHisotgrams"] = scanningHistogramTimespan;
            Console.WriteLine("-----------Durations-----");
            Console.WriteLine(JsonConvert.SerializeObject(timeDict));

            using var file = File.OpenText(@"C:\Users\karymes\source\repos\ImageHistogram\photos\test\reference.json");
            var serializer = new JsonSerializer();
            var reference = (Dictionary<string, List<string>>)serializer.Deserialize(file, typeof(Dictionary<string, List<string>>));

            var totalTestData = 0;
            var hitDict = new Dictionary<string, int>();

            foreach (var item in Directory.GetFiles(@"C:\Users\karymes\source\repos\ImageHistogram\photos\test"))
            {
                if (!item.EndsWith(".jpg"))
                {
                    continue;
                }

                totalTestData++;
                var image = Image.Load<Rgba32>(item);
                var scanned = await scan.FindMostSimilarAsync(image);
                var familiarName = Path.GetFileName(item);
                Console.WriteLine($"Image: {familiarName},");
                var referenceItems = reference[familiarName];
                foreach (var evaluation in scanned.Evaluations)
                {
                    var logLine = $"\tFound: {evaluation.Hit.item.FamiliarName};Measure: {evaluation.Hit.response.Measure};Measure: {evaluation.Hit.response.Name}";

                    if (referenceItems.Contains(evaluation.Hit.item.FamiliarName))
                    {
                        if (!hitDict.TryGetValue(evaluation.Hit.response.Name, out var soFar))
                        {
                            soFar = 0;
                        }

                        hitDict[evaluation.Hit.response.Name] = soFar + 1;
                        logLine += ";Result:Correct";
                    }
                    else
                    {
                        logLine += ";Result:Wrong";
                    }

                    Console.WriteLine(logLine);
                }
            }
            Console.WriteLine("-----------Hits-----");
            Console.WriteLine($"Total: {totalTestData}");
            Console.WriteLine(JsonConvert.SerializeObject(hitDict));
        }

        public void Evaluate()
        {

        }
    }
}
