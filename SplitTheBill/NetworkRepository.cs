using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Provider;
using Android.OS;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using SQLite;
using System.IO;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Net;
using System.Text;
using Android.Net;
using Android.Telephony;
using Newtonsoft.Json.Linq;

namespace SplitTheBill
{
	public static class NetworkRepository
	{
		public static NetworkInfo GetNetworkInfo(Context Context)
		{
			ConnectivityManager lvarManager = (ConnectivityManager) Context.GetSystemService("connectivity");
			return lvarManager.ActiveNetworkInfo;
		}

		public static Boolean IsConnected(Context Context)
		{
			NetworkInfo info = NetworkRepository.GetNetworkInfo(Context);
			return (info != null && info.IsConnected);
		}
			
		public static Boolean IsConnectedWifi(Context Context)
		{
			NetworkInfo info = NetworkRepository.GetNetworkInfo(Context);
			return (info != null && info.IsConnected && info.Type == ConnectivityType.Wifi);
		}

		public static Boolean IsConnectedMobile(Context Context)
		{
			NetworkInfo info = NetworkRepository.GetNetworkInfo(Context);
			return (info != null && info.IsConnected && info.Type == ConnectivityType.Mobile);
		}

		public static Boolean IsNewDataAviable()
		{
			DBRepository db = new DBRepository ();
			DateTime lvarLastUpdate = new DateTime ();
			if (db.GetLastUpdateDate () == "") {
				return true;
			} else {
				lvarLastUpdate = Convert.ToDateTime (db.GetLastUpdateDate ());
			}

			WebClient lvarClient = new WebClient ();
			System.Uri lvarUri = new System.Uri ("http://api.fixer.io/latest");

			System.Threading.AutoResetEvent waiter = new System.Threading.AutoResetEvent (false);
			DateTime lvarUpdate = new DateTime ();

			byte[] lvarData = lvarClient.DownloadData (lvarUri);
			string lvarResult = Encoding.UTF8.GetString (lvarData);
			var lvarObject = JObject.Parse (lvarResult);
			lvarUpdate = Convert.ToDateTime (lvarObject ["date"].ToString ());
		
			if (lvarUpdate.CompareTo (lvarLastUpdate) > 0)
				return true;
			else
				return false;
		}

		public static void RefreshExchangeRates(string Base)
		{
			DBRepository db = new DBRepository();
			WebClient lvarClient = new WebClient ();
			System.Uri lvarUri = new System.Uri ("http://api.fixer.io/latest?base=" + Base);

			Byte[] lvarData = lvarClient.DownloadData (lvarUri);
			string lvarResult = "";
			lvarResult = Encoding.UTF8.GetString (lvarData);
			var lvarObject = JObject.Parse (lvarResult);
			string lvarBase = lvarObject ["base"].ToString ();
			DateTime lvarDate = Convert.ToDateTime (lvarObject ["date"].ToString());

			foreach (JProperty lvarItem in lvarObject["rates"]) {
				String lvarName = lvarItem.Name;
				Decimal lvarRate = Convert.ToDecimal(lvarItem.Value);
				db.UpdateCurrency(lvarName, lvarRate, lvarDate.ToString("yyyy-MM-dd"));
			}
			db.UpdateCurrency (lvarBase, 1, lvarDate.ToString("yyyy-MM-dd"));
			db.UpdateLastUpdateDate(lvarDate);
		}
	}
}

