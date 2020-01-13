
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
                Console.Write("Введите ссылку: ");
                url = Console.ReadLine();
                var test = new ParserWorker<RecipeFull[]>(new EdimdomaPageParser());

                test.Settings = new EdimdomaPageSettings(url);
                test.Start();
                test.OnNewData += Parser_OnNewData;



            }

            while (Console.ReadKey(true).Key != ConsoleKey.Escape);

        }

        public static int countOfPages = 1;
        public static List<string> count = new List<string>();

        private static void Parser_OnNewData(object arg1, RecipeFull[] list)
        {
            RecipeFull element = list[0];
            string fileName = $"{element.WebSite} - Full Recipes.txt";
            Console.Write(
                $"Recipe:\n" +
                $"Url: {element.Url}\n" +
                $"Website: {element.WebSite}\n" +
                $"Title: {element.Title} \n" +
                $"Title Picture: {element.TitlePicture}\n" +
                $"Intoduction:{element.IntroductionContent}\n");
            foreach (var item in element.Ingredients)
                Console.Write($"{item.Title}: {item.Name} ---- {item.Unit}\n");

            Console.Write($"Steps of Recipe:\n");

            for (int i = 0; i < element.StepsOfRecipe.Length; i++)
            {
                Console.Write($"№{i + 1}. Picture: {element.StepsOfRecipe[i].UrlPicture}\n" +
                    $"Description: {element.StepsOfRecipe[i].Description}\n");
            }

            Console.Write($"End Content: {element.EndContentText}\nPictures:\n");

            foreach (var item in element.EndContentPictures)
            {
                Console.Write($"{item}\n");
            }
            Console.Write("Videos:\n");
            foreach (var item in element.ContentVideos)
            {
                Console.Write($"{item}\n");
            }
            Console.Write($"\n==============================================\n");
        }
       



    }
}
