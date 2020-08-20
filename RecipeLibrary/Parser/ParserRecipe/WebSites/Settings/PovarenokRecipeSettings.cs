using ObjectsLibrary.Parser.ParserRecipe.Core;

namespace ObjectsLibrary.Parser.ParserRecipe.WebSites
{
    public class PovarenokRecipeSettings : IParserRecipeSettings
    {
        public string Url { get; }


        public PovarenokRecipeSettings(string url)
        {
            Url = url;
        }
    }
}