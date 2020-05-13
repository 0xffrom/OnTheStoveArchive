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
using Android.Support.V4.Widget;
using ObjectsLibrary;
using Android.Graphics;
using System.Threading.Tasks;
using XamarinApp;

namespace AndroidApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon",
        ConfigurationChanges =
            Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    internal class SectionRecipesActivity : AppCompatActivity
    {
        private List<RecipeShort> recipeShorts;
        private SwipeRefreshLayout swipeRefreshLayout;
        private RecyclerView recyclerView;
        private RecipeAdapter recipeAdapter;
        private LinearLayoutManager linearLayoutManager;
        private RecipeScrollListener recipeListener;
        private RelativeLayout relativeLayoutBack;
        private Button buttonBack;
        private TextView textViewTitle;

        private string title;
        private string lastQuery;
        private int page = 1;
        private string section;

        private string messageErrorTitle;
        private string messageErrorText;
        private string messageErrorTextButton = "Назад";
        private Android.Support.V7.App.AlertDialog.Builder errorDialog;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_sections);

            relativeLayoutBack = FindViewById<RelativeLayout>(Resource.Id.relativeLayoutBack);
            buttonBack = FindViewById<Button>(Resource.Id.buttonBack);
            textViewTitle = FindViewById<TextView>(Resource.Id.textView_title);
            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerViewRecipesSections);
            swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);

            section = Intent.GetStringExtra("key");
            title = Intent.GetStringExtra("title");

            lastQuery = $"section={section}&page=1";
            textViewTitle.Text = "Раздел: " + title;

            relativeLayoutBack.Click += OnBackPressed;
            buttonBack.Click += OnBackPressed;

            linearLayoutManager = new LinearLayoutManager(this);
            recipeListener = new RecipeScrollListener(linearLayoutManager);

            swipeRefreshLayout.SetColorSchemeColors(Color.Orange, Color.DarkOrange);
            swipeRefreshLayout.Refresh += RefreshLayout;

            recyclerView.AddOnScrollListener(recipeListener);
            recipeListener.LoadMoreEvent += LoadMoreElements;

            string[] messagesError = Resources.GetStringArray(Resource.Array.messagesError);
            messageErrorTitle = messagesError[0];
            messageErrorText = messagesError[1];

            errorDialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle(messageErrorTitle)
                .SetMessage(messageErrorText)
                .SetCancelable(false)
                .SetNeutralButton(messageErrorTextButton, OnBackPressed);

            UpdateListView();
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
            UpdateListView($"section={section}&page=1");
        }

        private async void UpdateListView(string query = "section={section}", List<RecipeShort> recipeShorts = null)
        {
            query = query.Replace("{section}", $"{section}");

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

        private async Task<List<RecipeShort>> UpdateCollectionRecipes(string query)
        {
            return await Task.Run(function: () => HttpContext.GetPages(query));
        }

        private void OnRecipeClick(object sender, int position)
        {
            Intent intent = new Intent(this, typeof(RecipeActivity));
            intent.PutExtra("url", recipeShorts[position].Url);
            intent.PutExtra("recipeShort", DataContext.RecipeToByteArray(recipeShorts[position]));

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