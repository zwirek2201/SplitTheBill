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

namespace SplitTheBill.Models
{
    public class MemberModel
    {
        public string mvarID;
        public string mvarName;
        public string mvarSurname;
		public List<string> mvarPhoneNumbers = new List<string> ();
		public List<string> mvarEmailAddresses = new List<string>();
		public List<Models.ContributionModel> mvarContributions = new List<Models.ContributionModel>();
		public List<Models.PaymentModel> mvarPayments = new List<Models.PaymentModel> ();
		//public List<Models.PaymentModel> mvarOwed = new List<PaymentModel>();
		//public List<Models.PaymentModel> mvarOwes = new List<PaymentModel>();
		public Decimal mvarBalance = 0;

		[Newtonsoft.Json.JsonConstructor]
		public MemberModel(string ID, string Name, string Surname, List<string> PhoneNumbers, List<string> EmailAddresses)
        {
            mvarID = ID;
            mvarName = Name;
            mvarSurname = Surname;
			mvarPhoneNumbers = PhoneNumbers;
			mvarEmailAddresses = EmailAddresses;
        }

		public MemberModel(string ID, string Name, string Surname)
		{
			mvarID = ID;
			mvarName = Name;
			mvarSurname = Surname;
		}

		public void AddContribution(String Id, String Group, String Name, Decimal Amount, String Currency, DateTime Date)
		{
			Models.ContributionModel lvarContribution = new ContributionModel (Id, Group, Name, this.mvarID, Amount, Currency, Date);
			mvarContributions.Add (lvarContribution);
		}

		public void AddContribution(String Group, String Name, Decimal Amount, String Currency, DateTime Date)
		{
			Models.ContributionModel lvarContribution = new ContributionModel (Group, Name, this.mvarID, Amount, Currency, Date);
			mvarContributions.Add (lvarContribution);
		}

		public void AddOwesPayment(String From, String To, Models.GroupModel Group, Decimal Amount, Boolean Settled)
		{
			Models.PaymentModel lvarPayment = new PaymentModel (To, From, Group.mvarId, Amount, Settled);
			mvarPayments.Add (lvarPayment);
		}

		/*public Decimal GetOwedInGroupSum(Models.GroupModel Group)
		{
			List<Models.PaymentModel> lvarPayments = mvarOwed.FindAll (Pay => Pay.mvarGroup.mvarId == Group.mvarId);
			Decimal lvarSum = 0;
			foreach (Models.PaymentModel lvarPayment in lvarPayments) {
				lvarSum += lvarPayment.mvarAmount;
			}
			return lvarSum;
		}

		public Decimal GetOwesInGroupSum(Models.GroupModel Group)
		{
			List<Models.PaymentModel> lvarPayments = mvarOwes.FindAll (Pay => Pay.mvarGroup.mvarId == Group.mvarId);
			Decimal lvarSum = 0;
			foreach (Models.PaymentModel lvarPayment in lvarPayments) {
				lvarSum += lvarPayment.mvarAmount;
			}
			return lvarSum;
		}*/

		public Decimal GetTotalOwedSum()
		{
			List<Models.PaymentModel> lvarPayments = mvarPayments.FindAll(Pay => Pay.mvarToId == mvarID && Pay.mvarSettled == false);
			Decimal lvarSum = 0;
			foreach (Models.PaymentModel lvarPayment in lvarPayments) {
				lvarSum += lvarPayment.mvarAmount;
			}
			return lvarSum;
		}

		public Decimal GetTotalOwesSum()
		{
			List<Models.PaymentModel> lvarPayments = mvarPayments.FindAll(Pay => Pay.mvarFromId == mvarID && Pay.mvarSettled == false);
			Decimal lvarSum = 0;
			foreach (Models.PaymentModel lvarPayment in lvarPayments) {
				lvarSum += lvarPayment.mvarAmount;
			}
			return lvarSum;
		}
    }
}