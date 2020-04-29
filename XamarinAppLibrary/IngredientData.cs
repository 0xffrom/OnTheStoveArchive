
using ObjectsLibrary.Components;
using SQLite;
using System.Collections.Generic;
using System.IO;

namespace XamarinAppLibrary
{
    public class IngredientData
    {
        private static string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "onstove.db3");

        public static List<Ingredient> GetArrayIngredients()
        {
            var db = new SQLiteConnection(dbPath);

            db.CreateTable<IngredientTable>();

            var ingredientsBd = db.Table<IngredientTable>().ToArray();

            List<Ingredient> ingredients = new List<Ingredient>(ingredientsBd.Length);

            for (int i = 0; i < ingredients.Count; i++)
            {
                if (ingredientsBd[i].Name == null || ingredientsBd[i].Unit == null || ingredientsBd[i].RecipeName == null)
                    continue;

                ingredients[i] = new Ingredient(ingredientsBd[i].Name, ingredientsBd[i].Unit, ingredientsBd[i].RecipeName);
            }

            return ingredients;
        }

        public static bool ExistsIngredient(Ingredient ingredient)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<IngredientTable>();

            return db.Table<IngredientTable>().FirstOrDefault(x => 
            x.Name == ingredient.Name && 
            x.Unit == ingredient.Unit &&
            x.RecipeName == ingredient.RecipeName) == null ? false : true;
        }


        public static void DeleteIngredient(Ingredient ingredient)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<IngredientTable>();

            int id = db.Table<IngredientTable>().First(x => 
            x.Name == ingredient.Name && 
            x.Unit == ingredient.Unit &&
            x.RecipeName == ingredient.RecipeName).Id;

            db.Delete<IngredientTable>(id);
        }


        public static void SaveIngredient(Ingredient ingredient)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<IngredientTable>();

            IngredientTable ingredientTable = new IngredientTable();

            ingredientTable.Name = ingredient.Name;
            ingredientTable.Unit = ingredient.Unit;
            ingredientTable.RecipeName = ingredient.RecipeName;

            db.Insert(ingredientTable);

        }

    }
}