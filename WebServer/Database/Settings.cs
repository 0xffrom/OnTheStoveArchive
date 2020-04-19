namespace WebServer.DataBase

{
    public static class Settings
    {
        public const string Host = "localhost";

        public const string Port = "3306";

        public const string User = "root";

        public const string Password = "password";

        public const string Database = "recipes";

        // Таблица вида:
        // TABLE recipesFull (id INT NOT NULL AUTO_INCREMENT PRIMARY KEY, url TEXT NOT NULL, date DATETIME, recipe BLOB);
        public const string Table = "recipesFull";

        // Период обновления рецепта в часах.
        public const int HourDiff = 24;

        // Строка подключения к базе данных MySQL.
        public static string GetStringConnection() => "Server=" + Host + ";Database=" + Database + ";port=" + Port +
                                                      ";User Id=" + User + ";password=" + Password + ";charset=utf8;";
    }
}