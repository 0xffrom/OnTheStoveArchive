using SQLite;

namespace AndroidLibrary
{
    [Table("Recipe")]
    public class IngredientTable
    {
        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }
        public string Name { get; }
        public string Unit { get; }
        public string RecipeName { get; }

        public IngredientTable()
        {
            //
        }

        public IngredientTable(string name, string unit, string recipeName) : this()
        {
            Name = name;
            Unit = unit;
            RecipeName = recipeName;
        }
    }
}