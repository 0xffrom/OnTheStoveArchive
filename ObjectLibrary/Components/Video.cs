using System;

namespace ObjectsLibrary.Components
{
    /// <summary>Видео.</summary>
    [Serializable]
    public class Video
    {
        /// <value>Интернет адрес на видео.</value>
        public string Url { get; set; }

        public Video()
        {

        }
        public Video(string url) : this()
        {
            Url = url;
        }
    }
}
