namespace HTMLPARCER_CORE
{
    public class Ingredient
    {
        public Ingredient(string title, string name, string unit)
        {
            Title = title;
            Name = name;
            Unit = unit;
        }
        
        public string Name { get; }
        public string Unit { get; }
        public string Title { get; }
    }
}