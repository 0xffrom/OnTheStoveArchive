using System;
using RecipeLibrary.Objects.Boxes;
using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.Objects
{
    
    [Serializable]
    public class RecipeFull
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public Picture TitlePicture { get;set; }
        public string Description { get; set;}
        public IngredientBox[] IngredientsBoxes { get; set;}
        public StepRecipeBox[] StepRecipesBoxes { get; set;}
        public AdditionalBox Additional { get;set; }

        public RecipeFull(
            string url,
            string title,
            Picture titlePicture,
            string description,
            IngredientBox[] ingredientsBoxes,
            StepRecipeBox[] stepRecipesBoxes,
            AdditionalBox additional = null)

        {
            Url = url;
            Title = title;
            TitlePicture = titlePicture;
            Description = description;
            IngredientsBoxes = ingredientsBoxes;
            StepRecipesBoxes = stepRecipesBoxes;
            Additional = additional;
        }

        public RecipeFull()
        {
            // (0-.oo)
        }
    }
    
}