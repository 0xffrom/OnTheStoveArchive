using ObjectsLibrary.Components;
using System;
using System.Runtime.Serialization;

namespace ObjectsLibrary
{
    /// <summary>Краткое описание рецепта, полученное со страницы поиска кулинарного сайта.</summary>
    [Serializable]
    public class RecipeShort
    {
        /// <value>Название рецепта.</value>
        public string Title { get; }

        /// <value>Изображение рецепта.</value>
        /// /// <see cref="Image"/>ИК
        public Image Image { get;}

        /// <value>Интернет адрес рецепта.</value>

        public string Url { get; }

        /// <value>Индекс популярности рецепта.</value>
        /// <remarks>Используется для сортировки рецептов сервером.</remarks>
        [IgnoreDataMember]
        public double IndexPopularity { get; }

        public RecipeShort(string title, Image image, string url) : this()
        {
            Title = title;
            Image = image;
            Url = url;
        }

        public RecipeShort(string title, Image image, string url, double indexPopularity) : this(title, image, url)
        {
            IndexPopularity = indexPopularity;
        }

        public RecipeShort()
        {
            //
        }
    }
}