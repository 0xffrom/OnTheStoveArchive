using System;
using RecipeLibrary.Objects;
using System.Collections.Generic;
using RecipeLibrary.Parser.ParserRecipe.Core;
using  RecipeLibrary.Parser.ParserPage.Core;
using RecipeLibrary.Parser.ParserPage.povarenok.ru;

namespace RecipeLibrary
{
    class GetPage
    {
        private static void Parser_OnNewData(object arg1, RecipeShort[] list)
        {
            
        }
        
        // response example: ....code=popular&page=1&findName=Чебурек
        public GetPage(string section, int page, string findName = null)
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
