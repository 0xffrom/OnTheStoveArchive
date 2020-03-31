using System;
using RecipeLibrary.Objects.Boxes.Elements;


namespace RecipeLibrary.Objects.Boxes
{
    [Serializable]
    public class PictureBox
    {
        public Picture[] Pictures { get; set; }

        public PictureBox(Picture[] pictures)
        {
            Pictures = pictures;
        }
    }
}
