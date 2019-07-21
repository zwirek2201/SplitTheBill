
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
	public class FragmentCurrencies : Android.Support.V4.App.Fragment
	{
		private List<CurrencyModel> mvarCurrencies;
		private Boolean mvarOpened = false;
		private CurrencyModel mvarSelectedCurrency;

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create your fragment here
		}

		public override string ToString ()
		{
			return "Currencies";
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle	 savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			var lvarView = inflater.Inflate(Resource.Layout.FragmentCurrencies, container, false);

			BeginStartup (lvarView);
			return lvarView;
		}

		private void BeginStartup(View View)
		{
			DBRepository db = new DBRepository ();
			List<CurrencyModel> lvarCurrencies = new List<CurrencyModel> ();
			mvarCurrencies = db.GetCurrencies ();
			lvarCurrencies = new List<CurrencyModel>(mvarCurrencies);
			lvarCurrencies.Remove (lvarCurrencies.Find (cur => cur.mvarSymbol == db.ExchangeRateBase));
			if (lvarCurrencies.Count > 0) {
				lvarCurrencies = lvarCurrencies.OrderBy (Cur => Cur.mvarSymbol).ToList();
				mvarSelectedCurrency = mvarCurrencies.Find (cur => cur.mvarSymbol == db.ExchangeRateBase);
				EditText lvarAmount = View.FindViewById<EditText> (Resource.Id.etAmount);
				lvarAmount.Text = "1";
				lvarAmount.Gravity = GravityFlags.Right | GravityFlags.CenterVertical;
				lvarAmount.Click += LvarAmount_Click;
				lvarAmount.TextChanged += LvarAmount_TextChanged;
				TextView lvarSymbol = View.FindViewById<TextView> (Resource.Id.txtSymbol);
				lvarSymbol.Text = db.ExchangeRateBase;
				lvarSymbol.Gravity = GravityFlags.Left | GravityFlags.CenterVertical;
				lvarSymbol.Click += LvarBase_Click;
				ListView lvarListView = View.FindViewById<ListView> (Resource.Id.lvCurrencies);
				CurrencyListViewAdapter lvarAdapter = new CurrencyListViewAdapter (this.Context, lvarCurrencies);
				lvarListView.Adapter = lvarAdapter;
				lvarAdapter.NotifyDataSetChanged ();
			} else {
				RelativeLayout lvarLayout = View.FindViewById<RelativeLayout> (Resource.Id.rlCurrenciesBackground);
				lvarLayout.RemoveViewAt (0);
				lvarLayout.RemoveViewAt (0);
				lvarLayout.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#f2f2f2"));
				ImageView lvarImage = new ImageView (this.Context);
				RelativeLayout.LayoutParams lvarImageParams = new RelativeLayout.LayoutParams ((int)(150 * Resources.DisplayMetrics.Density), (int)(180 * Resources.DisplayMetrics.Density));
				lvarImageParams.AddRule (LayoutRules.AlignParentTop);
				lvarImageParams.AddRule (LayoutRules.CenterHorizontal);
				lvarImage.LayoutParameters = lvarImageParams;
				//lvarImage.SetImageResource (Resource.Drawable.Currencies_big);
				lvarImage.SetPadding ((int)(10 * Resources.DisplayMetrics.Density), (int)(30 * Resources.DisplayMetrics.Density), (int)(10 * Resources.DisplayMetrics.Density),(int)(10 * Resources.DisplayMetrics.Density));
				lvarImage.SetScaleType (ImageView.ScaleType.CenterInside);
				lvarImage.Id = 123456789;
				lvarLayout.AddView (lvarImage);
				TextView lvarText = new TextView (this.Context);
				RelativeLayout.LayoutParams lvarTextParams = new RelativeLayout.LayoutParams (WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.WrapContent);
				lvarTextParams.AddRule(LayoutRules.Below,123456789);
				lvarText.LayoutParameters = lvarTextParams;
				lvarText.SetPadding ((int)(10 * Resources.DisplayMetrics.Density), (int)(0 * Resources.DisplayMetrics.Density), (int)(10 * Resources.DisplayMetrics.Density),0);
				lvarText.SetTextSize (ComplexUnitType.Dip, 15);
				lvarText.Gravity = GravityFlags.Center;
				lvarText.SetTextColor (Android.Graphics.Color.ParseColor ("#000000"));
				lvarText.Text = "No data to show\nConnect to the Internet to download data";
				lvarText.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#00ffffff"));
				lvarLayout.AddView (lvarText);
			}	
		}

		void LvarAmount_TextChanged (object sender, Android.Text.TextChangedEventArgs e)
		{
			EditText lvarSender = (EditText)sender;
			if (lvarSender.Length() > 0) {
				ListView lvarListView = View.FindViewById<ListView> (Resource.Id.lvCurrencies);
				UpdateListView(CalculateNewRates(mvarSelectedCurrency,Convert.ToDecimal(lvarSender.Text)));
			}
		}

		void LvarAmount_Click (object sender, EventArgs e)
		{
			EditText lvarAmount = View.FindViewById<EditText> (Resource.Id.etAmount);
			lvarAmount.SetSelection (lvarAmount.Length ());
		}

		void LvarBase_Click (object sender, EventArgs e)
		{
			if (!mvarOpened) {
				RelativeLayout lvarLayout = View.FindViewById<RelativeLayout> (Resource.Id.rlCurrenciesBackground);
				LinearLayout lvarDarkener = new LinearLayout (this.Context);
				RelativeLayout.LayoutParams lvarParams = new RelativeLayout.LayoutParams (WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.MatchParent);
				lvarParams.AddRule (LayoutRules.Below, View.FindViewById<LinearLayout> (Resource.Id.HllBaseBackground).Id);
				lvarParams.AddRule (LayoutRules.AlignParentLeft);
				lvarParams.AddRule (LayoutRules.AlignParentBottom);
				lvarParams.AddRule (LayoutRules.AlignParentRight);
				lvarDarkener.LayoutParameters = lvarParams;
				lvarDarkener.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#80000000"));
				lvarDarkener.Click += LvarDarkener_Click;
				lvarLayout.AddView (lvarDarkener, lvarLayout.ChildCount);
				lvarLayout.AddView (GetCurrenciesView (), lvarLayout.ChildCount);
				mvarOpened = true;
			}
		}

		void LvarDarkener_Click (object sender, EventArgs e)
		{
			RelativeLayout lvarLayout = View.FindViewById<RelativeLayout> (Resource.Id.rlCurrenciesBackground);
			lvarLayout.RemoveViewAt (lvarLayout.ChildCount - 1);
			lvarLayout.RemoveViewAt (lvarLayout.ChildCount - 1);
			mvarOpened = false;
		}

		private ScrollView GetCurrenciesView()
		{
			ScrollView lvarScroll = new ScrollView (this.Context);
			lvarScroll.Tag = "ScrollView";
			RelativeLayout.LayoutParams lvarParams = new RelativeLayout.LayoutParams ((int)(250*Resources.DisplayMetrics.Density), (int)((7*45) * Resources.DisplayMetrics.Density));
			lvarParams.AddRule (LayoutRules.Below, View.FindViewById<LinearLayout> (Resource.Id.HllBaseBackground).Id);
			lvarParams.AddRule (LayoutRules.CenterHorizontal);
			lvarScroll.LayoutParameters = lvarParams;
			LinearLayout lvarWrapper = new LinearLayout (this.Context);
			lvarWrapper.LayoutParameters = new LinearLayout.LayoutParams (WindowManagerLayoutParams.MatchParent, WindowManagerLayoutParams.MatchParent);
			lvarWrapper.Orientation = Orientation.Vertical;
			List<CurrencyModel> lvarCurrencies = new List<CurrencyModel>(mvarCurrencies);
			lvarCurrencies = lvarCurrencies.OrderBy (cur => cur.mvarSymbol).ToList();
			foreach (CurrencyModel lvarCurrency in lvarCurrencies) {
				TextView lvarText = new TextView (this.Context);
				lvarText.LayoutParameters = new LinearLayout.LayoutParams (WindowManagerLayoutParams.MatchParent, (int)(45 * Resources.DisplayMetrics.Density));
				lvarText.SetTextSize (ComplexUnitType.Dip, 20);
				lvarText.SetTextColor(Android.Graphics.Color.ParseColor("#000000"));
				lvarText.Gravity = GravityFlags.Center;
				lvarText.Tag = lvarCurrency.mvarSymbol;
				if(lvarCurrency == mvarSelectedCurrency)
					lvarText.SetBackgroundColor(Android.Graphics.Color.ParseColor("#d2d2d2"));
				else
					lvarText.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#f2f2f2"));
				
				lvarText.Text = lvarCurrency.mvarSymbol;
				lvarText.Click += LvarText_Click;
				lvarWrapper.AddView (lvarText);
			}
			lvarScroll.AddView (lvarWrapper);
			return lvarScroll;
		}

		void LvarText_Click (object sender, EventArgs e)
		{
			TextView lvarText = (TextView)sender;
			RelativeLayout lvarLayout = View.FindViewById<RelativeLayout> (Resource.Id.rlCurrenciesBackground);
			CurrencyModel lvarNewBase = mvarCurrencies.Find (Cur => Cur.mvarSymbol == lvarText.Tag.ToString());
			TextView lvarBase = View.FindViewById<TextView> (Resource.Id.txtSymbol);
			lvarBase.Text = lvarNewBase.mvarSymbol;
			EditText lvarAmount = View.FindViewById<EditText> (Resource.Id.etAmount);
			UpdateListView (CalculateNewRates (lvarNewBase,Convert.ToDecimal(lvarAmount.Text)));
			lvarLayout.RemoveViewAt (lvarLayout.ChildCount - 1);
			lvarLayout.RemoveViewAt (lvarLayout.ChildCount - 1);
			mvarSelectedCurrency = mvarCurrencies.Find (Cur => Cur.mvarSymbol == lvarNewBase.mvarSymbol);
			mvarOpened = false;
		}

		private List<CurrencyModel> CalculateNewRates(CurrencyModel Base, Decimal Multiplier)
		{
			EditText lvarAmount = View.FindViewById<EditText> (Resource.Id.etAmount);
			Decimal lvarRatio = 1 / Base.mvarRate;
			List<CurrencyModel> lvarCurrencies = mvarCurrencies.ConvertAll (Cur => new CurrencyModel (Cur.mvarSymbol, Cur.mvarRate, Cur.mvarDate, Cur.mvarChange));
			lvarCurrencies.Remove (lvarCurrencies.Find(Cur =>Cur.mvarSymbol == Base.mvarSymbol));
			lvarCurrencies = lvarCurrencies.OrderBy (Cur => Cur.mvarSymbol).ToList();
			foreach (CurrencyModel lvarCurrency in lvarCurrencies) {
				lvarCurrency.mvarRate = (lvarCurrency.mvarRate * lvarRatio) * Multiplier;
			}
			return lvarCurrencies;
		}

		private void UpdateListView(List<CurrencyModel> Items)
		{
			ListView lvarListView = View.FindViewById<ListView> (Resource.Id.lvCurrencies);
			CurrencyListViewAdapter lvarAdapter = (CurrencyListViewAdapter)lvarListView.Adapter;
			lvarAdapter.mvarItems = Items;
			lvarAdapter.NotifyDataSetChanged ();
		}

		public void Refresh()
		{
			DBRepository db = new DBRepository ();
			if (db.BaseCurrencyChanged) {
				String lvarNewBase = db.ExchangeRateBase; 
				CurrencyModel lvarNewCurrency = mvarCurrencies.Find (Cur => Cur.mvarSymbol == lvarNewBase);
				mvarSelectedCurrency = lvarNewCurrency;
				UpdateListView (CalculateNewRates (lvarNewCurrency, Convert.ToDecimal (View.FindViewById<EditText> (Resource.Id.etAmount).Text)));
				TextView lvarBase = View.FindViewById<TextView> (Resource.Id.txtSymbol);
				lvarBase.Text = lvarNewBase;
				db.BaseCurrencyChanged = false; 
			}
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			base.OnActivityCreated (savedInstanceState);
		}
	}
}

