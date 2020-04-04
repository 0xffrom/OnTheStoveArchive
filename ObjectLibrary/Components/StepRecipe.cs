using System;

namespace ObjectsLibrary.Components
{
    /// <summary>Шаг приготовления кулинарного рецепта.</summary>
    [Serializable]
    public class StepRecipe
    {
        /// <value>Изображение шага кулинарного рецепта.</value>
        /// <see cref="Image"/>
        public Image Image { get; set; }

        /// <value>Описание шага кулинарного рецепта.</value>
        public string Description { get; set; }

        public StepRecipe(string description, Image image)
        {
            Description = description;
            Image = image;
        }

    }
}