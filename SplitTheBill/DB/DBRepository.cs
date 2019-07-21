using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using Android.Widget;
using Android.Content;

namespace SplitTheBill
{
    public class DBRepository
    {

        private SqliteConnection GetConnection()
        {
            string connectionPath = "URI=file:" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SplitTheBill.db3");
            var connection = new SqliteConnection(connectionPath);
            connection.Open();
            return connection;
        }

		//SETUP
        public void CreateDatabase()
        {
            SqliteConnection connection = GetConnection();
            SqliteCommand command = new SqliteCommand(connection);
            command.CommandText = "create table if not exists Groups (Id integer primary key autoincrement, Name text not null, Color integer, Favourite bool)";
            command.ExecuteNonQuery();
            command.CommandText = "create table if not exists Members (Id integer primary key autoincrement, Name text, Surname text)";
            command.ExecuteNonQuery();
			command.CommandText = "create table if not exists Phones (Id integer primary key autoincrement, Member integer, Label text, Number text, foreign key (Member) references Members(Id))";
			command.ExecuteNonQuery ();
			command.CommandText = "create table if not exists Emails (Id integer primary key autoincrement, Member integer, Label text, Address text, foreign key (Member) references Members(Id))";
			command.ExecuteNonQuery ();
			command.CommandText = "create table if not exists MembersInGroups(Id integer primary key autoincrement, [Group] integer, Member integer, foreign key ([Group]) references Groups(Id), foreign key(Member) references Members(Id))";
            command.ExecuteNonQuery();
			command.CommandText = "create table if not exists Contributions (Id integer primary key autoincrement, Name text, Member integer, [Group] integer, Amount float, Currency string, Date datetime, foreign key (Member) references Members(Id), foreign key ([Group]) references Groups(Id))";
			command.ExecuteNonQuery ();
			command.CommandText = "create table if not exists Payments (Id integer primary key autoincrement, [Group], [From] integer, [To] integer, Amount float, Settled bool, foreign key ([Group]) references Groups(Id), foreign key ([From]) references Members(Id), foreign key ([To]) references Members(Id))";
			command.ExecuteNonQuery ();
			command.CommandText = "create table if not exists ExchangeRates (Id integer primary key autoincrement, Symbol text, Rate float, Date text, Change float)";
			command.ExecuteNonQuery ();
			command.CommandText = "create table if not exists AppSettings (Id integer primary key autoincrement, Name text not null, Value text not null)";
			command.ExecuteNonQuery();
			command.CommandText = "insert into Members values ('0','Me','')";	
			command.ExecuteNonQuery ();
			command.CommandText = "insert into AppSettings (Name,Value) values ('LastCurrencyUpdateDate','')";
			command.ExecuteNonQuery ();
			command.CommandText = "insert into AppSettings(Name,Value) values ('ExchangeRateBase','HRK')";
			command.ExecuteNonQuery ();
			command.CommandText = "insert into AppSettings (Name, Value) values ('LastUpdateFailed','true')";
			command.ExecuteNonQuery ();
			command.CommandText = "insert into AppSettings (Name, Value) values ('InitialSetupDone','false')";
			command.ExecuteNonQuery ();
			command.CommandText = "insert into AppSettings (Name, Value) values ('MainScreenDataChanged','false')";
			command.ExecuteNonQuery ();
			command.CommandText = "insert into AppSettings (Name, Value) values ('BaseCurrencyChanged','false')";
			command.ExecuteNonQuery ();
			connection.Close();
			Test ();
			InsertCurrencies ();
            }
		private void InsertCurrencies()
		{
			SqliteConnection connection = GetConnection();
			SqliteCommand command = new SqliteCommand(connection);
			command.CommandText = "insert into ExchangeRates (Symbol) values ('AUD');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('BGN');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('BRL');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('CAD');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('CHF');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('CNY');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('CZK');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('DKK');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('EUR');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('GBP');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('HKD');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('HRK');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('HUF');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('IDR');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('ILS');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('INR');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('JPY');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('KRW');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('MXN');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('MYR');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('NOK');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('NZD');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('PHP');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('PLN');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('RON');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('RUB');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('SEK');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('SGD');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('THB');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('TRY');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('USD');";
			command.CommandText += "insert into ExchangeRates (Symbol) values ('ZAR');";
			command.ExecuteNonQuery();
			connection.Close ();
		}
        
		private void Test()
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand command = new SqliteCommand(lvarConnection);
			command.CommandText = "insert into Groups (Name) values ('GrupaDupa');";
			command.CommandText += "insert into Groups (Name) values ('Dupa2');";
			command.CommandText += "insert into Members (Name, Surname) values ('Marcin','Bator');";
			command.CommandText += "insert into Members (Name, Surname) values ('Katarzyna','Karol');";
			command.CommandText += "insert into Members (Name, Surname) values ('Kinga','Otolinska');";
			command.CommandText += "insert into Members (Name, Surname) values ('Rafa','Nadal');";
			command.CommandText += "insert into Members (Name, Surname) values ('Huan','Pluton');";
			command.CommandText += "insert into Members (Name, Surname) values ('Dupa','Kuppa');";
			command.CommandText += "insert into MembersInGroups ([Group], Member) values (1,0);";
			command.CommandText += "insert into MembersInGroups ([Group], Member) values (1,1);";
			command.CommandText += "insert into MembersInGroups ([Group], Member) values (1,2);";
			command.CommandText += "insert into MembersInGroups ([Group], Member) values (1,3);";
			command.CommandText += "insert into MembersInGroups ([Group], Member) values (1,4);";
			command.CommandText += "insert into MembersInGroups ([Group], Member) values (1,5);";
			command.CommandText += "insert into MembersInGroups ([Group], Member) values (2,0);";
			command.CommandText += "insert into MembersInGroups ([Group], Member) values (2,2);";
			command.ExecuteNonQuery();
			lvarConnection.Close ();
		}
	//GROUPS
        public List<Models.GroupModel> GetGroups()
        {
            List<Models.GroupModel> lvarGroups = new List<Models.GroupModel>();
			List<Models.MemberModel> lvarMembers = GetMembers ();
            SqliteConnection lvarConnection = GetConnection();

			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
            lvarCommand.CommandText = "SELECT Id, Name, Color, Favourite FROM Groups";

            SqliteDataReader lvarReader = lvarCommand.ExecuteReader();
            while(lvarReader.Read())
            {
				Models.GroupModel lvarGroup = new Models.GroupModel(lvarReader[0].ToString(), lvarReader[1].ToString(), lvarReader[2].ToString(), (lvarReader[3] == DBNull.Value)?false:Convert.ToBoolean(lvarReader[3]));
                lvarGroups.Add(lvarGroup);
            }
			lvarReader.Close ();
			lvarCommand.CommandText = "SELECT [Group], count(Member) FROM MembersInGroups group by [Group]";
			lvarReader = lvarCommand.ExecuteReader();
			while(lvarReader.Read())
			{
				Models.GroupModel lvarGroup = lvarGroups.Find (Group => Group.mvarId == lvarReader [0].ToString ());
				lvarGroup.mvarMemberCount = Convert.ToInt32(lvarReader [1]);
			}
			lvarReader.Close ();
			lvarCommand.CommandText = "select [Group], Amount, Currency from Contributions";
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Models.GroupModel lvarGroup = lvarGroups.Find (Group => Group.mvarId == lvarReader [0].ToString ());
				if(lvarGroup != null) {
					Decimal lvarRate = GetCurrentExchangeRate (lvarReader [2].ToString ());
					if(lvarRate == 0)
						lvarGroup.mvarGroupAverage += Convert.ToDecimal (lvarReader [1]);
					else
						lvarGroup.mvarGroupAverage += Convert.ToDecimal (Convert.ToDecimal(lvarReader [1]) / GetCurrentExchangeRate(lvarReader[2].ToString()));
				}
			}
			foreach (Models.GroupModel lvarGroup in lvarGroups) {
				lvarGroup.mvarGroupAverage = lvarGroup.mvarGroupAverage / lvarGroup.mvarMemberCount;
			}
			lvarReader.Close ();
            lvarConnection.Close();
            
			return lvarGroups;
        }

		/*public List<Models.GroupModel> GetGroupsWithContributions()
		{
			List<Models.GroupModel> lvarGroups = GetGroups ();
			foreach (Models.GroupModel lvarGroup in lvarGroups) {
				List<Models.MemberModel> lvarMembers = GetMembersInGroupWithContributions (lvarGroup.mvarId);
				lvarGroup.mvarMembers = lvarMembers;
			}
			return lvarGroups;
		}*/

		public Models.GroupModel GetGroupById(String GroupId)
		{
			Models.GroupModel lvarGroup = null;
			List<Models.MemberModel> lvarMembers = GetMembers ();
			SqliteConnection lvarConnection = GetConnection();

			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = String.Format("select Id, Name, Color, Favourite from Groups where Id = {0}", GroupId);

			SqliteDataReader lvarReader = lvarCommand.ExecuteReader();
			while (lvarReader.Read ()) {
				lvarGroup = new Models.GroupModel(lvarReader[0].ToString(), lvarReader[1].ToString(), lvarReader[2].ToString(), (lvarReader[3] == DBNull.Value)?false:Convert.ToBoolean(lvarReader[3]));
			}
			lvarReader.Close ();
			lvarCommand.CommandText = "SELECT count(Member) FROM MembersInGroups where [Group]=" + GroupId;
			lvarReader = lvarCommand.ExecuteReader();
			while(lvarReader.Read())
			{
				if(lvarGroup != null)
				lvarGroup.mvarMemberCount = Convert.ToInt32(lvarReader [0]);
			}
			lvarReader.Close ();
			lvarCommand.CommandText = "select Amount, Currency from Contributions where [Group] = " + GroupId;
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Decimal lvarRate = GetCurrentExchangeRate (lvarReader [1].ToString ());
				if (lvarRate == 0)
					lvarGroup.mvarGroupAverage += Convert.ToDecimal (lvarReader [0]);
				else
					lvarGroup.mvarGroupAverage += Convert.ToDecimal (lvarReader [0]) / lvarRate;
			}
			lvarReader.Close ();
			if (lvarGroup.mvarMemberCount != 0)
				lvarGroup.mvarGroupAverage = lvarGroup.mvarGroupAverage / lvarGroup.mvarMemberCount;
			else
				lvarGroup.mvarGroupAverage = 0;
			lvarConnection.Close();
			return lvarGroup;
			}

		public string AddGroup(String Name, String Color, List<Models.MemberModel> Members, Boolean Favourite)
        {
            SqliteConnection lvarConnection = GetConnection();

			String mvarFavourite;
			if (Favourite)
				mvarFavourite = "1";
			else
				mvarFavourite = "0";
			
			String lvarInsertString = "insert into Groups (Name, Color, Favourite) values ('" + Name + "','" + Color + "'," + mvarFavourite + ")";
                SqliteCommand lvarCommand = new SqliteCommand(lvarInsertString, lvarConnection);
                lvarCommand.ExecuteNonQuery();
			String lvarGroupId = GetLastId (lvarConnection);
			foreach (Models.MemberModel lvarMember in Members) {
				lvarCommand.CommandText = "insert into MembersInGroups ([Group], Member) values(" + lvarGroupId + "," + lvarMember.mvarID + ")";
				lvarCommand.ExecuteNonQuery ();
			}
            lvarConnection.Close();
			return lvarGroupId;
        }

		public void UpdateGroupName (String GroupId, String Name)
		{
			SqliteConnection lvarConnection = GetConnection ();
			SqliteCommand lvarCommand = new SqliteCommand (lvarConnection);

			lvarCommand.CommandText = "Update Groups set Name = '" + Name + "' where Id = '" + GroupId + "'";
			lvarCommand.ExecuteNonQuery();
			lvarConnection.Close ();
		}

		public void UpdateGroupeFavourite(String GroupId, Boolean Favourite)
		{
			SqliteConnection lvarConnection = GetConnection ();
			SqliteCommand lvarCommand = new SqliteCommand (lvarConnection);

			String lvarFavouriteString = "0";

			if (Favourite)
				lvarFavouriteString = "1";

			lvarCommand.CommandText = "Update Groups set Favourite = '" + lvarFavouriteString + "' where Id = '" + GroupId + "'";
			lvarCommand.ExecuteNonQuery();
			lvarConnection.Close ();
		}

		public void RemoveGroups (List<Models.GroupModel> Groups)
		{
			SqliteConnection lvarConnection = GetConnection ();
			SqliteCommand lvarCommand = new SqliteCommand (lvarConnection);

			foreach (Models.GroupModel lvarGroup in Groups) {
				lvarCommand.CommandText = "delete from Groups where Id = " + lvarGroup.mvarId;
				lvarCommand.ExecuteNonQuery ();
				lvarCommand.CommandText = "delete from MembersInGroups where [Group] = " + lvarGroup.mvarId;
				lvarCommand.ExecuteNonQuery ();
				lvarCommand.CommandText = "delete from Payments where [Group] = " + lvarGroup.mvarId;
				lvarCommand.ExecuteNonQuery ();
				lvarCommand.CommandText = "delete from Contributions where [Group] = " + lvarGroup.mvarId;
				lvarCommand.ExecuteNonQuery ();
			}
			lvarConnection.Close ();
		}

		public void AddMemberToGroup(Models.MemberModel Member, Models.GroupModel Group)
		{
			SqliteConnection lvarConnection = GetConnection ();
			SqliteCommand lvarCommand = new SqliteCommand (lvarConnection);

			lvarCommand.CommandText = "insert into MembersInGroups (Group, Member) values (" + Group.mvarId + "," + Member.mvarID + ")";
			lvarCommand.ExecuteNonQuery ();
			lvarConnection.Close ();

		}

        public void ClearGroups()
        {
            SqliteConnection lvarConnection = GetConnection();

            SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "delete from MembersInGroups";
			lvarCommand.ExecuteNonQuery ();
			lvarCommand.CommandText = "delete from Groups";
        	lvarCommand.ExecuteNonQuery();
            lvarConnection.Close();
        }

        public Boolean CheckIfGroupExists(String name)
        {
            SqliteConnection lvarConnection = GetConnection();

            String lvarCommandString = "select * from Groups where Name = '" + name + "'";
            SqliteCommand lvarCommand = new SqliteCommand(lvarCommandString, lvarConnection);
            SqliteDataReader lvarReader = lvarCommand.ExecuteReader();
            Boolean output = lvarReader.HasRows;
            lvarConnection.Close();

            return output;
        }

	//MEMBERS
		public string AddMember(String Name, String Surname, List<String> Phones, List<String> Emails)
		{
			SqliteConnection lvarConnection = GetConnection();

			String lvarInsertString = "insert into Members (Name, Surname) values ('" + Name + "','" + Surname + "')";
			SqliteCommand lvarCommand = new SqliteCommand(lvarInsertString, lvarConnection);
			lvarCommand.ExecuteNonQuery();
			String lvarUserId = GetLastId (lvarConnection);
			foreach (String lvarPhone in Phones) {
				lvarCommand.CommandText = "insert into Phones (Member, Label, Number) values (" + lvarUserId + ",'','" + lvarPhone + "')";
				lvarCommand.ExecuteNonQuery ();
			}
			foreach (String lvarEmail in Emails) {
				lvarCommand.CommandText = "insert into Emails (Member, Label, Address) values (" + lvarUserId + ",'','" + lvarEmail + "')";
				lvarCommand.ExecuteNonQuery ();
			}
			lvarConnection.Close();
			return lvarUserId;
		}

		public Boolean CheckIfMemberExists(String Name, String Surname)
		{
			SqliteConnection lvarConnection = GetConnection();

			String lvarCommandString = "select * from Members where Name = '" + Name + "' and Surname = '" + Surname + "'";
			SqliteCommand lvarCommand = new SqliteCommand(lvarCommandString, lvarConnection);
			SqliteDataReader lvarReader = lvarCommand.ExecuteReader();
			Boolean output = lvarReader.HasRows;
			lvarConnection.Close();

			return output;
		}

		public List<Models.MemberModel> GetMembers()
		{
		//INITIALIZATION
			List<Models.MemberModel> lvarMembers = new List<Models.MemberModel>();
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			SqliteDataReader lvarReader = null;

		//GET MEMBERS DATA
			lvarCommand.CommandText = "SELECT Members.Id, Name, Surname FROM Members";
			lvarReader = lvarCommand.ExecuteReader();
			while(lvarReader.Read())
			{
				Models.MemberModel lvarMember = new Models.MemberModel(lvarReader[0].ToString(), lvarReader[1].ToString(), lvarReader[2].ToString());
				lvarMembers.Add(lvarMember);
			}
			lvarReader.Close ();

		//GET MEMBERS PHONES
			lvarCommand.CommandText = "SELECT * FROM Phones";
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Models.MemberModel lvarMember = lvarMembers.Find (Member => Member.mvarID == lvarReader [1].ToString());
				lvarMember.mvarPhoneNumbers.Add(lvarReader [3].ToString ());
			}
			lvarReader.Close ();

		//GET MEMBERS EMAILS
			lvarCommand.CommandText = "SELECT * FROM Emails";
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Models.MemberModel lvarMember = lvarMembers.Find (Member => Member.mvarID == lvarReader [1].ToString());
				lvarMember.mvarEmailAddresses.Add(lvarReader [3].ToString ());
			}
			lvarReader.Close ();

		//GET MEMBERS CONTRIBUTIONS
			lvarCommand.CommandText = "SELECT * FROM Contributions";
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Models.MemberModel lvarMember = lvarMembers.Find (Member => Member.mvarID == lvarReader [2].ToString());
				lvarMember.AddContribution(lvarReader[0].ToString(), lvarReader[3].ToString(), lvarReader [1].ToString (), Convert.ToDecimal (lvarReader [4]), lvarReader[5].ToString(), Convert.ToDateTime(lvarReader[6]));
			}
			lvarReader.Close ();

		//GET MEMBERS PAYMENTS
			lvarCommand.CommandText = "SELECT * FROM Payments;";
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Models.MemberModel lvarMemberFrom = lvarMembers.Find (Member => Member.mvarID == lvarReader [2].ToString());
				Models.MemberModel lvarMemberTo = lvarMembers.Find (Member => Member.mvarID == lvarReader [3].ToString());

				Models.PaymentModel lvarPayment = new SplitTheBill.Models.PaymentModel (lvarReader[0].ToString(), lvarMemberFrom, lvarMemberTo, lvarReader[1].ToString(), Convert.ToDecimal(lvarReader [4]), Convert.ToBoolean(lvarReader[5]));
				//Models.PaymentModel lvarOwedPayment = new SplitTheBill.Models.PaymentModel (lvarReader[0].ToString(), lvarMemberFrom, null, lvarReader[1].ToString(), Convert.ToDecimal(lvarReader [4]), Convert.ToBoolean(lvarReader[5]));
				//lvarMemberFrom.mvarOwes.Add (lvarOwesPayment);
				//lvarMemberTo.mvarOwed.Add (lvarOwedPayment);
				lvarMemberFrom.mvarPayments.Add(lvarPayment);
				lvarMemberTo.mvarPayments.Add (lvarPayment);
			}
			lvarReader.Close ();
			lvarConnection.Close();
			return lvarMembers;
		}

		public Models.MemberModel GetMember(String MemberId)
		{
			//INITIALIZATION
			Models.MemberModel lvarMember = null;
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			SqliteDataReader lvarReader = null;

			//GET MEMBERS DATA
			lvarCommand.CommandText = "SELECT Members.Id, Name, Surname FROM Members where Id = " + MemberId;
			lvarReader = lvarCommand.ExecuteReader();
			while(lvarReader.Read())
			{
				lvarMember = new Models.MemberModel(lvarReader[0].ToString(), lvarReader[1].ToString(), lvarReader[2].ToString());
			}
			lvarReader.Close ();
			//GET MEMBERS PHONES
			lvarCommand.CommandText = "SELECT * FROM Phones where Member = " + MemberId;
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				lvarMember.mvarPhoneNumbers.Add(lvarReader [3].ToString ());
			}
			lvarReader.Close ();

			//GET MEMBERS EMAILS
			lvarCommand.CommandText = "SELECT * FROM Emails where Member = " + MemberId;
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				lvarMember.mvarEmailAddresses.Add(lvarReader [3].ToString ());
			}
			lvarReader.Close ();

			//GET MEMBERS CONTRIBUTIONS
			lvarCommand.CommandText = "SELECT * FROM Contributions where Member = " + MemberId;
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				lvarMember.AddContribution(lvarReader[0].ToString(), lvarReader[3].ToString(), lvarReader [1].ToString (), Convert.ToDecimal (lvarReader [4]), lvarReader[5].ToString(), Convert.ToDateTime(lvarReader[6]));
			}
			lvarReader.Close ();

			//GET MEMBERS PAYMENTS
			lvarCommand.CommandText = "SELECT * FROM Payments where [From] = " + MemberId + " or [To] = " + MemberId;
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Models.PaymentModel lvarPayment = new SplitTheBill.Models.PaymentModel (lvarReader [0].ToString (), lvarReader [2].ToString(), lvarReader[3].ToString(), lvarReader [1].ToString (), Convert.ToDecimal (lvarReader [4]), Convert.ToBoolean (lvarReader [5]));
				lvarMember.mvarPayments.Add (lvarPayment);
			}
			lvarReader.Close ();
			lvarConnection.Close();
			return lvarMember;
		}

		public List<Models.MemberModel> GetMembersInGroup(String GroupId)
		{
			
		//INITIALIZATION
			List<Models.MemberModel> lvarMembers = new List<Models.MemberModel>();
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			SqliteDataReader lvarReader = null;

		//GET MEMBERS DATA
			lvarCommand.CommandText = "SELECT Members.Id, Name, Surname FROM MembersInGroups join Members on MembersInGroups.Member = Members.Id where [Group] = " + GroupId + " order by Surname asc, Name asc";
			lvarReader = lvarCommand.ExecuteReader();
			while(lvarReader.Read())
			{
				Models.MemberModel lvarMember = new Models.MemberModel(lvarReader[0].ToString(), lvarReader[1].ToString(), lvarReader[2].ToString());
				lvarMembers.Add(lvarMember);
			}
			lvarReader.Close ();
		
		//GET MEMBERS PHONES
			lvarCommand.CommandText = "SELECT * FROM Phones";
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Models.MemberModel lvarMember = lvarMembers.Find (Member => Member.mvarID == lvarReader [1].ToString());
				if(lvarMember != null)
					lvarMember.mvarPhoneNumbers.Add(lvarReader [3].ToString ());
			}
			lvarReader.Close ();
		
		//GET MEMBERS EMAILS
			lvarCommand.CommandText = "SELECT * FROM Emails";
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				String lvarMemberId = lvarReader.GetInt32(1).ToString ();
				Models.MemberModel lvarMember = lvarMembers.Find (Member => Member.mvarID == lvarMemberId);
				if(lvarMember != null)
					lvarMember.mvarEmailAddresses.Add(lvarReader [3].ToString ());
			}
			lvarReader.Close ();

		//GET MEMBERS CONTRIBUTIONS
			lvarCommand.CommandText = "SELECT * FROM Contributions where [Group] = " + GroupId;
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Models.MemberModel lvarMember = lvarMembers.Find (Member => Member.mvarID == lvarReader [2].ToString());
				lvarMember.AddContribution(lvarReader[0].ToString(), lvarReader[3].ToString(), lvarReader [1].ToString (), Convert.ToDecimal (lvarReader [4]), lvarReader[5].ToString(), Convert.ToDateTime(lvarReader[6]));
			}
			lvarReader.Close ();

		//GET MEMBERS PAYMENTS
			lvarCommand.CommandText = "SELECT * FROM Payments where [Group] = " + GroupId;
			lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Models.MemberModel lvarMemberFrom = lvarMembers.Find (Member => Member.mvarID == lvarReader [1].ToString());
				Models.MemberModel lvarMemberTo = lvarMembers.Find (Member => Member.mvarID == lvarReader [2].ToString());
				Models.PaymentModel lvarPayment = new SplitTheBill.Models.PaymentModel (lvarReader[0].ToString(), lvarMemberFrom, lvarMemberTo, lvarReader[1].ToString(), Convert.ToDecimal(lvarReader [4]), Convert.ToBoolean(lvarReader[5].ToString()));
				//Models.PaymentModel lvarOwedPayment = new SplitTheBill.Models.PaymentModel (lvarReader[0].ToString(), lvarMemberFrom, null, lvarReader[1].ToString(), Convert.ToDecimal(lvarReader [4]), Convert.ToBoolean(lvarReader[5]));
				//lvarMemberFrom.mvarOwes.Add (lvarOwesPayment);
				//lvarMemberTo.mvarOwed.Add (lvarOwedPayment);
				if(lvarMemberFrom != null)
					lvarMemberFrom.mvarPayments.Add(lvarPayment);
				if(lvarMemberTo != null)
					lvarMemberTo.mvarPayments.Add (lvarPayment);
			}
			lvarReader.Close ();
			lvarConnection.Close();
			return lvarMembers;
		}
			
		//PAYMENTS

		public String AddPayment(Models.GroupModel Group, Models.MemberModel From, Models.MemberModel To, Decimal Amount)
		{
			SqliteConnection lvarConnection = GetConnection();

			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "insert into Payments ([Group],[From],[To],Amount,Settled) values (" + Group.mvarId + "," + From.mvarID + "," + To.mvarID + "," + Amount + ",0)";
			lvarCommand.ExecuteNonQuery ();
			String lvarPaymentId = GetLastId (lvarConnection);
			lvarConnection.Close ();
			return lvarPaymentId;
		}

		public void ClearPaymentsInGroup(String GroupId)
		{
			SqliteConnection lvarConnection = GetConnection();

			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "delete from Payments where [Group] = " + GroupId;
			lvarCommand.ExecuteNonQuery ();
			lvarConnection.Close ();
		}

		public void ChangePaymentSettled(String PaymentId, Boolean State)
		{
			SqliteConnection lvarConnection = GetConnection();

			String lvarStateString = "";

			if (State)
				lvarStateString = "1";
			else
				lvarStateString = "0";
			
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "update Payments set Settled = " + lvarStateString + " where Id = " + PaymentId;
			lvarCommand.ExecuteNonQuery ();
		}

		public List<Models.PaymentModel> GetPaymentsInGroup(String GroupId)
		{
			List<Models.PaymentModel> lvarPayments = new List<SplitTheBill.Models.PaymentModel> ();

			SqliteConnection lvarConnection = GetConnection();

			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "select Id, [From], [To], [Group], Amount, Settled from Payments where [Group] = " + GroupId;
			SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
			while (lvarReader.Read ()) {
				Models.PaymentModel lvarPayment = new Models.PaymentModel (lvarReader [0].ToString (), lvarReader [1].ToString (), lvarReader [2].ToString (), lvarReader [3].ToString (), Convert.ToDecimal (lvarReader [4]), Convert.ToBoolean (lvarReader [5]));
				lvarPayments.Add (lvarPayment);
			}

			return lvarPayments;
		}
		public String PerformContribution(String GroupId, String MemberId, String Name, Decimal Amount, String Currency, DateTime Date)
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "delete from Payments where [Group] = " + GroupId;
			lvarCommand.ExecuteNonQuery ();
			lvarCommand.CommandText = "insert into Contributions ([Group], Member, Name, Amount, Currency, Date) values (" + GroupId + "," + MemberId + ",'" + Name.ToString() + "'," + Amount.ToString().Replace(",",".") + ",'" + Currency + "','" + Date.ToString("yyyy-MM-dd HH:mm:ss") + "')";
			lvarCommand.ExecuteNonQuery ();
			String lvarContId = GetLastId (lvarConnection);
			lvarConnection.Close ();

			return lvarContId;
		}

		public void RemoveContributions(List<Models.ContributionModel> Contributions)
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			foreach (Models.ContributionModel lvarContribution in Contributions) {
				lvarCommand.CommandText = "delete from Contributions where Id = " + lvarContribution.mvarId;
				lvarCommand.ExecuteNonQuery ();
			}
			lvarConnection.Close ();
		}


		//CURRENCY

		public void ClearCurrencies()
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "delete from ExchangeRates";
			lvarCommand.ExecuteNonQuery ();

			lvarConnection.Close ();
		}

		public String GetLastUpdateDate()
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "select Value from AppSettings where Name = 'LastCurrencyUpdateDate'";
			SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
			String lvarOutput = "";
			while (lvarReader.Read ()) {
				lvarOutput = lvarReader [0].ToString ();
			}
			lvarReader.Close ();
			lvarConnection.Close ();
			return lvarOutput;
		}

		public void UpdateLastUpdateDate(DateTime Date)
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "update AppSettings set Value = '" + Date.ToString("yyyy-MM-dd") + "' where Name = 'LastCurrencyUpdateDate'";
			lvarCommand.ExecuteNonQuery ();

			lvarConnection.Close ();
		}

		public void AddCurrency(string Symbol, decimal Rate, String Date)
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "insert into ExchangeRates (Symbol, Rate, Date, Change) values ('" + Symbol + "'," + Rate + ",'" + Date + "',0)";
			lvarCommand.ExecuteNonQuery ();

			lvarConnection.Close ();
		}

		public void UpdateCurrency (string Symbol, decimal Rate, String Date)
		{
			Decimal lvarCurrentRate = GetCurrentExchangeRate (Symbol);
			Decimal lvarChange = 0;
			if (lvarCurrentRate != 0)
				lvarChange = Rate - lvarCurrentRate;

			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "update ExchangeRates set Rate = " + Rate + ", Date = '" + Date + "', Change = " + lvarChange + " where Symbol = '" + Symbol + "'";
			lvarCommand.ExecuteNonQuery ();

			lvarConnection.Close ();
		}

		public void UpdateCurrencyToNewBase (string Symbol, decimal Rate, String Date)
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "update ExchangeRates set Rate = " + Rate + ", Date = '" + Date + "'" + " where Symbol = '" + Symbol + "'";
			lvarCommand.ExecuteNonQuery ();

			lvarConnection.Close ();
		}

		public List<string> GetCurrencySymbols()
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "select Symbol from ExchangeRates";
			SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
			List<string> lvarOutput = new List<string> ();

			while (lvarReader.Read ()) {
				lvarOutput.Add (lvarReader [0].ToString());
			}
			lvarOutput.Add (ExchangeRateBase);
			lvarOutput.Sort ();
			lvarReader.Close ();
			lvarConnection.Close();

			return lvarOutput;
		}

		public decimal GetCurrentExchangeRate(string Symbol)
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "select Rate from ExchangeRates where Symbol = '" + Symbol + "'";
			SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
			decimal lvarOutput = 0; 
		
			while (lvarReader.Read ()) {
				if(lvarReader[0] != DBNull.Value)
					lvarOutput = Convert.ToDecimal (lvarReader [0]);
			}

			lvarReader.Close ();
			lvarConnection.Close();
			return lvarOutput;
		}

		public List<CurrencyModel> GetCurrencies()
		{
			SqliteConnection lvarConnection = GetConnection();
			SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
			lvarCommand.CommandText = "select Id, Symbol, Rate, Date, Change from ExchangeRates where Date = (select max(Date) from ExchangeRates)";
			SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
			List<CurrencyModel> lvarOutput = new List<CurrencyModel>(); 

			while (lvarReader.Read ()) {
				lvarOutput.Add (new CurrencyModel (lvarReader [0].ToString(), lvarReader [1].ToString(), Convert.ToDecimal(lvarReader [2]), Convert.ToDateTime(lvarReader [3].ToString()),Convert.ToDecimal(lvarReader[4])));
			}
				
			lvarReader.Close ();
			lvarConnection.Close();
			return lvarOutput;
		}

		public Boolean LastUpdateFailed
		{
			get { SqliteConnection lvarConnection = GetConnection();
				SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
				lvarCommand.CommandText = "select Value from AppSettings where Name = 'LastUpdateFailed'";
				SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
				Boolean lvarOutput = false; 

				while (lvarReader.Read ()) {
					lvarOutput = Convert.ToBoolean (lvarReader [0]);
				}

				lvarReader.Close ();
				lvarConnection.Close();
				return lvarOutput; }

			set { SqliteConnection lvarConnection = GetConnection();
				SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
				lvarCommand.CommandText = "update AppSettings set Value = '" + value.ToString() + "' where Name = 'LastUpdateFailed'";
				lvarCommand.ExecuteNonQuery ();
				lvarConnection.Close(); }
		}

		public String ExchangeRateBase
		{
			get { 	SqliteConnection lvarConnection = GetConnection();
				SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
				lvarCommand.CommandText = "select Value from AppSettings where Name = 'ExchangeRateBase'";
				SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
				String lvarOutput = "";
				while (lvarReader.Read ()) {
					lvarOutput = lvarReader [0].ToString ();
				}

				lvarReader.Close ();
				lvarConnection.Close();

				return lvarOutput; }
			
			set { SqliteConnection lvarConnection = GetConnection();
				SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
				lvarCommand.CommandText = "update AppSettings set Value = '" + value.ToString() + "' where Name = 'ExchangeRateBase'";
				lvarCommand.ExecuteNonQuery ();
				lvarConnection.Close(); }
		}

		public Boolean InitialSetupDone
		{
			get { SqliteConnection lvarConnection = GetConnection();
				SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
				lvarCommand.CommandText = "select Value from AppSettings where Name = 'InitialSetupDone'";
				SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
				Boolean lvarOutput = false; 

				while (lvarReader.Read ()) {
					lvarOutput = Convert.ToBoolean (lvarReader [0]);
				}

				lvarReader.Close ();
				lvarConnection.Close();
				return lvarOutput; }

			set { SqliteConnection lvarConnection = GetConnection();
				SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
				lvarCommand.CommandText = "update AppSettings set Value = '" + value.ToString() + "' where Name = 'InitialSetupDone'";
				lvarCommand.ExecuteNonQuery ();
				lvarConnection.Close(); }
		}

		public Boolean MainScreenDataChanged
		{
			get { SqliteConnection lvarConnection = GetConnection();
				SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
				lvarCommand.CommandText = "select Value from AppSettings where Name = 'MainScreenDataChanged'";
				SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
				Boolean lvarOutput = false; 

				while (lvarReader.Read ()) {
					lvarOutput = Convert.ToBoolean (lvarReader [0]);
				}

				lvarReader.Close ();
				lvarConnection.Close();
				return lvarOutput; }

			set { SqliteConnection lvarConnection = GetConnection();
				SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
				lvarCommand.CommandText = "update AppSettings set Value = '" + value.ToString() + "' where Name = 'MainScreenDataChanged'";
				lvarCommand.ExecuteNonQuery ();
				lvarConnection.Close(); }
		}

		public Boolean BaseCurrencyChanged
		{
			get { SqliteConnection lvarConnection = GetConnection();
				SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
				lvarCommand.CommandText = "select Value from AppSettings where Name = 'BaseCurrencyChanged'";
				SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
				Boolean lvarOutput = false; 

				while (lvarReader.Read ()) {
					lvarOutput = Convert.ToBoolean (lvarReader [0]);
				}

				lvarReader.Close ();
				lvarConnection.Close();
				return lvarOutput; }

			set { SqliteConnection lvarConnection = GetConnection();
				SqliteCommand lvarCommand = new SqliteCommand(lvarConnection);
				lvarCommand.CommandText = "update AppSettings set Value = '" + value.ToString() + "' where Name = 'BaseCurrencyChanged'";
				lvarCommand.ExecuteNonQuery ();
				lvarConnection.Close(); }
		}


		//MISC
		public String GetLastId(SqliteConnection Connection)
		{
			SqliteConnection lvarConnection = Connection;

			String lvarCommandString = "select last_insert_rowid()";
			SqliteCommand lvarCommand = new SqliteCommand(lvarCommandString, lvarConnection);
			SqliteDataReader lvarReader = lvarCommand.ExecuteReader ();
			String lvarId = "";
			while (lvarReader.Read ()) {
				lvarId = lvarReader [0].ToString ();
			}
			lvarReader.Close ();
			return lvarId;
		}
			
    }
}
