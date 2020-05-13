using SQLite;

namespace AndroidLibrary
{
    [Table("Recipe")]
    public class RecipeTable
    {
        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }
        public string Name { get; }
        public byte[] Recipe { get; }

        public RecipeTable()
        {
            //
        }

        public RecipeTable(string name, byte[] recipe) : this()
        {
            Name = name;
            Recipe = recipe;
        }
    }
}