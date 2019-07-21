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
using Android.Support.V4.View;
using Android.Util;
using Android.Support.V4.App;

namespace SplitTheBill
{
	public class SlidingTabScrollView : HorizontalScrollView
	{

		private const int TITLE_OFFSET_DIPS = 24;
		private const int TAB_VIEW_PADDING_DIPS = 0;
		private const int TAB_VIEW_TEXT_SIZE_SP = 12;

		private int mTitleOffset;

		private int mTabViewLayoutID;
		private int mTabViewTextViewID;

		private ViewPager mViewPager;
		private ViewPager.IOnPageChangeListener mViewPagerPageChangeListener;

		private static SlidingTabStrip mTabStrip;

		private int mScrollState;

		public interface TabColorizer
		{
			int GetIndicatorColor(int position);
			int GetDividerColor(int position);
		}

		public SlidingTabScrollView(Context context) : this(context, null) { }

		public SlidingTabScrollView(Context context, IAttributeSet attrs) : this(context, attrs, 0) { }

		public SlidingTabScrollView (Context context, IAttributeSet attrs, int defaultStyle) : base(context, attrs, defaultStyle)
		{
			//Disable the scroll bar
			HorizontalScrollBarEnabled = false;

			//Make sure the tab strips fill the view
			FillViewport = true;
			this.SetBackgroundColor(Android.Graphics.Color.ParseColor("#1E90FF")); //Blue color

			mTitleOffset = (int)(TITLE_OFFSET_DIPS * Resources.DisplayMetrics.Density);

			mTabStrip = new SlidingTabStrip(context);
			this.AddView(mTabStrip, LayoutParams.MatchParent, LayoutParams.MatchParent);
		}

		public TabColorizer CustomTabColorizer
		{
			set { mTabStrip.CustomTabColorizer = value; }
		}

		public int [] SelectedIndicatorColor
		{
			set { mTabStrip.SelectedIndicatorColors = value; }
		}

		public int [] DividerColors
		{
			set { mTabStrip.DividerColors = value; }
		}

		public ViewPager.IOnPageChangeListener OnPageListener
		{
			set { mViewPagerPageChangeListener = value; }
		}

		public ViewPager ViewPager
		{
			set
			{
				mTabStrip.RemoveAllViews();

				mViewPager = value;
				if (value != null)
				{
					value.PageSelected += value_PageSelected;
					value.PageScrollStateChanged += value_PageScrollStateChanged;
					value.PageScrolled += value_PageScrolled;
					PopulateTabStrip();
				}
			}
		}

		void value_PageScrolled(object sender, ViewPager.PageScrolledEventArgs e)
		{
			int tabCount = mTabStrip.ChildCount;

			if ((tabCount == 0) || (e.Position < 0) || (e.Position >= tabCount))
			{
				//if any of these conditions apply, return, no need to scroll
				return;
			}

			mTabStrip.OnViewPagerPageChanged(e.Position, e.PositionOffset);

			View selectedTitle = mTabStrip.GetChildAt(e.Position);

			int extraOffset = (selectedTitle != null ? (int)(e.Position * selectedTitle.Width) : 0);

			ScrollToTab(e.Position, extraOffset);

			if (mViewPagerPageChangeListener != null)
			{
				mViewPagerPageChangeListener.OnPageScrolled(e.Position, e.PositionOffset, e.PositionOffsetPixels);
			}

		}

		void value_PageScrollStateChanged(object sender, ViewPager.PageScrollStateChangedEventArgs e)
		{
			mScrollState = e.State;


			if (mViewPagerPageChangeListener != null)
			{
				mViewPagerPageChangeListener.OnPageScrollStateChanged(e.State);
			}
		}

		void value_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
		{
			SlidingTabAdapter lvarAdapter = (SlidingTabAdapter)mViewPager.Adapter;

			switch (e.Position) {
			case 0:
				FragmentMain lvarFragment = (FragmentMain)lvarAdapter.lvarItems [e.Position];
				lvarFragment.Refresh ();
				break;
			case 2:
				FragmentCurrencies lvarCurrencies = (FragmentCurrencies)lvarAdapter.lvarItems [e.Position];
				lvarCurrencies.Refresh ();
				break;
			}


			if (mScrollState == ViewPager.ScrollStateIdle)
			{
				mTabStrip.OnViewPagerPageChanged(e.Position, 0f);
				ScrollToTab(e.Position, 0);
			}

			if (mViewPagerPageChangeListener != null)
			{
				mViewPagerPageChangeListener.OnPageSelected(e.Position);
			}
		}

		private void PopulateTabStrip()
		{
			SlidingTabAdapter adapter = (SlidingTabAdapter)mViewPager.Adapter;

			int lvarChildCount = adapter.lvarItems.Count();
			mTabStrip.WeightSum = 1 * lvarChildCount;
			for (int i = 0; i < lvarChildCount; i++)
			{
				RelativeLayout lvarLayout = new RelativeLayout (Context);
				lvarLayout.LayoutParameters = new LinearLayout.LayoutParams (0, LayoutParams.MatchParent, 1);
				lvarLayout.SetGravity (GravityFlags.Center);
				ImageView lvarIcon = new ImageView(Context);
				lvarIcon.SetImageResource (adapter.GetIcon (i));
				lvarIcon.SetScaleType (ImageView.ScaleType.CenterInside);
				lvarIcon.Id = 1;
				lvarIcon.SetPadding (0, (int)(7 * Resources.DisplayMetrics.Density), 0, 0);
				TextView lvarText = new TextView (Context);
				lvarText.Text = adapter.GetItem (i).ToString ();
				lvarText.Id = 2;
				lvarText.SetTextSize (ComplexUnitType.Dip, 11);
				lvarText.SetAllCaps (true);
				lvarText.SetTextColor (Android.Graphics.Color.ParseColor ("#ffffff"));
				lvarText.Gravity = GravityFlags.CenterHorizontal;
				RelativeLayout.LayoutParams lvarIconParams = new RelativeLayout.LayoutParams (LayoutParams.MatchParent, LayoutParams.MatchParent);
				lvarIconParams.AddRule (LayoutRules.AlignParentTop);
				lvarIconParams.AddRule (LayoutRules.Above, lvarText.Id);
				RelativeLayout.LayoutParams lvarTextParams = new RelativeLayout.LayoutParams (LayoutParams.MatchParent,(int)(20*Resources.DisplayMetrics.Density));
				lvarTextParams.AddRule (LayoutRules.AlignParentBottom);
				lvarTextParams.AddRule (LayoutRules.CenterHorizontal);
				lvarIcon.LayoutParameters = lvarIconParams;
				lvarText.LayoutParameters = lvarTextParams;
				lvarLayout.AddView (lvarIcon);
				lvarLayout.AddView (lvarText);
				lvarLayout.Click += tabView_Click;
				lvarLayout.Tag = i;
				mTabStrip.AddView (lvarLayout);
			}
		}

		void tabView_Click(object sender, EventArgs e)
		{
			RelativeLayout clickTab = (RelativeLayout)sender;
			int pageToScrollTo = (int)clickTab.Tag;
			mViewPager.CurrentItem = pageToScrollTo;
		}

		private TextView CreateDefaultTabView(Android.Content.Context context)
		{
			TextView textView = new TextView(context);
			textView.Gravity = GravityFlags.Center;
			textView.SetTextSize(ComplexUnitType.Sp, TAB_VIEW_TEXT_SIZE_SP);
			textView.Typeface = Android.Graphics.Typeface.DefaultBold;

			//textView.LayoutParameters = new TableLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent, 50.0f);
			textView.SetAllCaps(true);

			int padding = (int)(5* Resources.DisplayMetrics.Density);
			textView.SetPadding(padding, padding, padding, padding);

			return textView;
		}

		protected override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();

			if (mViewPager != null)
			{
				ScrollToTab(mViewPager.CurrentItem, 0);
			}
		}

		private void ScrollToTab(int tabIndex, int extraOffset)
		{
			int tabCount = mTabStrip.ChildCount;

			if (tabCount == 0 || tabIndex < 0 || tabIndex >= tabCount)
			{
				//No need to go further, dont scroll
				return;
			}

			View selectedChild = mTabStrip.GetChildAt(tabIndex);
			if (selectedChild != null)
			{
				int scrollAmountX = selectedChild.Left + extraOffset;

				if (tabIndex >0 || extraOffset > 0)
				{
					scrollAmountX -= mTitleOffset;
				}

				this.ScrollTo(scrollAmountX, 0);
			}

		}

	}
}