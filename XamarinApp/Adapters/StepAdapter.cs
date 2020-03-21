using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Android.Content;
using Android.Net;
using Android.Views;
using Android.Widget;
using XamarinApp.Library.Objects;
using XamarinApp.Library.Objects.Boxes;
using XamarinApp.Library.Objects.Boxes.Elements;

namespace XamarinApp
{
    public class StepAdapter : BaseAdapter<RecipeFull>
    {
        private readonly RecipeFull _recipeFull;
        private readonly Context _context;
        private readonly List<StepRecipeBox> _stepRecipeBoxes;

        public StepAdapter(Context context, RecipeFull recipeFull)
        {
            _context = context;
            _recipeFull = recipeFull;
            _stepRecipeBoxes = new List<StepRecipeBox>();
            var lol = recipeFull.StepRecipesBoxes[1];
            _stepRecipeBoxes.AddRange(recipeFull.StepRecipesBoxes);
        }

        public override long GetItemId(int position)
        {
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            view = LayoutInflater.From(_context).Inflate(Resource.Layout.list_steps, null, false);


            var stepDescription = view.FindViewById<TextView>(Resource.Id.stepDescription);
            stepDescription.Text = _stepRecipeBoxes[position].Description;


            var imageView = view.FindViewById<ImageView>(Resource.Id.stepImage);
            // TODO: Сделать несколько фоток, PictureBox.
            var url = _stepRecipeBoxes[position].PictureBox.Pictures.First().Url;

            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
                       + url.Split('/')[^1];

            var uri = Uri.Parse(path);
            if (!File.Exists(path))
                DownloadPicture(new WebClient(), url, path, uri, imageView);
            else
                imageView.SetImageURI(uri);

            return view;
        }

        private async void DownloadPicture(WebClient client, string url, string path, Android.Net.Uri uri,
            ImageView imageView)
        {
            await client.DownloadFileTaskAsync(url, path);
            imageView.SetImageURI(uri);
        }

        public override int Count => _stepRecipeBoxes.Count;

        public override RecipeFull this[int position] => _recipeFull;
    }
}