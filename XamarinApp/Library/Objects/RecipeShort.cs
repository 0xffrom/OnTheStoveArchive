using RecipesAndroid.Objects.Boxes.Elements;

namespace RecipesAndroid.Objects
{
    /// <summary>
    /// Объект, который представляет собой короткое описание рецепта.
    /// В краткое представление рецепта входит:
    /// а) Картинка.
    /// б) Название рецепта.
    /// </summary>
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
