using System;
using RecipeLibrary.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipeLibrary.Parser.ParserRecipe.Core;
using RecipeLibrary.Parser.ParserPage.Core;
using RecipeLibrary.Parser.ParserPage.povarenok.ru;

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