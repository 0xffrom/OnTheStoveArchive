using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ObjectsLibrary;
using Square.Picasso;
using XamarinAppLibrary;

namespace XamarinApp.Fragments
{
    public class RecipeDescriptionFragment : Android.Support.V4.App.Fragment
    {
        private RecipeFull _recipeFull;
        private ImageView _image;
        private TextView _description;
        private TextView _cpfcRecipe;
        private TextView _authorNameRecipe;
        private TextView _additionalInfoRecipe;
        private TextView _urlRecipe;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var arguments = Arguments;
            
            if (arguments != null)
            {
                _recipeFull = Data.ByteArrayToObject<RecipeFull>(arguments.GetByteArray("recipeFull"));
            }
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.recipe_description_fragment, container, false);
            
            _image = view.FindViewById<ImageView>(Resource.Id.imageMainRecipe);
            _description = view.FindViewById<TextView>(Resource.Id.titleMainDescription);
            _cpfcRecipe = view.FindViewById<TextView>(Resource.Id.CPFCRecipe);
            _authorNameRecipe = view.FindViewById<TextView>(Resource.Id.authorNameRecipe);
            _additionalInfoRecipe = view.FindViewById<TextView>(Resource.Id.additionalInfoRecipe);
            _urlRecipe = view.FindViewById<TextView>(Resource.Id.urlRecipe);
            
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Picasso.With(View.Context)
                .Load(_recipeFull.TitleImage.ImageUrl)
                .Into(_image);

            _description.Text += _recipeFull.Description;

            if (_recipeFull.Additional.CPFC != null)
            {
                if (_recipeFull.Additional.CPFC.Calories != 0)
                    _cpfcRecipe.Text += $"Калории: {_recipeFull.Additional.CPFC.Calories} Ккал.{System.Environment.NewLine}";
                if (_recipeFull.Additional.CPFC.Protein != 0)
                    _cpfcRecipe.Text += $"Белки: {_recipeFull.Additional.CPFC.Protein} г.{System.Environment.NewLine}";
                if (_recipeFull.Additional.CPFC.Fats != 0)
                    _cpfcRecipe.Text += $"Жиры: {_recipeFull.Additional.CPFC.Fats} г.{System.Environment.NewLine}";
                if (_recipeFull.Additional.CPFC.Carbohydrates != 0)
                    _cpfcRecipe.Text += $"Углеводы: {_recipeFull.Additional.CPFC.Carbohydrates} г.{System.Environment.NewLine}";
            }

            _authorNameRecipe.Text = $"Рецепт от: {_recipeFull.Additional.AuthorName}";

            if (_recipeFull.Additional.CountPortions != 0)
                _additionalInfoRecipe.Text += $"Количество порций: {_recipeFull.Additional.CountPortions}.{System.Environment.NewLine}";
            if (_recipeFull.Additional.PrepMinutes != 0)
                _additionalInfoRecipe.Text += $"Количество минут на готовку: {_recipeFull.Additional.PrepMinutes} мин.";

            _urlRecipe.Text = $"Ссылка на рецепт: {_recipeFull.Url}";
            
            base.OnActivityCreated(savedInstanceState);
        }
    }
}