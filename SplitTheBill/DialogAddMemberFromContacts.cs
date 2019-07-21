using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.App;
using Android.Runtime;
using Android.Provider;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Xamarin.Contacts;
using System.ComponentModel;


namespace SplitTheBill
{
	public class DialogAddMemberFromContacts:DialogFragment
	{
		private Context mvarContext;
		public List<Contact> mvarContacts;
		public event EventHandler<DialogContactEventArgs> DialogClosed;

		public DialogAddMemberFromContacts (Context Context, List<Contact> Contacts)
		{
			mvarContext = Context;
			mvarContacts = Contacts;
		}

		public override void OnStart ()
		{
			base.OnStart ();
			Dialog.Window.SetLayout (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);

			var view = inflater.Inflate(Resource.Layout.AddMemberFromContacts, container, false);
			return view;
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle);
			base.OnActivityCreated (savedInstanceState);
			//Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialogAddMember_anim;

			EditText lvarSearchBar = Dialog.FindViewById<EditText> (Resource.Id.etxtSearch);
			lvarSearchBar.Visibility = ViewStates.Invisible;
			ListView lvarList = Dialog.FindViewById<ListView> (Resource.Id.lvContacts);
			ContactListViewAdapter lvarAdapter = new ContactListViewAdapter (mvarContext, mvarContacts);
			lvarList.Adapter = lvarAdapter;
			lvarList.ItemClick += LvarList_ItemClick;
			ImageButton lvarSearch = Dialog.FindViewById<ImageButton> (Resource.Id.ibtnSearchButton);
			lvarSearch.Click += LvarSearch_Click;
			lvarSearchBar.TextChanged += LvarSearchBar_TextChanged;

		}

		void LvarSearchBar_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			EditText lvarEdit = (EditText)sender;
			ListView lvarListView = Dialog.FindViewById<ListView> (Resource.Id.lvContacts);
			ContactListViewAdapter lvarAdapter = (ContactListViewAdapter)lvarListView.Adapter;
			lvarAdapter.mvarItems = mvarContacts.FindAll(lvarContact => lvarContact.DisplayName.ToLower().Contains(lvarEdit.Text.ToString().ToLower())).ToList();
			lvarListView.Adapter = lvarAdapter;
			//OGARNĄĆ WIELKOŚĆ ZNAKÓW
		}

		void LvarSearch_Click (object sender, EventArgs e)
		{
			EditText lvarSearchBar = Dialog.FindViewById<EditText> (Resource.Id.etxtSearch);
			if (lvarSearchBar.Visibility == ViewStates.Visible) {
				lvarSearchBar.Visibility = ViewStates.Invisible;
			}
			else{
				lvarSearchBar.Visibility = ViewStates.Visible;
				lvarSearchBar.RequestFocus ();
			}
		}

		void LvarList_ItemClick (object sender, AdapterView.ItemClickEventArgs e)
		{
			ListView lvarListView = Dialog.FindViewById<ListView> (Resource.Id.lvContacts);
			ContactListViewAdapter lvarAdapter = (ContactListViewAdapter)lvarListView.Adapter;
			Contact lvarContact = lvarAdapter.mvarItems [e.Position];
			DialogClosed (this, new DialogContactEventArgs{mvarContact = lvarContact});
			Dialog.Dismiss ();
		}
	}

	public class DialogContactEventArgs : EventArgs
	{
		public Contact mvarContact { get; set; }
	}

	public delegate void DialogContactEventHandler(object sender, DialogContactEventArgs args);
}

