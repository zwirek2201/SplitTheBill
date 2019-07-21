
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
using ImageViews.Rounded;

namespace SplitTheBill
{
	[Activity (Label = "AddContribution")]			
	public class AddContribution : DialogFragment
	{
		private String mvarGroupId;
		private LinearLayout mvarLastSelected = null;

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);	
			var view = inflater.Inflate (Resource.Layout.AddContribution, container, false);

			return view;
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.RightInRightOut_anim;
			base.OnActivityCreated (savedInstanceState);

			Dialog.Window.RequestFeature (WindowFeatures.NoTitle);
			base.OnCreate (savedInstanceState);

			mvarGroupId = Intent.GetStringExtra ("GroupId");

			DBRepository db = new DBRepository ();
			List<Models.MemberModel> lvarMembers = db.GetMembersInGroup (mvarGroupId); 

			ImageButton lvarBackButton = FindViewById<ImageButton> (Resource.Id.btnBackButton);
			lvarBackButton.Click += delegate(object sender, EventArgs e) {
			};
			PopulateScrollView (lvarMembers);
		}

		private void PopulateScrollView(List<Models.MemberModel> Members)
		{
			HorizontalScrollView lvarScrollView = FindViewById<HorizontalScrollView> (Resource.Id.HSVMembers);
			lvarScrollView.RemoveAllViews ();
			lvarScrollView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
			LinearLayout lvarChild = new LinearLayout (this);
			lvarChild.Orientation = Orientation.Horizontal;
			lvarChild.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);

			foreach (Models.MemberModel lvarMember in Members) {
				LinearLayout lvarLayout = new LinearLayout (this);
				lvarLayout.Orientation = Orientation.Vertical;
				lvarLayout.LayoutParameters = new LinearLayout.LayoutParams ((int)(75 * Resources.DisplayMetrics.Density),LinearLayout.LayoutParams.WrapContent);
				lvarLayout.SetPadding ((int)(5 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density));

				RoundedImageView lvarImage = new RoundedImageView (this);
				lvarImage.SetImageResource (Resource.Drawable.Member_default_thumbnail);
				lvarImage.IsOval = true;
				lvarImage.SetPadding (0, (int)(2 * Resources.DisplayMetrics.Density), 0, (int)(2 * Resources.DisplayMetrics.Density));
				lvarImage.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, (int)(60 * Resources.DisplayMetrics.Density));
				lvarImage.SetScaleType (ImageView.ScaleType.CenterInside);
				lvarLayout.AddView (lvarImage);

				TextView lvarText = new TextView (this);
				lvarText.Text = lvarMember.mvarName + " " + lvarMember.mvarSurname;
				lvarText.SetTextColor (Android.Graphics.Color.Black);
				lvarText.Gravity = GravityFlags.Center;

				lvarText.SetTextSize (Android.Util.ComplexUnitType.Dip, 12);
				lvarText.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
				lvarLayout.AddView (lvarText);

				lvarLayout.Tag = lvarScrollView.ChildCount;
				lvarLayout.Click += LvarLayout_Click;
				lvarChild.AddView (lvarLayout);
			}
			lvarScrollView.AddView (lvarChild);
		}

		void LvarLayout_Click (object sender, EventArgs e)
		{
			LinearLayout lvarLayout = (LinearLayout)sender;
			RoundedImageView lvarImage = (RoundedImageView)lvarLayout.GetChildAt (0);
			lvarImage.BorderColor = Android.Graphics.Color.ParseColor ("#1E90FF");
			lvarImage.BorderWidth = (int)(1.5*Resources.DisplayMetrics.Density);
			TextView lvarText = (TextView)lvarLayout.GetChildAt (1);
			lvarText.SetTextColor (Android.Graphics.Color.ParseColor ("#1E90FF"));
			if (mvarLastSelected != null) {
				RoundedImageView lvarImage2 = (RoundedImageView)mvarLastSelected.GetChildAt(0);
				lvarImage2.BorderWidth = 0;
				TextView lvarText2 = (TextView)mvarLastSelected.GetChildAt (1);
				lvarText2.SetTextColor (Android.Graphics.Color.ParseColor ("#000000"));
				mvarLastSelected = lvarLayout;
			} else {
				mvarLastSelected = lvarLayout;
			}	
		}
	}
}

