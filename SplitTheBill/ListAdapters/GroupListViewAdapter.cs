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
    class GroupListViewAdapter : BaseAdapter<Models.GroupModel>
    {
        public List<Models.GroupModel> mvarItems;
        private Context mvarContext;
		public Boolean mvarSelectionMode = false;
		public List<Models.GroupModel> mvarSelectedItems = new List<SplitTheBill.Models.GroupModel> ();

        public GroupListViewAdapter(Context Context, List<Models.GroupModel> Items)
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

        public override Models.GroupModel this[int position]
        {
            get { return mvarItems[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = LayoutInflater.From(mvarContext).Inflate(Resource.Layout.GroupListViewRow,null,false);

            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtGroupName);
            txtName.Text = mvarItems[position].mvarName;

            TextView txtMembers = row.FindViewById<TextView>(Resource.Id.txtMemberCount);
			if (mvarItems [position].mvarMemberCount > 0) {
				txtMembers.Text = mvarItems [position].mvarMemberCount.ToString () + " members";
			} else {
				txtMembers.Text = "No members";
			}

			if (!mvarItems [position].mvarFavourite)
				row.FindViewById<ImageView> (Resource.Id.ivFavourite).Visibility = ViewStates.Gone;

			if (mvarSelectionMode == true) {
				RelativeLayout lvarLayout = row.FindViewById<RelativeLayout> (Resource.Id.rlBackground);
				ImageView lvarIcon = new ImageView (mvarContext);
				RelativeLayout.LayoutParams lvarParams = new RelativeLayout.LayoutParams ((int)(30 * mvarContext.Resources.DisplayMetrics.Density), WindowManagerLayoutParams.MatchParent);
				lvarParams.AddRule (LayoutRules.AlignParentLeft);
				lvarIcon.SetScaleType (ImageView.ScaleType.CenterInside);
				lvarIcon.SetImageResource (Resource.Drawable.Icon_unchecked);
				lvarIcon.SetPadding ((int)(10 * mvarContext.Resources.DisplayMetrics.Density), (int)(10 * mvarContext.Resources.DisplayMetrics.Density), 0, (int)(10 * mvarContext.Resources.DisplayMetrics.Density));
				lvarIcon.Id = 1;
				lvarLayout.AddView (lvarIcon, 0, (ViewGroup.LayoutParams)lvarParams);
				LinearLayout lvarDetails = row.FindViewById<LinearLayout>(Resource.Id.VllGroupDetails);
				RelativeLayout.LayoutParams lvarParamsDetails = (RelativeLayout.LayoutParams)lvarDetails.LayoutParameters;
				lvarParamsDetails.RemoveRule (LayoutRules.AlignParentLeft);
				lvarParamsDetails.AddRule (LayoutRules.RightOf, 1);
				lvarDetails.LayoutParameters = lvarParamsDetails;
			}
				return row;
        }
    }
}