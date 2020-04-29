
using ObjectsLibrary.Components;
using SQLite;
using System.Collections.Generic;
using System.IO;

namespace XamarinAppLibrary
{
    public static class IngredientData
    {
        private static string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ingredients.db3");
        private static SQLiteConnection Db = new SQLiteConnection(dbPath);

        static IngredientData()
        {
            Db.CreateTable<IngredientTable>();
        }

        public static List<Ingredient> GetArrayIngredients()
        {
            var ingredientsDb = Db.Table<IngredientTable>().ToArray();

            List<Ingredient> ingredients = new List<Ingredient>(ingredientsDb.Length);

            for (int i = 0; i < ingredientsDb.Length; i++)
            {
                if (ingredientsDb[i].Name == null || ingredientsDb[i].Unit == null || ingredientsDb[i].RecipeName == null)
                    continue;

                ingredients.Add(new Ingredient(ingredientsDb[i].Name, ingredientsDb[i].Unit, ingredientsDb[i].RecipeName));
            }

            return ingredients;
        }

        public static bool ExistsIngredient(Ingredient ingredient)
        {
            return Db.Table<IngredientTable>().FirstOrDefault(x => 
            x.Name == ingredient.Name && 
            x.Unit == ingredient.Unit &&
            x.RecipeName == ingredient.RecipeName) == null ? false : true;
        }


        public static void DeleteIngredient(Ingredient ingredient)
        {
            int id = Db.Table<IngredientTable>().First(x => 
            x.Name == ingredient.Name && 
            x.Unit == ingredient.Unit &&
            x.RecipeName == ingredient.RecipeName).Id;

            Db.Delete<IngredientTable>(id);
        }


        public static void SaveIngredient(Ingredient ingredient)
        {
            IngredientTable ingredientTable = 
                new IngredientTable(ingredient.Name, ingredient.Unit, ingredient.RecipeName);

            Db.Insert(ingredientTable);

        }

    }
}