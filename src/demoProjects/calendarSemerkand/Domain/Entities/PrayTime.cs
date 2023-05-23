namespace Domain.Entities
{
    public class PrayTime
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public string? Imsak { get; set; }

        public string? Fajr { get; set; } //Sabah

        public string? Tulu { get; set; } //Güneş

        public string? Israk { get; set; }

        public string? Zuhr { get; set; } //Öğle

        public string? Asr { get; set; } //İkindi

        public string? Maghrib { get; set; } //Akşam

        public string? Isha { get; set; } //Yatsi

        public string? CityName { get; set; }

        public bool? IsDeleted { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; } = null!;

        public string? CountryName { get; set; }
        public PrayTime()
        {
        }

        public PrayTime(long id, DateTime date, string? imsak, string? fajr, string? tulu, string? ısrak, string? zuhr, string? asr, string? maghrib, string? ısha, string? cityName, bool? ısDeleted, int cityId, City city, string? countryName)
        {
            Id = id;
            Date = date;
            Imsak = imsak;
            Fajr = fajr;
            Tulu = tulu;
            Israk = ısrak;
            Zuhr = zuhr;
            Asr = asr;
            Maghrib = maghrib;
            Isha = ısha;
            CityName = cityName;
            IsDeleted = ısDeleted;
            CityId = cityId;
            City = city;
            CountryName = countryName;
        }
    }
}