using System;
using Android.App;
using Android.Content;

namespace SplitTheBill
{
	[BroadcastReceiver]
	[IntentFilter(new string[]{"android.net.conn.CONNECTIVITY_CHANGE"}, Priority = (int)IntentFilterPriority.LowPriority)]
	public class ConnectivityCheckReceiver : BroadcastReceiver
	{
		public ConnectivityCheckReceiver ()
		{
		}

		public override void OnReceive (Context context, Intent intent)
		{
			DBRepository db = new DBRepository ();
			if (NetworkRepository.IsConnected (context) && NetworkRepository.IsNewDataAviable () && db.LastUpdateFailed == true) {
				NetworkRepository.RefreshExchangeRates (db.ExchangeRateBase);

				db.LastUpdateFailed = false;
				Intent lvarUpdatedIntent = new Intent ("me.SplitTheBill.CurrencyUpdater.CurrenciesUpdatedSuccess");
				context.SendOrderedBroadcast (lvarUpdatedIntent, null);
			}
		}
	}
}

