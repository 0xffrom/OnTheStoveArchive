using RecipeLibrary.Parser.ParserRecipe.Core;

namespace RecipeLibrary.Parser.ParserRecipe.WebSites
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