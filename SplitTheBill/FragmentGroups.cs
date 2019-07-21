
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
	public class FragmentGroups : Android.Support.V4.App.Fragment
	{
		private List<Models.GroupModel> mvarGroups;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override string ToString ()
		{
			return "Groups";
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			base.OnCreateView (inflater, container, savedInstanceState);
			var lvarView = inflater.Inflate(Resource.Layout.FragmentGroupsList, container, false);

			ListView lvarListView = lvarView.FindViewById<ListView> (Resource.Id.lvGroups);
			var db = new DBRepository ();
			mvarGroups = db.GetGroups ();
			mvarGroups = mvarGroups.OrderByDescending (Group => Group.mvarFavourite).ToList();
			lvarListView.Adapter = new GroupListViewAdapter (this.Context, mvarGroups);
			lvarListView.ItemLongClick += LvarListViewGroups_ItemLongClick;
			Button lvarButton = lvarView.FindViewById<Button> (Resource.Id.btnAddGroup);
			lvarButton.Click += AddGroup_Click;
			lvarListView.ItemClick += LvarListViewGroups_ItemClick;

			ImageButton lvarToolBox1 = lvarView.FindViewById<ImageButton> (Resource.Id.btnToolBox1);
			lvarToolBox1.Click += LvarToolBox1_Click;
			ImageButton lvarToolBox2 = lvarView.FindViewById<ImageButton> (Resource.Id.btnToolBox2);
			lvarToolBox2.Click += LvarToolBox2_Click;
			ImageButton lvarToolBox3 = lvarView.FindViewById<ImageButton> (Resource.Id.btnToolBox3);
			lvarToolBox3.Click += LvarToolBox3_Click;
			ImageButton lvarToolBox4 = lvarView.FindViewById<ImageButton> (Resource.Id.btnToolBox4);
			lvarToolBox4.Click += LvarToolBox4_Click;

			if (mvarGroups.Count == 0) {
				lvarListView.Visibility = ViewStates.Gone;
			}
			lvarView.FindViewById<TextView> (Resource.Id.txtNothing).Text = "Nothing to see here!\nClick the add button to add a group";
			lvarView.FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Gone;

			return lvarView;
		}

		void LvarToolBox1_Click (object sender, EventArgs e)
		{

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
			ListView lvarListViewGroups = View.FindViewById<ListView> (Resource.Id.lvGroups);
			GroupListViewAdapter lvarAdapter = (GroupListViewAdapter)lvarListViewGroups.Adapter;
			if (lvarAdapter.mvarSelectionMode) {
				//ACTION FOR REMOVE BUTTON
				db.RemoveGroups(lvarAdapter.mvarSelectedItems);
				foreach (Models.GroupModel lvarGroup in lvarAdapter.mvarSelectedItems) {
					mvarGroups.Remove (lvarGroup);
				}
				lvarAdapter.mvarItems = mvarGroups.OrderByDescending (Group => Group.mvarFavourite).ToList();
				lvarAdapter.mvarSelectedItems.Clear ();
				lvarAdapter.mvarSelectionMode = false;
				lvarAdapter.NotifyDataSetChanged ();
				GetRegularToolBox ();

				if (mvarGroups.Count == 0) {
					lvarListViewGroups.Visibility = ViewStates.Gone;
					View.FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Visible;
				}

			} else {

			}
		}

		void LvarListViewGroups_ItemLongClick (object sender, AdapterView.ItemLongClickEventArgs e)
		{
			ListView lvarListViewGroups = View.FindViewById<ListView> (Resource.Id.lvGroups);
			GroupListViewAdapter lvarAdapterGroups = (GroupListViewAdapter)lvarListViewGroups.Adapter;
			LinearLayout lvarLayout = View.FindViewById<LinearLayout> (Resource.Id.HllToolBox);

			if(lvarAdapterGroups.mvarSelectionMode) {
				lvarAdapterGroups.mvarSelectionMode = false;
				lvarAdapterGroups.NotifyDataSetChanged();
				GetRegularToolBox ();
			}
			else {
				lvarAdapterGroups.mvarSelectionMode = true;
				lvarAdapterGroups.NotifyDataSetChanged();
				GetSelectionToolBox ();
			}
		}

		void LvarListViewGroups_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			ListView lvarListViewGroups= (ListView)sender;
			GroupListViewAdapter lvarAdapterGroups = (GroupListViewAdapter)lvarListViewGroups.Adapter;
			View lvarView = lvarListViewGroups.GetChildAt(e.Position - lvarListViewGroups.FirstVisiblePosition);
			RelativeLayout lvarLayout = lvarView.FindViewById<RelativeLayout>(Resource.Id.rlBackground);

			if(lvarAdapterGroups.mvarSelectionMode) {
				ImageView lvarImage = (ImageView)lvarLayout.GetChildAt(0);
				Models.GroupModel lvarContribution = lvarAdapterGroups.mvarItems[e.Position];
				if (lvarAdapterGroups.mvarSelectedItems.Contains (lvarContribution)) {
					lvarAdapterGroups.mvarSelectedItems.Remove (lvarContribution);
					lvarImage.SetImageResource (Resource.Drawable.Icon_unchecked);
					lvarImage.SetPadding ((int)(10 * Resources.DisplayMetrics.Density), (int)(10 * Resources.DisplayMetrics.Density), 0, (int)(10 * Resources.DisplayMetrics.Density));
				} else {
					lvarAdapterGroups.mvarSelectedItems.Add (lvarAdapterGroups.mvarItems [e.Position]);
					lvarImage.SetImageResource (Resource.Drawable.Icon_checked);
				}
			} 
			else {
				ListView lvarListView = (ListView)sender;
				GroupListViewAdapter lvarAdapter = (GroupListViewAdapter)lvarListView.Adapter;
				Models.GroupModel lvarGroup = lvarAdapter.mvarItems [e.Position];
				Intent lvarIntent = new Intent (this.Activity, typeof(GroupDetails));
				lvarIntent.PutExtra ("GroupId", lvarGroup.mvarId);
				StartActivityForResult (lvarIntent,99);
			}
		}
			
		private void GetRegularToolBox()
		{
			LinearLayout lvarLayout = View.FindViewById<LinearLayout> (Resource.Id.HllToolBox);
			ImageButton lvarRemoveButton = (ImageButton)lvarLayout.GetChildAt (4);
			lvarRemoveButton.SetImageResource (0);
			lvarRemoveButton.LayoutParameters = new LinearLayout.LayoutParams (0, ViewGroup.LayoutParams.MatchParent, 1);
			lvarRemoveButton.SetScaleType (ImageView.ScaleType.CenterInside);
			lvarRemoveButton.SetPadding (0,(int)(12 * Resources.DisplayMetrics.Density),0,(int)(12 * Resources.DisplayMetrics.Density));
		}

		private void GetSelectionToolBox()
		{
			LinearLayout lvarLayout = View.FindViewById<LinearLayout> (Resource.Id.HllToolBox);
			ImageButton lvarRemoveButton = (ImageButton)lvarLayout.GetChildAt (4);
			lvarRemoveButton.SetImageResource (Resource.Drawable.Remove);
			lvarRemoveButton.LayoutParameters = new LinearLayout.LayoutParams (0, ViewGroup.LayoutParams.MatchParent, 1);
			lvarRemoveButton.SetScaleType (ImageView.ScaleType.CenterInside);
			lvarRemoveButton.SetPadding (0,(int)(12 * Resources.DisplayMetrics.Density),0,(int)(12 * Resources.DisplayMetrics.Density));
		}

		void AddGroup_Click (object sender, EventArgs e)
		{
			Intent lvarIntent = new Intent (this.Activity, typeof(AddGroup));
			StartActivityForResult(lvarIntent, 0);
		}

		public override void OnActivityResult (int requestCode, int resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
				DBRepository db = new DBRepository ();

				ListView lvarListView = View.FindViewById<ListView> (Resource.Id.lvGroups);
				GroupListViewAdapter lvarAdapter = (GroupListViewAdapter)lvarListView.Adapter;
				mvarGroups = db.GetGroups ().OrderByDescending(Group => Group.mvarFavourite).ToList();
				lvarAdapter.mvarItems = mvarGroups;
				lvarAdapter.NotifyDataSetChanged ();

			if (mvarGroups.Count != 0) {
				lvarListView.Visibility = ViewStates.Visible;
				View.FindViewById<TextView> (Resource.Id.txtNothing).Visibility = ViewStates.Gone;
			}

		}
	}
}

