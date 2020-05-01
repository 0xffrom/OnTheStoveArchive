using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using ObjectsLibrary;
using XamarinApp.Fragments;
using XamarinAppLibrary;

namespace XamarinApp.Adapters
{
    public class PagerRecipeAdapter : FragmentPagerAdapter
    {
        private RecipeFull _recipeFull;
        private string[] pagesName = { "Описание", "Ингредиенты", "Пошаговый рецепт" };
        public PagerRecipeAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public PagerRecipeAdapter(Android.Support.V4.App.FragmentManager fm, RecipeFull recipeFull) : base(fm)
        {
            _recipeFull = recipeFull;
        }
        public override int Count { get; } = 3;
        
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            var arguments = new Bundle();
            
            arguments.PutByteArray("recipeFull", Data.RecipeToByteArray(_recipeFull));

            var fragment = position switch
            {
                0 => (Android.Support.V4.App.Fragment) new RecipeDescriptionFragment(),
                1 => new RecipeIngredientsFragment(),
                2 => new RecipeStepsFragment(),
                _ => new RecipeDescriptionFragment()
            };

            fragment.Arguments = arguments;

            return fragment;
        }
        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(pagesName[position]);
        }
    }
}