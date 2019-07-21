using System;

namespace SplitTheBill
{
	public class CurrencyModel
	{
		public string mvarId;
		public string mvarSymbol;
		public decimal mvarRate;
		public DateTime mvarDate;
		public decimal mvarChange;

		public CurrencyModel (string Symbol, decimal Rate, DateTime Date, decimal Change)
		{
			mvarSymbol = Symbol;
			mvarRate = Rate;
			mvarDate = Date;
			mvarChange = Change;
		}

		public CurrencyModel (string Id, string Symbol, decimal Rate, DateTime Date, decimal Change)
		{
			mvarId = Id;
			mvarSymbol = Symbol;
			mvarRate = Rate;
			mvarDate = Date;
			mvarChange = Change;
		}
	}
}

