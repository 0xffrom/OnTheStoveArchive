using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace AndroidApp
{
    public class RecipeListener : RecyclerView.OnScrollListener
    {
        private LinearLayoutManager layoutManager;
        private bool isLoading = false;

        public event EventHandler LoadMoreEvent;

        public RecipeListener(LinearLayoutManager layoutManager)
        {
            this.layoutManager = layoutManager;
        }

        public override void OnScrolled(Android.Support.V7.Widget.RecyclerView recyclerView, int dx, int dy)
        {
            base.OnScrolled(recyclerView, dx, dy);

            var visibleItemCount = recyclerView.ChildCount;
            var totalItemCount = recyclerView.GetAdapter().ItemCount;
            var pastVisiblesItems = layoutManager.FindFirstVisibleItemPosition();

            if ((visibleItemCount + pastVisiblesItems) >= totalItemCount - 4 && !isLoading && recyclerView.Clickable)
            {
                isLoading = true;

                LoadMoreEvent?.Invoke(this, null);
                // Loading code.

                isLoading = false;
            }
        }
    }
}