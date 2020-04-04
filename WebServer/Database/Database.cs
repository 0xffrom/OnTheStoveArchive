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

        //TABLE recipes (id INT NOT NULL AUTO_INCREMENT PRIMARY KEY, url TEXT NOT NULL, date DATETIME, json TEXT);

        public static RecipeFull GetRecipe(string url, MySqlConnection conn)
        {
            string sqlCommand = $"SELECT json FROM {Settings.Table} WHERE url = '{url}';";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);

            // Получение бинарного представления объекта из БД.
            byte[] result = (byte[])command.ExecuteScalar();

            return ByteArrayToRecipe(result);
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

        public static void AddRecipe(string url, RecipeFull recipeFull, MySqlConnection conn)
        {
            recipeFull.Url = url;

            byte[] buffer = RecipeToByteArray(recipeFull);

            var resultExists = IsExists(url, conn);

            var sqlCommand = !resultExists
                ? $"INSERT INTO {Settings.Table} VALUES (0,'{url}','{DateTime.Now:yyyy-MM-dd HH:mm:ss}',?json);"
                : $"UPDATE {Settings.Table} SET date = '{DateTime.Now:yyyy-MM-dd HH:mm:ss}', json = ?json WHERE url = '{url}';";

            var command = new MySqlCommand(sqlCommand, conn);

            // Добавляем в поле json бинарное представление объекта RecipeFull.
            command.Parameters.Add("?json", MySqlDbType.Blob).Value = buffer;
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