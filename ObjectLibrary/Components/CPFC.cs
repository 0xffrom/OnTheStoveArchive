using System;

namespace ObjectsLibrary.Components
{
    /// <summary>Калории, белки, жиры, углеводы.</summary>
    [Serializable]
    public class CPFC
    {
        /// <value>Количество калорий.</value>
        public double Calories { get; set; }

        /// <value>Количество белка.</value>
        public double Proteins { get; set; }

        /// <value>Количество жиров.</value>
        public double Fats { get; set; }

        /// <value>Количество углеводов.</value>
        public double Carbohydrates { get; set; }

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