using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Android.Content;
using Android.Net;
using Android.Views;
using Android.Widget;
using RecipesAndroid.Objects;

namespace XamarinApp
{
    public class RecipeShortAdapter : BaseAdapter<RecipeShort>
    {
        private readonly List<RecipeShort> _list;
        private Context _context;

        public RecipeShortAdapter(Context context, List<RecipeShort> list)
        {
            this._list = list;
            _context = context;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            //if (view == null)
            view = LayoutInflater.From(_context).Inflate(Resource.Layout.list_item, null, false);

            TextView textView = view.FindViewById<TextView>(Resource.Id.title);
            textView.Text = _list[position].Title;



            TextView textLink = view.FindViewById<TextView>(Resource.Id.textLink);
            textLink.Text = _list[position].Url.Split('/')[2];

            ImageView imageView = view.FindViewById<ImageView>(Resource.Id.imageTitle);

            string url = _list[position].Picture.Url;
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData)
            + url.Split('/')[^1];

            if (!File.Exists(path))
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(url, path);
                }
            }

            Uri uri;
            uri = Uri.Parse(path);

            imageView.SetImageURI(uri);


            return view;
        }

        public override int Count => _list.Count;

        public override RecipeShort this[int position] => _list[position];
    }
}