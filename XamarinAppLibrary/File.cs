using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ObjectsLibrary;

namespace XamarinAppLibrary
{
    public class File
    {
        Stream _path;
        public File(Java.IO.File path)
        {
            _path = path;
        }

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

        public RecipeFull GetRecipe(string url)
        {

        }

        public bool ContainsRecipe(string url)
        {

        }

        public void SaveRecipe(string url, RecipeFull recipeFull)
        {
            string fileName = _path.AbsolutePath + GetFileName(url) + ".bin";
            Java.IO.File file = new Java.IO.File(new Java.Net.URI(fileName));

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            using (Java.IO.FileWriter fileStream = new Java.IO.FileWriter(file))
            {
                Java.IO.ByteArrayOutputStream byteArrayOutputStream = new Java.IO.ByteArrayOutputStream();
                byteArrayOutputStream.WriteTo(fileStream);

                fileStream.Write(RecipeToByteArray(recipeFull);
                
            };

        }

        private static RecipeFull ByteArrayToRecipe(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();

            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            BinaryFormatter binForm = new BinaryFormatter();

            RecipeFull recipeFull = (RecipeFull)binForm.Deserialize(memStream);

            return recipeFull;
        }

        private static byte[] RecipeToByteArray(object obj)
        {
            if (obj == null)
                return null;

            using MemoryStream ms = new MemoryStream();

            var bf = new BinaryFormatter();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

    }
}