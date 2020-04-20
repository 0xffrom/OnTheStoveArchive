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

namespace XamarinApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon")]
    class SavedRecipesActivity : AppCompatActivity
    {
        private RecipeAdapter _adapter;
        private RecyclerView recyclerView;
        private Android.Support.V7.Widget.SearchView _searchView;
        private LinearLayoutManager linearLayoutManager;
        private RecipeShort[] recipeShorts;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_savedRecipes);

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

            _searchView = FindViewById<Android.Support.V7.Widget.SearchView>(Resource.Id.searchViewFind);

            _searchView.QueryTextChange += new EventHandler<QueryTextChangeEventArgs>((sender, args) =>
            {
                var adapter = new RecipeAdapter(this.recipeShorts.Where(x => x.Title.ToLower().Contains(args.NewText.ToLower())).ToArray(), this);
                adapter.ItemClick += OnItemClick;
                _adapter = adapter;
                recyclerView.SetAdapter(adapter);
            });

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewRecipes);
            linearLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(linearLayoutManager);


            recipeShorts = RecipeData.GetArrayRecipes();

            if (recipeShorts != null)
            {
                var adapter = new RecipeAdapter(this.recipeShorts, this);
                adapter.ItemClick += OnItemClick;
                _adapter = adapter;
                recyclerView.SetAdapter(adapter);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        void OnItemClick(object sender, int position)
        {
            Intent intent = new Intent(this, typeof(RecipeActivity));
            intent.PutExtra("url", recipeShorts[position].Url);
            intent.PutExtra("recipeShort", Data.RecipeToByteArray(recipeShorts[position]));

            StartActivity(intent);
        }
 

    }
}