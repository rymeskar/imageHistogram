using System;
using System.Collections.Generic;

namespace ImageHistogram
{
    public class ImageDatabase
    {
        public List<DatabaseItem> Items { get; }
        public TimeSpan InitializationTime { get; }
        public TimeSpan HistogramTime { get; }

        public ImageDatabase(List<DatabaseItem> items, TimeSpan initializationTime, TimeSpan histogramTime)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            InitializationTime = initializationTime;
            HistogramTime = histogramTime;
        }
    }
}
