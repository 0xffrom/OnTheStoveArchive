namespace WebServer.DataBase

{
    public static class Settings
    {
        public const string Host = "localhost";

        public const string Port = "3306";

        public const string User = "root";

        public const string Password = "password";

        public const string Database = "recipes";

        public const string Table = "recipes";

        // Период обновления рецепта.
        public const int HourDiff = 24;

        public static string GetStringConnection() => "Server=" + Host + ";Database=" + Database + ";port=" + Port +
                                                      ";User Id=" + User + ";password=" + Password + ";charset=utf8;";
    }
}