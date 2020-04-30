using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using ObjectsLibrary;
using Square.Picasso;
using System;
using System.Threading.Tasks;
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

            DrawerLayout();

            var buttonShare = FindViewById<ImageButton>(Resource.Id.shareRecipe);
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


            UpdateView();
        }

        private void DrawerLayout()
        {
            var frameDescription = FindViewById<View>(Resource.Id.frameDescription);
            var frameIngredients = FindViewById<View>(Resource.Id.frameIngredients);
            var frameSteps = FindViewById<View>(Resource.Id.frameSteps);

            var textViewMainDescription = FindViewById<TextView>(Resource.Id.textViewMainDescription);
            var textViewMainIngredients = FindViewById<TextView>(Resource.Id.textViewMainIngredients);
            var textViewMainRecipe = FindViewById<TextView>(Resource.Id.textViewMainRecipe);

            var rectangleMainDescription = FindViewById<View>(Resource.Id.rectangleMainDescription);
            var rectangleMainIngredients = FindViewById<View>(Resource.Id.rectangleMainIngredients);
            var rectangleMainRecipe = FindViewById<View>(Resource.Id.rectangleMainRecipe);

            SetColorStart(textViewMainDescription, textViewMainIngredients, textViewMainRecipe,
                rectangleMainDescription, rectangleMainIngredients, rectangleMainRecipe);

            textViewMainDescription.Click += (sender, args) =>
            {
                SetColorStart(textViewMainDescription, textViewMainIngredients, textViewMainRecipe,
                    rectangleMainDescription, rectangleMainIngredients, rectangleMainRecipe);

                frameDescription.Visibility = ViewStates.Visible;
                frameSteps.Visibility = ViewStates.Invisible;
                frameIngredients.Visibility = ViewStates.Invisible;
            };

            textViewMainIngredients.Click += (sender, args) =>
            {
                SetColorDefault(textViewMainDescription, textViewMainIngredients, textViewMainRecipe,
                    rectangleMainDescription, rectangleMainIngredients, rectangleMainRecipe);

                textViewMainIngredients.SetTextColor(ColorFeb22C);
                rectangleMainIngredients.SetBackgroundColor(ColorFeb22C);

                frameDescription.Visibility = ViewStates.Invisible;
                frameIngredients.Visibility = ViewStates.Visible;
                frameSteps.Visibility = ViewStates.Invisible;
            };

            textViewMainRecipe.Click += (sender, args) =>
            {
                SetColorDefault(textViewMainDescription, textViewMainIngredients, textViewMainRecipe,
                    rectangleMainDescription, rectangleMainIngredients, rectangleMainRecipe);

                textViewMainRecipe.SetTextColor(ColorFeb22C);
                rectangleMainRecipe.SetBackgroundColor(ColorFeb22C);

                frameDescription.Visibility = ViewStates.Invisible;
                frameIngredients.Visibility = ViewStates.Invisible;
                frameSteps.Visibility = ViewStates.Visible;
            };
        }

        private void SetColorDefault(TextView textViewMainDescription, TextView textViewMainIngredients,
            TextView textViewMainRecipe,
            View rectangleMainDescription, View rectangleMainIngredients, View rectangleMainRecipe)
        {
            textViewMainDescription.SetTextColor(ColorC4C4C4);
            textViewMainIngredients.SetTextColor(ColorC4C4C4);
            textViewMainRecipe.SetTextColor(ColorC4C4C4);

            rectangleMainDescription.SetBackgroundColor(ColorC4C4C4);
            rectangleMainIngredients.SetBackgroundColor(ColorC4C4C4);
            rectangleMainRecipe.SetBackgroundColor(ColorC4C4C4);
        }

        private void SetColorStart(TextView textViewMainDescription, TextView textViewMainIngredients,
            TextView textViewMainRecipe,
            View rectangleMainDescription, View rectangleMainIngredients, View rectangleMainRecipe)
        {
            SetColorDefault(textViewMainDescription, textViewMainIngredients, textViewMainRecipe,
                rectangleMainDescription, rectangleMainIngredients, rectangleMainRecipe);

            textViewMainDescription.SetTextColor(ColorFeb22C);
            rectangleMainDescription.SetBackgroundColor(ColorFeb22C);
        }

        private async void UpdateView()
        {
            if (RecipeData.ExistsRecipe(_url))
                buttonStar.SetBackgroundResource(Resources.GetIdentifier("round_star_white_24", "drawable", PackageName));

            try
            {
                recipeFull = await UpdateCollectionRecipes(_url);

                var listIngredients = FindViewById<ListView>(Resource.Id.listIngredients);
                var adapterIngredents = new IngredientsAdapter(this, recipeFull);
                if (adapterIngredents.Count > 0)
                    listIngredients.Adapter = adapterIngredents;
                var listSteps = FindViewById<ListView>(Resource.Id.listSteps);
                var adapterStep = new StepAdapter(this, recipeFull);
                listSteps.Adapter = adapterStep;


                #region Первая страница.

                var imageView = FindViewById<ImageView>(Resource.Id.imageMainRecipe);
                var title = FindViewById<TextView>(Resource.Id.titleRecipe);
                var description = FindViewById<TextView>(Resource.Id.titleMainDescription);
                var CPFCRecipe = FindViewById<TextView>(Resource.Id.CPFCRecipe);
                var authorNameRecipe = FindViewById<TextView>(Resource.Id.authorNameRecipe);
                var additionalInfoRecipe = FindViewById<TextView>(Resource.Id.additionalInfoRecipe);
                var urlRecipe = FindViewById<TextView>(Resource.Id.urlRecipe);

                title.Text = recipeFull.Title;

                Picasso.With(this)
                    .Load(recipeFull.TitleImage.ImageUrl)
                    .Into(imageView);

                description.Text += $"{recipeFull.Description.Replace("  ", string.Empty)}";

                if (recipeFull.Additional.CPFC != null)
                {
                    if (recipeFull.Additional.CPFC.Calories != 0)
                        CPFCRecipe.Text += $"Калории: {recipeFull.Additional.CPFC.Calories} Ккал.{System.Environment.NewLine}";
                    if (recipeFull.Additional.CPFC.Protein != 0)
                        CPFCRecipe.Text += $"Белки: {recipeFull.Additional.CPFC.Protein} г.{System.Environment.NewLine}";
                    if (recipeFull.Additional.CPFC.Fats != 0)
                        CPFCRecipe.Text += $"Жиры: {recipeFull.Additional.CPFC.Fats} г.{System.Environment.NewLine}";
                    if (recipeFull.Additional.CPFC.Carbohydrates != 0)
                        CPFCRecipe.Text += $"Углеводы: {recipeFull.Additional.CPFC.Carbohydrates} г.{System.Environment.NewLine}";
                }

                authorNameRecipe.Text = $"Рецепт от: {recipeFull.Additional.AuthorName}";

                if (recipeFull.Additional.CountPortions != 0)
                    additionalInfoRecipe.Text += $"Количество порций: {recipeFull.Additional.CountPortions}.{System.Environment.NewLine}";
                if (recipeFull.Additional.PrepMinutes != 0)
                    additionalInfoRecipe.Text += $"Количество минут на готовку: {recipeFull.Additional.PrepMinutes} мин.";

                urlRecipe.Text = $"Ссылка на рецепт: {recipeFull.Url}";
                #endregion
            }
            catch
            {
                new Android.Support.V7.App.AlertDialog.Builder(this)
                    .SetTitle("Ошибка :(")
                    .SetMessage("К сожалению, не удалось загрузить данный рецепт. Разработчики уже занимаются этим вопросом.");
                
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