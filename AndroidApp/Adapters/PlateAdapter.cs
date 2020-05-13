using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using XamarinApp.ViewHolders;
using System;
using System.Collections.Generic;
using XamarinApp;
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
        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }
}