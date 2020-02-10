using System;
using RecipeLibrary.Objects;
using System.Collections.Generic;
using RecipeLibrary.Parser.ParserRecipe.Core;
using RecipeLibrary.Parser.ParserPage.Core;
using RecipeLibrary.Parser.ParserPage.povarenok.ru;

namespace RecipeLibrary
{
    public class GetData
    {
        public List<RecipeShort> RecipeShorts { get; private set; } = new List<RecipeShort>();
        private RecipeFull _recipeFull;

        private const int countOfSites = 1;
        private int count = 0;

        public bool isSuccesful = false;

        private void Parser_OnNewData(object arg, RecipeShort[] list)
        {
            count++;
            foreach (var item in list)
                RecipeShorts.Add(item);

            if (count == countOfSites)
                isSuccesful = true;
        }

        // response example: ....code=popular&page=1&findName=Чебурек
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

    class GetRecipe
    {
    }
}