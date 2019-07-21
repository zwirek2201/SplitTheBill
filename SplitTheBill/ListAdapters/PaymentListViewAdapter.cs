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
using Xamarin.Contacts;
using ImageViews.Rounded;

namespace SplitTheBill
{
	class PaymentListViewAdapter : BaseAdapter<Models.PaymentModel>
	{
		public List<Models.PaymentModel> mvarItems;
		public List<Models.MemberModel> mvarMembers;
		private Context mvarContext;
		private String mvarGroupId;

		public PaymentListViewAdapter(Context Context, List<Models.PaymentModel> Items, List<Models.MemberModel> Members, String Group)
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

		public override Models.PaymentModel this[int position]
		{
			get { return mvarItems[position]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			DBRepository db = new DBRepository ();
			View row = LayoutInflater.From (mvarContext).Inflate (Resource.Layout.PaymentsListViewRow, null, false);

			TextView lvarNameFrom = row.FindViewById<TextView> (Resource.Id.txtNameFrom);
			TextView lvarNameTo = row.FindViewById<TextView> (Resource.Id.txtNameTo);
			Button lvarPaymentField = row.FindViewById<Button> (Resource.Id.btnPaymentAmount);
			//lvarPaymentField.Click += LvarPaymentField_Click;
			lvarPaymentField.Tag = position;

			Models.PaymentModel lvarPayment = mvarItems[position];
			Models.MemberModel lvarMemberFrom = mvarMembers.Find (Member => Member.mvarID == lvarPayment.mvarFromId);
			Models.MemberModel lvarMemberTo = mvarMembers.Find (Member => Member.mvarID == lvarPayment.mvarToId);

			if (lvarMemberFrom.mvarSurname != "")
				lvarNameFrom.Text = lvarMemberFrom.mvarName + " " + lvarMemberFrom.mvarSurname.Substring(0,1) + ".";
			else
				lvarNameFrom.Text = lvarMemberFrom.mvarName;

			if (lvarMemberTo.mvarSurname != "")
				lvarNameTo.Text = lvarMemberTo.mvarName + " " + lvarMemberTo.mvarSurname.Substring(0,1) + ".";
			else
				lvarNameTo.Text = lvarMemberTo.mvarName;

			if (lvarPayment.mvarSettled == true) {
				lvarPaymentField.Text = "SETTLED";
			} else {
				lvarPaymentField.Text = Math.Round(lvarPayment.mvarAmount,2) + " " + db.ExchangeRateBase;
			}

			return row;
		}
		/*
		void LvarPaymentField_Click (object sender, EventArgs e)
		{
			Button lvarButton = (Button)sender;
			mvarItems [Convert.ToInt32(lvarButton.Tag)].ChangePaymentSettled (true);
			lvarButton.Text = "SETTLED";
		}
		*/
	}
}