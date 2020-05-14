using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using ObjectsLibrary;
using System;
using XamarinApp;
using System.Collections.Generic;
using System.Threading.Tasks;
using AndroidLibrary;

namespace AndroidApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges =
            Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private RecyclerView recyclerView;
        private RecipeAdapter recipeAdapter;
        private DrawerLayout drawer;
        private SwipeRefreshLayout swipeRefreshLayout;
        private LinearLayoutManager linearLayoutManager;
        private NavigationView navigationView;
        private Button buttonMenu;
        private RecipeScrollListener recipeListener;
        private EditText editText;
        private Spinner spinner;
        private ArrayAdapter spinnerAdapter;
        private ActionBarDrawerToggle actionBarDrawerToggle;
        private Button findButton;

        private List<RecipeShort> recipeShorts;
        private int page = 1;
        private string lastQuery;

        private string messageErrorTitle;
        private string messageErrorText;
        private string messageErrorTextButton = "Повторить";
        private Android.Support.V7.App.AlertDialog.Builder errorDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            // Определяем компоненты: 
            recyclerView = FindViewById<RecyclerView>(Resource.Id.listRecipeShorts);
            swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            buttonMenu = FindViewById<Button>(Resource.Id.menu_button);
            editText = FindViewById<EditText>(Resource.Id.TextFind);
            spinner = FindViewById<Spinner>(Resource.Id.spinner);
            findButton = FindViewById<Button>(Resource.Id.findButton);

            linearLayoutManager = new LinearLayoutManager(this);
            recipeListener = new RecipeScrollListener(linearLayoutManager);

            // Инициализируем элементы:
            swipeRefreshLayout.SetColorSchemeColors(Color.Orange, Color.DarkOrange);
            swipeRefreshLayout.Refresh += RefreshLayout;

            recyclerView.AddOnScrollListener(recipeListener);
            recipeListener.LoadMoreEvent += LoadMoreElements;

            findButton.Click += OnClickFind;
            editText.KeyPress += FindByRecipeName;

            actionBarDrawerToggle = new ActionBarDrawerToggle(this, drawer,
                Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(actionBarDrawerToggle);
            actionBarDrawerToggle.SyncState();
            navigationView.SetNavigationItemSelectedListener(this);
            buttonMenu.SetBackgroundResource(Resources.GetIdentifier("round_menu_24", "drawable", PackageName));
            buttonMenu.Click += SetMenuButtonClick;

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(SelectedItemSpinner);
            spinnerAdapter =
                ArrayAdapter.CreateFromResource(this, Resource.Array.sort_array, Resource.Layout.spinner_text);
            spinnerAdapter.SetDropDownViewResource(Resource.Layout.spinner_text);
            spinner.Adapter = spinnerAdapter;


            string[] messagesError = Resources.GetStringArray(Resource.Array.messagesError);
            messageErrorTitle = messagesError[0];
            messageErrorText = messagesError[1];

            errorDialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle(messageErrorTitle)
                .SetMessage(messageErrorText)
                .SetCancelable(false)
                .SetNeutralButton(messageErrorTextButton, RefreshLayout);
        }

        private void OnClickFind(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(editText.Text))
                return;

            page = 1;
            lastQuery = $"section=recipe&recipeName={editText.Text}&page={page}";
            UpdateListView(lastQuery);
            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);
        }

        private void FindByRecipeName(object sender, View.KeyEventArgs e)
        {
            e.Handled = false;

            if (e.Event.Action != KeyEventActions.Down || e.KeyCode != Keycode.Enter)
                return;

            if (string.IsNullOrWhiteSpace(editText.Text))
                return;

            page = 1;
            Window.SetSoftInputMode(SoftInput.StateHidden);
            lastQuery = $"section=recipe&recipeName={editText.Text}&page={page}";
            UpdateListView(lastQuery);


            //Toast.MakeText(this, "Загрузка...", ToastLength.Short).Show();
            e.Handled = true;
        }
        private void LoadMoreElements(object sender, EventArgs e)
        {
            // Получаем строку для нового запроса:
            string query = lastQuery.Substring(0, lastQuery.IndexOf("page=") + 5) + (++page);
            // Обновляем коллекцию:
            UpdateListView(query, recipeShorts);
        }

        private void RefreshLayout(object sender, EventArgs e)
        {
            // Обновление странички.
            if (lastQuery == null)
                UpdateListView();
            else
                UpdateListView(lastQuery);
        }

        private void SetMenuButtonClick(object sender, EventArgs args)
        {
            // Кнопка "меню" на toolbar.
            drawer.OpenDrawer(GravityCompat.Start);
        }

        private async void UpdateListView(string query = "section=new", List<RecipeShort> recipeShorts = null)
        {
            // Запустить кружочек.
            swipeRefreshLayout.Post(() =>
            {
                swipeRefreshLayout.Refreshing = true;
                recyclerView.Clickable = false;
            });

            recyclerView.SetLayoutManager(linearLayoutManager);

            try
            {
                if (recipeShorts == null)
                {
                    // Тут какой-то костыль, трогать я не буду, простите :(
                    // Из книги "Техники индуса-программиста-под-андройд-на-ксамарине-с-нуля".
                    this.recipeShorts = await UpdateCollectionRecipes(query);
                    recipeAdapter = new RecipeAdapter(this.recipeShorts, this);
                    recipeAdapter.ItemClick += OnRecipeClick;
                    recyclerView.SetAdapter(recipeAdapter);
                }
                else
                {
                    var newRecipes = await UpdateCollectionRecipes(query);
                    recipeAdapter.AddItems(newRecipes);
                    recipeShorts.AddRange(newRecipes);
                }
            }
            catch (Exception exp)
            {
                errorDialog.SetMessage(exp.Message);
                errorDialog.Show();
            }
            finally
            {
                // Остановить кружочек.
                swipeRefreshLayout.Post(() =>
                {
                    swipeRefreshLayout.Refreshing = false;
                    recyclerView.Clickable = true;
                });
            }
        }

        public override void OnBackPressed()
        {
            if (drawer.IsDrawerOpen(GravityCompat.Start))
                drawer.CloseDrawer(GravityCompat.Start);
            else
                base.OnBackPressed();
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }
        
        private void OnRecipeClick(object sender, int position)
        {
            var intent = new Intent(this, typeof(RecipeActivity));
            intent.PutExtra("url", recipeShorts[position].Url);
            intent.PutExtra("recipeShort", DataContext.RecipeToByteArray(recipeShorts[position]));

            StartActivity(intent);
        }
        
        private void SelectedItemSpinner(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var item = spinner.GetItemAtPosition(e.Position);

            page = 1;
            string query = null;

            switch (item.ToString())
            {
                case "Популярные рецепты":
                    query = $"section=popular&page={page}";
                    UpdateListView(query);
                    break;
                case "Случайные рецепты":
                    query = $"section=random&page={page}";
                    UpdateListView(query);
                    break;
                case "Новые рецепты":
                    query = $"section=new&page={page}";
                    UpdateListView(query);
                    break;
            }

            lastQuery = query;
        }

        private async Task<List<RecipeShort>> UpdateCollectionRecipes(string query)
        {
            return await Task.Run(function: () => HttpContext.GetPages(query));
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
            int id = menuItem.ItemId;

            switch (id)
            {
                case Resource.Id.nav_favorite:
                {
                    Intent intent = new Intent(this, typeof(SavedRecipesActivity));
                    StartActivity(intent);
                    break;
                }
                case Resource.Id.nav_cart:
                {
                    Intent intent = new Intent(this, typeof(SavedIngredientsActivity));
                    StartActivity(intent);
                    break;
                }
                case Resource.Id.nav_section:
                {
                    Intent intent = new Intent(this, typeof(PlatesActivity));
                    StartActivity(intent);
                    break;
                }
                case Resource.Id.nav_app:
                {
                    Intent intent = new Intent(this, typeof(InfoActivity));
                    StartActivity(intent);
                    break;
                }
            }

            return CLoseDrawer(drawer).IsDrawerOpen(GravityCompat.Start);
        }

        private static DrawerLayout CLoseDrawer(DrawerLayout drawerLayout)
        {
            drawerLayout.CloseDrawer(GravityCompat.Start);
            return drawerLayout;
        }
    }
}