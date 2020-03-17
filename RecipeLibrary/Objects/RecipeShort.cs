using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.Objects
{
    
    // TODO: add comments
    public class RecipeShort
    {
        private string Title { get; }
        private Picture Picture { get; }
        private string Url { get; }

        public RecipeShort(string title, Picture picture, string url)
        {
            Title = title;
            Picture = picture;
            Url = url;
        }
    }
}
