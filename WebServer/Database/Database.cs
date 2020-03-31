using System;
using System.Text.Json;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using RecipeLibrary.Objects;

namespace WebServer.DataBase
{
    public static class Database
    {
        public static MySqlConnection GetConnection() => new MySqlConnection(Settings.GetStringConnection());
        
        //CREATE TABLE recipes (id INT NOT NULL AUTO_INCREMENT, url TEXT NOT NULL PRIMARY KEY, date DATETIME, json TEXT);
        
        public static RecipeFull GetRecipe(string url, MySqlConnection conn)
        {
            string sqlCommand = $"SELECT json FROM {Settings.Table} WHERE url = '{url}';";
            
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);

            string result = command.ExecuteScalar().ToString();
            
            return JsonSerializer.Deserialize<RecipeFull>(result);
        }
        
        public static void AddRecipe(RecipeFull recipeFull, MySqlConnection conn)
        {
            string url = recipeFull.Url;
            string json = JsonSerializer.Serialize(recipeFull);

            var resultExists = IsExists(url, conn);

            var sqlCommand = resultExists == "0"
                ? $"INSERT INTO {Settings.Table} VALUES (0,'{url}','{DateTime.Now:yyyy-MM-dd HH:mm:ss}','{json}');"
                : $"UPDATE {Settings.Table} SET date = {DateTime.Now:yyyy-MM-dd HH:mm:ss}, json = '{json}' WHERE url = '{url}';";
            
            var command = new MySqlCommand(sqlCommand, conn);
            
            command.ExecuteScalar();
        }


        public static bool IsNeedUpdate(string url, MySqlConnection conn)
        {
            // Тут проровека сначала на существование, потом на дату последней заливки.
            // Если их разница в часах больше чем дифферент константа - true, иначе false.
            
            var resultExists = IsExists(url, conn);

            // Если такой строки не существует - требуется обновление.
            if (resultExists == "0")
                return true;

            string sqlCommand = $"SELECT date FROM {Settings.Table} WHERE url = '{url}';";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);

            DateTime resultDate = DateTime.Parse(command.ExecuteScalar().ToString());
            

            return (DateTime.Now - resultDate).Hours > Settings.HourDiff;
        }

        private static string IsExists(string url, MySqlConnection conn)
        {
            string sqlCommand = $"SELECT EXISTS(SELECT id FROM {Settings.Table} WHERE url = '{url}');";

            var command = new MySqlCommand(sqlCommand, conn);

            return command.ExecuteScalar().ToString();
            ;
        }
    }
}