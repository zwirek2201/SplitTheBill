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
using ImageViews.Rounded;

namespace SplitTheBill
{
	[Activity (Label = "GroupDetails")]			
	public class GroupDetails : Activity
	{
		private String mvarGroupId = "";
		private List<Models.MemberModel> mvarMembers = null;
		private List<Models.PaymentModel> mvarPayments = null;
		private List<Models.ContributionModel> mvarContributions = null;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			Window.RequestFeature (WindowFeatures.NoTitle);
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.GroupDetails);

			DBRepository db = new DBRepository ();

			mvarGroupId = Intent.GetStringExtra ("GroupId");
		
			Models.GroupModel lvarGroup = db.GetGroupById (mvarGroupId);
			mvarMembers = db.GetMembersInGroup (mvarGroupId);
			mvarContributions = GetContributions ();

			EditText lvarGroupName = FindViewById<EditText> (Resource.Id.txtGroupName);
			lvarGroupName.Text = lvarGroup.mvarName;
			lvarGroupName.TextChanged += LvarGroupName_TextChanged;

			ImageView lvarFavourite = FindViewById<ImageView> (Resource.Id.ivFavourite);

			if (lvarGroup.mvarFavourite)
				lvarFavourite.SetImageResource (Resource.Drawable.Favourite_on);
			else
				lvarFavourite.SetImageResource (Resource.Drawable.Favourite_off);

			lvarFavourite.Click += LvarFavourite_Click;


			LinearLayout lvarWrapper = FindViewById<LinearLayout> (Resource.Id.VllWrapper);
			lvarWrapper.AddView(GetHeaderView("Members", GravityFlags.Left,10),0);

			PopulateScrollView (mvarMembers);
			List<Models.PaymentModel> lvarPayments = null;

			//if (db.BaseCurrencyChanged)
				//lvarPayments = GetPayments ().OrderByDescending (Pay => Pay.mvarSettled).ToList ();
			//else
				lvarPayments = db.GetPaymentsInGroup (mvarGroupId).OrderByDescending(Pay => Pay.mvarSettled).ToList ();
			
			ExpandableHeightListView lvarListViewPayments = FindViewById<ExpandableHeightListView> (Resource.Id.lvPayments);
			PaymentListViewAdapter lvarAdapterPayments = new PaymentListViewAdapter (this, lvarPayments, mvarMembers, mvarGroupId);

			lvarListViewPayments.Adapter = lvarAdapterPayments;
			lvarListViewPayments.setExpanded (true);
			lvarListViewPayments.AddHeaderView (GetHeaderView("Payments", GravityFlags.Left,5));

			ExpandableHeightListView lvarListViewContributions = FindViewById<ExpandableHeightListView> (Resource.Id.lvHistory);
			List<Models.ContributionModel> lvarContributions = mvarContributions.OrderByDescending(Cont => Cont.mvarDate).ToList();
			ContributionListViewAdapter lvarAdapterContributions = new ContributionListViewAdapter (this, lvarContributions, mvarMembers, mvarGroupId);
			lvarListViewContributions.Adapter = lvarAdapterContributions;
			lvarListViewContributions.setExpanded (true);
			lvarListViewContributions.AddHeaderView (GetHeaderView("Contributions",GravityFlags.Left,5));
			lvarListViewContributions.ItemLongClick += LvarListViewContributions_ItemLongClick;
			lvarListViewContributions.ItemClick += LvarListViewContributions_ItemClick;

			Button lvarAddContribution = FindViewById<Button> (Resource.Id.btnAddContribution);
			lvarAddContribution.Click += LvarAddContribution_Click;

			ImageButton lvarToolBox1 = FindViewById<ImageButton> (Resource.Id.btnToolBox1);
			lvarToolBox1.Click += LvarToolBox1_Click;
			ImageButton lvarToolBox2 = FindViewById<ImageButton> (Resource.Id.btnToolBox2);
			lvarToolBox2.Click += LvarToolBox2_Click;
			ImageButton lvarToolBox3 = FindViewById<ImageButton> (Resource.Id.btnToolBox3);
			lvarToolBox3.Click += LvarToolBox3_Click;
			ImageButton lvarToolBox4 = FindViewById<ImageButton> (Resource.Id.btnToolBox4);
			lvarToolBox4.Click += LvarToolBox4_Click;

			if (lvarContributions.Count == 0) {
				lvarListViewContributions.Visibility = ViewStates.Invisible;
				lvarListViewPayments.Visibility = ViewStates.Invisible;
				FindViewById<TextView> (Resource.Id.txtNothing).Text = "Nothing to see here!\nClick the add button to add a contribution";
			} else {
				FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Invisible;
			}
		}

		void LvarFavourite_Click (object sender, EventArgs e)
		{
			DBRepository db = new DBRepository ();
			Models.GroupModel lvarGroup = db.GetGroupById (mvarGroupId);
			ImageView lvarImage = FindViewById<ImageView> (Resource.Id.ivFavourite);
			if (lvarGroup.mvarFavourite) {
				db.UpdateGroupeFavourite (mvarGroupId, false);
				lvarImage.SetImageResource (Resource.Drawable.Favourite_off);
				lvarGroup.mvarFavourite = false;
			} else {
				db.UpdateGroupeFavourite (mvarGroupId, true);
				lvarImage.SetImageResource (Resource.Drawable.Favourite_on);
				lvarGroup.mvarFavourite = true;
			}
			db.MainScreenDataChanged = true;
		}

		void LvarGroupName_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			DBRepository db = new DBRepository ();
			EditText lvarText = (EditText)sender;
			if (!String.IsNullOrWhiteSpace(lvarText.Text)) {
				db.UpdateGroupName (mvarGroupId, lvarText.Text);
				db.MainScreenDataChanged = true;
			}
		}

		void LvarToolBox1_Click (object sender, EventArgs e)
		{
			ExpandableHeightListView lvarListViewContributions = FindViewById<ExpandableHeightListView> (Resource.Id.lvHistory);
			HeaderViewListAdapter lvarHeaderAdapter = (HeaderViewListAdapter)lvarListViewContributions.Adapter;
			ContributionListViewAdapter lvarAdapter = (ContributionListViewAdapter)lvarHeaderAdapter.WrappedAdapter;
			if (lvarAdapter.mvarSelectionMode) {
			
			} else {
			
			}
		}

		void LvarToolBox2_Click (object sender, EventArgs e)
		{

		}

		void LvarToolBox3_Click (object sender, EventArgs e)
		{

		}

		void LvarToolBox4_Click (object sender, EventArgs e)
		{
			DBRepository db = new DBRepository ();
			ExpandableHeightListView lvarListViewContributions = FindViewById<ExpandableHeightListView> (Resource.Id.lvHistory);
			HeaderViewListAdapter lvarHeaderAdapter = (HeaderViewListAdapter)lvarListViewContributions.Adapter;
			ContributionListViewAdapter lvarAdapter = (ContributionListViewAdapter)lvarHeaderAdapter.WrappedAdapter;
			if (lvarAdapter.mvarSelectionMode) {
			//ACTION FOR REMOVE BUTTON
				db.RemoveContributions(lvarAdapter.mvarSelectedItems);
				foreach (Models.ContributionModel lvarContribution in lvarAdapter.mvarSelectedItems) {
					mvarContributions.Remove (mvarContributions.Find(Cont => Cont.mvarId == lvarContribution.mvarId));

				}
				lvarAdapter.mvarItems = mvarContributions;
				lvarAdapter.mvarSelectedItems.Clear ();
				lvarAdapter.mvarSelectionMode = false;
				lvarAdapter.NotifyDataSetChanged ();
				GetRegularToolBox ();

				ExpandableHeightListView lvarListViewPayments = FindViewById<ExpandableHeightListView> (Resource.Id.lvPayments);
				HeaderViewListAdapter lvarHeaderAdapter2 = (HeaderViewListAdapter)lvarListViewPayments.Adapter;
				PaymentListViewAdapter lvarAdapterPayments = (PaymentListViewAdapter)lvarHeaderAdapter2.WrappedAdapter;	
				List<Models.PaymentModel> lvarPayments = GetPayments ();

				lvarAdapterPayments.mvarItems = lvarPayments.OrderByDescending(Pay => Pay.mvarSettled).ToList();
				lvarAdapterPayments.NotifyDataSetChanged ();
				db.MainScreenDataChanged = true;

				if (lvarAdapter.mvarItems.Count == 0) {
					lvarListViewContributions.Visibility = ViewStates.Gone;
					lvarListViewPayments.Visibility = ViewStates.Gone;
					FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Visible;
					FindViewById<TextView> (Resource.Id.txtNothing).Text = "Nothing to see here!\nClick the add button to add a contribution";
				}
			} else {
			
			}
		}
			
		void LvarListViewContributions_ItemLongClick (object sender, AdapterView.ItemLongClickEventArgs e)
		{
			ExpandableHeightListView lvarListViewContributions = FindViewById<ExpandableHeightListView> (Resource.Id.lvHistory);
			HeaderViewListAdapter lvarHeaderAdapter = (HeaderViewListAdapter)lvarListViewContributions.Adapter;
			ContributionListViewAdapter lvarAdapterContributions = (ContributionListViewAdapter)lvarHeaderAdapter.WrappedAdapter;
			LinearLayout lvarLayout = FindViewById<LinearLayout> (Resource.Id.HllToolBox);

			if(lvarAdapterContributions.mvarSelectionMode) {
				lvarAdapterContributions.mvarSelectionMode = false;
				lvarAdapterContributions.NotifyDataSetChanged();
				GetRegularToolBox ();
			}
			else {
				lvarAdapterContributions.mvarSelectionMode = true;
				lvarAdapterContributions.NotifyDataSetChanged();
				GetSelectionToolBox ();
			}
		}

		private void GetRegularToolBox()
		{
			LinearLayout lvarLayout = FindViewById<LinearLayout> (Resource.Id.HllToolBox);
			ImageButton lvarRemoveButton = (ImageButton)lvarLayout.GetChildAt (4);
			lvarRemoveButton.SetImageResource (0);
			lvarRemoveButton.LayoutParameters = new LinearLayout.LayoutParams (0, ViewGroup.LayoutParams.MatchParent, 1);
			lvarRemoveButton.SetScaleType (ImageView.ScaleType.CenterInside);
			lvarRemoveButton.SetPadding (0,(int)(12 * Resources.DisplayMetrics.Density),0,(int)(12 * Resources.DisplayMetrics.Density));
		}

		private void GetSelectionToolBox()
		{
			LinearLayout lvarLayout = FindViewById<LinearLayout> (Resource.Id.HllToolBox);
			ImageButton lvarRemoveButton = (ImageButton)lvarLayout.GetChildAt (4);
			lvarRemoveButton.SetImageResource (Resource.Drawable.Remove);
			lvarRemoveButton.LayoutParameters = new LinearLayout.LayoutParams (0, ViewGroup.LayoutParams.MatchParent, 1);
			lvarRemoveButton.SetScaleType (ImageView.ScaleType.CenterInside);
			lvarRemoveButton.SetPadding (0,(int)(12 * Resources.DisplayMetrics.Density),0,(int)(12 * Resources.DisplayMetrics.Density));
		}

		void LvarListViewContributions_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			ListView lvarListView= (ListView)sender;
			HeaderViewListAdapter lvarHeaderAdapter = (HeaderViewListAdapter)lvarListView.Adapter;
			ContributionListViewAdapter lvarAdapter = (ContributionListViewAdapter)lvarHeaderAdapter.WrappedAdapter;
			View lvarView = lvarListView.GetChildAt(e.Position - lvarListView.FirstVisiblePosition);
			RelativeLayout lvarLayout = lvarView.FindViewById<RelativeLayout>(Resource.Id.rlBackground);

			if(lvarAdapter.mvarSelectionMode) {
				ImageView lvarImage = (ImageView)lvarLayout.GetChildAt(0);
				Models.ContributionModel lvarContribution = lvarAdapter.mvarItems[e.Position-1];
				if (lvarAdapter.mvarSelectedItems.Contains (lvarContribution)) {
					lvarAdapter.mvarSelectedItems.Remove (lvarContribution);
					lvarImage.SetImageResource (Resource.Drawable.Icon_unchecked);
				} else {
					lvarAdapter.mvarSelectedItems.Add (lvarContribution);
					lvarImage.SetImageResource (Resource.Drawable.Icon_checked);
				}
			} 
			else {

			}
		}

		void LvarAddContribution_Click (object sender, EventArgs e)
		{
			FragmentTransaction lvarTransaction = FragmentManager.BeginTransaction();
			FragmentAddContribution lvarAddContribution = new FragmentAddContribution(mvarGroupId, this);
			lvarAddContribution.DialogClosed += LvarAddContribution_DialogClosed;
			lvarAddContribution.Show(lvarTransaction,"AddContribution");
		}

		void LvarAddContribution_DialogClosed (object sender, DialogEventContArgs e)
		{
			DBRepository db = new DBRepository ();
			CalcRepository calc = new CalcRepository ();

			Models.ContributionModel lvarContribution = (Models.ContributionModel)e.lvarContribution;
			lvarContribution.mvarId = db.PerformContribution (lvarContribution.mvarGroupId, lvarContribution.mvarMemberId, lvarContribution.mvarName, lvarContribution.mvarAmount, lvarContribution.mvarCurrency, lvarContribution.mvarDate);
			mvarContributions.Add (lvarContribution);
			ExpandableHeightListView lvarListViewContributions = FindViewById<ExpandableHeightListView> (Resource.Id.lvHistory);
			ExpandableHeightListView lvarListViewPayments = FindViewById<ExpandableHeightListView> (Resource.Id.lvPayments);

			HeaderViewListAdapter lvarHeaderAdapterContributions = (HeaderViewListAdapter)lvarListViewContributions.Adapter;
			HeaderViewListAdapter lvarHeaderAdapterPayments = (HeaderViewListAdapter)lvarListViewPayments.Adapter;

			ContributionListViewAdapter lvarContributionsAdapter = (ContributionListViewAdapter)lvarHeaderAdapterContributions.WrappedAdapter;
			PaymentListViewAdapter lvarPaymentsAdapter = (PaymentListViewAdapter)lvarHeaderAdapterPayments.WrappedAdapter;

			mvarMembers = db.GetMembersInGroup (mvarGroupId);
			lvarContributionsAdapter.mvarItems = GetContributions ();
			lvarContributionsAdapter.NotifyDataSetChanged ();

			List<Models.PaymentModel> lvarPayments = GetPayments ();

			lvarPaymentsAdapter.mvarItems = lvarPayments.OrderByDescending(Pay => Pay.mvarSettled).ToList();
			lvarPaymentsAdapter.NotifyDataSetChanged ();
			db.MainScreenDataChanged = true;

			if (mvarContributions.Count == 0) {
				lvarListViewContributions.Visibility = ViewStates.Invisible;
				lvarListViewPayments.Visibility = ViewStates.Invisible;
				FindViewById<TextView> (Resource.Id.txtNothing).Text = "Nothing to see here!\nClick the add button to add a contribution";
			} else {
				FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Gone;
				lvarListViewContributions.Visibility = ViewStates.Visible;
				lvarListViewPayments.Visibility = ViewStates.Visible;
			}	
		}
			
		private LinearLayout GetHeaderView(String Text, GravityFlags Gravity, int PaddingLeft)
		{
			LinearLayout lvarHeader = new LinearLayout (this);
			lvarHeader.LayoutParameters = new LinearLayout.LayoutParams (WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
			//lvarHeader.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#ffffff"));

			TextView lvarText = new TextView (this);
			lvarText.Text = Text;
			lvarText.SetTextSize (Android.Util.ComplexUnitType.Dip, 15);
			lvarText.SetTextColor (Android.Graphics.Color.ParseColor("#000000"));
			lvarText.Gravity = Gravity;
			lvarText.SetPadding ((int)(PaddingLeft * Resources.DisplayMetrics.Density), 0, 0, (int)(5 * Resources.DisplayMetrics.Density));
			lvarHeader.AddView (lvarText);

			return lvarHeader;
		}

		private List<Models.PaymentModel> GetPayments()
		{
			DBRepository db = new DBRepository ();
			CalcRepository calc = new CalcRepository ();

			List<Models.PaymentModel> lvarPayments = calc.GetPaymentsInGroup (mvarGroupId);
			//List<Models.PaymentModel> lvarPayments = db.GetPaymentsInGroup (mvarGroupId);
			//List<Models.PaymentModel> lvarPayments = lvarPayments.OrderBy (Pay => Pay.mvarSettled).ToList ();

			return lvarPayments;
		}



		private List<Models.ContributionModel> GetContributions()
		{
			DBRepository db = new DBRepository ();
			List<Models.ContributionModel> lvarContributions = new List<SplitTheBill.Models.ContributionModel> ();

			foreach (Models.MemberModel lvarMember in mvarMembers)
			{
				lvarContributions.AddRange (lvarMember.mvarContributions);
			}
			lvarContributions = lvarContributions.OrderByDescending (Cont => Cont.mvarDate).ToList();

			return lvarContributions;
		}

		private void PopulateScrollView(List<Models.MemberModel> Members)
		{
			HorizontalScrollView lvarScrollView = FindViewById<HorizontalScrollView> (Resource.Id.HSVMembers);
			lvarScrollView.RemoveAllViews ();
			lvarScrollView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
			LinearLayout lvarChild = new LinearLayout (this);
			lvarChild.Orientation = Orientation.Horizontal;
			lvarChild.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);

			for(int i = 0;i <=  Members.Count - 1; i++)
			{
				Models.MemberModel lvarMember = null;
					lvarMember = Members [i];

				LinearLayout lvarLayout = new LinearLayout (this);
				lvarLayout.Orientation = Orientation.Vertical;
				lvarLayout.LayoutParameters = new LinearLayout.LayoutParams ((int)(75 * Resources.DisplayMetrics.Density),LinearLayout.LayoutParams.WrapContent);
				lvarLayout.SetPadding ((int)(5 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density));

				RoundedImageView lvarImage = new RoundedImageView (this);

					lvarImage.SetImageResource (Resource.Drawable.Member_default_thumbnail);
			
				lvarImage.IsOval = true;
				lvarImage.SetPadding (0, (int)(2 * Resources.DisplayMetrics.Density), 0, (int)(2 * Resources.DisplayMetrics.Density));
				lvarImage.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, (int)(60 * Resources.DisplayMetrics.Density));
				lvarImage.SetScaleType (ImageView.ScaleType.CenterInside);
				lvarLayout.AddView (lvarImage);

				TextView lvarText = new TextView (this);
					lvarText.Text = lvarMember.mvarName + " " + lvarMember.mvarSurname;
				
				lvarText.SetTextColor (Android.Graphics.Color.Black);
				lvarText.Gravity = GravityFlags.Center;
				lvarText.SetTextSize (Android.Util.ComplexUnitType.Dip, 12);
				lvarText.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
				lvarLayout.AddView (lvarText);

				lvarChild.AddView (lvarLayout);
			}
			lvarScrollView.AddView (lvarChild);
		}

		protected override void OnPause ()
		{
			Intent lvarIntent = new Intent("");
			SetResult (Result.Ok, lvarIntent);
			Finish ();
			base.OnPause ();
		}
	}
}

