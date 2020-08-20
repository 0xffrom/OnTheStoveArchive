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
    public class RecipeScrollListener : RecyclerView.OnScrollListener
    {
        private LinearLayoutManager layoutManager;
        public event EventHandler LoadMoreEvent;

        public RecipeScrollListener(LinearLayoutManager layoutManager)
        {
            this.layoutManager = layoutManager;
        }

        public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
        {
            bool isLoading = false;
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