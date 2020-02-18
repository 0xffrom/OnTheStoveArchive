using RecipeLibrary.Objects;
using RecipeLibrary.Parser.ParserPage.Core;
using RecipeLibrary.Parser.ParserPage.WebSites;

namespace RecipeLibrary
{
    public partial class GetData
    {
        public void GetPage(string section, int page, string findName = null)
        {
            #region Povarenok.ru

            ParserPage<RecipeShort[]> povarenok = new ParserPage<RecipeShort[]>
                (new PovarenokPageParser(), new PovarenokPageSettings(section, page, findName));

            povarenok.OnNewData += Parser_OnNewData;
            povarenok.StartParsePage();

            #endregion

            #region Povar

            // TODO: Заменить фотографии.
            ParserPage<RecipeShort[]> povar = new ParserPage<RecipeShort[]>
                (new PovarPageParser(), new PovarPageSettings(section, page, findName));

            povar.OnNewData += Parser_OnNewData;
            povar.StartParsePage();

            #endregion
        }
    }
}