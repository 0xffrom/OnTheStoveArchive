using RecipeLibrary.Objects.Boxes.Elements;


namespace RecipeLibrary.Objects.Boxes
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
