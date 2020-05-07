
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
        public double Calories { get; set; }

        /// <value>Количество белка.</value>
        public double Protein { get; set; }

        /// <value>Количество жиров.</value>
        public double Fats { get; set; }

        /// <value>Количество углеводов.</value>
        public double Carbohydrates { get; set; }

        public CPFC()
        {
            //
        }

        public CPFC(double calories, double protein, double fats, double carbohydrates) : this()
        {
            Calories = calories;
            Protein = protein;
            Fats = fats;
            Carbohydrates = carbohydrates;
        }
    }
}