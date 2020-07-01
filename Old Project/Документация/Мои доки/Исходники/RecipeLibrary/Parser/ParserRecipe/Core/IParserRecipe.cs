using AngleSharp.Html.Dom;

namespace ObjectsLibrary.Parser.ParserRecipe.Core
{
    internal interface IParserRecipe<out T> where T : RecipeFull
    {
        /// <summary>
        /// Преобразует исходный <see cref="IHtmlDocument"/> IHtmlDocument в объект типа RecipeFull.
        /// </summary>
        /// <param name="document">Исходная веб-страница.</param>
        /// <param name="settings">Настройки для парсинга веб странички.</param>
        /// <returns>Объект вида <see cref="RecipeFull"/> RecipeFull</returns>
        T Parse(IHtmlDocument document, IParserRecipeSettings settings);

        /// <summary>
        /// Преобразует входную строку и извлекает из неё количество минут,
        /// требуемое для приготовления блюда.
        /// Например: 10 часов 5 минут, 32 мин, 1 ч. и 10 мин и т.д
        /// </summary>
        /// <param name="inputLine">Входная строка, содержащия время приготовлени кулинарного рецепта.</param>
        /// <returns>Количество минут.</returns>
        double ConvertToMinutes(string inputLine);
    }
}