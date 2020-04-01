using RecipeLibrary.Parser.ParserRecipe.Core;

namespace RecipeLibrary.Parser.ParserRecipe.WebSites
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