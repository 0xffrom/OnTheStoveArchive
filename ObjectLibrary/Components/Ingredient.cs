
using System;

namespace ObjectsLibrary.Components
{
    /// <summary>Ингредиент.</summary>
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class Ingredient
    {
        /// <value>Название ингредиента.</value>
        public string Name { get; }
        /// <value>Количество и мера измерения.</value>
        public string Unit { get; }
        /// <value>Название рецепта, которому принадлежит ингредиент.</value>
        public string RecipeName { get; }
        public Ingredient()
        {

        }
        public Ingredient(string name, string unit, string recipeName) : this()
        {
            Name = name;
            Unit = unit;
            RecipeName = recipeName;
        }
    }
}