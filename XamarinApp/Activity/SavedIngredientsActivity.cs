using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Widget;
using ObjectsLibrary;

using System.Threading.Tasks;
using XamarinAppLibrary;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using static Android.Support.V7.Widget.SearchView;
using ObjectsLibrary.Components;

namespace XamarinApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon")]
    class SavedIngredientsActivity : AppCompatActivity
    {
        private RecyclerView recyclerView;
        private LinearLayoutManager linearLayoutManager;
        private List<Ingredient> ingredients;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_savedIngredients);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewIngredients);
            linearLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(linearLayoutManager);

            var relativeLayoutBack = FindViewById<RelativeLayout>(Resource.Id.relativeLayoutBack);

            var buttonBack = FindViewById<Button>(Resource.Id.buttonBack);
            buttonBack.SetBackgroundResource(Resources.GetIdentifier("round_arrow_back_ios_24", "drawable", PackageName));
            relativeLayoutBack.Click += new EventHandler((sender, args) =>
            {
                base.OnBackPressed();
            });

            buttonBack.Click += new EventHandler((sender, args) =>
            {
                base.OnBackPressed();
            });

            ingredients = IngredientData.GetArrayIngredients().ToList();

            if (ingredients != null)
            {
                var adapter = new SavedIngredientsAdapter(ingredients);
                recyclerView.SetAdapter(adapter);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }
}