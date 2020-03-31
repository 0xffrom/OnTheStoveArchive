using System;

namespace RecipeLibrary.Objects.Boxes.Elements
{
    /// <summary>
    /// Объект, который представляет собой url на видеоконтент.
    /// </summary>
    /// 
    [Serializable]
    public class Video
    {
        public string Url { get; set; }

        public Video(string url)
        {
            Url = url;
        }
    }
}
