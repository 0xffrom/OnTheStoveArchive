using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ObjectsLibrary;
using ObjectsLibrary.Components;
using SQLite;

namespace AndroidLibrary
{
    public static class RecipeData
    {
        private static string dbPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "recipes.db3");

        private static SQLiteConnection Db = new SQLiteConnection(dbPath);

        static RecipeData()
        {
            Db.CreateTable<RecipeTable>();
        }

        private static string GetFileRecipeName(string url)
        {
            // Пример: https://www.povarenok.ru/recipes/show/163893/
            if (url.Contains("https://www.povarenok.ru"))
            {
                return "pk" + url[38..^1];
            }
            // Пример: https://povar.ru/recipes/postnyi_apelsinovyi_keks-80038.html
            else if (url.Contains("https://povar.ru"))
            {
                return "pr" + url.Split('-')[^1].Split('.')[0];
            }
            // Пример: https://www.edimdoma.ru/retsepty/137347-syrnik-s-izyumom-i-tsukatami
            else if (url.Contains("https://www.edimdoma.ru"))
            {
                return "edm" + url[33..^1].Split('-')[0];
            }
            // Пример: https://eda.ru/recepty/zavtraki/amerikanskie-bliny-30600
            else if (url.Contains("https://eda.ru/"))
            {
                return "eda" + url.Split('-')[^1];
            }

            return url[8..^1];
        }

        public static RecipeShort[] GetArrayRecipes()
        {
            var recipes = Db.Table<RecipeTable>().ToArray();

            RecipeShort[] recipeShorts = new RecipeShort[recipes.Length];

            for (int i = 0; i < recipes.Length; i++)
            {
                if (recipes[i].Recipe == null || recipes[i].Name == null)
                    continue;

                recipeShorts[i] = DataContext.ByteArrayToObject<RecipeShort>(recipes[i].Recipe);
            }

            return recipeShorts;
        }

        public static bool ExistsRecipe(string url)
        {
            string fileName = GetFileRecipeName(url);

            return Db.Table<RecipeTable>().FirstOrDefault(x => x.Name == fileName) != null;
        }

        public static void DeleteRecipe(string url)
        {
            string fileName = GetFileRecipeName(url);

            int id = Db.Table<RecipeTable>().First(x => x.Name == fileName).Id;

            Db.Delete<RecipeTable>(id);
        }


        public static void SaveRecipe(string url, RecipeShort recipeShort)
        {
            string fileName = GetFileRecipeName(url);

            RecipeTable recipeTable = new RecipeTable(fileName, DataContext.RecipeToByteArray(recipeShort));

            Db.Insert(recipeTable);
        }
    }
}