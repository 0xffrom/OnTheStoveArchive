using RecipeLibrary.Parser.ParserRecipe.Core;

namespace RecipeLibrary.Parser.ParserRecipe.WebSites
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