using System;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using AndroidApp.Fragments;
using Java.Lang;
using ObjectsLibrary;
using AndroidLibrary;

namespace AndroidApp.Adapters
{
    public class PagerRecipeAdapter : FragmentPagerAdapter
    {
        private readonly RecipeFull _recipeFull;
        private readonly string[] pagesName = {"Описание", "Ингредиенты", "Пошаговый рецепт"};
        
        public PagerRecipeAdapter(FragmentManager fm, RecipeFull recipeFull) : base(fm)
        {
            _recipeFull = recipeFull;
        }

        public override int Count { get; } = 3;

        public override Fragment GetItem(int position)
        {
            var arguments = new Bundle();

            arguments.PutByteArray("recipeFull", DataContext.RecipeToByteArray(_recipeFull));

            var fragment = position switch
            {
                0 => (Fragment) new RecipeDescriptionFragment(),
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