using Core.Persistence.Repositories;

namespace Domain.Entities
{
    public class Time: Entity
    {
        public string Coordinate { get; set; }

        public string Abbrevation { get; set; }

        public double? BasicOffset { get; set; }

        public double? DstOffset { get; set; }

        public double? TotalOffset { get; set; }

        public string? F6 { get; set; }

        public string OldLocalTime { get; set; }
        public string NewLocalTime { get; set; }
        public double? NewTotalOffset { get; set; }

        public int? CityId { get; set; }

        public string OldLocalTime1 { get; set; }
        public string NewLocalTime1 { get; set; }
        public double? NewTotalOffset1 { get; set; }
        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string? CityName { get; set; }
        public string? CountryName { get; set; }
        public virtual City City { get; set; } = null!;
        public Time()
        {
        }

        public Time(string coordinate, string abbrevation, double? basicOffset, double? dstOffset, double? totalOffset, string? f6, string oldLocalTime, string newLocalTime, double? newTotalOffset, int? cityId, string oldLocalTime1, string newLocalTime1, double? newTotalOffset1, decimal? latitude, decimal? longitude, string? cityName, string? countryName, City city)
        {
            Coordinate = coordinate;
            Abbrevation = abbrevation;
            BasicOffset = basicOffset;
            DstOffset = dstOffset;
            TotalOffset = totalOffset;
            F6 = f6;
            OldLocalTime = oldLocalTime;
            NewLocalTime = newLocalTime;
            NewTotalOffset = newTotalOffset;
            CityId = cityId;
            OldLocalTime1 = oldLocalTime1;
            NewLocalTime1 = newLocalTime1;
            NewTotalOffset1 = newTotalOffset1;
            Latitude = latitude;
            Longitude = longitude;
            CityName = cityName;
            CountryName = countryName;
            City = city;
        }
    }
}