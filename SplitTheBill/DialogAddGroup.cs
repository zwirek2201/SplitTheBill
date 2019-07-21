using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace SplitTheBill
{
	class DialogAddGroup:DialogFragment
    {
        private Context mvarContext;
        private ImageButton mvarLastButtonClicked = null;
		private List<Models.MemberModel> mvarMembers;
		private List<Models.MemberModel> mvarSelectedMembers = new List<Models.MemberModel>();
		private MembersListViewAdapter mvarAdapter;

        public DialogAddGroup(Context context)
        {
            mvarContext = context;
        }
			
		//public override void OnStart ()
		//{
		//	base.OnStart ();
		//	//Dialog.Window.SetLayout ((int)(300*Resources.DisplayMetrics.Density), (int)(450*Resources.DisplayMetrics.Density));
		//}
        
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.AddGroup, container, false);
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
			Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialogAddGroup_anim;
            Dialog.FindViewById<EditText>(Resource.Id.etxtName).SetHintTextColor(Android.Graphics.Color.ParseColor("#505050"));
            Dialog.FindViewById<EditText>(Resource.Id.etxtDescription).SetHintTextColor(Android.Graphics.Color.ParseColor("#505050"));

            Dialog.FindViewById<ImageButton>(Resource.Id.color_ff6961).Click += ColorButton_Click;
            Dialog.FindViewById<ImageButton>(Resource.Id.color_77dd77).Click += ColorButton_Click;
            Dialog.FindViewById<ImageButton>(Resource.Id.color_a4bcf8).Click += ColorButton_Click;
            Dialog.FindViewById<ImageButton>(Resource.Id.color_b19cd9).Click += ColorButton_Click;
            Dialog.FindViewById<ImageButton>(Resource.Id.color_cfcfc4).Click += ColorButton_Click;
            Dialog.FindViewById<ImageButton>(Resource.Id.color_ffb347).Click += ColorButton_Click;
            Dialog.FindViewById<ImageButton>(Resource.Id.color_ffd1dc).Click += ColorButton_Click;
            Dialog.FindViewById<ImageButton>(Resource.Id.btnBack).Click += DialogAddGroup_Click;
			Dialog.FindViewById<Button>(Resource.Id.btnAddMember).Click += AddFriend_Click;

			ListView myListView = Dialog.FindViewById<ListView>(Resource.Id.lvMembers);
			DBRepository DB = new DBRepository ();
			mvarMembers = DB.GetMembers ();
			mvarMembers = mvarMembers.OrderBy (Member => Member.mvarName).ToList ();
			mvarAdapter = new MembersListViewAdapter(mvarContext, mvarMembers, mvarSelectedMembers);
			myListView.Adapter = mvarAdapter;
			myListView.ItemClick += MyListView_ItemClick;
			}

        void AddFriend_Click (object sender, EventArgs e)
        {
			Models.MemberModel lvarMember = null;
			FragmentTransaction lvarTransaction = FragmentManager.BeginTransaction();
			DialogAddMember lvarAddMember = new DialogAddMember(mvarContext);
			lvarAddMember.DialogClosed += LvarAddMember_DialogClosed;;
			lvarAddMember.Show(lvarTransaction,"AddMember");
        }

        void LvarAddMember_DialogClosed (object sender, DialogEventArgs e)
        {
			Models.MemberModel lvarMember = e.lvarMember;
			DBRepository DB = new DBRepository ();
			DB.AddMember (lvarMember.mvarName, lvarMember.mvarSurname, lvarMember.mvarPhoneNumbers, lvarMember.mvarEmailAddresses);
			lvarMember.mvarID = DB.GetLastId ();
			ListView lvarListView = Dialog.FindViewById<ListView>(Resource.Id.lvMembers);
			MembersListViewAdapter lvarAdapter = (MembersListViewAdapter)lvarListView.Adapter;
			mvarMembers.Add (lvarMember);
			mvarSelectedMembers.Add (lvarMember);
			mvarMembers = mvarMembers.OrderBy (Member => Member.mvarName).ToList ();
			mvarAdapter.mvarItems = mvarMembers;
			mvarAdapter.mvarSelectedItems = mvarSelectedMembers;
        }

		void MyListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
			ListView lvarList = (ListView)sender;
			RelativeLayout lvarRow = (RelativeLayout)lvarList.GetChildAt(e.Position - lvarList.FirstVisiblePosition);
			ListView lvarListView = Dialog.FindViewById<ListView>(Resource.Id.lvMembers);
			MembersListViewAdapter lvarAdapter = (MembersListViewAdapter)lvarListView.Adapter;
			if(mvarSelectedMembers.Contains(mvarMembers[e.Position]) == true)
			{
				ImageView lvarButton = lvarRow.FindViewById<ImageView> (Resource.Id.ivSelect);
				LinearLayout lvarBackground = lvarRow.FindViewById<LinearLayout> (Resource.Id.VllBackground);
				lvarButton.SetImageResource (Resource.Drawable.lv_uncheck);
				lvarBackground.SetBackgroundColor (Color.ParseColor ("#ffffff"));
				mvarSelectedMembers.Remove (mvarMembers [e.Position]);
				mvarAdapter.mvarSelectedItems = mvarSelectedMembers;
			} 
			else 
			{
				ImageView lvarButton = lvarRow.FindViewById<ImageView> (Resource.Id.ivSelect);
				LinearLayout lvarBackground = lvarRow.FindViewById<LinearLayout> (Resource.Id.VllBackground);
				lvarButton.SetImageResource (Resource.Drawable.lv_check);
				lvarBackground.SetBackgroundColor (Color.ParseColor ("#30000000"));
				mvarSelectedMembers.Add (mvarMembers [e.Position]);
				mvarAdapter.mvarSelectedItems = mvarSelectedMembers;
			}
        }

        private void DialogAddGroup_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
			ImageButton lvarButton = (ImageButton)sender;
			RelativeLayout lvarBackground = Dialog.FindViewById<RelativeLayout> (Resource.Id.RlBarBackground);
            switch(lvarButton.Id)
            {
                case Resource.Id.color_77dd77:
                    lvarBackground.SetBackgroundColor(Color.ParseColor("#77dd77"));
                    break;
                case Resource.Id.color_a4bcf8:
                    lvarBackground.SetBackgroundColor(Color.ParseColor("#a4bcf8"));
                    break;
                case Resource.Id.color_b19cd9:
                    lvarBackground.SetBackgroundColor(Color.ParseColor("#b19cd9"));
                    break;
                case Resource.Id.color_cfcfc4:
                    lvarBackground.SetBackgroundColor(Color.ParseColor("#cfcfc4"));
                    break;
                case Resource.Id.color_ff6961:
                    lvarBackground.SetBackgroundColor(Color.ParseColor("#ff6961"));
                    break;
                case Resource.Id.color_ffb347:
                    lvarBackground.SetBackgroundColor(Color.ParseColor("#ffb347"));
                    break;
                case Resource.Id.color_ffd1dc:
                    lvarBackground.SetBackgroundColor(Color.ParseColor("#ffd1dc"));
                    break;
            }
            
			if(mvarLastButtonClicked != null)
            {
                mvarLastButtonClicked.SetPadding(0, (int)(10 * Resources.DisplayMetrics.Density), 0, (int)(10 * Resources.DisplayMetrics.Density));
				mvarLastButtonClicked = lvarButton;
            }
            else
            {
                mvarLastButtonClicked = lvarButton;
            }
            lvarButton.SetPadding(0, (int)(8 * Resources.DisplayMetrics.Density), 0, (int)(8 * Resources.DisplayMetrics.Density));
        }
			
    }
}