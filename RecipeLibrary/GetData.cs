﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ObjectsLibrary.Objects;
using ObjectsLibrary.Parser;
using ObjectsLibrary.Parser.ParserPage.Core;
using ObjectsLibrary.Parser.ParserPage.WebSites;
using ObjectsLibrary.Parser.ParserRecipe.Core;
using ObjectsLibrary.Parser.ParserRecipe.WebSites;

namespace ObjectsLibrary
{
    public  class GetData
    {


        public static async Task<RecipeShort[]> GetPage(string section, int page, string findName = null)
        {
            List<RecipeShort> recipeShorts = new List<RecipeShort>();

            var povarenok = new ParserPage<RecipeShort[]>
                (new PovarenokPageParser(), new PovarenokPageSettings(section, page, findName));

            var povar = new ParserPage<RecipeShort[]>
                (new PovarPageParser(), new PovarPageSettings(section, page, findName));

            ParserPage<RecipeShort[]> edimdoma = new ParserPage<RecipeShort[]>
                (new EdimDomaPageParser(), parserSettings: new EdimDomaPageSettings(section, page, findName));

            await Task.WhenAll(ParseRecipe(edimdoma, recipeShorts), ParseRecipe(povarenok, recipeShorts), ParseRecipe(povar, recipeShorts));

            return recipeShorts.OrderByDescending(x => x.IndexPopularity).ToArray();
        }

        private static async Task ParseRecipe(ParserPage<RecipeShort[]> T, List<RecipeShort> recipeShorts)
        {
            try
            {
                recipeShorts.AddRange(await T.Worker());
            }
            catch (ParserException exp)
            {
                Console.WriteLine($"Возникла ошибка при парсинге сайта {exp.WebSite}");
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Возникла ошибка: {exp}");
            }
        }

        public static async Task<RecipeFull> GetRecipe(string url)
        {
            IParserRecipe<RecipeFull> obj = null;
            IParserRecipeSettings settings = null;
            
            if (url.Contains("https://www.povarenok.ru"))
            {
                obj = new PovarenokRecipeParser();
                settings = new PovarenokRecipeSettings(url);
            }
            else if (url.Contains("https://povar.ru"))
            {
                 obj = new PovarRecipeParser();
                 settings = new PovarRecipeSettings(url);
            }
            else if (url.Contains("https://www.edimdoma.ru"))
            {
                obj = new EdimdomaRecipeParser();
                settings = new EdimdomaRecipeSettings(url);
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
                throw new ParserException("Произошла ошибка при парсинге рецепта. Подробности: " + e);
            }

            
        }
    }
}