using System;
using System.Collections.Generic;
using Android.Widget;
using AndroidLibrary;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Content;
using AndroidApp.General;
using XamarinApp;

namespace AndroidApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon",
        ConfigurationChanges =
            Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    internal class PlatesActivity : AppCompatActivity
    {
        private RecyclerView recyclerView;
        private LinearLayoutManager linearLayoutManager;
        private List<Plate> plates;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_plates);


            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewRecipesPlates);
            linearLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(linearLayoutManager);

            var relativeLayoutBack = FindViewById<RelativeLayout>(Resource.Id.relativeLayoutBack);
            var buttonBack = FindViewById<Button>(Resource.Id.buttonBack);

            relativeLayoutBack.Click += OnBackPressed;
            buttonBack.Click += OnBackPressed;

            plates = new List<Plate>()
            {
                new Plate("Основные блюда", "osnovnye", "горячее"),
                new Plate("Супы", "supy", "супы"),
                new Plate("Салаты", "salaty", "салаты"),
                new Plate("Выпечка", "vypechka", "выпечка"),
                new Plate("Десерты", "deserty", "десерты"),
                new Plate("Закуски", "zakuski", "закуски"),
                new Plate("Соусы", "sousy", "соусы")
            };

            var adapter = new PlateAdapter(plates, this);
            adapter.ItemClick += OnPlateClick;
            recyclerView.SetAdapter(adapter);
        }

        private void OnPlateClick(object sender, int position)
        {
            Intent intent = new Intent(this, typeof(SectionRecipesActivity));
            intent.PutExtra("key", plates[position].Key);
            intent.PutExtra("title", plates[position].Title);

            StartActivity(intent);
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