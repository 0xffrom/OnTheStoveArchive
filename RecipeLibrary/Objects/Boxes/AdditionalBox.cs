using System;
using RecipeLibrary.Objects.Boxes.Elements;


namespace RecipeLibrary.Objects.Boxes
{

    [Serializable]
    public class AdditionalBox
    {
        public string Description { get; set; }
        public PictureBox PictureBox { get; set; }
        public Video Video { get; set; }

        public AdditionalBox(string description = null, PictureBox pictureBox = null, Video video = null)
        {
            Description = description;
            PictureBox = pictureBox;
            Video = video;
        }
    }
}
