using XamarinApp.Library.Objects.Boxes.Elements;


namespace RecipesAndroid.Objects.Boxes
{
    /// <summary>
    /// Объект, который представляет собой набор картинок.
    /// </summary>
    public class PictureBox
    {
        public Picture[] Pictures { get; }

        public PictureBox(Picture[] pictures)
        {
            Pictures = pictures;
        }
    }
}