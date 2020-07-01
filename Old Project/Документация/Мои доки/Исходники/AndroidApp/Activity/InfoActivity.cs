using System;
using Android.Widget;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using XamarinApp;

namespace AndroidApp
{
    [Activity(Label = "На плите!", Theme = "@style/AppTheme.NoActionBar", Icon = "@drawable/icon",
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    internal class InfoActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_info);

            var relativeLayoutBack = FindViewById<RelativeLayout>(Resource.Id.relativeLayoutBack);
            var buttonBack = FindViewById<Button>(Resource.Id.buttonBack);

            relativeLayoutBack.Click += OnBackPressed;
            buttonBack.Click += OnBackPressed;
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