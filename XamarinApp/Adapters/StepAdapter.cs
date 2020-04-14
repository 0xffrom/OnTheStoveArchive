using Android.Content;
using Android.Views;
using Android.Widget;
using ObjectsLibrary;
using ObjectsLibrary.Components;
using Square.Picasso;
using System.Collections.Generic;

namespace XamarinApp
{
    public class StepAdapter : BaseAdapter<RecipeFull>
    {
        private readonly RecipeFull _recipeFull;
        private readonly Context _context;
        private readonly List<StepRecipe> _stepRecipeBoxes;

        public StepAdapter(Context context, RecipeFull recipeFull)
        {
            _context = context;
            _recipeFull = recipeFull;
            _stepRecipeBoxes = new List<StepRecipe>();
            if (recipeFull.StepsRecipe != null)
                _stepRecipeBoxes.AddRange(recipeFull.StepsRecipe);
        }

        public override long GetItemId(int position)
        {
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = LayoutInflater.From(_context).Inflate(Resource.Layout.list_steps, null, false);


            var stepDescription = view.FindViewById<TextView>(Resource.Id.stepDescription);
            stepDescription.Text = _stepRecipeBoxes[position].Description;


            var imageView = view.FindViewById<ImageView>(Resource.Id.stepImage);
            
            var url = _stepRecipeBoxes[position].Image.ImageUrl;

            Picasso.With(_context)
                .Load(url)
                .Into(imageView);

            return view;
        }

        public override int Count => _stepRecipeBoxes.Count;

        public override RecipeFull this[int position] => _recipeFull;
    }
}