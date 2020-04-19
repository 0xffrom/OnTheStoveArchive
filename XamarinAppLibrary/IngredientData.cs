using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ObjectsLibrary.Components;

namespace XamarinAppLibrary
{
    public class IngredientData
    {
        private static string path =
           System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        private static string GetFileIngredientName(Ingredient ingredient)
        {
            string nameFull = ingredient.Name + ingredient.Unit;
            int hash = nameFull.GetHashCode();

            return "";
        }

    }
}