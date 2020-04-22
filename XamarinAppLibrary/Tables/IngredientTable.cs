using SQLite;

namespace XamarinAppLibrary
{
    [Table("Recipe")]
    public class IngredientTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string RecipeName { get; set; }
        public IngredientTable()
        {
        }

        public IngredientTable(string name, string unit) : this()
        {
            Name = name;
            Unit = unit;
        }
    }
}