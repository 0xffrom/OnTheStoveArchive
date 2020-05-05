using XamarinApp;
using Android.OS;
using Android.Views;
using Android.Widget;
using ObjectsLibrary;
using AndroidLibrary;

namespace AndroidApp.Fragments
{
    public class RecipeIngredientsFragment : Android.Support.V4.App.Fragment
    {
        private RecipeFull _recipeFull;
        private ListView _listIngredients;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var arguments = Arguments;

            if (arguments != null)
            {
                _recipeFull = Data.ByteArrayToObject<RecipeFull>(arguments.GetByteArray("recipeFull"));
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_recipe_ingredients, container, false);

            _listIngredients = view.FindViewById<ListView>(Resource.Id.listIngredients);

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            var listIngredientsAdapter = new IngredientsAdapter(View.Context, _recipeFull);

            if (listIngredientsAdapter.Count > 0)
                _listIngredients.Adapter = listIngredientsAdapter;

            base.OnActivityCreated(savedInstanceState);
        }
    }
}