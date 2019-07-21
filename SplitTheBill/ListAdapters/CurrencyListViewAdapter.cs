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
using Android.Graphics;
using Xamarin.Contacts;
using ImageViews.Rounded;

namespace SplitTheBill
{
	class CurrencyListViewAdapter : BaseAdapter<CurrencyModel>
	{
		public List<CurrencyModel> mvarItems;
		private Context mvarContext;
		public CurrencyListViewAdapter(Context Context, List<CurrencyModel> Items)
		{
			mvarContext = Context;
			mvarItems = Items;
		}
		public override int Count
		{
			get { return mvarItems.Count; }
		}
		public override long GetItemId(int position)
		{
			return position;
		}

		public override CurrencyModel this[int position]
		{
			get { return mvarItems[position]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = LayoutInflater.From (mvarContext).Inflate (Resource.Layout.CurrenciesListViewRow, null, false);
		
			TextView lvarRate = row.FindViewById<TextView> (Resource.Id.txtRate);
			lvarRate.Text = Math.Round(mvarItems [position].mvarRate,2).ToString();
			if (Math.Round (mvarItems [position].mvarRate, 2) == 0)
				lvarRate.Text = "< 0.01";
			TextView lvarSymbol = row.FindViewById<TextView> (Resource.Id.txtSymbol);
			lvarSymbol.Text = mvarItems [position].mvarSymbol;

			ImageView lvarArrow = row.FindViewById<ImageView> (Resource.Id.ivArrowChange);
			TextView lvarChange = row.FindViewById<TextView> (Resource.Id.txtValueChange);
			lvarChange.Gravity = GravityFlags.Left | GravityFlags.CenterVertical;

			if (Math.Round(mvarItems [position].mvarChange,2) > 0) {
				lvarArrow.SetImageResource (Resource.Drawable.Arrow_up);
				lvarChange.SetTextColor (Android.Graphics.Color.Green);
			} else if (Math.Round(mvarItems [position].mvarChange,3) < 0) {
				lvarArrow.SetImageResource (Resource.Drawable.Arrow_down);
				lvarChange.SetTextColor (Android.Graphics.Color.Red);
			} else {
				lvarArrow.Visibility = ViewStates.Gone;
				lvarChange.Visibility = ViewStates.Gone;
			}
			lvarChange.Text = Math.Abs(Math.Round (mvarItems [position].mvarChange,2)).ToString();

			return row;
		}
	}
}