using Android.Support.V7.Widget;
using Android.Views;
using ObjectsLibrary.Components;
using XamarinApp;
using System.Collections.Generic;
using AndroidLibrary;
using XamarinApp.ViewHolders;

namespace AndroidApp
{
    public class SavedIngredientsAdapter : RecyclerView.Adapter
    {
        private readonly List<Ingredient> _items;
        public override int ItemCount => _items.Count;

        public SavedIngredientsAdapter(List<Ingredient> ingredients)
        {
            _items = ingredients;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            SavedIngredientsViewHolder vh = holder as SavedIngredientsViewHolder;

            vh.Title.Text = _items[position].Name;
            vh.Unit.Text = _items[position].Unit;
            vh.RecipeName.Text = _items[position].RecipeName;
        }

        private void RemoveItem(int position)
        {
            NotifyItemRemoved(position);
            IngredientData.DeleteIngredient(_items[position]);
            _items.RemoveAt(position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.list_ingredients_saved, parent, false);
            var vh = new SavedIngredientsViewHolder(itemView, RemoveItem);
            return vh;
        }
    }
}