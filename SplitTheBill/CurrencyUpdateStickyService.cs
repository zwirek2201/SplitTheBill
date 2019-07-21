using System;
using Android.App;
using Android.Content;
using Android.Widget;

namespace SplitTheBill
{
	[Service]
	[IntentFilter(new[] { "me.SplitTheBill.CurrencyUpdater.TimeForUpdate" })]
	public class CurrencyUpdateStickyService : Service
	{
		public CurrencyUpdateStickyService ()
		{
		}
			
		public override Android.OS.IBinder OnBind (Intent intent)
		{
			return null;
		}

		public override StartCommandResult OnStartCommand (Android.Content.Intent intent, StartCommandFlags flags, int startId)
		{
			DBRepository db = new DBRepository ();
			if (NetworkRepository.IsConnected(this)) {
				if (NetworkRepository.IsNewDataAviable()) {
					NetworkRepository.RefreshExchangeRates ("HRK");
					db.LastUpdateFailed = false;
					Intent lvarUpdatedIntent = new Intent ("me.SplitTheBill.CurrencyUpdater.CurrenciesUpdatedSuccess");
					SendOrderedBroadcast (lvarUpdatedIntent, null);
				}
			} else {
				db.LastUpdateFailed = true;
				Intent lvarUpdatedIntent = new Intent ("me.SplitTheBill.CurrencyUpdater.CurrenciesUpdateFail");
				SendOrderedBroadcast (lvarUpdatedIntent, null);
			}
			return StartCommandResult.Sticky;
		}
	}
}





























