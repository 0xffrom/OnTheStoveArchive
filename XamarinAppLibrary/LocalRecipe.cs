using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ObjectsLibrary;

namespace XamarinAppLibrary
{
    public static class LocalRecipe
    {
        private static string path =
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private static string GetFileName(string url)
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

        public static RecipeFull GetRecipe(string url)
        {
            string filePath = Path.Combine(path, "recipes", GetFileName(url) + ".bin");

            return GetLocalRecipe(filePath);
        }

        private static RecipeFull GetLocalRecipe(string filePath)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            object obj;

            using (StreamReader fileStream = new StreamReader(filePath))
            {
                obj = binaryFormatter.Deserialize(fileStream.BaseStream);
            };

            return (RecipeFull)obj;
        }

        public static RecipeFull[] GetArrayRecipes()
        {
            string dirPath = Path.Combine(path, "recipes");

            var filePaths = Directory.GetFiles(dirPath);

            RecipeFull[] recipes = new RecipeFull[filePaths.Length];

            for (int i = 0; i < filePaths.Length; i++)
            {
                recipes[i] = GetLocalRecipe(filePaths[i]);
            }

            return recipes;
        }

        public static bool ExistsRecipe(string url)
        {
            string filePath = Path.Combine(path, "recipes", GetFileName(url) + ".bin");

            return System.IO.File.Exists(filePath);
        }

        public static void DeleteRecipe(string url)
        {
            string filePath = Path.Combine(path, "recipes", GetFileName(url) + ".bin");

            System.IO.File.Delete(filePath);
        }

        public static void SaveRecipe(string url, RecipeFull recipeFull)
        {
            string filePath = Path.Combine(path, "recipes", GetFileName(url) + ".bin");

            if (!System.IO.Directory.Exists(Path.Combine(path, "recipes")))
                Directory.CreateDirectory(Path.Combine(path, "recipes"));

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (var fileStream = File.Create(filePath))
            {
                binaryFormatter.Serialize(fileStream, recipeFull);
            };

        }

    }
}