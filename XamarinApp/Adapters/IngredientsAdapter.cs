using Android.Content;
using Android.Views;
using Android.Widget;
using ObjectsLibrary;
using ObjectsLibrary.Components;
using XamarinAppLibrary;

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

            if (recipeFull.Ingredients != null)
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

            var checkBox = view.FindViewById<CheckBox>(Resource.Id.ingredientCheck);
            checkBox.CheckedChange += (sender, e) =>
                 {
                     if (e.IsChecked)
                     {
                         IngredientData.SaveIngredient(_ingredients[position]);
                     }
                     else
                     {
                         IngredientData.DeleteIngredient(_ingredients[position]);
                     }
                 };

            if (IngredientData.ExistsIngredient(_ingredients[position]))
                checkBox.Checked = true;
            else
                checkBox.Checked = false;

            return view;
        }



        public override int Count => _ingredients.Length;

        public override RecipeFull this[int position] => _recipeFull;
    }
}