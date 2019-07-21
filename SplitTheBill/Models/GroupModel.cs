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
using Newtonsoft.Json;
namespace SplitTheBill.Models
{
    public class GroupModel
    {
        public string mvarId;
        public string mvarName;
		public List<MemberModel> mvarMembers;
        public string mvarColorHex;
		public int mvarMemberCount;
		public Decimal mvarGroupAverage;
		public Boolean mvarFavourite;

		public GroupModel(string Id, string Name, string Color, Boolean Favourite)
        {
            mvarId = Id;
            mvarName = Name;
			mvarFavourite = Favourite;
        }

		[Newtonsoft.Json.JsonConstructor]
		public GroupModel(string Name, string Color, List<MemberModel> Members, Boolean Favourite) 
        {
            mvarName = Name;
			mvarMembers = Members;
			mvarMemberCount = mvarMembers.Count ();
			mvarFavourite = Favourite;
        }

        public void AddMember(MemberModel Member)
        {
            mvarMembers.Add(Member);
			mvarMemberCount = mvarMembers.Count ();
        }
    }
}