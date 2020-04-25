using ObjectsLibrary;
using ObjectsLibrary.Parser;
using ObjectsLibrary.Parser.ParserPage.Core;
using ObjectsLibrary.Parser.ParserPage.WebSites;
using ObjectsLibrary.Parser.ParserRecipe.Core;
using ObjectsLibrary.Parser.ParserRecipe.WebSites;
using RecipeLibrary.Parser.ParserPage.WebSites;
using RecipeLibrary.Parser.ParserRecipe.WebSites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeLibrary
{
    public class GetData
    {
        public static async Task<RecipeShort[]> GetPage(string section, int page, string findName = null)
        {
            List<RecipeShort> recipeShorts = new List<RecipeShort>();

            var povarenok = new ParserPage<RecipeShort[]>
                (new PovarenokPageParser(), new PovarenokPageSettings(section, page, findName));

            var povar = new ParserPage<RecipeShort[]>
                (new PovarPageParser(), new PovarPageSettings(section, page, findName));

            var edimdoma = new ParserPage<RecipeShort[]>
                (new EdimDomaPageParser(), new EdimDomaPageSettings(section, page, findName));

            await Task.WhenAll(ParseRecipe(edimdoma, recipeShorts), ParseRecipe(povarenok, recipeShorts), ParseRecipe(povar, recipeShorts));

            return recipeShorts.OrderByDescending(x => x.IndexPopularity).ToArray();
        }

        private static async Task ParseRecipe(ParserPage<RecipeShort[]> T, List<RecipeShort> recipeShorts)
            => recipeShorts.AddRange(await T.Worker());

        public static async Task<RecipeFull> GetRecipe(string url)
        {
            IParserRecipe<RecipeFull> obj = null;
            IParserRecipeSettings settings = null;

            if (url.Contains("www.povarenok.ru"))
            {
                obj = new PovarenokRecipeParser();
                settings = new PovarenokRecipeSettings(url);
            }
            else if (url.Contains("povar.ru"))
            {
                obj = new PovarRecipeParser();
                settings = new PovarRecipeSettings(url);
            }
            else if (url.Contains("www.edimdoma.ru"))
            {
                obj = new EdimdomaRecipeParser();
                settings = new EdimdomaRecipeSettings(url);
            }
            else
                throw new ParserException("Неизвестный сайт.");
            
            var recipe = new ParserRecipe<RecipeFull>(obj, settings);
            return await recipe.Worker();
            


        }
    }
}