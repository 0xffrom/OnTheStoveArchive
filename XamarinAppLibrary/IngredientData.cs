using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ObjectsLibrary.Components;
using SQLite;

namespace XamarinAppLibrary
{
    public class IngredientData
    {
        private static string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "onstove.db3");

        public static IngredientTable[] GetArrayIngredients()
        {
            var db = new SQLiteConnection(dbPath);

            db.CreateTable<RecipeTable>();

            return db.Table<IngredientTable>().ToArray();
        }

        public static bool ExistsIngredient(Ingredient ingredient, string recipeName)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<IngredientTable>();

            return db.Table<IngredientTable>().FirstOrDefault(x => 
            x.Name == ingredient.Name && 
            x.Unit == ingredient.Unit &&
            x.RecipeName == recipeName) == null ? false : true;
        }


        public static void DeleteIngredient(Ingredient ingredient, string recipeName)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<IngredientTable>();

            int id = db.Table<IngredientTable>().First(x => 
            x.Name == ingredient.Name && 
            x.Unit == ingredient.Unit && 
            (x.RecipeName == recipeName || x.RecipeName == null || x.RecipeName == string.Empty)).Id;

            db.Delete<RecipeTable>(id);
        }


        public static void SaveIngredient(Ingredient ingredient, string recipeName)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<IngredientTable>();

            IngredientTable ingredientTable = new IngredientTable();

            ingredientTable.Name = ingredient.Name;
            ingredientTable.Unit = ingredient.Unit;
            ingredientTable.RecipeName = recipeName;

            db.Insert(ingredientTable);

        }

    }
}