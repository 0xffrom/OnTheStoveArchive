namespace XamarinApp.General
{
    public class Plate
    {
        public string Title { get; }
        public string Background { get; }
        public string Key { get;  }
        public Plate()
        {
            //
        }

        public Plate(string title, string background, string key) : this()
        {
            Title = title;
            Background = background;
            Key = key;
        }
    }
}