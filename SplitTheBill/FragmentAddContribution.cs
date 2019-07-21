
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
using ImageViews.Rounded;

namespace SplitTheBill
{
	public class FragmentAddContribution : DialogFragment
	{
		public event EventHandler<DialogEventContArgs> DialogClosed;

		LinearLayout mvarLastSelected = null;
		private Context mvarContext;
		private string mvarGroupId="";

		public FragmentAddContribution(String GroupId, Context Context)
		{
			mvarContext = Context;
			mvarGroupId = GroupId;
		}

		public override void OnStart ()
		{
			base.OnStart ();
			Dialog.Window.SetLayout (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			base.OnCreateView (inflater, container, savedInstanceState);
			return inflater.Inflate(Resource.Layout.AddContribution, container, false);
		}

		public override void OnActivityCreated (Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.UpInDownOut_anim;

			base.OnActivityCreated (savedInstanceState);
		
			WindowManagerLayoutParams lvarParams = Dialog.Window.Attributes;
			lvarParams.Gravity = GravityFlags.Bottom | GravityFlags.CenterHorizontal;
			lvarParams.HorizontalMargin = 0;
			lvarParams.VerticalMargin = 0;
			Dialog.Window.Attributes = lvarParams;
			DBRepository db = new DBRepository ();

			//CLICK EVENTS
			Spinner lvarSpinner = Dialog.FindViewById<Spinner>(Resource.Id.sSpinner);
			List<CurrencyModel> lvarCurrencies = db.GetCurrencies ();
			List<String> lvarSymbols = new List<string> ();
			foreach (CurrencyModel lvarCurrency in lvarCurrencies) {
				lvarSymbols.Add(lvarCurrency.mvarSymbol);
			}
			lvarSymbols.Sort();
			ArrayAdapter lvarAdapter = new ArrayAdapter (mvarContext,Resource.Layout.SpinnerLayout,lvarSymbols);
			lvarSpinner.Adapter = lvarAdapter;

			ImageButton lvarBackButton = Dialog.FindViewById<ImageButton>(Resource.Id.btnBackButton);
			lvarBackButton.Click += delegate(object sender, EventArgs e) {
				Dialog.Dismiss();
			};

			Button lvarDoneButton = Dialog.FindViewById<Button> (Resource.Id.btnDoneButton);
			lvarDoneButton.Click += LvarDoneButton_Click;

			List<Models.MemberModel> lvarMembers = db.GetMembersInGroup (mvarGroupId); 
			PopulateScrollView (lvarMembers);
		}

		void LvarDoneButton_Click (object sender, EventArgs e)
		{
			LinearLayout lvarSelectedMember = mvarLastSelected;
			EditText lvarName = Dialog.FindViewById<EditText> (Resource.Id.etName);
			EditText lvarAmount1 = Dialog.FindViewById<EditText> (Resource.Id.etAmount1);
			EditText lvarAmount2 = Dialog.FindViewById<EditText> (Resource.Id.etAmount2);
			Spinner lvarCurrency = Dialog.FindViewById<Spinner> (Resource.Id.sSpinner);

			if (lvarSelectedMember != null) {
				if (lvarAmount1.Text != "") {
					Decimal lvarAmount = Convert.ToDecimal (lvarAmount1.Text + "." + lvarAmount2.Text);

					Models.ContributionModel lvarContribution = new Models.ContributionModel (mvarGroupId, lvarName.Text, mvarLastSelected.Tag.ToString (), lvarAmount, lvarCurrency.SelectedItem.ToString(), DateTime.Now);

					DialogClosed (this, new DialogEventContArgs{ lvarContribution = lvarContribution });
					Dialog.Dismiss ();
				}
				else {
					Toast.MakeText (mvarContext, "Don't forget about the amount!", ToastLength.Short).Show();
				}
			} else {
				Toast.MakeText (mvarContext, "Choose a contributor", ToastLength.Short).Show();
			}
		}

		private void PopulateScrollView(List<Models.MemberModel> Members)
		{
			HorizontalScrollView lvarScrollView = Dialog.FindViewById<HorizontalScrollView> (Resource.Id.HSVMembers);
			lvarScrollView.RemoveAllViews ();
			lvarScrollView.ScrollBarStyle = ScrollbarStyles.OutsideOverlay;
			LinearLayout lvarChild = new LinearLayout (mvarContext);
			lvarChild.Orientation = Orientation.Horizontal;
			lvarChild.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);

			foreach (Models.MemberModel lvarMember in Members) {
				LinearLayout lvarLayout = new LinearLayout (mvarContext);
				lvarLayout.Orientation = Orientation.Vertical;
				lvarLayout.Tag = lvarMember.mvarID;
				lvarLayout.LayoutParameters = new LinearLayout.LayoutParams ((int)(60 * Resources.DisplayMetrics.Density),LinearLayout.LayoutParams.WrapContent);
				lvarLayout.SetPadding ((int)(5 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density), (int)(5 * Resources.DisplayMetrics.Density));

				RoundedImageView lvarImage = new RoundedImageView (mvarContext);
				lvarImage.SetImageResource (Resource.Drawable.Member_default_thumbnail);
				lvarImage.IsOval = true;
				lvarImage.SetPadding (0, (int)(2 * Resources.DisplayMetrics.Density), 0, (int)(2 * Resources.DisplayMetrics.Density));
				lvarImage.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, (int)(50 * Resources.DisplayMetrics.Density));
				lvarImage.SetScaleType (ImageView.ScaleType.CenterInside);
				lvarLayout.AddView (lvarImage);

				TextView lvarText = new TextView (mvarContext);
				lvarText.Text = lvarMember.mvarName + " " + lvarMember.mvarSurname;
				lvarText.SetTextColor (Android.Graphics.Color.Black);
				lvarText.Gravity = GravityFlags.Center;

				lvarText.SetTextSize (Android.Util.ComplexUnitType.Dip, 10);
				lvarText.LayoutParameters = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
				lvarLayout.AddView (lvarText);

				lvarLayout.Click += LvarLayout_Click;
				lvarChild.AddView (lvarLayout);
			}
			lvarScrollView.AddView (lvarChild);
		}

		void LvarLayout_Click (object sender, EventArgs e)
		{
			LinearLayout lvarLayout = (LinearLayout)sender;

			if (lvarLayout != mvarLastSelected) {
				RoundedImageView lvarImage = (RoundedImageView)lvarLayout.GetChildAt (0);
				lvarImage.BorderColor = Android.Graphics.Color.ParseColor ("#1E90FF");
				lvarImage.BorderWidth = (int)(1.5 * Resources.DisplayMetrics.Density);
				TextView lvarText = (TextView)lvarLayout.GetChildAt (1);
				lvarText.SetTextColor (Android.Graphics.Color.ParseColor ("#1E90FF"));
				if (mvarLastSelected != null) {
					RoundedImageView lvarImage2 = (RoundedImageView)mvarLastSelected.GetChildAt (0);
					lvarImage2.BorderWidth = 0;
					TextView lvarText2 = (TextView)mvarLastSelected.GetChildAt (1);
					lvarText2.SetTextColor (Android.Graphics.Color.ParseColor ("#000000"));
					mvarLastSelected = lvarLayout;
				} else {
					mvarLastSelected = lvarLayout;
				}
			} else {
				mvarLastSelected = null;
			}
		}
	}

	public class DialogEventContArgs : EventArgs
	{
		public Models.ContributionModel lvarContribution { get; set; }
	}

	public delegate void DialogContEventHandler(object sender, DialogEventContArgs args);
}

