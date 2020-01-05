namespace HTMLPARCER_CORE
{
    public class RecipeFull
    {
        public string WebSite { get; }
        public string Title { get; }
        public string UrlPicture { get; }
        public string Url { get; }
        public Ingrident[] Ingridents { get; }
        public StepRecipe[] StepsOfRecipe { get; }
        public string Descriprion { get; }
        public string Recipe { get; }

    }

    public class Ingrident
    {
        Ingrident(string name, int count)
        {
            Name = name;
            Count = count;
        }
        
        public string Name { get; }
        public int Count { get; }
    }

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