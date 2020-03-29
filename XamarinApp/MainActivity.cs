using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using RecipesAndroid;
using RecipesAndroid.Objects;
using XamarinApp.Library;
using XamarinApp.Library.Objects;

namespace XamarinApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon", MainLauncher = true)]
    
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private ListView _listView;
        private List<RecipeShort> recipes;
        private RecipeShortAdapter adapter;
        public static string lastUrl;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Цели на ночь:
            // TODO: Переделать МЕНЮ.
            // TODO: Передать спиннер.
            // TODO: Пофиксить баги с открытием.
            // TODO: Добавить кнопки на рецептах.
            // TODO: Добавить сохранение
            // TODO: Поработать над сохранением картинок.
            
            // TODO: Тотальный рефакторинг.
            // TODO: Загрузка доп.рецептов при прокрутке.

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.activity_search);
            
            SetListView();
            
            SetPlateRecipe();
            
            SetSpinner();
            
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            
            var buttonMenu = FindViewById<Button>(Resource.Id.menu_button);

            
            buttonMenu.Click += delegate(object sender, EventArgs args)
            {
                DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
                
                drawer.OpenDrawer(GravityCompat.Start);
                
                var menu_buttonM = FindViewById<Button>(Resource.Id.menu_buttonM);
                menu_buttonM.Animation = new RotateAnimation(0, 180); 
                menu_buttonM.Click += delegate(object sender, EventArgs args)
                {

                    if (drawer.IsDrawerOpen(GravityCompat.Start))
                        OnBackPressed();
                };
                
            };
            
            
        }

        
        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }
        
        private void SetSpinner()
        {
            Spinner spinner = FindViewById<Spinner>(Resource.Id.spinner);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);


            var adapter =
                ArrayAdapter.CreateFromResource(this, Resource.Array.sort_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;
        }

        private void SetPlateRecipe()
        {
            EditText edittext = FindViewById<EditText>(Resource.Id.TextFind);

            edittext.KeyPress += (object sender, View.KeyEventArgs e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    UpdateListView($"getPage?section=recipe&recipeName={edittext.Text}");
                    Toast.MakeText(this, "Загрузка...", ToastLength.Short).Show();
                    e.Handled = true;
                }
            };
        }

        private void SetListView()
        {
            _listView = FindViewById<ListView>(Resource.Id.listRecipeShorts);

            _listView.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs args)
            {
                lastUrl = recipes[int.Parse(args.Id.ToString())].Url;
                Intent intent = new Intent(this, typeof(RecipeActivity));
                StartActivity(intent);
            };
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e) {  
            Spinner spinner = (Spinner) sender;
            var item = spinner.GetItemAtPosition(e.Position);
            
            switch (item.ToString())
            {
                case "По популярности":
                    UpdateListView("getPage?section=popular");
                    break;
                case "По случайности":
                    UpdateListView("getPage?section=random");
                    break;
                case "По новизне":
                    UpdateListView("getPage?section=new");
                    break;
            }
            

            string toast = $"Сортировка {spinner.GetItemAtPosition(e.Position).ToString().ToLower()}";  
            Toast.MakeText(this, toast, ToastLength.Long).Show();  
        } 
        
        private async Task UpdateCollectionRecipes(string query)
        {
            await Task.Run(() =>
            {
                recipes = HttpGet.GetRecipes(query);
            });
        }
        private async void UpdateListView(string query = "getPage?section=popular")
        {
            await UpdateCollectionRecipes(query);

            _listView = FindViewById<ListView>(Resource.Id.listRecipeShorts);
            
            adapter = new RecipeShortAdapter(this, recipes);

            _listView.Adapter = adapter;
        }

       
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }
    }
}

