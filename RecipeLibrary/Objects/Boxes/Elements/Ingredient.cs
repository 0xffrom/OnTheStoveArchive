namespace RecipeLibrary.Objects.Boxes.Elements
{
    /// <summary>
    /// Объект, который представляет собой ингредиент с названием и единицей измерения.
    /// </summary>
    public class Ingredient
    {
        public Ingredient(string name, string unit)
        {
            Name = name;
            Unit = unit;
        }

        public string Name { get; }
        public string Unit { get; }
    }
}