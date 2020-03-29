using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using Android.Content;
using Android.Net;
using Android.Views;
using Android.Widget;
using Square.Picasso;
using XamarinApp.Library.Objects;
using XamarinApp.Library.Objects.Boxes;
using XamarinApp.Library.Objects.Boxes.Elements;

namespace XamarinApp
{
    public class StepAdapter : BaseAdapter<RecipeFull>
    {
        private readonly RecipeFull _recipeFull;
        private readonly Context _context;
        private readonly List<StepRecipeBox> _stepRecipeBoxes;

        public StepAdapter(Context context, RecipeFull recipeFull)
        {
            _context = context;
            _recipeFull = recipeFull;
            _stepRecipeBoxes = new List<StepRecipeBox>();
            var lol = recipeFull.StepRecipesBoxes[1];
            _stepRecipeBoxes.AddRange(recipeFull.StepRecipesBoxes);
        }

        public override long GetItemId(int position)
        {
            return position;
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            view = LayoutInflater.From(_context).Inflate(Resource.Layout.list_steps, null, false);


            var stepDescription = view.FindViewById<TextView>(Resource.Id.stepDescription);
            stepDescription.Text = _stepRecipeBoxes[position].Description;


            var imageView = view.FindViewById<ImageView>(Resource.Id.stepImage);
            // TODO: Сделать несколько фоток, PictureBox.
            var url = _stepRecipeBoxes[position].PictureBox.Pictures.First().Url;

            Picasso.With(_context)
                .Load(url)
                .Into(imageView);

            return view;
        }
        
        public override int Count => _stepRecipeBoxes.Count;

        public override RecipeFull this[int position] => _recipeFull;
    }
}