using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Java.Lang;
using Android.Support.V7.App;

namespace SplitTheBill
{
	[BroadcastReceiver]
	[IntentFilter(new[] { "CurrencyUpdateAction" })]
	public class CurrencyUpdateService : BroadcastReceiver
	{		
		public override void OnReceive (Context context, Intent intent)
		{
			var nMgr = (NotificationManager)context.GetSystemService (Context.NotificationService);
			var notification = new Notification (Resource.Drawable.Icon, "Message from demo service");
			var pendingIntent = PendingIntent.GetActivity (context,130, new Intent (context, typeof(CurrencyUpdateService)), 0);
			notification.SetLatestEventInfo (context, "ONRECEIVE", "Exchange rates have been updated!", pendingIntent);
			nMgr.Notify (0, notification);
		}
	}
}

