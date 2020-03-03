using RecipesAndroid.Objects.Boxes.Elements;


namespace RecipesAndroid.Objects.Boxes
{
    /// <summary>
    /// Данный объект представляет собый набор из дополнительной информации для рецепта.
    /// В доп.информацию входят:
    /// а) Описание рецепта
    /// б) Набор картинок.
    /// с) Видео.
    /// 
    /// Замечание: любой из подобъектов является необязательным.
    /// </summary>
    public class AdditionalBox
    {
        public string Description { get; }
        public PictureBox PictureBox { get; }
        public Video Video { get; }

        public AdditionalBox(string description = null, PictureBox pictureBox = null, Video video = null)
        {
            Description = description;
            PictureBox = pictureBox;
            Video = video;
        }
    }
}
