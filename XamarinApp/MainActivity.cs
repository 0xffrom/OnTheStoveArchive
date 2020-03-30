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
using XamarinAppLibrary;

namespace XamarinApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon", MainLauncher = true)]
    
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private ListView _listView;
        private List<RecipeShort> _recipes;
        private DrawerLayout _drawer;
        
        public static string LastUrl { get; private set; }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // TODO: Расширение картинки? 
            // TODO: Добавить шрифты.
            // TODO: Добавить кнопки на рецептах.
            // TODO: Добавить сохранение
            
            // TODO: Тотальный рефакторинг.
            // TODO: Загрузка доп.рецептов при прокрутке.

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            
   
            
            SetContentView(Resource.Layout.activity_search);
            
            SetListView();
            
            SetPlateRecipe();
            
            SetSpinner();
            
            _drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            var toggle = new ActionBarDrawerToggle(this, _drawer, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            _drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            
            var buttonMenu = FindViewById<Button>(Resource.Id.menu_button);

            
            buttonMenu.Click += delegate(object sender, EventArgs args)
            {
                _drawer.OpenDrawer(GravityCompat.Start);
                
                var menu_buttonM = FindViewById<Button>(Resource.Id.menu_buttonM);
                menu_buttonM.Animation = new RotateAnimation(0, 180); 
                menu_buttonM.Click += delegate(object sender, EventArgs args)
                {

                    if (_drawer.IsDrawerOpen(GravityCompat.Start))
                        OnBackPressed();
                };
                
            };
            
            
        }

        
        public override void OnBackPressed()
        {
            if(_drawer.IsDrawerOpen(GravityCompat.Start))
            {
                _drawer.CloseDrawer(GravityCompat.Start);
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
            var spinner = FindViewById<Spinner>(Resource.Id.spinner);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);


            var spinnerAdapter =
                ArrayAdapter.CreateFromResource(this, Resource.Array.sort_array, Resource.Layout.spinner_text);
            spinnerAdapter.SetDropDownViewResource(Resource.Layout.spinner_text);
            spinner.Adapter = spinnerAdapter;
        }

        private void SetPlateRecipe()
        {
            EditText edittext = FindViewById<EditText>(Resource.Id.TextFind);

            edittext.KeyPress += (object sender, View.KeyEventArgs e) =>
            {
                e.Handled = false;
                
                if (e.Event.Action != KeyEventActions.Down || e.KeyCode != Keycode.Enter) return;
                
                UpdateListView($"getPage?section=recipe&recipeName={edittext.Text}");
                Toast.MakeText(this, "Загрузка...", ToastLength.Short).Show();
                e.Handled = true;
            };
        }

        private void SetListView()
        {
            _listView = FindViewById<ListView>(Resource.Id.listRecipeShorts);

            _listView.ItemClick += (sender, args) =>
            {
                LastUrl = _recipes[int.Parse(args.Id.ToString())].Url;
                Intent intent = new Intent(this, typeof(RecipeActivity));
                StartActivity(intent);
            };
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e) {  
            
            var spinner = (Spinner) sender;
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
            

            var toast = $"Сортировка {spinner.GetItemAtPosition(e.Position).ToString().ToLower()}";  
            Toast.MakeText(this, toast, ToastLength.Long).Show();  
        } 
        
        private async Task<List<RecipeShort>> UpdateCollectionRecipes(string query) =>  
            await Task.Run(function: () => _recipes = HttpGet.GetRecipes(query));
        
        private async void UpdateListView(string query = "getPage?section=popular") =>
            FindViewById<ListView>(Resource.Id.listRecipeShorts).Adapter = 
                new RecipeShortAdapter(this, await UpdateCollectionRecipes(query));


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem) =>
            // Закрывает отрисовщик и возвращает, закрыт ли он или нет.
            CLoseDrawer(_drawer).IsDrawerOpen(GravityCompat.Start);

        private static DrawerLayout CLoseDrawer(DrawerLayout drawerLayout)
        {
            drawerLayout.CloseDrawer(GravityCompat.Start);
            return drawerLayout;
        }
    }
}

