using RecipesAndroid.Objects.Boxes;
using RecipesAndroid.Objects.Boxes.Elements;

namespace RecipesAndroid.Objects
{
    /// <summary>
    /// ������ ������������ �� ���� ������������� ������� ����������� �������, � ������� ������:
    /// �) ���������.
    /// �) ������������ ��������.
    /// �) ��������� ��������, �������������� ����������� ������������ ����.
    /// �) ��������� ��������, �������������� ���� ����������� �������.
    /// �) �������������� �������� �������.
    /// </summary>
    public class RecipeFull
    {
        public string Url { get; }
        public string Title { get; }
        public Picture TitlePicture { get; }
        public string Description { get; }
        public IngredientBox[] IngredientsBoxes { get; }
        public StepRecipeBox[] StepRecipesBoxes { get; }
        public AdditionalBox Additional { get; }

        public RecipeFull(string url,
            string title,
            Picture picture,
            string description,
            IngredientBox[] ingredientsBoxes,
            StepRecipeBox[] stepRecipeBoxes,
            AdditionalBox additional = null)

        {
            Url = url;
            Title = title;
            TitlePicture = picture;
            Description = description;
            IngredientsBoxes = ingredientsBoxes;
            StepRecipesBoxes = stepRecipeBoxes;
            Additional = additional;
        }
    }
}