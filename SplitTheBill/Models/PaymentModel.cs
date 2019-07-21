using System;

namespace SplitTheBill.Models
{
	public class PaymentModel
	{
		public String mvarId;
		public Models.MemberModel mvarFrom = null;
		public String mvarFromId; 
		public Models.MemberModel mvarTo = null;
		public String mvarToId;
		public Models.GroupModel mvarGroup;
		public String mvarGroupId;
		public Decimal mvarAmount;
		public String mvarCurrency;
		public Boolean mvarSettled;

		public PaymentModel (Models.MemberModel From, Models.MemberModel To, String Group, Decimal Amount, Boolean Settled)
		{
			DBRepository db = new DBRepository ();
			mvarFrom = From;
			if(mvarFrom != null)
				mvarFromId = From.mvarID;
			mvarTo = To;
			if(mvarTo != null)
				mvarToId = To.mvarID;
			mvarGroupId = Group;
			mvarAmount = Amount;
			mvarSettled = Settled;
			//mvarCurrency = Currency;	
		}

		public PaymentModel (String Id, Models.MemberModel From, Models.MemberModel To, String Group, Decimal Amount, Boolean Settled)
		{
			DBRepository db = new DBRepository ();
			mvarId = Id;
			mvarFrom = From;
			if (mvarFrom != null)
			mvarFromId = mvarFrom.mvarID;
			mvarTo = To;
			if(mvarTo != null)
			mvarToId = mvarTo.mvarID;
			mvarGroupId = Group;
			mvarAmount = Amount;
			mvarSettled = Settled;
			//mvarCurrency = Currency;	
		}

		public PaymentModel (String From, String To, String Group, Decimal Amount, Boolean Settled)
		{
			DBRepository db = new DBRepository ();
			mvarFromId = From;
			mvarToId = To;
			mvarGroupId = Group;
			mvarAmount = Amount;
			mvarSettled = Settled;
			//mvarCurrency = Currency;	
		}

		public PaymentModel (String Id, String From, String To, String Group, Decimal Amount, Boolean Settled)
		{
			DBRepository db = new DBRepository ();
			mvarId = Id;
			mvarFromId = From;
			mvarToId = To;
			mvarGroupId = Group;
			mvarAmount = Amount;
			mvarSettled = Settled;
			//mvarCurrency = Currency;	
		}

		public void ChangePaymentSettled(Boolean State)
		{
			DBRepository db = new DBRepository ();
			db.ChangePaymentSettled (mvarId, State);
		}
	}
}

