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
using ImageViews.Rounded;

namespace SplitTheBill
{
	class ContributionListViewAdapter : BaseAdapter<Models.ContributionModel>
	{
		public List<Models.ContributionModel> mvarItems;
		public List<Models.MemberModel> mvarMembers;
		private Context mvarContext;
		private String mvarGroupId;
		public List<Models.ContributionModel> mvarSelectedItems = new List<SplitTheBill.Models.ContributionModel> ();
		public Boolean mvarSelectionMode = false;
		public Boolean mvarExpanded = false;

		public ContributionListViewAdapter(Context Context, List<Models.ContributionModel> Items, List<Models.MemberModel> Members, String Group)
		{
			mvarContext = Context;
			mvarItems = Items;
			mvarMembers = Members;
			mvarGroupId = Group;
		}

		public override int Count
		{
			get { return mvarItems.Count; }
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override Models.ContributionModel this [int position] {
			get { return mvarItems [position]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = LayoutInflater.From (mvarContext).Inflate (Resource.Layout.ContributionsListViewRow, null, false);

			TextView lvarName = row.FindViewById<TextView> (Resource.Id.txtName);
			TextView lvarContributionName = row.FindViewById<TextView> (Resource.Id.txtContributionName);
			TextView lvarContributionField = row.FindViewById<TextView> (Resource.Id.txtContribution);
			TextView lvarDate = row.FindViewById<TextView> (Resource.Id.txtDate);

			Models.ContributionModel lvarContribution = mvarItems [position];
			Models.MemberModel lvarMember = mvarMembers.Find (Member => Member.mvarID == lvarContribution.mvarMemberId);

			if (lvarMember.mvarSurname != "")
				lvarName.Text = lvarMember.mvarName + " " + lvarMember.mvarSurname.Substring(0,1) + ".";
			else
				lvarName.Text = lvarMember.mvarName;
			
			lvarContributionName.Text = lvarContribution.mvarName;
			lvarContributionField.Text = lvarContribution.mvarAmount + " " + lvarContribution.mvarCurrency;

			if (lvarContribution.mvarDate.ToString ("yy-MM-dd") == DateTime.Today.ToString ("yy-MM-dd")) {
				lvarDate.Text = lvarContribution.mvarDate.ToString ("t");
			} else if (lvarContribution.mvarDate.ToString ("yy-MM-dd") == DateTime.Today.AddDays (-1).ToString ("yy-MM-dd")) {
				lvarDate.Text = "Yesterday";
			} else {
				lvarDate.Text = lvarContribution.mvarDate.ToString ("dd.MM.yyyy");
			}

			if (mvarSelectionMode == true) {
				RelativeLayout lvarLayout = row.FindViewById<RelativeLayout> (Resource.Id.rlBackground);
				ImageView lvarIcon = new ImageView (mvarContext);
				RelativeLayout.LayoutParams lvarParams = new RelativeLayout.LayoutParams ((int)(30 * mvarContext.Resources.DisplayMetrics.Density), WindowManagerLayoutParams.MatchParent);
				lvarParams.AddRule (LayoutRules.AlignParentLeft);
				lvarIcon.SetScaleType (ImageView.ScaleType.CenterInside);
				lvarIcon.SetImageResource (Resource.Drawable.Icon_unchecked);
				lvarIcon.Id = 1;
				lvarLayout.AddView (lvarIcon, 0, (ViewGroup.LayoutParams)lvarParams);
				RelativeLayout.LayoutParams lvarParamsName = (RelativeLayout.LayoutParams)lvarName.LayoutParameters;
				lvarParamsName.RemoveRule (LayoutRules.AlignParentLeft);
				lvarParamsName.AddRule (LayoutRules.RightOf, 1);
				lvarName.LayoutParameters = lvarParamsName;
			}
			return row;
		}

		public override void NotifyDataSetChanged ()
		{
			mvarItems = mvarItems.OrderByDescending (Con => Con.mvarDate).ToList();
			base.NotifyDataSetChanged ();
		}
	}
}