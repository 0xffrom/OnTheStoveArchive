
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
                var tvoiRecept =
                       new ParserWorker<RecipeShort[]>(new TvoireceptyParser());

                tvoiRecept.Settings = new TvoireceptySettings(10000);
                tvoiRecept.Start();

                tvoiRecept.OnNewData += Parser_OnNewData;
                
                

            }

            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }

        public static int countOfPages = 1;
        public static int countOfRecipes = 0;
        public static List<string> count = new List<string>();
        private static void Parser_OnNewData(object arg1, RecipeShort[] list)
        {

            foreach (var item in list)
            {
                try
                {
                    count.Add(item.WebSite);
                    int countOfSite = count.Where(i => (i == item.WebSite)).ToArray().Length;
                    File.AppendAllText(@$"recipes/{item.WebSite}.txt", ($"Рецепт №{countOfSite}\nНазвание: {item.Title}\nКартинка: {item.UrlPicture}\nСсылка: {item.Url}\n\n"));

                }
                catch(Exception)
                {
                    File.AppendAllText(@$"recipes/{item.WebSite} - error.txt", ($"Название: {item.Title}\nКартинка: {item.UrlPicture}\nСсылка: {item.Url}\n\n"));
                }
            }
            countOfRecipes += list.Length;
            Console.WriteLine($"Всего страниц:{countOfPages++}. Всего рецептов: {countOfRecipes}");
        }

       

    }
}
