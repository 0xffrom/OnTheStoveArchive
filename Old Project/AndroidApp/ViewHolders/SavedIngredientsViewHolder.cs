using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace XamarinApp.ViewHolders
{
    public class SavedIngredientsViewHolder : RecyclerView.ViewHolder
    {
        public TextView Title { get; }
        public TextView Unit { get; }
        public TextView RecipeName { get; }

        public SavedIngredientsViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            var CartButton = itemView.FindViewById<Button>(Resource.Id.cart_button_saved_ingredient);
            Title = itemView.FindViewById<TextView>(Resource.Id.title_saved_ingredient);
            Unit = itemView.FindViewById<TextView>(Resource.Id.unit_saved_ingredient);
            RecipeName = itemView.FindViewById<TextView>(Resource.Id.recipe_saved_ingredient);

            CartButton.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }
}