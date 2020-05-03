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
            _recipeFull = recipeFull;
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
            convertView = LayoutInflater.From(_context).Inflate(Resource.Layout.list_ingredients, null, false);

            var ingredientName = convertView.FindViewById<TextView>(Resource.Id.ingredientName);
            var ingredientUnit = convertView.FindViewById<TextView>(Resource.Id.ingredientUnit);
            var checkBox = convertView.FindViewById<CheckBox>(Resource.Id.ingredientCheck);
         
            ingredientName.Text = _ingredients[position].Name;
            ingredientUnit.Text = _ingredients[position].Unit;


            checkBox.CheckedChange += (sender, e) =>
            {
                if (e.IsChecked)
                {
                    Toast.MakeText(_context, "Ингредиент добавлен в корзину продуктов!", ToastLength.Short).Show();
                    IngredientData.SaveIngredient(_ingredients[position]);
                }
                else
                {
                    Toast.MakeText(_context, "Ингредиент убран из корзины продуктов!", ToastLength.Short).Show();
                    IngredientData.DeleteIngredient(_ingredients[position]);
                }
            };

            if (IngredientData.ExistsIngredient(_ingredients[position]))
                checkBox.Checked = true;
            else
                checkBox.Checked = false;

            return convertView;
        }
  
        public override int Count => _ingredients.Length;

        public override RecipeFull this[int position] => _recipeFull;
    }
}