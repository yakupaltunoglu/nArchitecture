using Core.Persistence.Repositories;

namespace Domain.Entities
{
    public class City: Entity
    {
        public string? Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double LongitudeDelta { get; set; }

        public double GreenwichDelta { get; set; }

        public double Offset { get; set; }

        public double StandartMeridian { get; set; }

        public double LongitudeDeltaFromService { get; set; }

        public string? GreenwichtenZamanCinsindenFarki { get; set; }

        public string? IhlasTemkin { get; set; }

        public string? SehirKisa { get; set; }

        public string? StandartBoylamQuery { get; set; }
        public DateTime? CreatedOn { get; set; }
        public virtual ICollection<PrayTime> PrayTimes { get; } = new List<PrayTime>();
        public virtual ICollection<DaylightSavingTime> DaylightSavingTimes { get; } = new List<DaylightSavingTime>();

        public string? CountryName { get; set; }

        public City()
        {
        }

        public City(int id, string? name, double latitude, double longitude, double longitudeDelta, double greenwichDelta, double offset, double standartMeridian, double longitudeDeltaFromService, string? greenwichtenZamanCinsindenFarki, string? ıhlasTemkin, string? sehirKisa, string? standartBoylamQuery, DateTime? createdOn, ICollection<PrayTime> prayTimes, ICollection<DaylightSavingTime> daylightSavingTimes, string? countryName) : this()
        {
            Id = id;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            LongitudeDelta = longitudeDelta;
            GreenwichDelta = greenwichDelta;
            Offset = offset;
            StandartMeridian = standartMeridian;
            LongitudeDeltaFromService = longitudeDeltaFromService;
            GreenwichtenZamanCinsindenFarki = greenwichtenZamanCinsindenFarki;
            IhlasTemkin = ıhlasTemkin;
            SehirKisa = sehirKisa;
            StandartBoylamQuery = standartBoylamQuery;
            CreatedOn = createdOn;
            PrayTimes = prayTimes;
            DaylightSavingTimes = daylightSavingTimes;
            CountryName = countryName;
        }
    }
}