using RecipeLibrary.Parser.ParserRecipe.Core;

namespace RecipeLibrary.Parser.ParserRecipe.povarenok.ru
{
    public class PovarenokRecipeSettings : IParserRecipeSettings
    {
        public string Url { get; }

        PovarenokRecipeSettings(string url)
        {
            Url = url;
        }
    }
}