using System.Collections.Generic;

namespace ImageHistogram
{
    public class ImageDatabase
    {
        public List<DatabaseItem> Items { get; }

        public ImageDatabase(List<DatabaseItem> items)
        {
            this.Items = items;
        }
    }
}
