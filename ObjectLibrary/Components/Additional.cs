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


    }
}