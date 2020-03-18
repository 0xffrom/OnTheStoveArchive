using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using XamarinApp.Library;
using XamarinApp.Library.Objects;
using Picture = XamarinApp.Library.Objects.Boxes.Elements.Picture;

namespace XamarinApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon")]
    public class RecipeActivity : AppCompatActivity
    {
        private string _url;
        private RecipeFull _recipeFull;

        private View _frameRecipe;
        // #FEB22C: 254 178 44
        Color colorFEB22C = new Color(254, 178, 44);

        // #C4C4C4: 196 196 196
        Color colorC4C4C4 = new Color(196, 196, 196);
        
      
        
        private void SetColorDefault(TextView textViewMainDescription, TextView textViewMainIngredients,
            TextView textViewMainRecipe,
            View rectangleMainDescription, View rectangleMainIngredients, View rectangleMainRecipe)
        {
            textViewMainDescription.SetTextColor(colorC4C4C4);
            textViewMainIngredients.SetTextColor(colorC4C4C4);
            textViewMainRecipe.SetTextColor(colorC4C4C4);

            rectangleMainDescription.SetBackgroundColor(colorC4C4C4);
            rectangleMainIngredients.SetBackgroundColor(colorC4C4C4);
            rectangleMainRecipe.SetBackgroundColor(colorC4C4C4);
        }

        private void SetColorStart(TextView textViewMainDescription, TextView textViewMainIngredients,
            TextView textViewMainRecipe,
            View rectangleMainDescription, View rectangleMainIngredients, View rectangleMainRecipe)
        {
            SetColorDefault(textViewMainDescription, textViewMainIngredients, textViewMainRecipe,
                rectangleMainDescription, rectangleMainIngredients, rectangleMainRecipe);
                
            textViewMainDescription.SetTextColor(colorFEB22C);
            rectangleMainDescription.SetBackgroundColor(colorFEB22C);
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.recipe_main);

            _frameRecipe = FindViewById<View>(Resource.Id.frameRecipe);
            
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
            };

            textViewMainIngredients.Click += (sender, args) =>
            {
                SetColorDefault(textViewMainDescription, textViewMainIngredients, textViewMainRecipe,
                    rectangleMainDescription, rectangleMainIngredients, rectangleMainRecipe);
                
                textViewMainIngredients.SetTextColor(colorFEB22C);
                rectangleMainIngredients.SetBackgroundColor(colorFEB22C);
            };
            
            textViewMainRecipe.Click += (sender, args) =>
            {
                SetColorDefault(textViewMainDescription, textViewMainIngredients, textViewMainRecipe,
                    rectangleMainDescription, rectangleMainIngredients, rectangleMainRecipe);
                
                textViewMainRecipe.SetTextColor(colorFEB22C);
                rectangleMainRecipe.SetBackgroundColor(colorFEB22C);
            };
            #endregion


           
            _url = MainActivity.lastUrl;

            var buttonBack = FindViewById<Button>(Resource.Id.buttonBack);

            buttonBack.Click += new EventHandler((sender, args) =>
            {
                //  StartActivity(new Intent(this, typeof(MainActivity)));
                base.OnBackPressed();
            });

            UpdateView();
        }

        private async void UpdateView()
        {
            _recipeFull = await UpdateCollectionRecipes(_url);
            var title = FindViewById<TextView>(Resource.Id.titleRecipe);
            title.Text = _recipeFull.Title;

            var imageView = FindViewById<ImageView>(Resource.Id.imageMainRecipe);

            Toast.MakeText(this, $"{_recipeFull.TitlePicture}...", ToastLength.Short).Show();

            Picture picture = _recipeFull.TitlePicture;
            var url = picture.Url;

            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
                       + _url.Replace('/', '_').Replace(':', '_');

            if (!File.Exists(path))
                DownloadPicture(new WebClient(), url, path);

            var uri = Android.Net.Uri.Parse(path);

            imageView.SetImageURI(uri);

            var description = FindViewById<TextView>(Resource.Id.titleMainDescription);
            description.Text = _recipeFull.Description;
        }

        private void DownloadPicture(WebClient client, string url, string path) =>
            client.DownloadFile(url, path);

        private async Task<RecipeFull> UpdateCollectionRecipes(string url)
        {
            return await Task.Run(() => HttpGet.GetPage(url));
        }
    }
}