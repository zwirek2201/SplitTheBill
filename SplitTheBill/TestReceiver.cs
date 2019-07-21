using System;
using Android.Content;
using Android.App;

namespace SplitTheBill
{
	[BroadcastReceiver]
	[IntentFilter(new string[]{"android.intent.action.TIME_TICK"}, Priority = (int)IntentFilterPriority.LowPriority)]

	public class TestReceiver : BroadcastReceiver
	{
		public TestReceiver ()
		{
		}

		public override void OnReceive (Context context, Intent intent)
		{
			//
		}
	}
}

