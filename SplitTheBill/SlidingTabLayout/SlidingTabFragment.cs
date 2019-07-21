
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;

namespace SplitTheBill
{
	public class SlidingTabFragment : Fragment
	{
		private List<int> mvarItems;
		private List<String> mvarHeaders;
		//private SlidingTabScrollView mvarScrollView;
		private ViewPager mvarViewPager;

		public SlidingTabFragment (List<int> Items, List<String> Headers)
		{
			mvarItems = Items;
			mvarHeaders = Headers;
		}
		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			return inflater.Inflate(Resource.Layout.SlidingViewLayout, container, false);

		}

		public override void OnViewCreated (View view, Bundle savedInstanceState)
		{

			//mvarScrollView = view.FindViewById<SlidingTabScrollView> (Resource.Id.SlidingTabs);
			//mvarViewPager = view.FindViewById<ViewPager> (Resource.Id.ViewPager);

			//mvarViewPager.Adapter = new SamplePagerAdapter (mvarItems, mvarHeaders);
			//mvarScrollView.ViewPager = mvarViewPager;
		}
	}
}

