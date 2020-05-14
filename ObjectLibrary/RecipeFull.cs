using ObjectsLibrary.Components;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
        public Image TitleImage { get; set; }

        /// <value>Описание рецепта.</value>
        public string Description { get; set; }

        /// <value>Массив ингредиентов рецепта.</value>
        /// <see cref="Ingredient"/> 
        public Ingredient[] Ingredients { get; set; }

        /// <value>Шаги приготовления рецепта.</value>
        /// <see cref="StepRecipe"/>
        public StepRecipe[] StepsRecipe { get; set; }

        /// <value>Дополнительное описание рецепта.</value>
        /// <see cref="Additional"/>
        public Additional Additional { get; set; }

        public RecipeFull()
        {
            // (0-.oo)
        }
        public RecipeFull(string url, string title, Image titleImage, string description, Ingredient[] ingredients,
            StepRecipe[] stepsRecipe, Additional additional) : this()
        {
            Url = url;
            Title = title;
            TitleImage = titleImage;
            Description = description;
            Ingredients = ingredients;
            StepsRecipe = stepsRecipe;
            Additional = additional;
        }
    }
}