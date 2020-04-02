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
        
        public Ingredient(string name, string unit)
        {
            Name = name;
            Unit = unit;
        }
    }
}