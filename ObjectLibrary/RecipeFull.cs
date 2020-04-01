using System;
using System.Runtime.Serialization;
using ObjectsLibrary.Objects.Boxes;
using ObjectsLibrary.Objects.Boxes.Elements;

namespace ObjectsLibrary.Objects
{
    
    [Serializable]
    public class RecipeFull
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public Picture TitlePicture { get;set; }
        public string Description { get; set;}
        public Ingredient[] Ingredients{ get; set;}
        public StepRecipeBox[] StepRecipesBoxes { get; set; }
        public AdditionalBox Additional { get; set; }
        
        public RecipeFull(
            string url,
            string title,
            Picture titlePicture,
            string description,
            Ingredient[] ingredients,
            StepRecipeBox[] stepRecipesBoxes,
            AdditionalBox additional = null)

        {
            Url = url;
            Title = title;
            TitlePicture = titlePicture;
            Description = description;
            Ingredients = ingredients;
            StepRecipesBoxes = stepRecipesBoxes;
            Additional = additional;
        }
        
        public RecipeFull()
        {
            // (0-.oo)
        }
    }
    
}