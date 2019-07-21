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
using System.ComponentModel;
using Xamarin.Contacts;
using System.Threading;


namespace SplitTheBill
{
	public class DialogAddMember:DialogFragment
	{
		public Models.MemberModel mvarMember;
		private Context mvarContext;
		public event EventHandler<DialogEventArgs> DialogClosed;

		public DialogAddMember (Context context)
		{
			mvarContext = context;
		}
		public override void OnStart ()
		{
			base.OnStart ();
			Dialog.Window.SetLayout (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
		}
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);	
			var view = inflater.Inflate (Resource.Layout.AddMember, container, false);

			return view;
		}
		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.RightInRightOut_anim;
			base.OnActivityCreated (savedInstanceState);

			Dialog.FindViewById<EditText>(Resource.Id.etxtName).SetHintTextColor(Android.Graphics.Color.ParseColor("#505050"));
			Dialog.FindViewById<EditText>(Resource.Id.etxtSurname).SetHintTextColor(Android.Graphics.Color.ParseColor("#505050"));
			Button lvarDoneButton = Dialog.FindViewById<Button> (Resource.Id.btnDoneButton);
			lvarDoneButton.Click += LvarDoneButton_Click;
			ImageButton lvarBackButton = Dialog.FindViewById<ImageButton> (Resource.Id.btnBackButton);
			lvarBackButton.Click += LvarBackButton_Click;
			Button lvarAddFromContactsButton = Dialog.FindViewById<Button> (Resource.Id.btnContacts);
			lvarAddFromContactsButton.Click += LvarAddFromContactsButton_Click;
			TextView lvarAddPhone = Dialog.FindViewById<TextView> (Resource.Id.txtAddPhone);
			lvarAddPhone.Click += (object sender, EventArgs e) => { AddPhone("");};
			TextView lvarAddEmail = Dialog.FindViewById<TextView> (Resource.Id.txtAddEmail);
			lvarAddEmail.Click += (object sender, EventArgs e) => { AddEmail("");};
			AddPhone ("");
			AddEmail ("");
		}

		void lvardeletePhone_Click (object sender,EventArgs e, int position)
		{
			Toast.MakeText (mvarContext, position.ToString (), ToastLength.Short);
			LinearLayout lvarPhones = Dialog.FindViewById<LinearLayout> (Resource.Id.VllPhones);
			lvarPhones.RemoveViewAt (position);
			if (lvarPhones.ChildCount == 2) {
				LinearLayout lvarChild = (LinearLayout)lvarPhones.GetChildAt (0);
				lvarChild.RemoveViewAt (1);
				lvarChild.WeightSum = 85;
			}
		}
		void lvardeleteEmail_Click (object sender,EventArgs e, int position)
		{
			LinearLayout lvarEmails = Dialog.FindViewById<LinearLayout> (Resource.Id.VllEmails);
			lvarEmails.RemoveViewAt (position);
			if (lvarEmails.ChildCount == 2) {
				LinearLayout lvarChild = (LinearLayout)lvarEmails.GetChildAt (0);
				lvarChild.RemoveViewAt (1);
				lvarChild.WeightSum = 85;
			}
		}

		void AddPhone(String Number)
		{
			LinearLayout lvarPhones = Dialog.FindViewById<LinearLayout> (Resource.Id.VllPhones);
			if (lvarPhones.ChildCount == 2) {
				//ADD DELETE BUTTON IN FIRST CHILD ELEMENT
				LinearLayout lvarFirstChild = (LinearLayout)lvarPhones.GetChildAt (0);
				lvarFirstChild.WeightSum = 100;
				ImageButton lvarButton = new ImageButton (mvarContext);
				lvarButton.LayoutParameters = new LinearLayout.LayoutParams (0, (int)(35 * Resources.DisplayMetrics.Density), 15);
				lvarButton.SetBackgroundColor(Color.ParseColor("#70ffffff"));
				lvarButton.SetScaleType (ImageView.ScaleType.FitCenter);
				lvarButton.SetPadding (0, (int)(10 * Resources.DisplayMetrics.Density),0, (int)(10 * Resources.DisplayMetrics.Density));
				lvarButton.SetImageResource (Resource.Drawable.close);
				lvarButton.Click += (object sender2, EventArgs e2) => {lvardeletePhone_Click(sender2,e2,0);};
				lvarFirstChild.AddView (lvarButton,1);
			}
			//ADD NEW CHILD WITH DELETE BUTTON
			//Layout
			LinearLayout lvarChildLayout2 = new LinearLayout(mvarContext);
			lvarChildLayout2.Orientation = Orientation.Horizontal;
			if (lvarPhones.ChildCount == 1) {
				lvarChildLayout2.WeightSum = 85;
			} else {
				lvarChildLayout2.WeightSum = 100;
			}
			LinearLayout.LayoutParams lvarLayoutParameters2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent,(int)(35*Resources.DisplayMetrics.Density));
			lvarLayoutParameters2.SetMargins ((int)(20 * Resources.DisplayMetrics.Density), 0, (int)(20 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density));
			//EditText
			EditText lvarPhone2 = new EditText (mvarContext);
			lvarPhone2.SetHintTextColor(Android.Graphics.Color.ParseColor("#505050"));
			lvarPhone2.Gravity = GravityFlags.Left;
			lvarPhone2.SetTextSize (Android.Util.ComplexUnitType.Dip, 20);
			lvarPhone2.SetRawInputType (Android.Text.InputTypes.ClassPhone);
			lvarPhone2.SetPadding ((int)(10*Resources.DisplayMetrics.Density),(int)(5*Resources.DisplayMetrics.Density),0,0);
			lvarPhone2.SetBackgroundColor(Android.Graphics.Color.ParseColor("#70ffffff"));
			lvarPhone2.Hint = "Phone number";
			lvarPhone2.Text = Number;
			lvarPhone2.SetTextColor(new Android.Graphics.Color(Color.ParseColor("#000000")));
			lvarPhone2.LayoutParameters = new LinearLayout.LayoutParams(0,(int)(35*Resources.DisplayMetrics.Density),85);
			lvarChildLayout2.AddView (lvarPhone2,0);
			//Button
			if (lvarPhones.ChildCount > 1) {
				ImageButton lvarButton2 = new ImageButton (mvarContext);
				lvarButton2.LayoutParameters = new LinearLayout.LayoutParams (0, (int)(35 * Resources.DisplayMetrics.Density), 15);
				lvarButton2.SetBackgroundColor (Color.ParseColor ("#70ffffff"));
				lvarButton2.SetScaleType (ImageView.ScaleType.FitCenter);
				lvarButton2.SetPadding (0, (int)(10 * Resources.DisplayMetrics.Density), 0, (int)(10 * Resources.DisplayMetrics.Density));
				lvarButton2.SetImageResource (Resource.Drawable.close);
				lvarButton2.Click += (object sender2, EventArgs e2) => {
					lvardeletePhone_Click (sender2, e2, lvarPhones.ChildCount - 2);
				};
				lvarChildLayout2.AddView (lvarButton2, 1);
			}
			//AddView
			lvarPhones.AddView(lvarChildLayout2,lvarPhones.ChildCount-1,(ViewGroup.LayoutParams)lvarLayoutParameters2);
		}

		void AddEmail (String Address)
		{
			LinearLayout lvarEmails = Dialog.FindViewById<LinearLayout> (Resource.Id.VllEmails);
			if (lvarEmails.ChildCount == 2) {
				//ADD DELETE BUTTON IN FIRST CHILD ELEMENT
				LinearLayout lvarFirstChild = (LinearLayout)lvarEmails.GetChildAt (0);
				lvarFirstChild.WeightSum = 100;
				ImageButton lvarButton = new ImageButton (mvarContext);
				lvarButton.LayoutParameters = new LinearLayout.LayoutParams (0, (int)(35 * Resources.DisplayMetrics.Density), 15);
				lvarButton.SetBackgroundColor(Color.ParseColor("#70ffffff"));
				lvarButton.SetScaleType (ImageView.ScaleType.FitCenter);
				lvarButton.SetPadding (0, (int)(10 * Resources.DisplayMetrics.Density),0, (int)(10 * Resources.DisplayMetrics.Density));
				lvarButton.SetImageResource (Resource.Drawable.close);
				lvarButton.Click += (object sender2, EventArgs e2) => {lvardeleteEmail_Click(sender2,e2,0);};
				lvarFirstChild.AddView (lvarButton,1);
			}
			//ADD NEW CHILD WITH DELETE BUTTON
			//Layout
			LinearLayout lvarChildLayout2 = new LinearLayout(mvarContext);
			lvarChildLayout2.Orientation = Orientation.Horizontal;
			if (lvarEmails.ChildCount == 1) {
				lvarChildLayout2.WeightSum = 85;
			} else {
				lvarChildLayout2.WeightSum = 100;
			}
			LinearLayout.LayoutParams lvarLayoutParameters2 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent,(int)(35*Resources.DisplayMetrics.Density));
			lvarLayoutParameters2.SetMargins ((int)(20 * Resources.DisplayMetrics.Density), 0, (int)(20 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density));
			//EditText
			EditText lvarEmail2 = new EditText (mvarContext);
			lvarEmail2.SetHintTextColor(Android.Graphics.Color.ParseColor("#505050"));
			lvarEmail2.Gravity = GravityFlags.Left;
			lvarEmail2.SetTextSize (Android.Util.ComplexUnitType.Dip, 20);
			lvarEmail2.SetRawInputType (Android.Text.InputTypes.TextVariationEmailAddress);
			lvarEmail2.SetPadding ((int)(10*Resources.DisplayMetrics.Density),(int)(5*Resources.DisplayMetrics.Density),0,0);
			lvarEmail2.SetBackgroundColor(Android.Graphics.Color.ParseColor("#70ffffff"));
			lvarEmail2.Hint = "Email address";
			lvarEmail2.Text = Address;
			lvarEmail2.SetTextColor(new Android.Graphics.Color(Color.ParseColor("#000000")));
			lvarEmail2.LayoutParameters = new LinearLayout.LayoutParams(0,(int)(35*Resources.DisplayMetrics.Density),85);
			lvarChildLayout2.AddView (lvarEmail2,0);
			//Button
			if (lvarEmails.ChildCount > 1) {
				ImageButton lvarButton2 = new ImageButton (mvarContext);
				lvarButton2.LayoutParameters = new LinearLayout.LayoutParams (0, (int)(35 * Resources.DisplayMetrics.Density), 15);
				lvarButton2.SetBackgroundColor (Color.ParseColor ("#70ffffff"));
				lvarButton2.SetScaleType (ImageView.ScaleType.FitCenter);
				lvarButton2.SetPadding (0, (int)(10 * Resources.DisplayMetrics.Density), 0, (int)(10 * Resources.DisplayMetrics.Density));
				lvarButton2.SetImageResource (Resource.Drawable.close);
				lvarButton2.Click += (object sender2, EventArgs e2) => {
					lvardeleteEmail_Click (sender2, e2, lvarEmails.ChildCount - 2);
				};
				lvarChildLayout2.AddView (lvarButton2, 1);
			}
			//AddView
			lvarEmails.AddView(lvarChildLayout2,lvarEmails.ChildCount-1,(ViewGroup.LayoutParams)lvarLayoutParameters2);
		}

		void LvarBackButton_Click (object sender, EventArgs e)
		{
			Dialog.Dismiss ();
		}

		void LvarAddFromContactsButton_Click (object sender, EventArgs e)
		{
			List<Contact> mvarContacts = new List<Contact>();
			ProgressDialog lvarProgress = ProgressDialog.Show (mvarContext, "Please wait...", "Loading contacts");

			new Thread (new ThreadStart (delegate {
				var book = new Xamarin.Contacts.AddressBook (mvarContext);
				foreach (Xamarin.Contacts.Contact contact in book.ToList().OrderBy(lvarItem => lvarItem.FirstName)) {
					if ((contact.FirstName != null || contact.LastName != null) && (contact.Emails.Count () > 0 || contact.Phones.Count () > 0))
						mvarContacts.Add (contact);
				}
				FragmentTransaction lvarTransaction = FragmentManager.BeginTransaction();
				DialogAddMemberFromContacts lvarAddMember = new DialogAddMemberFromContacts(mvarContext, mvarContacts);
				lvarAddMember.DialogClosed += LvarAddMember_DialogClosed;
				lvarAddMember.Show(lvarTransaction,"AddMemberFromContacts");
				this.Activity.RunOnUiThread(() => lvarProgress.Hide());
			})).Start ();
		}

		void LvarDoneButton_Click (object sender, EventArgs e)
		{
			EditText lvarName = Dialog.FindViewById<EditText> (Resource.Id.etxtName);
			EditText lvarSurname = Dialog.FindViewById<EditText> (Resource.Id.etxtSurname);

			if (String.IsNullOrWhiteSpace (lvarName.Text) == true && String.IsNullOrWhiteSpace (lvarSurname.Text) == true) {
				Toast.MakeText (mvarContext, "Fill in name or surname", ToastLength.Short).Show ();
			} else {
				List<String> lvarPhones = new List<String> ();
				List<String> lvarEmails = new List<String> ();
				LinearLayout lvarPhonesLayout = Dialog.FindViewById<LinearLayout> (Resource.Id.VllPhones);
				for (int i = 0; i < lvarPhonesLayout.ChildCount - 1; i++) {
					LinearLayout lvarPhone = (LinearLayout)lvarPhonesLayout.GetChildAt (i);
					EditText lvarPhoneText = (EditText)lvarPhone.GetChildAt (0);
					if (lvarPhoneText.Text != "") {
						lvarPhones.Add (lvarPhoneText.Text);
					}
				}
				LinearLayout lvarEmailsLayout = Dialog.FindViewById<LinearLayout> (Resource.Id.VllEmails);
				for (int i = 0; i < lvarEmailsLayout.ChildCount - 1; i++) {
					LinearLayout lvarEmail = (LinearLayout)lvarEmailsLayout.GetChildAt (i);
					EditText lvarEmailText = (EditText)lvarEmail.GetChildAt (0);
					if (lvarEmailText.Text != "") {
						lvarEmails.Add (lvarEmailText.Text);
					}
				}
				mvarMember = new Models.MemberModel ("", lvarName.Text, lvarSurname.Text, lvarPhones, lvarEmails);
				DialogClosed (this, new DialogEventArgs{ lvarMember = mvarMember });
				Dialog.Dismiss ();
			}
		}

		void LvarAddMember_DialogClosed (object sender, DialogContactEventArgs e)
		{
			EditText lvarFirstName = Dialog.FindViewById<EditText> (Resource.Id.etxtName);
			lvarFirstName.Text = e.mvarContact.FirstName;
			EditText lvarLastName = Dialog.FindViewById<EditText> (Resource.Id.etxtSurname);
			lvarLastName.Text = e.mvarContact.LastName;
			LinearLayout lvarPhones = Dialog.FindViewById<LinearLayout> (Resource.Id.VllPhones);
			List<String> lvarNumbers = new List<String>();
			List<String> lvarAddresses = new List<String> ();

			for (int i = 0; i < lvarPhones.ChildCount - 1; i++) {
				lvarPhones.RemoveViewAt (i);
			}
			LinearLayout lvarEmails = Dialog.FindViewById<LinearLayout> (Resource.Id.VllEmails);
			for (int i = 0; i < lvarEmails.ChildCount - 1; i++) {
				lvarEmails.RemoveViewAt (i);
			}
			if (e.mvarContact.Phones.Count () > 0) {
				foreach (Phone lvarPhone in e.mvarContact.Phones.ToList()) {
					String lvarPhoneEdit = lvarPhone.Number.ToString ().Replace ("-", "").Replace (" ", "");
					if(!lvarNumbers.Contains(lvarPhoneEdit))
					{
						AddPhone (lvarPhoneEdit);
						lvarNumbers.Add (lvarPhoneEdit);
					}
				}
			} else {
				AddPhone ("");
			}
			if (e.mvarContact.Emails.Count () > 0) {
				foreach (Email lvarEmail in e.mvarContact.Emails.ToList()) {
					if (!lvarAddresses.Contains (lvarEmail.Address.ToString ())) {
						lvarAddresses.Add (lvarEmail.Address.ToString ());
						AddEmail (lvarEmail.Address.ToString ());
					}
				}		
			} else {
				AddEmail ("");
			}
		}
	}

	public class DialogEventArgs : EventArgs
	{
		public Models.MemberModel lvarMember { get; set; }
	}
		
	public delegate void DialogEventHandler(object sender, DialogEventArgs args);

}