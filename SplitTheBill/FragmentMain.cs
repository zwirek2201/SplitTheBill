
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

namespace SplitTheBill
{
	public class FragmentMain : Android.Support.V4.App.Fragment
	{
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override string ToString ()
		{
			return "Me";
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			base.OnCreateView (inflater, container, savedInstanceState);
			var lvarView = inflater.Inflate(Resource.Layout.FragmentMain, container, false);

			DBRepository db = new DBRepository ();
			Models.MemberModel lvarMember = db.GetMember ("0");
			TextView lvarOwes = lvarView.FindViewById<TextView> (Resource.Id.txtOwes);
			lvarOwes.Text = Math.Round (lvarMember.GetTotalOwesSum (), 2) + " " + db.ExchangeRateBase;
			TextView lvarOwed = lvarView.FindViewById<TextView> (Resource.Id.txtOwed);
			lvarOwed.Text = Math.Round (lvarMember.GetTotalOwedSum (), 2) + " " + db.ExchangeRateBase;
			TextView lvarBalance = lvarView.FindViewById<TextView> (Resource.Id.txtBalance);
			Decimal lvarBalanceAmount = lvarMember.GetTotalOwedSum () - lvarMember.GetTotalOwesSum ();
			if (lvarBalanceAmount > 0) {
				lvarBalance.SetTextColor (Android.Graphics.Color.Green);
			} else if (lvarBalanceAmount < 0) {
				lvarBalance.SetTextColor (Android.Graphics.Color.Red);
			} else {
				lvarBalance.SetTextColor (Android.Graphics.Color.Black);
			}
			lvarBalance.Text = Math.Round (lvarBalanceAmount, 2) + " " + db.ExchangeRateBase;

			ExpandableHeightListView lvarListViewPayments = lvarView.FindViewById<ExpandableHeightListView> (Resource.Id.lvPayments);
			List<Models.PaymentModel> lvarPayments = lvarMember.mvarPayments.FindAll(Pay => Pay.mvarSettled == false).ToList().OrderBy (Pay => Pay.mvarSettled).ToList();
			List<Models.MemberModel> lvarMembers = db.GetMembers ();
			PaymentListViewAdapter lvarAdapterPayments = new PaymentListViewAdapter (this.Context, lvarPayments, lvarMembers, "");
			lvarListViewPayments.Adapter = lvarAdapterPayments;
			lvarListViewPayments.setExpanded (true);

			lvarListViewPayments.AddHeaderView (GetHeaderView("Your payments", GravityFlags.Left,5));

			ExpandableHeightListView lvarListViewGroups = lvarView.FindViewById<ExpandableHeightListView> (Resource.Id.lvGroups);
			List<Models.GroupModel> lvarGroups = db.GetGroups ();
			lvarGroups = lvarGroups.FindAll (Group => Group.mvarFavourite == true).ToList ();
			GroupListViewAdapter lvarAdapterGroups = new GroupListViewAdapter (this.Context, lvarGroups);
			lvarListViewGroups.Adapter = lvarAdapterGroups;
			lvarListViewGroups.setExpanded (true);
			
			lvarListViewGroups.AddHeaderView (GetHeaderView("Favourite groups",GravityFlags.Left,5));

			if (lvarGroups.Count == 0 && lvarPayments.Count == 0) {
				lvarListViewGroups.Visibility = ViewStates.Gone;
				lvarListViewPayments.Visibility = ViewStates.Gone;
				lvarView.FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Visible;
			} else {
				lvarView.FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Gone;
				lvarListViewGroups.Visibility = ViewStates.Visible;
				lvarListViewPayments.Visibility = ViewStates.Visible;
			}
			lvarView.FindViewById<TextView> (Resource.Id.txtNothing).Text = "Nothing to see here!\nFlag a group as your favourite or add a contribution";

			lvarListViewGroups.ItemClick += LvarListViewGroups_ItemClick;
			return lvarView;
		}

		void LvarListViewGroups_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			ListView lvarListView = (ListView)sender;
			HeaderViewListAdapter lvarAdapterHeader = (HeaderViewListAdapter)lvarListView.Adapter;
			GroupListViewAdapter lvarAdapter = (GroupListViewAdapter)lvarAdapterHeader.WrappedAdapter;
			Models.GroupModel lvarGroup = lvarAdapter.mvarItems [e.Position-1];
			Intent lvarIntent = new Intent (this.Activity, typeof(GroupDetails));
			lvarIntent.PutExtra ("GroupId", lvarGroup.mvarId);
			StartActivityForResult (lvarIntent,99);
		}

		public override void OnActivityResult (int requestCode, int resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			/*
			DBRepository db = new DBRepository ();

			ListView lvarListView = View.FindViewById<ListView> (Resource.Id.lvGroups);
			HeaderViewListAdapter lvarAdapterHeader = (HeaderViewListAdapter)lvarListView.Adapter;
			GroupListViewAdapter lvarAdapter = (GroupListViewAdapter)lvarAdapterHeader.WrappedAdapter;
			List<Models.GroupModel> lvarGroups = db.GetGroups ().FindAll (Group => Group.mvarFavourite == true).ToList ();

			lvarAdapter.mvarItems = lvarGroups;
			lvarAdapter.NotifyDataSetChanged ();

			if (lvarGroups.Count != 0) {
				lvarListView.Visibility = ViewStates.Visible;
				View.FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Gone;
			}
			*/
			Refresh ();
		}

		private LinearLayout GetHeaderView(String Text, GravityFlags Gravity, int PaddingLeft)
		{
			LinearLayout lvarHeader = new LinearLayout (this.Context);
			lvarHeader.LayoutParameters = new LinearLayout.LayoutParams (WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
			//lvarHeader.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#ffffff"));

			TextView lvarText = new TextView (this.Context);
			lvarText.Text = Text;
			lvarText.SetTextSize (Android.Util.ComplexUnitType.Dip, 15);
			lvarText.SetTextColor (Android.Graphics.Color.ParseColor("#000000"));
			lvarText.Gravity = Gravity;
			lvarText.SetPadding ((int)(PaddingLeft * Resources.DisplayMetrics.Density), 0, 0, (int)(5 * Resources.DisplayMetrics.Density));
			lvarHeader.AddView (lvarText);

			return lvarHeader;
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);
			Refresh ();
		}

		void LvarButton_Click (object sender, EventArgs e)
		{
			Intent lvarIntent = new Intent (this.Activity, typeof(AddGroup));
			StartActivityForResult(lvarIntent, 0);
		}
			
		public void Refresh()
		{
				
			DBRepository db = new DBRepository ();
			if (db.MainScreenDataChanged) {
				Models.MemberModel lvarMember = db.GetMember ("0");

				TextView lvarOwes = View.FindViewById<TextView> (Resource.Id.txtOwes);
				lvarOwes.Text = Math.Round (lvarMember.GetTotalOwesSum (), 2) + " " + db.ExchangeRateBase;
				TextView lvarOwed = View.FindViewById<TextView> (Resource.Id.txtOwed);
				lvarOwed.Text = Math.Round (lvarMember.GetTotalOwedSum (), 2) + " " + db.ExchangeRateBase;
				TextView lvarBalance = View.FindViewById<TextView> (Resource.Id.txtBalance);
				Decimal lvarBalanceAmount = lvarMember.GetTotalOwedSum () - lvarMember.GetTotalOwesSum ();
				if (lvarBalanceAmount > 0) {
					lvarBalance.SetTextColor (Android.Graphics.Color.Green);
				} else if (lvarBalanceAmount < 0) {
					lvarBalance.SetTextColor (Android.Graphics.Color.Red);
				} else {
					lvarBalance.SetTextColor (Android.Graphics.Color.Black);
				}
				lvarBalance.Text = Math.Round (lvarBalanceAmount, 2) + " " + db.ExchangeRateBase;


				ExpandableHeightListView lvarListViewPayments = View.FindViewById<ExpandableHeightListView> (Resource.Id.lvPayments);
				List<Models.PaymentModel> lvarPayments = lvarMember.mvarPayments.FindAll (Pay => Pay.mvarSettled == false).ToList ();
				List<Models.MemberModel> lvarMembers = db.GetMembers ();
				HeaderViewListAdapter lvarAdapter1 = (HeaderViewListAdapter)lvarListViewPayments.Adapter;
				PaymentListViewAdapter lvarAdapterPayments = (PaymentListViewAdapter)lvarAdapter1.WrappedAdapter;
				lvarAdapterPayments.mvarItems = lvarPayments;
				lvarAdapterPayments.mvarMembers = lvarMembers;
				lvarAdapterPayments.NotifyDataSetChanged ();
				lvarListViewPayments.setExpanded (true);

				ExpandableHeightListView lvarListViewGroups = View.FindViewById<ExpandableHeightListView> (Resource.Id.lvGroups);
				List<Models.GroupModel> lvarGroups = db.GetGroups ();
				lvarGroups = lvarGroups.FindAll (Group => Group.mvarFavourite == true).ToList ();
				HeaderViewListAdapter lvarAdapter2 = (HeaderViewListAdapter)lvarListViewGroups.Adapter;
				GroupListViewAdapter lvarAdapterGroups = (GroupListViewAdapter)lvarAdapter2.WrappedAdapter;
				lvarAdapterGroups.mvarItems = lvarGroups;
				lvarListViewGroups.setExpanded (true);
				lvarAdapterGroups.NotifyDataSetChanged ();
				db.MainScreenDataChanged = false;

				if (lvarGroups.Count == 0 && lvarPayments.Count == 0) {
					lvarListViewGroups.Visibility = ViewStates.Gone;
					lvarListViewPayments.Visibility = ViewStates.Gone;
					View.FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Visible;
				} else {
					View.FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Gone;
					lvarListViewGroups.Visibility = ViewStates.Visible;
					lvarListViewPayments.Visibility = ViewStates.Visible;
				}
			}
		}
	}
}

