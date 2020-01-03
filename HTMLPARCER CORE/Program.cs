
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
                    new ParserWorker<RecipeShort[]>(new EdaParser());

                EdaParser.Settings = new EdaSettings(100000);
                EdaParser.Start();

                EdaParser.OnNewData += Parser_OnNewData;
                /*
                var PovarParser =
                  new ParserWorker<RecipeShort[]>(new PovarParser());

                PovarParser.Settings = new PovarSettings(100000);
                PovarParser.Start();

                PovarParser.OnNewData += Parser_OnNewData;

                var Povarenok =
                      new ParserWorker<RecipeShort[]>(new PovarenokParser());

                Povarenok.Settings = new PovarenokSettings(100000);
                Povarenok.Start();

                Povarenok.OnNewData += Parser_OnNewData;

                var russiaFood =
                     new ParserWorker<RecipeShort[]>(new RussianfoodParser());

                russiaFood.Settings = new RussianfoodSettings(100000);
                russiaFood.Start();

                russiaFood.OnNewData += Parser_OnNewData;

                var vkuso =
                  new ParserWorker<RecipeShort[]>(new VkusoParser());

                vkuso.Settings = new VkusoSettings(100000);
                vkuso.Start();

                vkuso.OnNewData += Parser_OnNewData;
                */
            }

            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }

        public static int countOfPages = 0;
        public static int countOfRecipes = 0;
        private static void Parser_OnNewData(object arg1, RecipeShort[] list)
        {
            foreach (var item in list)
            {
                
                File.AppendAllText(@$"recipes/{item.WebSite}.txt", ($"Название: {item.Title}\nКартинка: {item.UrlPicture}\nСсылка: {item.Url}\n\n"));
                
            }
            countOfRecipes += list.Length;
            Console.WriteLine($"Всего страниц:{countOfPages++}. Всего рецептов: {countOfRecipes}");
        }

       

    }
}
