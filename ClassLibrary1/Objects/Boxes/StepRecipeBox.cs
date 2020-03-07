
namespace RecipesAndroid.Objects.Boxes
{
    /// <summary>
    /// Объект, который представляет собой некий шаг рецепта. Он содержит
    /// а) Описание шага.
    /// б) Представление шага в виде коллекции картинок.
    /// </summary>
    public class StepRecipeBox
    {
        public PictureBox PictureBox { get; }
        public string Description { get; }

        public StepRecipeBox(string description, PictureBox pictureBox)
        {
            Description = description;
            PictureBox = pictureBox;
        }


    }
}