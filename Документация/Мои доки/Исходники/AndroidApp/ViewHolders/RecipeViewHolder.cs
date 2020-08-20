using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace XamarinApp.ViewHolders
{
    public class RecipeViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; }
        public TextView Title { get; }
        public TextView Link { get; }

        public RecipeViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Image = itemView.FindViewById<ImageView>(Resource.Id.imageTitle);
            Title = itemView.FindViewById<TextView>(Resource.Id.title);
            Link = itemView.FindViewById<TextView>(Resource.Id.textLink);

            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }
}