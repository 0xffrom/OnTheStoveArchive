using System;

namespace ObjectsLibrary.Components
{
    /// <summary>Ингредиент.</summary>
    [Serializable]
    public class Ingredient
    {
        /// <value>Название ингредиента.</value>
        public string Name { get; set; }
        /// <value>Количество и мера измерения.</value>
        public string Unit { get; set; }
        /// <value>Название рецепта, которому принадлежит ингредиент.</value>
        public string RecipeName { get; set; }
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