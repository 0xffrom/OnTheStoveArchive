using System;

namespace ObjectsLibrary.Components
{
    /// <summary>Изображение.</summary>
    [Serializable]
    public class Image
    {
        /// <value>Интернет адрес на изображение.</value>
        public string ImageUrl { get; set; }

        public Image()
        {

        }

        public Image(string url) : this()
        {
            ImageUrl = url;
        }
    }
}
