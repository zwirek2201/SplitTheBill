
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
using System.IO;

namespace SplitTheBill
{
	[Activity (Label = "Split The Bill", Icon = "@drawable/logo")]			
	public class InitialStartup : Activity
	{
		List<String> mvarCurrencies;
		String mvarSelectedCurrency;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			RequestWindowFeature(WindowFeatures.NoTitle);
			base.OnCreate (savedInstanceState);
			DBRepository db = new DBRepository ();

			//System.IO.File.Delete(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "SplitTheBill.db3"));
			if (System.IO.File.Exists (Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), "SplitTheBill.db3")) == false) {
				db.CreateDatabase ();
			}

			mvarCurrencies = db.GetCurrencySymbols ();
			mvarCurrencies.Sort ();
			if (!db.InitialSetupDone) {
				SetContentView (Resource.Layout.InitialSetup);
				Spinner lvarSpinner = FindViewById<Spinner>(Resource.Id.spBaseCurrency);
				ArrayAdapter lvarAdapter = new ArrayAdapter (this,Resource.Layout.SpinnerLayout,mvarCurrencies);
				lvarSpinner.Adapter = lvarAdapter;
				FindViewById<LinearLayout> (Resource.Id.VllStep2).Visibility = ViewStates.Invisible;
				FindViewById<Button>(Resource.Id.btnDone1).Click += Button1_Click;
				FindViewById<Button>(Resource.Id.btnDownload).Click += Download_Click;
			} else {
				StartActivity (typeof(MainActivity));
				this.Finish ();
			}
		}

		void Button1_Click (object sender, EventArgs e)
		{
			DBRepository db = new DBRepository ();
			Spinner lvarSpinner = FindViewById<Spinner>(Resource.Id.spBaseCurrency);
			mvarSelectedCurrency = lvarSpinner.SelectedItem.ToString ();
			db.ExchangeRateBase = mvarSelectedCurrency;
			LinearLayout lvarStep2 = FindViewById<LinearLayout> (Resource.Id.VllStep2);
			lvarStep2.Visibility = ViewStates.Visible;
			LinearLayout lvarStep1 = FindViewById<LinearLayout> (Resource.Id.VllStep1);
			FindViewById<Button> (Resource.Id.btnDone1).Enabled = false;
			FindViewById<Button> (Resource.Id.btnDone1).Text = "DONE";
			FindViewById<Button> (Resource.Id.btnDone1).SetTextColor (Android.Graphics.Color.ParseColor ("#80505050"));
			if (NetworkRepository.IsConnected (this)) {
				FindViewById<TextView> (Resource.Id.txtConnectToDownload).Visibility = ViewStates.Invisible;
				FindViewById<Button> (Resource.Id.btnDownload).Enabled = true;
			} else {
				ConnectivityRestoredReceiver lvarReceiver = new ConnectivityRestoredReceiver (this);
				IntentFilter lvarIntentFilter = new IntentFilter ("android.net.conn.CONNECTIVITY_CHANGE");
				RegisterReceiver (lvarReceiver, lvarIntentFilter);
				FindViewById<Button>(Resource.Id.btnDownload).SetTextColor (Android.Graphics.Color.ParseColor ("#80505050"));
				FindViewById<Button> (Resource.Id.btnDownload).Enabled = false;
			}
			FindViewById<Button> (Resource.Id.btnStart).Enabled = true;
			FindViewById<Button>(Resource.Id.btnStart).Click += Start_Click;
		}

		void Download_Click (object sender, EventArgs e)
		{
			DBRepository db = new DBRepository ();
			FindViewById<Button> (Resource.Id.btnDownload).Text = "Downloading";
			NetworkRepository.RefreshExchangeRates (mvarSelectedCurrency);
			FindViewById<Button> (Resource.Id.btnDownload).Enabled = false;
			FindViewById<Button> (Resource.Id.btnDownload).Text = "Downloaded";
			FindViewById<Button> (Resource.Id.btnDownload).SetTextColor (Android.Graphics.Color.ParseColor ("#80505050"));
			FindViewById<LinearLayout> (Resource.Id.VllStep3).Visibility = ViewStates.Visible;
		}

		void Start_Click (object sender, EventArgs e)
		{
			DBRepository db = new DBRepository ();
			db.InitialSetupDone = true;
			StartActivity (typeof(MainActivity));
			this.Finish ();
		}

		public void ConnectionRestored()
		{
			FindViewById<TextView> (Resource.Id.txtConnectToDownload).Visibility = ViewStates.Gone;
			FindViewById<Button> (Resource.Id.btnDownload).Enabled = true;
			FindViewById<Button> (Resource.Id.btnDownload).SetTextColor (Android.Graphics.Color.LightGreen);
		}

		public void ConnectionInteruppted()
		{
			FindViewById<TextView> (Resource.Id.txtConnectToDownload).Visibility = ViewStates.Visible;
			FindViewById<Button> (Resource.Id.btnDownload).Enabled = false;
			FindViewById<Button> (Resource.Id.btnDownload).SetTextColor (Android.Graphics.Color.ParseColor ("#80505050"));
		}

		class ConnectivityRestoredReceiver : BroadcastReceiver
		{
			private InitialStartup mvarActivity;

			public ConnectivityRestoredReceiver(InitialStartup Activity)
			{
				mvarActivity = Activity;
			}

			public override void OnReceive (Context context, Intent intent)
			{	if (NetworkRepository.IsConnected (context))
					mvarActivity.ConnectionRestored ();
				else
					mvarActivity.ConnectionInteruppted ();
					
			}
		}
	}
}

