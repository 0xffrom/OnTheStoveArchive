using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace XamarinApp.ViewHolders
{
    public class PlateViewHolder : RecyclerView.ViewHolder
    {
        public RelativeLayout Background { get;  }
        public TextView Title { get; }

        public PlateViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Title = itemView.FindViewById<TextView>(Resource.Id.textView_title);
            Background = itemView.FindViewById<RelativeLayout>(Resource.Id.relativeLayout_background);

            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }
}