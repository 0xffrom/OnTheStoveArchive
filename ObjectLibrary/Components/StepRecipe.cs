
using System;

namespace ObjectsLibrary.Components
{
    /// <summary>Шаг приготовления кулинарного рецепта.</summary>
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class StepRecipe
    {
        /// <value>Описание шага кулинарного рецепта.</value>
        public string Description { get; set; }

        /// <value>Изображение шага кулинарного рецепта.</value>
        /// <see cref="Image"/>
        public Image Image { get; set; }
        public StepRecipe()
        {
            //
        }

        public StepRecipe(string description, Image image) : this()
        {
            Description = description;
            Image = image;
        }

    }
}