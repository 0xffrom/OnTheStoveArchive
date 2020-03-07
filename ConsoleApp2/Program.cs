using System;
using RecipesAndroid;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = HttpGet.GetRecipes();
            foreach (var item in list)
            {
                Console.WriteLine(item.Title);
            }
        }
    }
}
