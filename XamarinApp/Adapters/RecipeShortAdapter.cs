using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.Lang;
using Square.Picasso;
using ObjectsLibrary.Objects;
using Toolbar = Android.Widget.Toolbar;

namespace XamarinApp
{
    public class RecipeShortAdapter : BaseAdapter<RecipeShort>
    {
        private readonly List<RecipeShort> _list;
        private readonly Context _context;

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
            
            var textView = view.FindViewById<TextView>(Resource.Id.title);
            textView.Text = _list[position]?.Title;
            
            var textLink = view.FindViewById<TextView>(Resource.Id.textLink);
            var urlArray = _list[position]?.Url.Split('/');
            if (urlArray != null && urlArray.Length >= 2)
                textLink.Text = urlArray[2];

            var imageView = view.FindViewById<ImageView>(Resource.Id.imageTitle);
            var url = _list[position]?.Picture.Url;
            

            
            Picasso.With(_context)
                .Load(url)
                .Into(imageView);
            
            
            return view;
        }
        
        
        public override int Count => _list.Count;

        public override RecipeShort this[int position] => _list[position];
    }
}