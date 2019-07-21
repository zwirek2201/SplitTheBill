
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

namespace SplitTheBill
{
	[Activity (Label = "Advertisment")]			
	public class Advertisment : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			Window.RequestFeature (WindowFeatures.NoTitle);
			SetContentView (Resource.Layout.Advertisement);
			FindViewById<ImageView>(Resource.Id.ivClose).Click += Close_Click;
		}

		void Close_Click (object sender, EventArgs e)
		{
			StartActivity (typeof(InitialStartup));
			this.Finish ();
		}
	}
}

