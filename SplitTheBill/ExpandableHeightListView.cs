using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Views;
using Java.Lang;

namespace SplitTheBill
{
	public class ExpandableHeightListView : ListView
	{

		System.Boolean expanded = false;

		public ExpandableHeightListView (Context context) : base (context) {}

		public ExpandableHeightListView (Context context, IAttributeSet attrs) : base(context, attrs) {}

		public ExpandableHeightListView(Context context, IAttributeSet attrs,int defStyle) : base(context, attrs, defStyle){}

		public System.Boolean isExpanded()
		{
			return expanded;
		}

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			if (isExpanded())
			{
				// Calculate entire height by providing a very large height hint.
				// But do not use the highest 2 bits of this integer; those are
				// reserved for the MeasureSpec mode.
				int expandSpec = Android.Views.View.MeasureSpec.MakeMeasureSpec(int.MaxValue >> 2, MeasureSpecMode.AtMost);
				base.OnMeasure(widthMeasureSpec, expandSpec);

				ViewGroup.LayoutParams lvarParams = LayoutParameters;
				lvarParams.Height = MeasuredHeight;
			}
			else
			{
				base.OnMeasure (widthMeasureSpec, heightMeasureSpec);
			}
		}
			
		public void setExpanded(System.Boolean expanded)
		{
			this.expanded = expanded;
		}
	}
}

