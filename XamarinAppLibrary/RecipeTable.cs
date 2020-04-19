using SQLite;

namespace XamarinAppLibrary
{
    [Table("Recipe")]
    class RecipeTable
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        [MaxLength(16), Column("_name")]
        public string Name { get; set; }

        [Column("_recipe")]
        public byte[] Recipe { get; set; }

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