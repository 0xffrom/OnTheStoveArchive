
using HTMLPARCER_CORE.Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace HTMLPARCER_CORE
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            do
            {
                
                string url;

                string[] lines;

                lines = File.ReadAllLines("povarenok.ru.txt");

                for (int i = 0; i < lines.Length; i++)
                {
                    url = lines[i];
                    var test = new ParserWorker<RecipeFull[]>(new PovarenokParserPage());

                    test.Settings = new PovarenokPageSettings(url);
                    test.Start();
                    test.OnNewData += Parser_OnNewData;
                    Thread.Sleep(2000);
                }
                

            }

            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }

        public static int countOfPages = 1;
        public static List<string> count = new List<string>();

        private static void Parser_OnNewData(object arg1, RecipeFull[] list)
        {
            RecipeFull element = list[0];
            string fileName = $"{element.WebSite} - Full Recipes.txt";
            File.AppendAllText(fileName, 
                $"Recipe:\n" +
                $"Url: {element.Url}\n" +
                $"Website: {element.WebSite}\n" +
                $"Title Picture: {element.TitlePicture}\n" +
                $"Intoduction:{element.IntroductionContent}\n");
            foreach (var item in element.Ingredients)
                File.AppendAllText(fileName, $"{item.Title}: {item.Name} ---- {item.Unit}\n");

            File.AppendAllText(fileName, $"Steps of Recipe:\n");

            for (int i = 0; i < element.StepsOfRecipe.Length; i++)
            {
                File.AppendAllText(fileName, $"№{i+1}. Picture: {element.StepsOfRecipe[i].UrlPicture}\n" +
                    $"Description: {element.StepsOfRecipe[i].Description}\n");
            }

            File.AppendAllText(fileName, $"End Content: {element.EndContentText}\nPictures:\n");

            foreach (var item in element.EndContentPictures)
            {
                File.AppendAllText(fileName, $"{item}\n");
            }
            File.AppendAllText(fileName, $"\n==============================================\n");
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
