
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
	public class FragmentSettings : Android.Support.V4.App.Fragment
	{

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override string ToString ()
		{
			return "Settings";
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle	 savedInstanceState)
		{
			var lvarView = inflater.Inflate(Resource.Layout.FragmentSettings, container, false);

			DBRepository db = new DBRepository ();
			List<String> lvarCurrencies = db.GetCurrencySymbols ();
			lvarCurrencies.Sort ();
			String lvarBase = db.ExchangeRateBase;

			TextView lvarBaseText = lvarView.FindViewById<TextView> (Resource.Id.txtBaseText);
			lvarBaseText.Gravity = GravityFlags.Left | GravityFlags.CenterVertical;

			Spinner lvarSpinner = lvarView.FindViewById<Spinner>(Resource.Id.sSpinner);
			ArrayAdapter lvarAdapter = new ArrayAdapter (this.Context,Resource.Layout.SpinnerLayout,lvarCurrencies);
			lvarSpinner.Adapter = lvarAdapter;
			lvarSpinner.SetSelection (lvarCurrencies.IndexOf(lvarBase));
			lvarSpinner.ItemSelected += LvarSpinner_ItemSelected;
			return lvarView;
		}

		void LvarSpinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			DBRepository db = new DBRepository ();
			CalcRepository calc = new CalcRepository ();
			Spinner lvarSpinner = View.FindViewById<Spinner>(Resource.Id.sSpinner);

			String lvarSelectedItem = lvarSpinner.SelectedItem.ToString ();
			if (lvarSelectedItem != db.ExchangeRateBase) {
				db.ExchangeRateBase = lvarSelectedItem;
				calc.UpdateNewRates (lvarSelectedItem);
				db.BaseCurrencyChanged = true;
				db.MainScreenDataChanged = true;
				foreach (Models.GroupModel lvarGroup in db.GetGroups()) {
					List<Models.PaymentModel> lvarPayments = calc.GetPaymentsInGroup (lvarGroup.mvarId);
				}
			}
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);
		}
	}
}

