using ObjectsLibrary.Components;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AndroidLibrary
{
    public static class IngredientData
    {
        private static string dbPath =
            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                "ingredients.db3");

        private static SQLiteConnection dbConnection = new SQLiteConnection(dbPath);
        static IngredientData()
        {
            dbConnection.CreateTable<IngredientTable>();
        }

        public static List<Ingredient> GetArrayIngredients()
        {
            var ingredientsDb = dbConnection.Table<IngredientTable>().ToArray();

            List<Ingredient> ingredients = new List<Ingredient>(ingredientsDb.Length);
            ingredients.AddRange(from t in ingredientsDb
                where t.Name != null && t.Unit != null && t.RecipeName != null
                select new Ingredient(t.Name, t.Unit, t.RecipeName));

            return ingredients;
        }

        public static bool ExistsIngredient(Ingredient ingredient)
        {
            return dbConnection.Table<IngredientTable>().FirstOrDefault(x =>
                x.Name == ingredient.Name &&
                x.Unit == ingredient.Unit &&
                x.RecipeName == ingredient.RecipeName) != null;
        }


        public static void DeleteIngredient(Ingredient ingredient)
        {
            var ingredientDb = dbConnection.Table<IngredientTable>().FirstOrDefault(x =>
                x.Name == ingredient.Name &&
                x.RecipeName == ingredient.RecipeName);

            if (ingredientDb == null)
                return;

            dbConnection.Delete<IngredientTable>(ingredientDb.Id);
        }


        public static void SaveIngredient(Ingredient ingredient)
        {
            IngredientTable ingredientTable =
                new IngredientTable(ingredient.Name, ingredient.Unit, ingredient.RecipeName);

            dbConnection.Insert(ingredientTable);
        }
    }
}