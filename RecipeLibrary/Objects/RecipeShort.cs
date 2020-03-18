using RecipeLibrary.Objects.Boxes.Elements;
using RecipeLibrary.Objects.Boxes;

namespace RecipeLibrary.Objects
{
    
    // TODO: add comments
    public class RecipeShort
    {
        public string Title { get; }
        public Picture Picture { get; }
        public string Url { get; }

        public RecipeShort(string title, Picture picture, string url)
        {
            Title = title;
            Picture = picture;
            Url = url;
        }
    }
}
