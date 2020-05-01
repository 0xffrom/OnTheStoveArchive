using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using ObjectsLibrary;
using Square.Picasso;
using System;
using System.Threading.Tasks;
using XamarinApp.Adapters;
using XamarinAppLibrary;

namespace XamarinApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon",
        ConfigurationChanges=Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class RecipeActivity : AppCompatActivity
    {
        private string _url;
        private RecipeFull recipeFull;
        private RecipeShort recipeShort;

        public readonly Color ColorFeb22C = new Color(254, 178, 44);
        public readonly Color ColorC4C4C4 = new Color(196, 196, 196);

        Button buttonStar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            _url = Intent.GetStringExtra("url");
            recipeShort = Data.ByteArrayToObject<RecipeShort>(Intent.GetByteArrayExtra("recipeShort"));


            SetContentView(Resource.Layout.recipe_main);
            UpdateView();
            

            var buttonShare = FindViewById<Button>(Resource.Id.shareRecipe);
            buttonShare.Click += new EventHandler((sender, args) =>
            {
                Intent intent = new Intent(Intent.ActionSend);
                intent.SetType("text/plain");
                String textToSend = $"Мне понравился рецепт приготовления блюда под названием '{recipeFull.Title}'. " +
                $"Ссылка на блюдо: {recipeFull.Url}. " +
                $"А также не забудь попробовать приложение 'На плите'!";
                intent.PutExtra(Intent.ExtraText, textToSend);
                try
                {
                    StartActivity(Intent.CreateChooser(intent, "Поделиться рецептом."));
                }
                catch (Android.Content.ActivityNotFoundException)
                {
                    Toast.MakeText(ApplicationContext, "Some error", ToastLength.Short).Show();
                }
            });
            var relativeLayoutBack = FindViewById<RelativeLayout>(Resource.Id.relativeLayoutBack);
            relativeLayoutBack.Click += new EventHandler((sender, args) =>
            {
                base.OnBackPressed();
            });

            var buttonBack = FindViewById<Button>(Resource.Id.buttonBack);
            buttonBack.Click += new EventHandler((sender, args) =>
            {
                base.OnBackPressed();
            });
            

            buttonStar = FindViewById<Button>(Resource.Id.starRecipe);
            buttonStar.Click += new EventHandler((sender, args) =>
            {
                if (!RecipeData.ExistsRecipe(_url))
                {
                    RecipeData.SaveRecipe(_url, recipeShort);
                    buttonStar.SetBackgroundResource(Resources.GetIdentifier("round_star_white_24", "drawable", PackageName));
                }
                else
                {
                    RecipeData.DeleteRecipe(_url);
                    buttonStar.SetBackgroundResource(Resources.GetIdentifier("round_star_border_white_24", "drawable", PackageName));
                }
            });


        }
        
        private async void UpdateView()
        {
            if (RecipeData.ExistsRecipe(_url))
                buttonStar.SetBackgroundResource(Resources.GetIdentifier("round_star_white_24", "drawable", PackageName));

            try
            {
                recipeFull = await UpdateCollectionRecipes(_url);

                var _title = FindViewById<TextView>(Resource.Id.titleRecipe);
                _title.Text = recipeFull.Title;


                var adapter = new PagerRecipeAdapter(SupportFragmentManager, recipeFull);

                ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewPager);
                viewPager.Adapter = (adapter);

            }
            
            // Обработка случая, когда рецепт битый.
            catch
            {
                var dialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                    .SetTitle("Ошибка :(")
                    .SetNeutralButton("Хорошо", 
                    (sender, e) => 
                    {
                        base.OnBackPressed();
                    })
                    .SetMessage("К сожалению, не удалось загрузить данный рецепт. Разработчики уже занимаются этим вопросом.");

                dialog.Show();
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private static async Task<RecipeFull> UpdateCollectionRecipes(string url) => await Task.Run(() => HttpGet.GetRecipe(url));
    }
}