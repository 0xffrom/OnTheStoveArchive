using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using RecipesAndroid;
using RecipesAndroid.Objects;
using RecipesAndroid.Objects.Boxes.Elements;

namespace XamarinApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ListView _listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_search);

            List<RecipeShort> list = new List<RecipeShort>()
            {
                new RecipeShort("Тут титле", new Picture("Пикча"), "url"),
                new RecipeShort("Тут титле", new Picture("Пикча"), "url")
            };

            _listView = FindViewById<ListView>(Resource.Id.listRecipeShorts);
            
            RecipeShortAdapter adapter = new RecipeShortAdapter(this, list);
            _listView.Adapter = adapter;


        }
       
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}

