using System;

namespace AlphaXSpreadSamplesExplorer.Models
{
    public class SampleItem
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string IconData { get; set; }
        public Type SampleType { get; set; }

        public SampleItem()
        {
            Category = "General";
        }

        public SampleItem(string key, string title, string description, string category, string iconData, Type sampleType)
        {
            Key = key;
            Title = title;
            Description = description;
            Category = category;
            IconData = iconData;
            SampleType = sampleType;
        }
    }
}
