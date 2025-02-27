using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Xml;
using TimeAndDate.Services.Common;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.DataTypes.Time;

namespace TimeAndDate.Services.DataTypes.Holiday
{
	public class Holiday
	{
		/// <summary>
		/// Identifier for the holiday definition. Please note that this id 
		/// is not unique, not even with a single year – the same holiday 
		/// event may be returned multiple time because it is observed on a 
		/// different day, or because it is scheduled in a different calendar 
		/// (Hebrew or Muslim calendar) and happens multiple times within a 
		/// Gregorian year. Use the Uid field for purposes where you need a 
		/// unique identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public int Id { get; set; }
		
		/// <summary>
		/// Id for the shown holiday instance. The id is designed to be unique 
		/// across all holiday instances generated by the timeanddate.com API 
		/// services and respects different calendars and other reasons that 
		/// may cause events to occurs multiple times within one Gregorian year.
		///
		/// Example: 0007d600000007db
		/// </summary>
		/// <value>
		/// The Uid.
		/// </value>
		public string Uid { get; set; }
		
		/// <summary>
		/// Holiday/Observance name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; set; }
		
		/// <summary>
		/// Date/time of the holiday instance. Most holidays do have a specific 
		/// time – in this case the time components will be skipped. Some 
		/// special events like equinoxes and solstices include the exact time 
		/// of the event as well, in this case the timestamp will be in local 
		/// time zone (including time zone data) (countries with multiple 
		/// timezones: local time in capital city). 
		/// </summary>
		/// <value>
		/// The date.
		/// </value>
		public TADTime Date { get; set; } 
		
		/// <summary>
		/// Further information about the specific holiday. The URL points to 
		/// the timeanddate.com web page.
		/// 
		/// Example: http://www.timeanddate.com/holidays/us/new-year-day
		/// </summary>
		/// <value>
		/// The URL.
		/// </value>
		public Uri Url { get; set; }
		
		/// <summary>
		/// Country of the holiday instance.
		/// </summary>
		/// <value>
		/// The country.
		/// </value>
		public Country Country { get; set; }
		
		/// <summary>
		/// Summary of locations where this holiday instance is valid. Element is 
		/// only present if the holiday instance does not affect the whole country.
		/// </summary>
		/// <value>
		/// The locations.
		/// </value>
		public string Locations { get; set; }
		
		/// <summary>
		/// States/subdivisions that are affected by this holiday instance. This 
		/// element is only present if the holiday instance is not valid in the 
		/// whole country.
		/// </summary>
		/// <value>
		/// The states.
		/// </value>
		public List<HolidayState> States { get; set; }
		
		/// <summary>
		/// A short description of the holiday instance.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Description { get; set; }
		
		/// <summary>
		/// Classification of the holiday. Most days have only one classification, 
		/// bust some have multiple types associated. This happens e.g. in 
		/// conjunction with religious days that also are flag days.
		///
		/// Example: National Holiday
		/// </summary>
		/// <value>
		/// The types.
		/// </value>
		public List<string> Types { get; set; }
				
		private Holiday ()
		{
			Types = new List<string> ();
			States = new List<HolidayState> ();
		}
		
		public static explicit operator Holiday (XmlNode node)
		{
			var model = new Holiday ();
			var id = node.Attributes ["id"];			
			var url = node.Attributes ["url"];
			var name = node.SelectSingleNode ("name");
			var locations = node.SelectSingleNode ("locations");
			var uid = node.SelectSingleNode ("uid");
			var country = node.SelectSingleNode ("country");
			var desc = node.SelectSingleNode ("oneliner");
			var types = node.SelectSingleNode ("types");
			var states = node.SelectSingleNode ("states");
			var date = node.SelectSingleNode ("date");

			if (id != null)
				model.Id = Int32.Parse (id.InnerText);
			
			if (uid != null)
				model.Uid = uid.InnerText;
			
			if (url != null && !String.IsNullOrEmpty (url.InnerText))
				model.Url = new Uri (url.InnerText);
			
			if (country != null)
				model.Country = (Country)country;
			
			if (desc != null)
				model.Description = desc.InnerText;
			
			if (date != null)
			{
				model.Date = (TADTime)date;
			}
				
			if (name != null)
				model.Name = name.InnerText;
			
			if (locations != null)
				model.Locations = locations.InnerText;
			
			if (types != null)
			{
				foreach (XmlNode child in types.ChildNodes)
					model.Types.Add (child.InnerText);
			}
			
			if (states != null)
				foreach (XmlNode child in states.ChildNodes)
					model.States.Add ((HolidayState)child);	
			
			return model;
		}			
	}
}

