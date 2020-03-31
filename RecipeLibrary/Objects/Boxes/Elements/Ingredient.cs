using System;

namespace RecipeLibrary.Objects.Boxes.Elements
{
    [Serializable]
    public class Ingredient
    {
        public Ingredient(string name, string unit)
        {
            Name = name;
            Unit = unit;
        }

        public string Name { get; set; }
        public string Unit { get; set; }
    }
}