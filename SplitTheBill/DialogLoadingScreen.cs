using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.App;
using Android.Runtime;
using Android.Provider;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Xamarin.Contacts;
using System.ComponentModel;


namespace SplitTheBill
{
	public class DialogLoadingScreen:DialogFragment
	{
		private Context mvarContext;
		public DialogLoadingScreen(Context Context)
		{
			mvarContext = Context;
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);

			var view = inflater.Inflate(Resource.Layout.LoadingScreen, container, false);
			return view;
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature (WindowFeatures.NoTitle);
			base.OnActivityCreated (savedInstanceState);
		}
	}
}

