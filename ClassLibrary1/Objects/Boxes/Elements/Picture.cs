namespace RecipesAndroid.Objects.Boxes.Elements
{
    /// <summary>
    /// Объект, который представляет собой представление картинки в виде url.
    /// </summary>
    public class Picture
    {
        public string Url { get; }

        public Picture(string url)
        {
            Url = url;
        }
    }
}
