using System;
using ObjectsLibrary.Objects.Boxes.Elements;


namespace ObjectsLibrary.Objects.Boxes
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
