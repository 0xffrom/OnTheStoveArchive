using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using ObjectsLibrary;
using XamarinApp.General;

namespace XamarinApp.Listeners
{
    class RecipesFilter : Filter
    {
        private readonly RecipeAdapter _adapter;
        public RecipesFilter(RecipeAdapter adapter)
        {
            _adapter = adapter;
        }

        protected override FilterResults PerformFiltering(ICharSequence constraint)
        {
            var returnObj = new FilterResults();
            var results = new List<RecipeShort>();
            if (_adapter._originalData == null)
                _adapter._originalData = _adapter._items;

            if (constraint == null) return returnObj;

            if (_adapter._originalData != null && _adapter._originalData.Any())
            {
                results.AddRange(
                    _adapter._originalData.Where(
                        chemical => chemical.Title.ToLower().Contains(constraint.ToString())));
            }

            // Nasty piece of .NET to Java wrapping, be careful with this!
            returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
            returnObj.Count = results.Count;

            constraint.Dispose();

            return returnObj;
        }

        protected override void PublishResults(ICharSequence constraint, FilterResults results)
        {
            using (var values = results.Values)
                _adapter._items = values.ToArray<Java.Lang.Object>()
                    .Select(r => r.ToNetObject<RecipeShort>()).ToList();

            _adapter.NotifyDataSetChanged();

            // Don't do this and see GREF counts rising
            constraint.Dispose();
            results.Dispose();
        }
    }
}