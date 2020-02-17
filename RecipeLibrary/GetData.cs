using System;
using System.Collections.Generic;
using System.Linq;
using RecipeLibrary.Objects;
using RecipeLibrary.Parser;
using RecipeLibrary.Parser.ParserPage.Core;
using RecipeLibrary.Parser.ParserRecipe.Core;
using RecipeLibrary.Parser.ParserPage.WebSites;
using RecipeLibrary.Parser.ParserRecipe.WebSites;

namespace RecipeLibrary
{
    public class GetData
    {
        public bool IsCompleted = false;

        public RecipeFull RecipeFull;

        private void Parser_OnNewData(object arg, RecipeFull recipeFull)
        {
            RecipeFull = recipeFull;
            IsCompleted = true;
        }

        public void GetRecipe(string url)
        {
            #region Povarenok.ru

            IParserRecipe<RecipeFull> obj = null;
            IParserRecipeSettings settings = null;

            if (url.Contains("https://www.povarenok.ru"))
            {
                obj = new PovarenokRecipeParser();
                settings = new PovarenokRecipeSettings(url);
            }
            else
                throw new ParserException("Неизвестный сайт.");

            try
            {
                ParserRecipe<RecipeFull> recipe = new ParserRecipe<RecipeFull>(obj, settings);

                recipe.OnNewData += Parser_OnNewData;
                recipe.StartParseRecipe();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception. Message: {e.Message}. Source: {e.Source}");
                IsCompleted = true;
            }

            #endregion
        }


        public List<RecipeShort> RecipeShorts { get; private set; } = new List<RecipeShort>();

        private int countOfSites = 2;

        private static Random rng = new Random((int) DateTime.Now.Ticks & 0x0000FFFF);

        private void Parser_OnNewData(object arg, RecipeShort[] list)
        {
            countOfSites--;
            
            foreach (var item in list)
                RecipeShorts.Add(item);
            
            if (countOfSites != 0) return;
            
            // Random Sort
            RecipeShorts = RecipeShorts.Select(i => new {I = i, sort = rng.Next()}).OrderBy(i => i.sort)
                .Select(i => i.I).ToList();
            
            IsCompleted = true;

        }


        public void GetPage(string section, int page, string findName = null)
        {
            #region Povarenok.ru

            ParserPage<RecipeShort[]> povarenok = new ParserPage<RecipeShort[]>
                (new PovarenokPageParser(), new PovarenokPageSettings(section, page, findName));

            povarenok.OnNewData += Parser_OnNewData;
            povarenok.StartParsePage();

            ParserPage<RecipeShort[]> povar = new ParserPage<RecipeShort[]>
                (new PovarPageParser(), new PovarPageSettings(section, page, findName));

            povar.OnNewData += Parser_OnNewData;
            povar.StartParsePage();

            #endregion
        }
    }
}