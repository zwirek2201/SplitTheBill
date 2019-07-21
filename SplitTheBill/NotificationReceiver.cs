using System;
using Android.Content;
using Android.App;

namespace SplitTheBill
{
	[BroadcastReceiver]
	[IntentFilter(new string[]{"me.SplitTheBill.CurrencyUpdater.CurrenciesUpdatedSuccess"}, Priority = (int)IntentFilterPriority.LowPriority)]
	public class NotificationReceiver : BroadcastReceiver
	{
		public NotificationReceiver ()
		{
		}

		public override void OnReceive (Context context, Intent intent)
		{
			var nMgr = (NotificationManager)context.GetSystemService (Context.NotificationService);
			var notification = new Notification (Resource.Drawable.Groups,"Exchange rates have been updated");
			var pendingIntent = PendingIntent.GetActivity (context, 130 , new Intent (context, typeof(NotificationReceiver)), 0);
			notification.SetLatestEventInfo (context, "Split The Bill", "Exchange rates have been updated!", pendingIntent);
			nMgr.Notify (0, notification);
		}
	}
}

