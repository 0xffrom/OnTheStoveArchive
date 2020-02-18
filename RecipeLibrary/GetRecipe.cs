using System;
using RecipeLibrary.Objects;
using RecipeLibrary.Parser;
using RecipeLibrary.Parser.ParserRecipe.Core;
using RecipeLibrary.Parser.ParserRecipe.WebSites;

namespace RecipeLibrary
{
    public partial class GetData
    {
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
            else if (url.Contains("htpps://povar.ru"))
            {
                // obj = new PovarRecipeParser();
                // settings = new PovarRecipeSettings(url);
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
    }
}