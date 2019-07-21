using System;

namespace SplitTheBill.Models
{
	public class ContributionModel
	{
		public String mvarId = "";
		public String mvarMemberId;
		public String mvarName;
		public String mvarGroupId;
		public Decimal mvarAmount;
		public String mvarCurrency;
		public DateTime mvarDate;

		public ContributionModel (String Id, String GroupId, String Name, String MemberId, Decimal Amount, String Currency, DateTime Date)
		{
			mvarId = Id;
			mvarDate = Date;
			mvarMemberId = MemberId;
			mvarName = Name;
			mvarGroupId = GroupId;
			mvarAmount = Amount;
			mvarCurrency = Currency;
		}

		public ContributionModel (String GroupId, String Name, String MemberId, Decimal Amount, String Currency, DateTime Date)
		{
			mvarDate = Date;
			mvarName = Name;
			mvarMemberId = MemberId;
			mvarGroupId = GroupId;
			mvarAmount = Amount;
			mvarCurrency = Currency;
		}
	}
}

