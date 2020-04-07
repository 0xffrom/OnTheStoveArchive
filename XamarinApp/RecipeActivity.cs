using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ObjectsLibrary;
using Square.Picasso;
using System;
using System.Threading.Tasks;
using XamarinAppLibrary;

namespace XamarinApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon")]
    public class RecipeActivity : AppCompatActivity
    {
        private string _url;
        private RecipeFull _recipeFull;

        public readonly Color ColorFeb22C = new Color(254, 178, 44);
        public readonly Color ColorC4C4C4 = new Color(196, 196, 196);

        Button buttonStar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.recipe_main);

            var frameDescription = FindViewById<View>(Resource.Id.frameDescription);
            var frameIngredients = FindViewById<View>(Resource.Id.frameIngredients);
            var frameSteps = FindViewById<View>(Resource.Id.frameSteps);


            #region delegates

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
            #endregion



            _url = MainActivity.LastUrl;

            var buttonBack = FindViewById<LinearLayout>(Resource.Id.buttonBack);

            buttonBack.Click += new EventHandler((sender, args) =>
            {
                //  StartActivity(new Intent(this, typeof(MainActivity)));
                base.OnBackPressed();
            });

            buttonStar = FindViewById<Button>(Resource.Id.starRecipe);

            buttonStar.Click += new EventHandler((sender, args) =>
            {
                if (!LocalRecipe.ExistsRecipe(_url))
                {
                    LocalRecipe.SaveRecipe(_url, _recipeFull);
                    buttonStar.SetBackgroundResource(Resources.GetIdentifier("recipe_yellow_star", "drawable", PackageName));
                }
                else
                {
                    LocalRecipe.DeleteRecipe(_url);
                    buttonStar.SetBackgroundResource(Resources.GetIdentifier("recipe_white_star", "drawable", PackageName));
                }
            });


            UpdateView();
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
            if (LocalRecipe.ExistsRecipe(_url))
            {
                _recipeFull = LocalRecipe.GetRecipe(_url);
                buttonStar.SetBackgroundResource(Resources.GetIdentifier("recipe_yellow_star", "drawable", PackageName));
            }
            else
                _recipeFull = await UpdateCollectionRecipes(_url);

            var listIngredients = FindViewById<ListView>(Resource.Id.listIngredients);
            var adapterIngredents = new IngredientsAdapter(this, _recipeFull);
            if (adapterIngredents.Count > 0)
                listIngredients.Adapter = adapterIngredents;
            var listSteps = FindViewById<ListView>(Resource.Id.listSteps);
            var adapterStep = new StepAdapter(this, _recipeFull);
            listSteps.Adapter = adapterStep;


            #region Первая страница.

            var imageView = FindViewById<ImageView>(Resource.Id.imageMainRecipe);
            var title = FindViewById<TextView>(Resource.Id.titleRecipe);
            var description = FindViewById<TextView>(Resource.Id.titleMainDescription);
            var CPFCRecipe = FindViewById<TextView>(Resource.Id.CPFCRecipe);
            var authorNameRecipe = FindViewById<TextView>(Resource.Id.authorNameRecipe);
            var additionalInfoRecipe = FindViewById<TextView>(Resource.Id.additionalInfoRecipe);
            var urlRecipe = FindViewById<TextView>(Resource.Id.urlRecipe);

            title.Text = _recipeFull.Title;

            Picasso.With(this)
                .Load(_recipeFull.TitleImage.ImageUrl)
                .Into(imageView);

            description.Text += $"{_recipeFull.Description.Replace("  ",string.Empty)}";

            if (_recipeFull.Additional.CPFC != null)
            {
                if (_recipeFull.Additional.CPFC.Calories != 0)
                    CPFCRecipe.Text += $"Калории: {_recipeFull.Additional.CPFC.Calories} Ккал.{System.Environment.NewLine}";
                if(_recipeFull.Additional.CPFC.Protein != 0)
                    CPFCRecipe.Text += $"Белки: {_recipeFull.Additional.CPFC.Protein} г.{System.Environment.NewLine}";
                if (_recipeFull.Additional.CPFC.Fats != 0)
                    CPFCRecipe.Text += $"Жиры: {_recipeFull.Additional.CPFC.Fats} г.{System.Environment.NewLine}";
                if (_recipeFull.Additional.CPFC.Carbohydrates != 0)
                    CPFCRecipe.Text += $"Углеводы: {_recipeFull.Additional.CPFC.Carbohydrates} г.{System.Environment.NewLine}";
            }

            authorNameRecipe.Text = $"Рецепт от: {_recipeFull.Additional.AuthorName}";

            if (_recipeFull.Additional.CountPortions != 0)
                additionalInfoRecipe.Text += $"Количество порций: {_recipeFull.Additional.CountPortions}.{System.Environment.NewLine}";
            if (_recipeFull.Additional.PrepMinutes != 0)
                additionalInfoRecipe.Text += $"Количество минут на готовку: {_recipeFull.Additional.PrepMinutes} мин.";

            urlRecipe.Text = $"Ссылка на рецепт: {_recipeFull.Url}";
            #endregion
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private static async Task<RecipeFull> UpdateCollectionRecipes(string url) => await Task.Run(() => HttpGet.GetPage(url));
    }
}