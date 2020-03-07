namespace RecipesAndroid.Objects.Boxes.Elements
{
    /// <summary>
    /// Объект, который представляет собой url на видеоконтент.
    /// </summary>
    public class Video
    {
        public string Url { get; }

        public Video(string url)
        {
            Url = url;
        }
    }
}
