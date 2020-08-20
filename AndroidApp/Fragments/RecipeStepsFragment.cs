using XamarinApp;
using Android.OS;
using Android.Views;
using Android.Widget;
using ObjectsLibrary;
using AndroidLibrary;

namespace AndroidApp.Fragments
{
    public class RecipeStepsFragment : Android.Support.V4.App.Fragment
    {
        private RecipeFull _recipeFull;
        private ListView _listSteps;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var arguments = Arguments;

            if (arguments != null)
            {
                _recipeFull = DataContext.ByteArrayToObject<RecipeFull>(arguments.GetByteArray("recipeFull"));
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_recipe_steps, container, false);

            _listSteps = view.FindViewById<ListView>(Resource.Id.listSteps);

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            var adapterStep = new StepAdapter(View.Context, _recipeFull);
            _listSteps.Adapter = adapterStep;

            base.OnActivityCreated(savedInstanceState);
        }
    }
}