
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
using System.Timers;

namespace SplitTheBill
{
	[Activity (Label = "Split The Bill", MainLauncher = true, Icon = "@drawable/icon2", Theme="@android:style/Theme.NoTitleBar.Fullscreen")]			
	public class SplashScreen : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			//RequestWindowFeature (WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.Splash);

			Timer lvarTimer = new Timer ();
			lvarTimer.Interval = 1000;
			lvarTimer.Start();
			lvarTimer.Elapsed += LvarTimer_Elapsed;
			// Create your application here
		}

		void LvarTimer_Elapsed (object sender, ElapsedEventArgs e)
		{
			Timer lvarTimer = (Timer)sender;
			lvarTimer.Stop ();
			StartActivity (typeof(Advertisment));
			this.Finish ();
		}
	}
}

