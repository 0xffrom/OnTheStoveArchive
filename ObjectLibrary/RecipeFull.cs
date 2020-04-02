using System;
using ObjectsLibrary.Components;

namespace ObjectsLibrary
{
    /// <summary>Полное описание рецепта.</summary>
    [Serializable]
    public class RecipeFull
    {
        /// <value>Адрес рецепта.</value>
        public string Url { get; set; }
        
        /// <value>Название рецепта.</value>
        public string Title { get; set; }
        
        /// <value>Главное изображение.</value>
        /// <see cref="Image"/>
        public Image TitleImage { get;set; }
        /// <value>Описание рецепта.</value>
        public string Description { get; set;}
        
        /// <value>Массив ингредиентов рецепта.</value>
        /// <see cref="Ingredient"/> 
        public Ingredient[] Ingredients{ get; set;}
        
        /// <value>Шаги приготовления рецепта.</value>
        /// <see cref="StepRecipe"/>
        public StepRecipe[] StepRecipesBoxes { get; set; }
        
        /// <value>Дополнительное описание рецепта.</value>
        /// <see cref="Additional"/>
        public Additional Additional { get; set; }
        
        public RecipeFull(
            string url,
            string title,
            Image image,
            string description,
            Ingredient[] ingredients,
            StepRecipe[] stepRecipesBoxes,
            Additional additional)

        {
            Url = url;
            Title = title;
            TitleImage = image;
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