namespace AndroidApp.General
{
    public class Plate
    {
        public string Title { get; }
        public string Background { get; }
        public string Key { get; }
        
        public Plate(string title, string background, string key) 
        {
            Title = title;
            Background = background;
            Key = key;
        }
    }
}