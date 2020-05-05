using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using ObjectsLibrary;
using System;
using System.Threading.Tasks;
using AndroidApp.Adapters;
using AndroidLibrary;
using XamarinApp;

namespace AndroidApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon",
        ConfigurationChanges =
            Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    internal class RecipeActivity : AppCompatActivity
    {
        private string urlRecipe;
        private RecipeFull recipeFull;
        private RecipeShort recipeShort;

        private Button buttonBack;
        private FloatingActionButton buttonStar;
        private Button buttonShare;
        private Button buttonWeb;
        private RelativeLayout relativeLayoutBack;
        private TextView title;
        private ViewPager viewPager;

        private string messageErrorTitle;
        private string messageErrorText;
        private string messageErrorTextButton;
        private Android.Support.V7.App.AlertDialog.Builder errorDialog;

        private string GetMessageOnButtonShare() =>
            $"Мне понравился рецепт под названием '{recipeFull.Title}'. " +
            $"Подробнее ты можешь прочитать о нём по ссылке: {recipeFull.Url}." +
            $"Не забудь также попробовать приложение 'На плите' (доступно в Play Market)!";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_recipe);

            title = FindViewById<TextView>(Resource.Id.titleRecipe);
            buttonShare = FindViewById<Button>(Resource.Id.shareRecipe);
            buttonStar = FindViewById<FloatingActionButton>(Resource.Id.starRecipe);
            relativeLayoutBack = FindViewById<RelativeLayout>(Resource.Id.relativeLayoutBack);
            viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
            buttonBack = FindViewById<Button>(Resource.Id.buttonBack);
            buttonWeb = FindViewById<Button>(Resource.Id.webRecipe);

            // Получаем необходимые данные: 
            urlRecipe = Intent.GetStringExtra("url");
            recipeShort = Data.ByteArrayToObject<RecipeShort>(Intent.GetByteArrayExtra("recipeShort"));

            string[] messagesError = Resources.GetStringArray(Resource.Array.messagesError);
            messageErrorTitle = messagesError[0];
            messageErrorText = messagesError[1];
            messageErrorTextButton = messagesError[2];

            errorDialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle(messageErrorTitle)
                .SetMessage(messageErrorText)
                .SetCancelable(false)
                .SetNeutralButton(messageErrorTextButton, OnBackPressed);
        }

        protected override async void OnStart()
        {
            base.OnStart();

            // Проверка на избранный рецепт: 
            if (RecipeData.ExistsRecipe(urlRecipe))
                // Если избранный, то сделать звёздочку закрашенной.
                buttonStar.SetBackgroundResource(
                    Resources.GetIdentifier("outline_favorite_24", "drawable", PackageName));
            try
            {
                recipeFull = await UpdateCollectionRecipes(urlRecipe);
                title.Text = recipeFull.Title;
                viewPager.Adapter = (new PagerRecipeAdapter(SupportFragmentManager, recipeFull));
            }
            catch (AndroidException exp)
            {
                errorDialog.SetMessage(exp.Message);
            }
            // Обработка случая, когда рецепт не загружается:
            catch
            {
                errorDialog.SetMessage(messageErrorText);
                errorDialog.Show();
            }

            buttonShare.Click += OnShareButton;
            buttonStar.Click += OnAddFavRecipe;
            relativeLayoutBack.Click += OnBackPressed;
            buttonBack.Click += OnBackPressed;
            buttonWeb.Click += OnStartWeb;
        }

        private void OnBackPressed(object sender, EventArgs args) => base.OnBackPressed();

        private void OnStartWeb(object sender, EventArgs args)
        {
            Android.Net.Uri address = Android.Net.Uri.Parse(recipeFull.Url);
            Intent openLinkIntent = new Intent(Intent.ActionView, address);

            if (openLinkIntent.ResolveActivity(PackageManager) != null)
            {
                StartActivity(openLinkIntent);
            }
        }

        private void OnAddFavRecipe(object sender, EventArgs args)
        {
            if (!RecipeData.ExistsRecipe(urlRecipe))
            {
                Toast.MakeText(this, "Рецепт добавлен в избранное", ToastLength.Short).Show();
                RecipeData.SaveRecipe(urlRecipe, recipeShort);
                buttonStar.SetImageResource(Resources.GetIdentifier("outline_favorite_24", "drawable", PackageName));
                return;
            }

            // else:
            RecipeData.DeleteRecipe(urlRecipe);
            buttonStar.SetImageResource(Resources.GetIdentifier("outline_favorite_border_24", "drawable",
                PackageName));
        }

        private void OnShareButton(object sender, EventArgs args)
        {
            Intent intent = new Intent(Intent.ActionSend);

            intent.SetType("text/plain");
            String textToSend = GetMessageOnButtonShare();

            intent.PutExtra(Intent.ExtraText, textToSend);
            try
            {
                StartActivity(Intent.CreateChooser(intent, "Поделиться рецептом."));
            }
            catch (Android.Content.ActivityNotFoundException)
            {
                Toast.MakeText(ApplicationContext, "Some error", ToastLength.Short).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Загрузка рецепта с сервера.
        /// </summary>
        /// <param name="url">Интернет адрес рецепта.</param>
        /// <returns><see cref="RecipeFull"/></returns>
        private static async Task<RecipeFull> UpdateCollectionRecipes(string url)
        {
            return await Task.Run(() => HttpGet.GetRecipe(url));
        }
    }
}