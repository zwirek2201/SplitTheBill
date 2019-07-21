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
using Android.Graphics;
using Xamarin.Contacts;
using ImageViews.Rounded;

namespace SplitTheBill
{
	class ContactListViewAdapter : BaseAdapter<Contact>
	{
		public List<Contact> mvarItems;
		private Context mvarContext;
		public ContactListViewAdapter(Context Context, List<Contact> Items)
		{
			mvarContext = Context;
			mvarItems = Items;
		}
		public override int Count
		{
			get { return mvarItems.Count; }
		}
		public override long GetItemId(int position)
		{
			return position;
		}

		public override Contact this[int position]
		{
			get { return mvarItems[position]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			//View row = convertView;
			//if(row == null)
				View row = LayoutInflater.From (mvarContext).Inflate (Resource.Layout.ContactsListViewRow, null, false);
				
			RoundedImageView lvarThumbnail = row.FindViewById<RoundedImageView>(Resource.Id.ivThumbnail);
				//ImageView lvarThumbnail = row.FindViewById<ImageView> (Resource.Id.ivThumbnail);
				if(mvarItems[position].GetThumbnail() != null)
				lvarThumbnail.SetImageBitmap (mvarItems [position].GetThumbnail ());

				TextView txtName = row.FindViewById<TextView> (Resource.Id.txtName);
				txtName.Text = mvarItems [position].FirstName;
				if (mvarItems [position].Nickname != null)
					txtName.Text += " \"" + mvarItems [position].Nickname + "\" ";
				else
					txtName.Text += " ";
				txtName.Text += mvarItems [position].LastName;
			return row;
		//}
		}
	}
}