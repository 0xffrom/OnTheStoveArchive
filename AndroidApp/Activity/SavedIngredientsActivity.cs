using System;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using AndroidLibrary;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using ObjectsLibrary.Components;

namespace AndroidApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon",
        ConfigurationChanges =
            Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    internal class SavedIngredientsActivity : AppCompatActivity
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

            relativeLayoutBack.Click += OnBackPressed;
            buttonBack.Click += OnBackPressed;

            ingredients = IngredientData.GetArrayIngredients().ToList();

            if (ingredients == null) return;
            
            var adapter = new SavedIngredientsAdapter(ingredients);
            recyclerView.SetAdapter(adapter);
        }

        private void OnBackPressed(object sender, EventArgs args) => base.OnBackPressed();

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}