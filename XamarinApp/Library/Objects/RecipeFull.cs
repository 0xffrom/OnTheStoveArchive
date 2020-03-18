using XamarinApp.Library.Objects.Boxes;
using XamarinApp.Library.Objects.Boxes.Elements;

namespace XamarinApp.Library.Objects
{
    
    // TODO: add comments
    public class RecipeFull
    {
        public string Title { get; }
        public Picture TitlePicture { get; }
        public string Description { get; }
        public IngredientBox[] IngredientsBoxes { get; }
        public StepRecipeBox[] StepRecipesBoxes { get; }
        public AdditionalBox Additional { get; }

        public RecipeFull(
            string title,
            Picture titlePicture,
            string description,
            IngredientBox[] ingredientsBoxes,
            StepRecipeBox[] stepRecipeBoxes,
            AdditionalBox additional = null)

        {
            Title = title;
            TitlePicture = titlePicture;
            Description = description;
            IngredientsBoxes = ingredientsBoxes;
            StepRecipesBoxes = stepRecipeBoxes;
            Additional = additional;
        }
    }
    
}