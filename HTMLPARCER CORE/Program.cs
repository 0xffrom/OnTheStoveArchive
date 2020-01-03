
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
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            do
            {
                var EdaParser =
                    new ParserWorker<RecipeShort[]>(new VkosoParser());

                EdaParser.Settings = new VkusoSettings(1000);
                EdaParser.Start();

                EdaParser.OnNewData += Parser_OnNewData;

            }

            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }

        public static int count = 1;
        private static void Parser_OnNewData(object arg1, RecipeShort[] list)
        {
            foreach (var item in list)
            {
                
                File.AppendAllText(@$"{item.WebSite}.txt", ($"Название: {item.Title}\nКартинка: {item.UrlPicture}\nСсылка: {item.Url}\n\n"));
               
            }
            Console.WriteLine($"Добавлена страница №{count++}. Кол-во: {list.Length}");
        }

       

    }
}
