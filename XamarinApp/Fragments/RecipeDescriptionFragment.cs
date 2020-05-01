using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using ObjectsLibrary;
using XamarinAppLibrary;

namespace XamarinApp.Fragments
{
    public class RecipeDescriptionFragment : AndroidX.Fragment.App.Fragment
    {
        private RecipeFull _recipeFull;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Bundle arguments = Arguments;
            if (arguments != null)
            {
                _recipeFull = Data.ByteArrayToObject<RecipeFull>(arguments.GetByteArray("recipeFull"));
            }
            
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.recipe_description_fragment, container, false);
            
            // Отрисовываем 
            return view;

            return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}