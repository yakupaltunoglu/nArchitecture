using System;
namespace TimeAndDate.Services.DataTypes.Astro
{
	[Flags]
	public enum AstronomyEventClass
	{
		/// <summary>
		/// Combination of all known classes.
		/// </summary>
		All = 1,
		/// <summary>
		/// Day length. 
		/// </summary>
		DayLength = 1 << 1,
		/// <summary>
		/// Meridian (Noon, highest point) and Anti-Meridian (lowest point) events.
		/// </summary>
		Meridian = 1 << 2,
		/// <summary>
		/// Moon phase events. Additionally to the phase events (only occurring on 
		/// four days per lunar month), an additional attribute for the current moon 
		/// phase is reported for every day.
		/// </summary>
		Phase = 1 << 3,
		/// <summary>
		/// Set and rise events. Event times take atmospheric refraction into account.
		/// </summary>
		SetRise = 1 << 4,
		/// <summary>
		/// Combination of all 3 twilight classes.
		/// </summary>
		AllTwilights = 1 << 5,
		/// <summary>
		/// Civil twilight (-6°).
		/// </summary>
		CivilTwilight = 1 << 6,
		/// <summary>
		/// Nautical twilight (-12°).
		/// </summary>
		NauticalTwilight = 1 << 7,
		/// <summary>
		/// Astronomical twilight (-18°).
		/// </summary>
		AstronomicalTwilight = 1 << 8		
	}
}

