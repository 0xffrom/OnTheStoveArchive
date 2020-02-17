using System;
using RecipeLibrary.Objects;
using System.Collections.Generic;
using RecipeLibrary.Parse;
using RecipeLibrary.Parser.ParserPage.Core;
using RecipeLibrary.Parser.ParserPage.povarenok.ru;
using RecipeLibrary.Parser.ParserRecipe.Core;
using RecipeLibrary.Parser.ParserRecipe.WebSites;

namespace RecipeLibrary
{
    public class GetData
    {
        public List<RecipeShort> RecipeShorts { get; private set; } = new List<RecipeShort>();

        private int countOfSites = 1;
        public bool IsCompleted = false;

        private void Parser_OnNewData(object arg, RecipeShort[] list)
        {
            countOfSites--;
            foreach (var item in list)
                RecipeShorts.Add(item);
            if (countOfSites == 0)
                IsCompleted = true;
        }

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

        public void GetPage(string section, int page, string findName = null)
        {
            #region Povarenok.ru

            ParserPage<RecipeShort[]> povarenok = new ParserPage<RecipeShort[]>
                (new PovarenokPageParser(), new PovarenokPageSettings(section, page, findName));
            povarenok.OnNewData += Parser_OnNewData;
            povarenok.StartParsePage();

            #endregion
        }
    }
}