using ObjectsLibrary.Parser.ParserRecipe.Core;

namespace ObjectsLibrary.Parser.ParserRecipe.WebSites
{
    public class EdimdomaRecipeSettings : IParserRecipeSettings
    {
        public string Url { get; }

        public EdimdomaRecipeSettings(string url)
        {
            Url = url;
        }
    }
}