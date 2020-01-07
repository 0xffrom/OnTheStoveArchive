
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
                var test =
                       new ParserWorker<RecipeFull[]>(new PovarenokParserPage());

                test.Settings = new PovarenokPageSettings("https://www.povarenok.ru/recipes/show/25691/");
                test.Start();
                test.OnNewData += Parser_OnNewData;
                
                

            }

            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }

        public static int countOfPages = 1;
        public static List<string> count = new List<string>();

        private static void Parser_OnNewData(object arg1, RecipeFull[] list)
        {
            Console.WriteLine("the end");
        }
        private static void Parser_OnNewData(object arg1, RecipeShort[] list)
        {

            foreach (var item in list)
            {
                try
                {
                    File.AppendAllText(@$"{item.WebSite}-url.txt", ($"{item.Url}\n"));

                }
                catch(Exception)
                {
                    File.AppendAllText(@$"{item.WebSite}-url.txt", ($"{item.Url}\n"));
                }
            }
            Console.WriteLine($"Всего страниц: {countOfPages++}");
        }

       

    }
}
