using System;
using System.Runtime.Serialization;
using ObjectsLibrary.Objects.Boxes.Elements;

namespace ObjectsLibrary.Objects
{
    
    [Serializable]
    public class RecipeShort
    {
        public string Title { get; set; }
        public Picture Picture { get; set; }
        public string Url { get; set; }
        [IgnoreDataMember] 
        public double IndexPopularity { get; set; }

        public RecipeShort(string title, Picture picture, string url) : this()
        {
            Title = title;
            Picture = picture;
            Url = url;
        }

        public RecipeShort(string title, Picture picture, string url, double indexPopularity) : this(title, picture, url)
        {
            IndexPopularity = indexPopularity;
        }
        
        public RecipeShort()
        {
            //
        }
    }
}
