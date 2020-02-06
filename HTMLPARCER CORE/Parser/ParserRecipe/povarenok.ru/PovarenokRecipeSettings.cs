namespace RecipeLibrary.ParseRecipe
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