using System;
using RecipeLibrary.Objects.Boxes.Elements;

namespace RecipeLibrary.Objects.Boxes
{
    [Serializable]
    public class IngredientBox
    {
        public Ingredient[] Ingredients { get; set; }
        public string Title { get; set; }

        public IngredientBox(string title, Ingredient[] ingredients)
        {
            Title = title;
            Ingredients = ingredients;
        }
    }
}
