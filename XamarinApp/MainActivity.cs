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
using Android.Views.Animations;
using Android.Widget;
using ObjectsLibrary;
using System;
using System.Threading.Tasks;
using XamarinAppLibrary;

namespace XamarinApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private RecyclerView recyclerView;
        private RecipeShort[] _recipes;
        private DrawerLayout _drawer;
        private ProgressBar progressBar;


        private int page = 1;
        private const string message = "Идёт загрузка рецептов, подождите, пожалуйста.";
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
            progressBar = FindViewById<ProgressBar>(Resource.Id.loadingProgressBar);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.listRecipeShorts);

            recyclerView.HasFixedSize = true;

            UpdateListView();

            SetPlateRecipe();

            SetSpinner();

            _drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            var toggle = new ActionBarDrawerToggle(this, _drawer, Resource.String.navigation_drawer_open,
                Resource.String.navigation_drawer_close);
            _drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            var navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            var buttonMenu = FindViewById<Button>(Resource.Id.menu_button);
            buttonMenu.Click += SetButtonClick;
        }

        private void SetButtonClick(object sender, EventArgs args)
        {
            _drawer.OpenDrawer(GravityCompat.Start);

            var menu_buttonM = FindViewById<Button>(Resource.Id.menu_buttonM);
            menu_buttonM.Animation = new RotateAnimation(0, 180);
            menu_buttonM.Click += delegate (object sender, EventArgs args)
            {
                if (_drawer.IsDrawerOpen(GravityCompat.Start))
                    OnBackPressed();
            };

        }

        private async void UpdateListView(string query = "getPage?section=popular")
        {
            progressBar.Visibility = ViewStates.Visible;

            var mLayoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(mLayoutManager);
            var adapter = new RecipeAdapter(await UpdateCollectionRecipes(query), this);
            adapter.ItemClick += OnItemClick;
            recyclerView.SetAdapter(adapter);

            progressBar.Visibility = ViewStates.Invisible;
        }

        public override void OnBackPressed()
        {
            if (_drawer.IsDrawerOpen(GravityCompat.Start))
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
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(SelectedItemSpinner);


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

                page = 1;

                if (e.Event.Action != KeyEventActions.Down || e.KeyCode != Keycode.Enter) return;

                UpdateListView($"getPage?page={page}&section=recipe&recipeName={edittext.Text}");
                Toast.MakeText(this, "Загрузка...", ToastLength.Short).Show();


                e.Handled = true;
            };
        }

        void OnItemClick(object sender, int position)
        {
            LastUrl = _recipes[position].Url;
            Intent intent = new Intent(this, typeof(RecipeActivity));
            StartActivity(intent);
        }


        private void SelectedItemSpinner(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = (Spinner)sender;
            var item = spinner.GetItemAtPosition(e.Position);

            page = 1;

            switch (item.ToString())
            {
                case "По популярности":
                    UpdateListView($"getPage?page={page}&section=popular");
                    break;
                case "По случайности":
                    UpdateListView($"getPage?page={page}&section=random");
                    break;
                case "По новизне":
                    UpdateListView($"getPage?page={page}&section=new");
                    break;
            }


            var toast = $"Сортировка {spinner.GetItemAtPosition(e.Position).ToString().ToLower()}";
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

        private async Task<RecipeShort[]> UpdateCollectionRecipes(string query)
        {
            return await Task.Run(function: () => _recipes = HttpGet.GetRecipes(query));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem menuItem)
        {
            // Закрывает отрисовщик и возвращает, закрыт ли он или нет.
            return CLoseDrawer(_drawer).IsDrawerOpen(GravityCompat.Start);
        }

        private static DrawerLayout CLoseDrawer(DrawerLayout drawerLayout)
        {
            drawerLayout.CloseDrawer(GravityCompat.Start);
            return drawerLayout;
        }
    }
}