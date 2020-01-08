
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

            while(true)
            {

                string url;
                Console.Write("Введите ссылку: ");
                url = Console.ReadLine();
                var test = new ParserWorker<RecipeFull[]>(new PovarenokParserPage());

                test.Settings = new PovarenokPageSettings(url);
                test.Start();
                test.OnNewData += Parser_OnNewData;



            }

        }

        public static int countOfPages = 1;
        public static List<string> count = new List<string>();

        private static void Parser_OnNewData(object arg1, RecipeFull[] list)
        {
            RecipeFull element = list[0];
            Console.WriteLine(
                $"Recipe:\n" +
                $"Url: {element.Url}\n" +
                $"Website: {element.WebSite}\n" +
                $"Title Picture: {element.TitlePicture}\n" +
                $"Intoduction:{element.IntroductionContent}\n");
            foreach (var item in element.Ingredients)
                Console.WriteLine($"{item.Title}: {item.Name} ---- {item.Unit}\n");

            Console.WriteLine($"Steps of Recipe:\n");

            for (int i = 0; i < element.StepsOfRecipe.Length; i++)
            {
                Console.WriteLine($"№{i + 1}. Picture: {element.StepsOfRecipe[i].UrlPicture}\n" +
                    $"Description: {element.StepsOfRecipe[i].Description}\n");
            }

            Console.WriteLine($"End Content: {element.EndContentText}\nPictures:\n");

            foreach (var item in element.EndContentPictures)
            {
                Console.WriteLine($"{item}\n");
            }
            Console.WriteLine($"\n==============================================\n");
        }
        private static void Parser_OnNewData(object arg1, RecipeShort[] list)
        {

            foreach (var item in list)
            {
                try
                {
                    File.AppendAllText(@$"{item.WebSite}-url.txt", ($"{item.Url}\n"));

                }
                catch (Exception)
                {
                    File.AppendAllText(@$"{item.WebSite}-url.txt", ($"{item.Url}\n"));
                }
            }
            Console.WriteLine($"Всего страниц: {countOfPages++}");
        }



    }
}
