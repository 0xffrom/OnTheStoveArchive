using ObjectsLibrary.Parser.ParserRecipe.Core;

namespace ObjectsLibrary.Parser.ParserRecipe.WebSites
{
    public class EdimDomaRecipeSettings : IParserRecipeSettings
    {
        public string Url { get; }

        public EdimDomaRecipeSettings(string url)
        {
            Url = url;
        }
    }
}