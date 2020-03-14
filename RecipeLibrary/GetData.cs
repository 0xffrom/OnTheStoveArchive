using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RecipeLibrary.Objects;
using RecipeLibrary.Parser;
using RecipeLibrary.Parser.ParserPage.Core;
using RecipeLibrary.Parser.ParserPage.WebSites;
using RecipeLibrary.Parser.ParserRecipe.Core;
using RecipeLibrary.Parser.ParserRecipe.WebSites;

namespace RecipeLibrary
{
    public partial class GetData
    {
        private static readonly Random Rng =
            new Random((int) DateTime.Now.Ticks & 0x0000FFFF);


        public static async Task<List<RecipeShort>> GetPage(string section, int page, string findName = null)
        {
            List<RecipeShort> recipeShorts = new List<RecipeShort>();

            var povarenok = new ParserPage<RecipeShort[]>
                (new PovarenokPageParser(), new PovarenokPageSettings(section, page, findName));

            var povar = new ParserPage<RecipeShort[]>
                (new PovarPageParser(), new PovarPageSettings(section, page, findName));

            ParserPage<RecipeShort[]> edimdoma = new ParserPage<RecipeShort[]>
                (new EdimDomaPageParser(), new EdimDomaPageSettings(section, page, findName));

            await Task.WhenAll(ParseRecipe(edimdoma, recipeShorts), ParseRecipe(povarenok, recipeShorts), ParseRecipe(povar, recipeShorts));

            return recipeShorts;
        }

        private static async Task ParseRecipe(ParserPage<RecipeShort[]> T, List<RecipeShort> recipeShorts)
            => recipeShorts.AddRange(await T.Worker());

        public static async Task<RecipeFull> GetRecipe(string url)
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
                var recipe = new ParserRecipe<RecipeFull>(obj, settings);
                return await recipe.Worker();
            }
            catch (Exception e)
            {
                throw new ParserException("Произошла ошибка при парсинге рецепта. Подробности: " + e.Message);
            }

            #endregion
        }
    }
}