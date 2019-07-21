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
using Java.Util;

namespace SplitTheBill
{
    [Activity(Label = "SplitTheBill")]
    public class MainActivity : FragmentActivity
    {
		private ViewPager mvarViewPager;
		private SlidingTabScrollView mvarScrollView;
		//private UpdateReceiver mvarReceiver;

		protected override void OnCreate(Bundle bundle)
        {
			base.OnCreate(bundle);
			RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.SlidingViewLayout);
		
			var db = new DBRepository ();
            List<Models.GroupModel> lvarGroups = new List<Models.GroupModel>();

			mvarScrollView = FindViewById<SlidingTabScrollView>(Resource.Id.SlidingTabs);
			mvarViewPager = FindViewById<ViewPager>(Resource.Id.ViewPager);
			List<Android.Support.V4.App.Fragment> lvarFragments = new List<Android.Support.V4.App.Fragment> ();
			List<int> lvarIcons = new List<int>();
			lvarFragments.Add (new FragmentMain ());
			lvarFragments.Add (new FragmentGroups());
			lvarFragments.Add (new FragmentCurrencies());
			lvarFragments.Add (new FragmentSettings());

			lvarIcons.Add (Resource.Drawable.Member);
			lvarIcons.Add (Resource.Drawable.Groups);
			lvarIcons.Add (Resource.Drawable.Currencies);
			lvarIcons.Add (Resource.Drawable.Settings);

				
			mvarViewPager.Adapter = new SlidingTabAdapter (SupportFragmentManager, lvarFragments, lvarIcons);
			mvarScrollView.ViewPager = mvarViewPager;

			StartUpdateServices ();
		}

		private void StartUpdateServices()
		{
			Intent lvarCurrencyIntent = new Intent ("me.SplitTheBill.CurrencyUpdater.TimeForUpdate");

			if(!AlarmSet(lvarCurrencyIntent))
			{
				Calendar lvarCalendar = Calendar.GetInstance (Java.Util.TimeZone.Default);
				lvarCalendar.TimeZone = Java.Util.TimeZone.GetTimeZone ("CET");

				lvarCalendar.Set (CalendarField.HourOfDay, 15);
				lvarCalendar.Set (CalendarField.Minute, 15);
				lvarCalendar.Set (CalendarField.Second, 0);
				var lvarManager = (AlarmManager)GetSystemService (Context.AlarmService);

				var lvarPendingIntent = PendingIntent.GetService (this, 0, lvarCurrencyIntent, PendingIntentFlags.UpdateCurrent);
				lvarManager.SetRepeating (AlarmType.RtcWakeup, lvarCalendar.TimeInMillis, AlarmManager.IntervalDay, lvarPendingIntent);
				//lvarManager.SetRepeating (AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 3000, AlarmManager.IntervalFifteenMinutes, lvarPendingIntent);
			}
			//mvarReceiver = new UpdateReceiver (this);
			//var lvarFilter = new IntentFilter ("me.SplitTheBill.CurrencyUpdater.CurrenciesUpdatedSuccess"){Priority = (int)IntentFilterPriority.HighPriority};
			//RegisterReceiver (mvarReceiver, lvarFilter);
		}

		protected override void OnDestroy ()
		{
			//UnregisterReceiver (mvarReceiver);
			base.OnDestroy ();
		}

		private Boolean AlarmSet(Intent Intent)
		{
			return PendingIntent.GetService (this, 0, Intent, PendingIntentFlags.NoCreate) != null;
		}

		public void ShowCurrenciesUpdated()
		{
			LinearLayout lvarLayout = FindViewById<LinearLayout> (Resource.Id.VllSlidingTabContainer);
			TextView lvarText = new TextView (this);
			lvarText.SetBackgroundColor (Android.Graphics.Color.Green);
			lvarText.SetTextColor (Android.Graphics.Color.White);
			lvarText.SetPadding (0, (int)(5 * Resources.DisplayMetrics.Density), 0, (int)(5 * Resources.DisplayMetrics.Density));
			lvarText.Text = "Currencies have been updated";
			lvarText.Gravity = GravityFlags.Center;
			lvarText.SetTextSize (Android.Util.ComplexUnitType.Dip, 12);
			lvarText.LayoutParameters = new LinearLayout.LayoutParams (WindowManagerLayoutParams.MatchParent, (int)(30 * Resources.DisplayMetrics.Density));
			lvarLayout.AddView (lvarText, 1);
		}

		/*class UpdateReceiver : BroadcastReceiver
		{
			private MainActivity mvarActivity;

			public UpdateReceiver(MainActivity Activity)
			{
				mvarActivity = (MainActivity)Activity;
			}

			public override void OnReceive (Context context, Intent intent)
			{
				mvarActivity.ShowCurrenciesUpdated ();
				//InvokeAbortBroadcast ();
			}
		}
		*/
		//var nMgr = (NotificationManager)context.GetSystemService (Context.NotificationService);
		//var notification = new Notification (Resource.Drawable.Icon, "Message from demo service");
		//var pendingIntent = PendingIntent.GetActivity (context,130, new Intent (context, typeof(CurrencyUpdateService)), 0);
		//notification.SetLatestEventInfo (context, "ONRECEIVE", "Exchange rates have been updated!", pendingIntent);
		//nMgr.Notify (0, notification);
    }
}

