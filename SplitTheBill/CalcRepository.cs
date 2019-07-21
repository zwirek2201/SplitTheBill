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
	public class CalcRepository
	{
		public CalcRepository ()
		{
		}

		public List<Models.PaymentModel> GetPaymentsInGroup(String GroupId)
		{
			//INITIALIZATION
			DBRepository db = new DBRepository ();
			Models.GroupModel lvarGroup = db.GetGroupById (GroupId);
			List<Models.MemberModel> lvarMembers = db.GetMembersInGroup (GroupId);

			List<Models.PaymentModel> lvarPayments = new List<SplitTheBill.Models.PaymentModel> ();
			List<Models.MemberModel> lvarPaying = new List<SplitTheBill.Models.MemberModel> ();
			List<Models.MemberModel> lvarPayed = new List<SplitTheBill.Models.MemberModel> ();

			Decimal lvarGroupAverage = lvarGroup.mvarGroupAverage;

			//MEMBERS ALLOCATION`
			foreach (Models.MemberModel lvarMember in lvarMembers) {
				List<Models.ContributionModel> lvarContributions = lvarMember.mvarContributions.FindAll (Cont => Cont.mvarGroupId == lvarGroup.mvarId);
				foreach (Models.ContributionModel lvarContribution in lvarContributions) {
					Decimal lvarRate = db.GetCurrentExchangeRate (lvarContribution.mvarCurrency);
					if(lvarRate == 0)
						lvarMember.mvarBalance += lvarContribution.mvarAmount;
					else
						lvarMember.mvarBalance += (lvarContribution.mvarAmount / lvarRate);
				}	
				lvarMember.mvarBalance = lvarGroupAverage - lvarMember.mvarBalance;

				if (lvarMember.mvarBalance > 0) {
					lvarPaying.Add (lvarMember);
				} else if (lvarMember.mvarBalance < 0) {
					lvarPayed.Add (lvarMember);
				}
			}

			db.ClearPaymentsInGroup (GroupId);
			//MAIN ALGORITHM
			while (lvarPaying.Count > 0 || lvarPayed.Count > 0) {
				Decimal lvarPayingContribution = lvarPaying [0].mvarBalance;
				Decimal lvarPayedContribution = lvarPayed [0].mvarBalance;
					
				if (Math.Round(lvarPayingContribution,2,MidpointRounding.AwayFromZero) < -Math.Round(lvarPayedContribution,2,MidpointRounding.AwayFromZero)) {
					Models.PaymentModel lvarPayment = new Models.PaymentModel (lvarPaying [0].mvarID, lvarPayed [0].mvarID, GroupId, lvarPayingContribution, false);
					lvarPayment.mvarId = db.AddPayment (lvarGroup, lvarPaying [0], lvarPayed [0], lvarPayingContribution);
					lvarPayments.Add(lvarPayment);
					lvarPayed [0].mvarBalance += lvarPayingContribution;
					lvarPaying.Remove (lvarPaying [0]);
				} else if (Math.Round(lvarPayingContribution,2,MidpointRounding.AwayFromZero) > -Math.Round(lvarPayedContribution,2,MidpointRounding.AwayFromZero)) {
					Models.PaymentModel lvarPayment = new Models.PaymentModel (lvarPaying [0].mvarID, lvarPayed [0].mvarID, GroupId, -lvarPayedContribution, false);
					lvarPayment.mvarId = db.AddPayment (lvarGroup, lvarPaying [0], lvarPayed [0], -lvarPayedContribution);
					lvarPayments.Add(lvarPayment);
					lvarPaying [0].mvarBalance += lvarPayedContribution;
					lvarPayed.Remove (lvarPayed [0]);
				} else {
					Models.PaymentModel lvarPayment = new Models.PaymentModel (lvarPaying [0].mvarID, lvarPayed [0].mvarID, GroupId, -lvarPayedContribution, false);
					lvarPayment.mvarId = db.AddPayment (lvarGroup, lvarPaying [0], lvarPayed [0], -lvarPayedContribution);
					lvarPayments.Add(lvarPayment);
					lvarPaying.Remove (lvarPaying [0]);
					lvarPayed.Remove (lvarPayed [0]);
				}
			}
			return lvarPayments;
		}

		public void UpdateNewRates(String NewBase)
		{
			DBRepository db = new DBRepository ();
			List<CurrencyModel> lvarCurrencies = db.GetCurrencies ();
			String lvarOldBase = db.ExchangeRateBase;

			Decimal lvarRatio = 1/lvarCurrencies.Find(Cur => Cur.mvarSymbol == NewBase).mvarRate;
			foreach (CurrencyModel lvarCurrency in lvarCurrencies) {
				lvarCurrency.mvarRate = (lvarCurrency.mvarRate * lvarRatio);
				db.UpdateCurrencyToNewBase (lvarCurrency.mvarSymbol, lvarCurrency.mvarRate, DateTime.Today.ToString ("yyyy-MM-dd"));
			}
		}
	}
}

