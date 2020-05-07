using Microsoft.EntityFrameworkCore;
using System;

namespace ObjectsLibrary.Components
{
    /// <summary>Изображение.</summary>
    [Serializable]
    [Keyless]
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class Image
    {
        /// <value>Интернет адрес на изображение.</value>
        public string ImageUrl { get; set; }

        public Image()
        {
            //
        }

        public Image(string imageUrl) : this()
        {
            ImageUrl = imageUrl;
        }
    }
}
