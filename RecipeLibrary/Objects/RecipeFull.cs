using RecipeLibrary.Objects.Boxes;
using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.Objects
{
    
    // TODO: add comments
    public class RecipeFull
    {
        public string Url { get; set; }
        public string Title { get; }
        public Picture TitlePicture { get; }
        public string Description { get; }
        public IngredientBox[] IngredientsBoxes { get; }
        public StepRecipeBox[] StepRecipesBoxes { get; }
        public AdditionalBox Additional { get; }

        public RecipeFull(
            string url,
            string title,
            Picture titlePicture,
            string description,
            IngredientBox[] ingredientsBoxes,
            StepRecipeBox[] stepRecipeBoxes,
            AdditionalBox additional = null)

        {
            Url = url;
            Title = title;
            TitlePicture = titlePicture;
            Description = description;
            IngredientsBoxes = ingredientsBoxes;
            StepRecipesBoxes = stepRecipeBoxes;
            Additional = additional;
        }
    }
    
}