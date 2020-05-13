
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjectsLibrary.Components
{
    /// <summary>Дополнительная информация рецепта.</summary>
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class Additional
    {
        /// <value>Имя автора.</value>
        public string AuthorName { get;  }

        /// <value>Количество порций.</value>
        public int CountPortions { get;  }

        /// <value>Количество минут для приготовления блюда.</value>
        public double PrepMinutes { get;  }

        /// <see cref="CPFC"/>
        public CPFC CPFC { get;  }
        /// <value>Интернет адрес на видео.</value>
        public string VideoUrl { get;  }

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

        public Additional(string authorName, int countPortions, double prepMinutes,CPFC cpfc, string videoUrl) : 
            this(authorName, countPortions, prepMinutes, cpfc)
        {
            VideoUrl = videoUrl;
        }

    }
}