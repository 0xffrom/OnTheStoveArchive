using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Android.Content;
using Android.Net;
using Android.Views;
using Android.Widget;
using ObjectsLibrary.Objects;
using ObjectsLibrary.Objects.Boxes;
using ObjectsLibrary.Objects.Boxes.Elements;

namespace XamarinApp
{
    public class IngredientsAdapter : BaseAdapter<RecipeFull>
    {
        private readonly RecipeFull _recipeFull;
        private readonly Context _context;
        private readonly Ingredient[] _ingredients;
        
        public IngredientsAdapter(Context context, RecipeFull recipeFull)
        {
            this._recipeFull = recipeFull;
            _context = context;

            _ingredients = recipeFull.Ingredients;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            view = LayoutInflater.From(_context).Inflate(Resource.Layout.list_ingredients, null, false);

            var ingredientName = view.FindViewById<TextView>(Resource.Id.ingredientName);
            ingredientName.Text = _ingredients[position].Name;

            var ingredientUnit = view.FindViewById<TextView>(Resource.Id.ingredientUnit);
            ingredientUnit.Text = _ingredients[position].Unit;
            
            /*
            var ingredientCategory = view.FindViewById<TextView>(Resource.Id.ingredientCategory);
            ingredientCategory.Text = _recipeFull.IngredientsBoxes.First(x=> 
                x.Ingredients.Contains(_ingredients[position])).Title;
                */

            return view;
        }

        public override int Count => _ingredients.Length;

        public override RecipeFull this[int position] => _recipeFull;
    }
}