using ObjectsLibrary.Parser.ParserRecipe.Core;

namespace ObjectsLibrary.Parser.ParserRecipe.WebSites
{
    public class PovarRecipeSettings : IParserRecipeSettings
    {
        public string Url { get; }

        public PovarRecipeSettings(string url)
        {
            Url = url;
        }
    }
}