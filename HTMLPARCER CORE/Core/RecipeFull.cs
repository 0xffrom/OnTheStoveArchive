namespace HTMLPARCER_CORE
{
    public class RecipeFull
    {
        public RecipeFull(string webSite, string title, string titlePicture, string url,
            Ingredient[] ingredients, StepRecipe[] stepsOfRecipe, string introductionContent, string endContent)
        {
            WebSite = webSite;
            Title = title;
            TitlePicture = titlePicture;
            Url = url;
            Ingredients = ingredients;
            StepsOfRecipe = stepsOfRecipe;
            IntroductionContent = introductionContent;
            EndContent = endContent;
        }

        public string WebSite { get; }
        public string Title { get; }
        public string TitlePicture { get; }
        public string Url { get; }
        public Ingredient[] Ingredients { get; }
        public StepRecipe[] StepsOfRecipe { get; }
        public string IntroductionContent { get; }
        public string EndContent { get; }



    }
}