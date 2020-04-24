using MySql.Data.MySqlClient;
using ObjectsLibrary;
using System;
using System.IO;

namespace WebServer.DataBase
{
    /// <summary>
    /// Класс, отвечающий за работу с базой данных MySQL.
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// Подключение к базе данных.
        /// </summary>
        /// <returns> Созданное подключение <see cref="MySqlConnection"/> к базе данных MySQL.</returns>
        public static MySqlConnection GetConnection() => new MySqlConnection(Settings.GetStringConnection());

        /// <summary>
        /// Метод возвращает из базы данных объект вида <see cref="RecipeFull"/>.
        /// </summary>
        /// <param name="url">URL адресс рецепта.</param>
        /// <param name="conn">Подключение к базе данных <see cref="MySqlConnection"/></param>
        /// <param name="size">Размер объекта в килобайтах.</param>
        /// <returns></returns>
        public static RecipeFull GetRecipe(string url, MySqlConnection conn, out double size)
        {
            string sqlCommand = $"SELECT recipe FROM {Settings.Table} WHERE url = '{url}';";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);

            // Получение бинарного представления объекта из БД.
            byte[] result = (byte[])command.ExecuteScalar();

            // Получение размера:
            size = result.Length * 1.0 / 1024;

            return ByteArrayToRecipe(result);
        }

        /// <summary>
        /// Добавление кулинарного рецепта типа RecipeFull <see cref="RecipeFull"/> в базу данных MySQL.
        /// </summary>
        /// <param name="url">URL рецепта.</param>
        /// <param name="recipeFull">Объект рецепта типа RecipeFull.</param>
        /// <param name="conn">Подключение к базе данных MySQL <see cref="MySqlConnection"/></param>
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

        /// <summary>
        /// Проверка на надобность обновление рецепта.
        /// </summary>
        /// <param name="url">URL рецепта.</param>
        /// <param name="conn">Подключение к базе данных MySQL <see cref="MySqlConnection"/></param>
        /// <returns>
        /// <see langword="true"/> если обновление рецепта требуется.
        /// <see langword="false"/> если обновление рецепта не требуется.
        /// </returns>
        public static bool IsNeedUpdate(string url, MySqlConnection conn)
        {
            // Проверка на существование рецепта в базе данных.
            var resultExists = IsExists(url, conn);

            // Если такого рецепта не существует - требуется обновление.
            if (!resultExists)
                return true;

            // Если рецепт существует, смотрим на дату последнего обновления.
            string sqlCommand = $"SELECT date FROM {Settings.Table} WHERE url = '{url}';";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);

            DateTime resultDate = DateTime.Parse(command.ExecuteScalar().ToString());

            // Если разница больше, чем {Settings.HourDiff} - обновить, иначе - нет.
            return (DateTime.Now - resultDate).TotalHours > Settings.HourDiff;
        }

        /// <summary>
        /// Проверка на наличие в базе данных кулинарного рецепта.
        /// </summary>
        /// <param name="url">URL рецепта.</param>
        /// <param name="conn">Подключение к базе данных MySQL <see cref="MySqlConnection"/></param>
        /// <returns></returns>
        public static bool IsExists(string url, MySqlConnection conn)
        {
            string sqlCommand = $"SELECT EXISTS(SELECT id FROM {Settings.Table} WHERE url = '{url}');";

            var command = new MySqlCommand(sqlCommand, conn);

            return command.ExecuteScalar().ToString() == "1";
        }

        /// <summary>
        /// Десериализация массива байтов в объект типа RecipeFull <see cref="RecipeFull"/>.
        /// </summary>
        /// <param name="arrBytes">Массив байтов.</param>
        /// <returns>Объект типа RecipeFull</returns>
        private static RecipeFull ByteArrayToRecipe(byte[] arrBytes)
        {
            MemoryStream memoryStream = new MemoryStream();

            memoryStream.Write(arrBytes, 0, arrBytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            NFX.Serialization.Slim.SlimSerializer slimSerializer =
                new NFX.Serialization.Slim.SlimSerializer();

            RecipeFull recipeFull = (RecipeFull)slimSerializer.Deserialize(memoryStream);

            return recipeFull;
        }

        /// <summary>
        /// Сериализация объекта в массив байтов.
        /// </summary>
        /// <param name="obj">Объект типа object.</param>
        /// <returns>Массив байтов.</returns>
        private static byte[] RecipeToByteArray(object obj)
        {
            if (obj == null)
                return null;

            using MemoryStream memoryStream = new MemoryStream();

            NFX.Serialization.Slim.SlimSerializer slimSerializer = new NFX.Serialization.Slim.SlimSerializer();
            slimSerializer.Serialize(memoryStream, obj);

            return memoryStream.ToArray();
        }
    }
}