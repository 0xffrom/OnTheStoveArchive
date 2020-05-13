
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjectsLibrary.Components
{
    /// <summary>Калории, белки, жиры, углеводы.</summary>
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class CPFC
    {
        /// <value>Количество калорий.</value>
        public double Calories { get; }

        /// <value>Количество белка.</value>
        public double Proteins { get; }

        /// <value>Количество жиров.</value>
        public double Fats { get;}

        /// <value>Количество углеводов.</value>
        public double Carbohydrates { get; }

        public CPFC()
        {
            //
        }

        public CPFC(double calories, double proteins, double fats, double carbohydrates) : this()
        {
            Calories = calories;
            Proteins = proteins;
            Fats = fats;
            Carbohydrates = carbohydrates;
        }
    }
}