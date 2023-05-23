using Core.Persistence.Repositories;

namespace Domain.Entities
{
    public class DaylightSavingTime:Entity
    {
 
        public string? CityName { get; set; }

        public string? Start { get; set; }

        public string? End { get; set; }

        public int? StartDiff { get; set; }

        public int EndDiff { get; set; }

        public int CityId { get; set; }
        public virtual City City { get; set; } = null!;


        public string? CountryName { get; set; }

        public DaylightSavingTime()
        {
        }

        public DaylightSavingTime(int id, string? cityName, string? start, string? end, int? startDiff, int endDiff, int cityId, City city, string? countryName)
        {
            Id = id;
            CityName = cityName;
            Start = start;
            End = end;
            StartDiff = startDiff;
            EndDiff = endDiff;
            CityId = cityId;
            City = city;
            CountryName = countryName;
        }
    }
}