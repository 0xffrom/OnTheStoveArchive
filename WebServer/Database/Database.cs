using MySql.Data.MySqlClient;
using ObjectsLibrary;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebServer.DataBase
{
    public static class Database
    {
        public static MySqlConnection GetConnection() => new MySqlConnection(Settings.GetStringConnection());

        //TABLE recipes (id INT NOT NULL AUTO_INCREMENT PRIMARY KEY, url TEXT NOT NULL, date DATETIME, recipe BLOB);

        public static RecipeFull GetRecipe(string url, MySqlConnection conn)
        {
            string sqlCommand = $"SELECT recipe FROM {Settings.Table} WHERE url = '{url}';";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);

            // Получение бинарного представления объекта из БД.
            byte[] result = (byte[])command.ExecuteScalar();

            return ByteArrayToRecipe(result);
        }

        private static RecipeFull ByteArrayToRecipe(byte[] arrBytes)
        {
            MemoryStream memoryStream = new MemoryStream();

            memoryStream.Write(arrBytes, 0, arrBytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            NFX.Serialization.Slim.SlimSerializer slimSerializer = 
                new NFX.Serialization.Slim.SlimSerializer();

            RecipeFull recipeFull = (RecipeFull) slimSerializer.Deserialize(memoryStream);

            return recipeFull;
        }

        private static byte[] RecipeToByteArray(object obj)
        {
            if (obj == null)
                return null;

            using MemoryStream memoryStream = new MemoryStream();

            NFX.Serialization.Slim.SlimSerializer slimSerializer = new NFX.Serialization.Slim.SlimSerializer();
            slimSerializer.Serialize(memoryStream, obj);

            return memoryStream.ToArray();
        }

        public static void AddRecipe(string url, RecipeFull recipeFull, MySqlConnection conn)
        {

            byte[] buffer = RecipeToByteArray(recipeFull);

            var resultExists = IsExists(url, conn);

            var sqlCommand = !resultExists
                ? $"INSERT INTO {Settings.Table} VALUES (0,'{url}','{DateTime.Now:yyyy-MM-dd HH:mm:ss}',?recipe);"
                : $"UPDATE {Settings.Table} SET date = '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', recipe = ?recipe WHERE url = '{url}';";

            var command = new MySqlCommand(sqlCommand, conn);

            // Добавляем в поле recipe бинарное представление объекта RecipeFull.
            command.Parameters.Add("?recipe", MySqlDbType.Blob).Value = buffer;
            command.ExecuteNonQuery();
        }


        public static bool IsNeedUpdate(string url, MySqlConnection conn)
        {
            // Тут проровека сначала на существование, потом на дату последней заливки.
            // Если их разница в часах больше чем дифферент константа - true, иначе false.

            var resultExists = IsExists(url, conn);

            // Если такой строки не существует - требуется обновление.
            if (!resultExists)
                return true;

            string sqlCommand = $"SELECT date FROM {Settings.Table} WHERE url = '{url}';";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);

            DateTime resultDate = DateTime.Parse(command.ExecuteScalar().ToString());


            return (DateTime.Now - resultDate).TotalHours > Settings.HourDiff;
        }

        private static bool IsExists(string url, MySqlConnection conn)
        {
            string sqlCommand = $"SELECT EXISTS(SELECT id FROM {Settings.Table} WHERE url = '{url}');";

            var command = new MySqlCommand(sqlCommand, conn);

            return command.ExecuteScalar().ToString() == "1";
        }
    }
}