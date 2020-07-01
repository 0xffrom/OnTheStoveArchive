using ObjectsLibrary.Parser.ParserRecipe.Core;

namespace ObjectsLibrary.Parser.ParserRecipe.WebSites
{
    public class EdaRecipeSettings : IParserRecipeSettings
    {
        public string Url { get; }

        public EdaRecipeSettings(string url)
        {
            Url = url;
        }
    }
}