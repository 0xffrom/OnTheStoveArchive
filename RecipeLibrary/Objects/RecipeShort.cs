using System;
using System.Runtime.Serialization;
using RecipeLibrary.Objects.Boxes.Elements;
using RecipeLibrary.Objects.Boxes;

namespace RecipeLibrary.Objects
{
    
    [Serializable]
    public class RecipeShort
    {
        public string Title { get; }
        public Picture Picture { get; }
        public string Url { get; }
        [IgnoreDataMember] 
        public double IndexPopularity { get; }

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
