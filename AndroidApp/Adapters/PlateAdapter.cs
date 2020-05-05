using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using AndroidApp.General;

namespace AndroidApp
{
    public class PlateAdapter : RecyclerView.Adapter
    {
        private readonly List<Plate> _items;
        private readonly Activity _activity;

        public event EventHandler<int> ItemClick;
        public override int ItemCount => _items.Count;

        public PlateAdapter(List<Plate> plates, Activity activity)
        {
            _activity = activity;
            _items = plates;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PlateViewHolder vh = holder as PlateViewHolder;

            vh.Title.Text = _items[position].Title;

            vh.Background.SetBackgroundResource(_activity.Resources.GetIdentifier(_items[position].Background,
                "drawable", _activity.PackageName));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_plates, parent, false);
            PlateViewHolder vh = new PlateViewHolder(itemView, OnClick);


            return vh;
        }

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }

    public class PlateViewHolder : RecyclerView.ViewHolder
    {
        public RelativeLayout Background { get; private set; }
        public TextView Title { get; private set; }

        public PlateViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.textView_title);
            Background = itemView.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_background);

            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }
}