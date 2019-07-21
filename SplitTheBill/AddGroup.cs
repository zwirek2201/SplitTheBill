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
using Newtonsoft.Json.Converters;
namespace SplitTheBill
{
	[Activity (Label = "AddGroup")]			
	public class AddGroup : Activity
	{
		private ImageButton mvarLastButtonClicked = null;
		private List<Models.MemberModel> mvarMembers;
		private List<Models.MemberModel> mvarSelectedMembers = new List<Models.MemberModel>();
		private MembersListViewAdapter mvarAdapter;
		private Boolean mvarFavourited = false;

		public Models.GroupModel mvarGroup;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature(WindowFeatures.NoTitle);
			Window.Attributes.WindowAnimations = Resource.Style.UpInDownOut_anim;
			base.OnCreate (savedInstanceState);
			SetContentView(Resource.Layout.AddGroup);

			// Create your application here
			FindViewById<EditText>(Resource.Id.etxtName).SetHintTextColor(Android.Graphics.Color.ParseColor("#505050"));
			FindViewById<EditText>(Resource.Id.etxtDescription).SetHintTextColor(Android.Graphics.Color.ParseColor("#505050"));

			FindViewById<Button>(Resource.Id.btnAddMember).Click += AddFriend_Click;
			FindViewById<ImageView>(Resource.Id.ivFavourite).Click += Favourite_Click;
			FindViewById<Button> (Resource.Id.btnDoneButton).Click += DialogAddGroup_Click;
			FindViewById<ImageButton>(Resource.Id.btnBackButton).Click += Back_Click;

			ListView myListView = FindViewById<ListView>(Resource.Id.lvMembers);
			DBRepository DB = new DBRepository ();
			mvarMembers = DB.GetMembers ();
			mvarAdapter = new MembersListViewAdapter(this, mvarMembers, mvarSelectedMembers);
			myListView.Adapter = mvarAdapter;
			myListView.ItemClick += MyListView_ItemClick;
		}

		void Back_Click (object sender, EventArgs e)
		{			
			Intent lvarIntent = new Intent("");
			lvarIntent.PutExtra ("Group", "");
			SetResult (Result.Ok, lvarIntent);
			Finish ();
		}

		void Favourite_Click (object sender, EventArgs e)
		{
			ImageView lvarFavourited = FindViewById<ImageView> (Resource.Id.ivFavourite);
			if (mvarFavourited) {
				lvarFavourited.SetImageResource (Resource.Drawable.Favourite_off);
				mvarFavourited = false;
			} else {
				lvarFavourited.SetImageResource (Resource.Drawable.Favourite_on);
				mvarFavourited = true;
			}
		}

		void AddFriend_Click (object sender, EventArgs e)
		{
			FragmentTransaction lvarTransaction = FragmentManager.BeginTransaction();
			DialogAddMember lvarAddMember = new DialogAddMember(this);
			lvarAddMember.DialogClosed += LvarAddMember_DialogClosed;;
			lvarAddMember.Show(lvarTransaction,"AddMember");
		}
			
		void LvarAddMember_DialogClosed (object sender, DialogEventArgs e)
		{
			Models.MemberModel lvarMember = e.lvarMember;
			DBRepository DB = new DBRepository ();
			String lvarMemberId = DB.AddMember (lvarMember.mvarName, lvarMember.mvarSurname, lvarMember.mvarPhoneNumbers, lvarMember.mvarEmailAddresses);
			lvarMember.mvarID = lvarMemberId;
			mvarMembers.Add (lvarMember);
			mvarSelectedMembers.Add (lvarMember);
			ListView lvarListView = FindViewById<ListView> (Resource.Id.lvMembers);
			MembersListViewAdapter lvarAdapter = (MembersListViewAdapter)lvarListView.Adapter;
			lvarAdapter.NotifyDataSetChanged ();
		}

		void MyListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			ListView lvarList = (ListView)sender;
			RelativeLayout lvarRow = (RelativeLayout)lvarList.GetChildAt(e.Position - lvarList.FirstVisiblePosition);
			ListView lvarListView = FindViewById<ListView>(Resource.Id.lvMembers);
			MembersListViewAdapter lvarAdapter = (MembersListViewAdapter)lvarListView.Adapter;
			if(mvarSelectedMembers.Contains(mvarMembers[e.Position]) == true)
			{
				ImageView lvarButton = lvarRow.FindViewById<ImageView> (Resource.Id.ivSelect);
				LinearLayout lvarBackground = lvarRow.FindViewById<LinearLayout> (Resource.Id.VllBackground);
				lvarButton.SetImageResource (Resource.Drawable.Icon_unchecked);
				lvarBackground.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#ffffff"));
				mvarSelectedMembers.Remove (mvarMembers [e.Position]);
				mvarAdapter.mvarSelectedItems = mvarSelectedMembers;
			} 
			else 
			{
				ImageView lvarButton = lvarRow.FindViewById<ImageView> (Resource.Id.ivSelect);
				LinearLayout lvarBackground = lvarRow.FindViewById<LinearLayout> (Resource.Id.VllBackground);
				lvarButton.SetImageResource (Resource.Drawable.Icon_checked);
				lvarBackground.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#30000000"));
				mvarSelectedMembers.Add (mvarMembers [e.Position]);
				mvarAdapter.mvarSelectedItems = mvarSelectedMembers;
			}
		}

		private void DialogAddGroup_Click(object sender, EventArgs e)
		{
			EditText lvarName = FindViewById<EditText> (Resource.Id.etxtName);
			DBRepository db = new DBRepository ();
			String lvarGroupId = db.AddGroup (lvarName.Text, null, mvarSelectedMembers, mvarFavourited);
			Intent lvarIntent = new Intent("");
			lvarIntent.PutExtra ("Group", lvarGroupId);
			SetResult (Result.Ok, lvarIntent);
			Finish();
		}
	}
}

