using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget; 
using XamarinApp.Library;
using XamarinApp.Library.Objects;
using XamarinApp.Library.Objects.Boxes.Elements;

namespace XamarinApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon")]
    public class RecipeActivity : AppCompatActivity
    {
        private string _url;
        private RecipeFull _recipeFull;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.recipe_main);

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
                       + _url.Replace('/', '_').Replace(':','_');

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