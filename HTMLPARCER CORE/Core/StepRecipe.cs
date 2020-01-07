namespace HTMLPARCER_CORE
{
    public class StepRecipe
    {
        public StepRecipe(string description, string urlPicture)
        {
            Description = description;
            UrlPicture = urlPicture;
        }
        public string UrlPicture { get; }
        public string Description { get; }
    }
}