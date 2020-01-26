/*
using System;
using System.Collections.Generic;
using System.Text;

namespace HTMLPARCER_CORE.Parse.WebSites.povarenok.ru
{
    public class PovarenokSettings : IParserSettings
    {
        public PovarenokSettings(int count)
        {
            MaxPage = count;
        }

        public PovarenokSettings(int minPage, int maxPage)
        {
            MinPage = minPage;
            MaxPage = maxPage;
        }

        public PovarenokSettings(string recipe, int minPage, int maxPage) : this(minPage, maxPage)
        {
            Recipe = GetRecipe(recipe);
        }

        public PovarenokSettings()
        {
            MaxPage = 1;
        }

        /// <summary>
        /// Метод для смены кодировки URL с UTF-8 на URL-encoding.
        /// </summary>
        /// <param name="recipe">Название рецепта в кодировке UTF-8.</param>
        /// <returns>Название рецепта в кодировке URL-encoding.</returns>
        public string GetRecipe(string recipe)
        {
            char[] alph = {'а','б','в','г','д','е','ё','ж','з',
                           'и','й','к','л','м','н','о','п','р',
                           'с','т','у','ф','х','ц','ч','ш','щ',
                           'ъ','ы','ь','э','ю','я'};

            string[] urlSymbols = {"E0","E1","E2","E3","E4","E5","B8","E6",
                                   "E7","E8","E9","EA","EB","EC","ED","EE",
                                   "EF","F0","F1","F2","F3","F4","F5","F6",
                                   "F7","F8","F9","FA","FB","FC","FD","FE",
                                   "FF"};
            string newStr = "%";
            recipe = recipe.ToLower();
            foreach (char symb in recipe)
                newStr += urlSymbols[Array.IndexOf(alph, symb)] + "%";

            newStr = newStr.Remove(newStr.Length - 1);

            return newStr;
        }


        public PovarenokSettings(string recipe) : this()
        {
            Recipe = GetRecipe(recipe);
        }

        public PovarenokSettings(string recipe, int count) : this(recipe)
        {
            MaxPage = count;
        }

        public string BaseUrl { get; set; } = "https://www.povarenok.ru/recipes";
        public string Prefix { get; set; } = "~{CurrentId}";
        public string PrefixFind { get; set; } = "search/~{CurrentIdPage}/?name=";
        public string SectionUrl { get; set; } = "/category/{CurrentIdSection}/~{CurrentIdPage}/";

        public string Recipe { get; set; }
        public int MinPage { get; set; } = 1;
        public int MaxPage { get; set; } = 0;
        public int MaxCountPage { get; set; } = 9202;

        public int SoupId { get; set; } = 2;
        public int HotMealsId { get; set; } = 5;
        public int SaladsId { get; set; } = 12;
        public int SnacksId { get; set; } = 15;
        public int BakeryId { get; set; } = 25;
        public int DessertId { get; set; } = 30;
        public int SauceId { get; set; } = 23;


    }
}
*/