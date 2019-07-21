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

namespace SplitTheBill
{
	class MembersListViewAdapter : BaseAdapter<Models.MemberModel>
	{
		public List<Models.MemberModel> mvarItems;
		private Context mvarContext;
		public List<Models.MemberModel> mvarSelectedItems;
		public MembersListViewAdapter(Context Context, List<Models.MemberModel> Items, List<Models.MemberModel> SelectedItems)
		{
			mvarContext = Context;
			mvarItems = Items;
			mvarSelectedItems = SelectedItems;
		}
		public override int Count
		{
			get { return mvarItems.Count; }
		}
		public override long GetItemId(int position)
		{
			return position;
		}

		public override Models.MemberModel this[int position]
		{
			get { return mvarItems[position]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = new View(mvarContext);

			if (mvarItems [position] != null) {
				if (mvarSelectedItems != null && mvarSelectedItems.Contains (mvarItems [position]) == true) {
					row = LayoutInflater.From (mvarContext).Inflate (Resource.Layout.MembersListViewRow, null, false);
					ImageView lvarButton = row.FindViewById<ImageView> (Resource.Id.ivSelect);
					LinearLayout lvarBackground = row.FindViewById<LinearLayout> (Resource.Id.VllBackground);
					lvarButton.SetImageResource (Resource.Drawable.Icon_checked);
					lvarBackground.SetBackgroundColor (Color.ParseColor ("#30000000"));
				} else {
					row = LayoutInflater.From (mvarContext).Inflate (Resource.Layout.MembersListViewRow, null, false);
					ImageView lvarButton = row.FindViewById<ImageView> (Resource.Id.ivSelect);
					LinearLayout lvarBackground = row.FindViewById<LinearLayout> (Resource.Id.VllBackground);
					lvarButton.SetImageResource (Resource.Drawable.Icon_unchecked);
					lvarBackground.SetBackgroundColor (Color.ParseColor ("#ffffff"));
				}
			}

			TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
			txtName.Text = mvarItems [position].mvarName + " " + mvarItems [position].mvarSurname;

			var lvarSelect = row.FindViewById<ImageView>(Resource.Id.ivSelect);
			lvarSelect.Focusable = false;
			lvarSelect.FocusableInTouchMode = false;
				return row;
		}
	}
}