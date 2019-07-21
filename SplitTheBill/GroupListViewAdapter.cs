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
    class GroupListViewAdapter : BaseAdapter<Models.GroupModel>
    {
        private List<Models.GroupModel> mvarItems;
        private Context mvarContext;

        public GroupListViewAdapter(Context Context, List<Models.GroupModel> Items)
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

        public override Models.GroupModel this[int position]
        {
            get { return mvarItems[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if(row == null)
            {
                row = LayoutInflater.From(mvarContext).Inflate(Resource.Layout.GroupsListViewRow,null,false);
            }

            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtName);
            txtName.Text = mvarItems[position].mvarName;

            TextView txtMembers = row.FindViewById<TextView>(Resource.Id.txtMembers);
            txtMembers.Text = mvarItems[position].mvarMembers.Count.ToString() + " members";
            return row;
        }
    }
}