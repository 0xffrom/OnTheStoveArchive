using RecipeLibrary.Objects.Boxes;
using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.Objects
{
    
    // TODO: add comments
    public class RecipeFull
    {
        private string Url { get; }
        private string Title { get; }
        private Picture TitlePicture { get; }
        private string Description { get; }
        private IngredientBox[] IngredientsBoxes { get; }
        private StepRecipeBox[] StepRecipesBoxes { get; }
        private AdditionalBox Additional { get; }

        public RecipeFull(string url,
            string title,
            Picture picture,
            string description,
            IngredientBox[] ingredientsBoxes,
            StepRecipeBox[] stepRecipeBoxes,
            AdditionalBox additional = null)

        {
            Url = url;
            Title = title;
            TitlePicture = picture;
            Description = description;
            IngredientsBoxes = ingredientsBoxes;
            StepRecipesBoxes = stepRecipeBoxes;
            Additional = additional;
        }
    }
}