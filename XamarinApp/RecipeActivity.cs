using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Net;
using Refractored.Controls;
using Square.Picasso;
using XamarinApp.Library;
using XamarinApp.Library.Objects;
using File = System.IO.File;
using Picture = XamarinApp.Library.Objects.Boxes.Elements.Picture;

namespace XamarinApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon")]
    public class RecipeActivity : AppCompatActivity
    {
        private string _url;
        private RecipeFull _recipeFull;

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
            
            var frameDescription = FindViewById<View>(Resource.Id.frameDescription);
            var frameIngredients = FindViewById<View>(Resource.Id.frameIngredients);
            var frameSteps =FindViewById<View>(Resource.Id.frameSteps);


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
                
                textViewMainIngredients.SetTextColor(colorFEB22C);
                rectangleMainIngredients.SetBackgroundColor(colorFEB22C);
                
                frameDescription.Visibility = ViewStates.Invisible;
                frameIngredients.Visibility = ViewStates.Visible;
                frameSteps.Visibility = ViewStates.Invisible;
            };
            
            textViewMainRecipe.Click += (sender, args) =>
            {
                SetColorDefault(textViewMainDescription, textViewMainIngredients, textViewMainRecipe,
                    rectangleMainDescription, rectangleMainIngredients, rectangleMainRecipe);
                
                textViewMainRecipe.SetTextColor(colorFEB22C);
                rectangleMainRecipe.SetBackgroundColor(colorFEB22C);
                
                frameDescription.Visibility = ViewStates.Invisible;
                frameIngredients.Visibility = ViewStates.Invisible;
                frameSteps.Visibility = ViewStates.Visible;
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
            
            var listIngredients = FindViewById<ListView>(Resource.Id.listIngredients);
            var adapterIngredents = new IngredientsAdapter(this, _recipeFull);
            listIngredients.Adapter = adapterIngredents;

            var listSteps = FindViewById<ListView>(Resource.Id.listSteps);
            var adapterStep = new StepAdapter(this, _recipeFull);
            listSteps.Adapter = adapterStep;


            
            var title = FindViewById<TextView>(Resource.Id.titleRecipe);
            title.Text = _recipeFull.Title;

            var imageView = FindViewById<ImageView>(Resource.Id.imageMainRecipe);
            
            Picture picture = _recipeFull.TitlePicture;
            var url = picture.Url;
            
            Picasso.With(this)
                .Load(url)
                .Into(imageView);


            // TODO: Сделать фотографию с закруглёныни уголками

            var description = FindViewById<TextView>(Resource.Id.titleMainDescription);
            description.Text = _recipeFull.Description;
            description.Selected = true;
        }
        

        private static async Task<RecipeFull> UpdateCollectionRecipes(string url) => await Task.Run(() => HttpGet.GetPage(url));
    }
}