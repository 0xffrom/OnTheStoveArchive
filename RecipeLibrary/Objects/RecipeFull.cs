using RecipeLibrary.Objects.Boxes;
using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.Objects
{
    /// <summary>
    /// Объект представляет из себя представление полного кулинарного рецепта, в который входят:
    /// а) Заголовок.
    /// б) Заголовочная картинка.
    /// в) Коллекция объектов, представляющие ингредиенты составляющих блюд.
    /// г) Коллекция объектов, представляющие шаги кулинарного рецепта.
    /// д) Дополнительное описание рецепта.
    /// </summary>
    public class RecipeFull
    {
        public string Title { get; }
        public Picture TitlePicture { get; }
        
        public string Description { get; }
        public IngredientBox[] IngredientsBoxes { get; }
        public StepRecipeBox[] StepRecipesBoxes { get; }
        public AdditionalBox Additional { get; }

        public RecipeFull(string title, 
                          Picture picture,
                          string description,
                          IngredientBox[] ingredientsBoxes,
                          StepRecipeBox[] stepRecipeBoxes,
                          AdditionalBox additional = null)

        {
            Title = title;
            TitlePicture = picture;
            Description = description;
            IngredientsBoxes = ingredientsBoxes;
            StepRecipesBoxes = stepRecipeBoxes;
            Additional = additional;
        }
    }
}
