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
using AndroidX.Fragment.App;
using XamarinApp.Fragments;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace XamarinApp.Adapters
{
    public class PagerRecipeAdapter : FragmentPagerAdapter
    {
        public PagerRecipeAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public PagerRecipeAdapter(FragmentManager fm) : base(fm)
        {
        }

        public PagerRecipeAdapter(FragmentManager fm, int behavior) : base(fm, behavior)
        {
        }

        public override int Count { get; } = 3;
        
        public override Fragment GetItem(int position)
        {
            return position switch
            {
                0 => (Fragment) new RecipeDescriptionFragment(),
                1 => new RecipeIngredientsFragment(),
                2 => new RecipeStepsFragment(),
                _ => new RecipeDescriptionFragment()
            };
        }
    }
}