using RecipeLibrary.Objects;
using System.Collections.Generic;

namespace RecipeLibrary
{
    class GetRecipe
    {
        static void Main(string[] args)
        {
            /*
            string url;
            Console.Write("Введите ссылку: ");
            url = Console.ReadLine();
            var test = new ParserWorker<RecipeFull[]>(new EdimdomaPageParser());

            test.Settings = new EdimdomaPageSettings(url);
            test.Start();
            test.OnNewData += Parser_OnNewData;
            */


        }

        public static int countOfPages = 1;
        public static List<string> count = new List<string>();

        private static void Parser_OnNewData(object arg1, RecipeFull[] list)
        {

        }




    }
}
