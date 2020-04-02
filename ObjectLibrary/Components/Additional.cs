using System;

namespace ObjectsLibrary.Components
{
    /// <summary>Дополнительная информация рецепта.</summary>
    [Serializable]
    public class Additional
    {
        /// <value>Имя автора.</value>
        public string AuthorName { get; set; }
        
         /// <value>Количество порций.</value>
        public int CountPortions { get; set; }
         
         /// <value>Количество минут для приготовления блюда.</value>
        public double PrepMinutes { get; set; }
         
        /// <see cref="CPFC"/>
        public CPFC CPFC { get; set; }

        public Additional()
        {
            //
        }

        public Additional(string authorName, int countPortions, double prepMinutes, CPFC cpfc) : this()
        {
            AuthorName = authorName;
            CountPortions = countPortions;
            PrepMinutes = prepMinutes;
            CPFC = cpfc;
        }

        /// <summary>
        /// Метод, извлекающий из строки количество минут.
        /// </summary>
        /// <param name="inputLine">Входная строка</param>
        /// <returns>Извлечённое из входной строки количество минут.</returns>
        public static double ConvertToMinutes(string inputLine)
        {
            // 1 час и 10 минут.
            inputLine = inputLine.Replace(" и", String.Empty);

            string[] arrayWords = inputLine.Split(' ');

            double minutes = 0;

            for (int i = 0; i < arrayWords.Length; i += 2)
            {
                if (arrayWords[i].Contains("мин"))
                    minutes += int.Parse(arrayWords[i + 1]);
                else if (arrayWords[i].Contains("час"))
                    minutes += int.Parse(arrayWords[i + 1]) * 60;
            }

            return minutes;
        }
    }
}