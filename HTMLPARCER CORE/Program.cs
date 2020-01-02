
using HTMLPARCER_CORE.Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HTMLPARCER_CORE
{
    class Program
    {
        static List<string> ListTitles = new List<string>();
        public 
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            do
            {
                ParserWorker<RecipeShort[]> parser =
                    new ParserWorker<RecipeShort[]>(new EdaParser());

                parser.Settings = new EdaSettings("Оливье");
                parser.Start();

                parser.OnNewData += Parser_OnNewData;
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }

        public static int count = 1;
        private static void Parser_OnNewData(object arg1, RecipeShort[] list)
        {
            foreach (var item in list)
            {
                
                File.AppendAllText(@"edaOlivie.txt", ($"Название: {item.Title}\nОписание: {item.ShortDescription}\nКартинка: {item.UrlPicture}\nСсылка: {item.Url}\n\n"));
               
            }
            Console.WriteLine($"Добавлена страница №{count++}. Кол-во: {list.Length}");
        }

       

    }
}
