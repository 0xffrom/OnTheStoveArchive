
using System;

namespace RecipeLibrary.Objects.Boxes
{
    [Serializable]
    public class StepRecipeBox
    {
        public PictureBox PictureBox { get; set; }
        public string Description { get; set; }

        public StepRecipeBox(string description, PictureBox pictureBox)
        {
            Description = description;
            PictureBox = pictureBox;
        }


    }
}