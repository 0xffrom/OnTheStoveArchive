using XamarinApp.Library.Objects.Boxes.Elements;
using XamarinApp.Library.Objects.Boxes;

namespace XamarinApp.Library.Objects
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