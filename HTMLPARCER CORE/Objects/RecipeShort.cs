using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.Objects
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

        public RecipeShort(string title, Picture picture)
        {
            Title = title;
            Picture = picture;
        }
    }
}
