using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace PaJaMa.Common
{
	public class Globalization
	{
		public static List<State> GetStates()
		{
			List<State> states = new List<State>();

			states.AddRange(new State[] {
			new State() { Abbreviation = "AL", StateName = "Alabama", Sequence = 1 },
			new State() { Abbreviation = "MT", StateName = "Montana", Sequence = 1 },
			new State() { Abbreviation = "AK", StateName = "Alaska", Sequence = 1 },
			new State() { Abbreviation = "NE", StateName = "Nebraska", Sequence = 1 },
			new State() { Abbreviation = "AZ", StateName = "Arizona", Sequence = 1 },
			new State() { Abbreviation = "NV", StateName = "Nevada", Sequence = 1 },
			new State() { Abbreviation = "AR", StateName = "Arkansas", Sequence = 1 },
			new State() { Abbreviation = "NH", StateName = "New Hampshire", Sequence = 1 },
			new State() { Abbreviation = "CA", StateName = "California", Sequence = 1 },
			new State() { Abbreviation = "NJ", StateName = "New Jersey", Sequence = 1 },
			new State() { Abbreviation = "CO", StateName = "Colorado", Sequence = 1 },
			new State() { Abbreviation = "NM", StateName = "New Mexico", Sequence = 1 },
			new State() { Abbreviation = "CT", StateName = "Connecticut", Sequence = 1 },
			new State() { Abbreviation = "NY", StateName = "New York", Sequence = 1 },
			new State() { Abbreviation = "DE", StateName = "Delaware", Sequence = 1 },
			new State() { Abbreviation = "NC", StateName = "North Carolina", Sequence = 1 },
			new State() { Abbreviation = "FL", StateName = "Florida", Sequence = 1 },
			new State() { Abbreviation = "ND", StateName = "North Dakota", Sequence = 1 },
			new State() { Abbreviation = "GA", StateName = "Georgia", Sequence = 1 },
			new State() { Abbreviation = "OH", StateName = "Ohio", Sequence = 1 },
			new State() { Abbreviation = "HI", StateName = "Hawaii", Sequence = 1 },
			new State() { Abbreviation = "OK", StateName = "Oklahoma", Sequence = 1 },
			new State() { Abbreviation = "ID", StateName = "Idaho", Sequence = 1 },
			new State() { Abbreviation = "OR", StateName = "Oregon", Sequence = 1 },
			new State() { Abbreviation = "IL", StateName = "Illinois", Sequence = 1 },
			new State() { Abbreviation = "PA", StateName = "Pennsylvania", Sequence = 1 },
			new State() { Abbreviation = "IN", StateName = "Indiana", Sequence = 1 },
			new State() { Abbreviation = "RI", StateName = "Rhode Island", Sequence = 1 },
			new State() { Abbreviation = "IA", StateName = "Iowa", Sequence = 1 },
			new State() { Abbreviation = "SC", StateName = "South Carolina", Sequence = 1 },
			new State() { Abbreviation = "KS", StateName = "Kansas", Sequence = 1 },
			new State() { Abbreviation = "SD", StateName = "South Dakota", Sequence = 1 },
			new State() { Abbreviation = "KY", StateName = "Kentucky", Sequence = 1 },
			new State() { Abbreviation = "TN", StateName = "Tennesee", Sequence = 1 },
			new State() { Abbreviation = "LA", StateName = "Louisiana", Sequence = 1 },
			new State() { Abbreviation = "TX", StateName = "Texas", Sequence = 1 },
			new State() { Abbreviation = "ME", StateName = "Maine", Sequence = 1 },
			new State() { Abbreviation = "UT", StateName = "Utah", Sequence = 1 },
			new State() { Abbreviation = "MD", StateName = "Maryland", Sequence = 1 },
			new State() { Abbreviation = "VT", StateName = "Vermont", Sequence = 1 },
			new State() { Abbreviation = "MA", StateName = "Massachusetts", Sequence = 1 },
			new State() { Abbreviation = "VA", StateName = "Virginia", Sequence = 1 },
			new State() { Abbreviation = "MI", StateName = "Michigan", Sequence = 1 },
			new State() { Abbreviation = "WA", StateName = "Washington", Sequence = 1 },
			new State() { Abbreviation = "DC", StateName = "Washington D.C.", Sequence = 1 },
			new State() { Abbreviation = "MN", StateName = "Minnesota", Sequence = 1 },
			new State() { Abbreviation = "WV", StateName = "West Virginia", Sequence = 1 },
			new State() { Abbreviation = "MS", StateName = "Mississippi", Sequence = 1 },
			new State() { Abbreviation = "WI", StateName = "Wisconsin", Sequence = 1 },
			new State() { Abbreviation = "MO", StateName = "Missouri", Sequence = 1 },
			new State() { Abbreviation = "WY", StateName = "Wyoming", Sequence = 1 },
			new State() { Abbreviation = "AB", StateName = "Alberta", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "BC", StateName = "British Columbia", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "MB", StateName = "Manitoba", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "NB", StateName = "New Brunswick", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "NL", StateName = "Newfoundland and Labrador", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "NT", StateName = "Northwest Territories", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "NS", StateName = "Nova Scotia", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "NU", StateName = "Nunavut", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "ON", StateName = "Ontario", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "PE", StateName = "Prince Edward Island", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "QC", StateName = "Quebec", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "SK", StateName = "Saskatchewan", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "YT", StateName = "Yukon", Country = "Canada", Sequence = 3 },
			new State() { Abbreviation = "PR", StateName = "Puerto Rico", Country = "U.S.A.", Sequence = 2 } // Push Puerto Rico to bottom of US, above Canada. 'Murica.
			});
			return states.OrderBy(s => s.Sequence).ThenBy(s => s.Abbreviation).ToList();
		}

		public static int GetWeekNumber(DateTime date)
		{
			DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
			Calendar cal = dfi.Calendar;
			return cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
		}

		public class State
		{
			public int Sequence { get; set; }
			public string Abbreviation { get; set; }
			public string StateName { get; set; }
			public string Country { get; set; }

			public State()
			{
				Country = "U.S.A.";
			}
			public string StateFull 
			{ 
				get
				{
				if (Country == "U.S.A.") // No need to show the Country for US States; looks too jumbled in text.
						return StateName + " (" + Abbreviation + ")";
					else
						return StateName + " (" + Abbreviation + ") - " + Country;
				}
			}
			//public override string ToString()
			//{
			//    if (Country == "U.S.A.") // No need to show the Country for US States; looks too jumbled in text.
			//        return StateName + " (" + Abbreviation + ")";
			//    else
			//        return StateName + " (" + Abbreviation + ") - " + Country;
			//}
		}
	}
}