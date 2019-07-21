using System;
using Android.Support.V4.View;
using System.Collections.Generic;
using Android.Views;
using Android.Support.V4.App;

namespace SplitTheBill
{
	public class SlidingTabAdapter : FragmentPagerAdapter
	{
		public List<Fragment> lvarItems;
		public List<int> lvarIcons;

		public SlidingTabAdapter (FragmentManager lvarFragmentManager, List<Fragment> Items, List<int> Icons) : base(lvarFragmentManager)
		{
			lvarItems = Items;
			lvarIcons = Icons;
		}

		public override int Count
		{
			get { return lvarItems.Count; }
		}

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return lvarItems[position];
		}

		public int GetIcon(int position)
		{
			return lvarIcons [position];
		}
	}
}

