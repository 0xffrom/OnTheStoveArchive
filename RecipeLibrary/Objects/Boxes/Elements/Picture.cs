using System;

namespace RecipeLibrary.Objects.Boxes.Elements
{
    [Serializable]
    public class Picture
    {
        public string Url { get; set; }

        public Picture(string url)
        {
            Url = url;
        }
    }
}
