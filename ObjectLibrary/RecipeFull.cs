using ObjectsLibrary.Components;
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjectsLibrary
{
    /// <summary>Полное описание рецепта.</summary>
    [Serializable]
    public class RecipeFull
    {
        [Key]
        public Guid Key { get; set; }
        /// <value>Адрес рецепта.</value>
        public string Url { get; }

        /// <value>Название рецепта.</value>
        public string Title { get; }

        /// <value>Главное изображение.</value>
        /// <see cref="Image"/>
        public Image TitleImage { get; }

        /// <value>Описание рецепта.</value>
        public string Description { get; }

        /// <value>Массив ингредиентов рецепта.</value>
        /// <see cref="Ingredient"/> 
        public Ingredient[] Ingredients { get; }

        /// <value>Шаги приготовления рецепта.</value>
        /// <see cref="StepRecipe"/>
        public StepRecipe[] StepsRecipe { get; }

        /// <value>Дополнительное описание рецепта.</value>
        /// <see cref="Additional"/>
        public Additional Additional { get; }

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