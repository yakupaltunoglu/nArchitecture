using System;
using System.Linq;
using Domain.Entities;
using Persistence.Contexts;
using static System.Math;


//http://praytimes.org/calculation
//http://praytimes.org/code/git/?a=viewblob&p=PrayTimes&h=cff3aa7fbc9ad958c5bfbb12fd64e5e7b9e26fb4&hb=HEAD&f=v1/csharp/PrayTime.cs
namespace Persistence.Helpers
{
    public class PrayTime2
    {
       

        private double derece_i;
        private double derece_s;
        private double derece_y;

        private double b4_i;  // imsak
        private double b4_s;  // sabah
        private double b4_g;  // gunes
        private double b4_is; // israk
        private double b4_o;  // ogle
        private double b4_k;  // ikindi
        private double b4_a;  // aksam
        private double b4_y;  // yatsi

        private double d8_i;
        private double d8_s;
        private double d8_g;
        private double d8_is;
        private double d8_o;
        private double d8_k;
        private double d8_a;
        private double d8_y;

        private City city;
        private double imsak, sabah, gunes, israk, ogle, ikindi, aksam, yatsi;
        private BaseDbContext _db;
        public PrayTime2(BaseDbContext db)
        {
            _db = db;
        }

        public void Calculate(int year, string cityName)
        {
            DateTimeHelper dateTimeHelper = new DateTimeHelper(_db);
            city = _db.Cities.Where(x => x.Name == cityName).FirstOrDefault();
            if (city == null)
                return;

            // reset internal values
            derece_i = city.Latitude < 45 && city.Latitude > -45 ? -18 : -19;
            derece_s = -17;
            derece_y = -17;
            //derece_i = -18;
            //derece_y = -16;
            var daycount = DateTime.IsLeapYear(year) ? 366 : 365;
            var date = new DateTime(year, 1, 1);
            for (var i = 0; i < daycount; i++)
            {

                // vakitlerin zaman_denklemi(b4) ve gunesin_egimi(d8) hesapliyoruz
                (b4_i, d8_i) = dateTimeHelper.get_b4_d8__(dateTimeHelper.ConvertToJulian(date.AddHours(6)), 1, city.Latitude, city.Longitude, city.StandartMeridian);
                (b4_g, d8_g) = dateTimeHelper.get_b4_d8__(dateTimeHelper.ConvertToJulian(date.AddHours(8)), 1, city.Latitude, city.Longitude, city.StandartMeridian);
                (b4_is, d8_is) = dateTimeHelper.get_b4_d8__(dateTimeHelper.ConvertToJulian(date.AddHours(9)), 1, city.Latitude, city.Longitude, city.StandartMeridian);
                (b4_o, d8_o) = dateTimeHelper.get_b4_d8__(dateTimeHelper.ConvertToJulian(date.AddHours(12)), 1, city.Latitude, city.Longitude, city.StandartMeridian);
                (b4_k, d8_k) = dateTimeHelper.get_b4_d8__(dateTimeHelper.ConvertToJulian(date.AddHours(14)), 1, city.Latitude, city.Longitude, city.StandartMeridian);
                (b4_a, d8_a) = dateTimeHelper.get_b4_d8__(dateTimeHelper.ConvertToJulian(date.AddHours(16)), 1, city.Latitude, city.Longitude, city.StandartMeridian);
                (b4_y, d8_y) = dateTimeHelper.get_b4_d8__(dateTimeHelper.ConvertToJulian(date.AddHours(18)), 1, city.Latitude, city.Longitude, city.StandartMeridian);

                IlkHesap(date);

                // update date
                date = date.AddDays(1);
            }
        }

        private void IlkHesap(DateTime date)
        {
            imsak = (12 - ((Acos((Sin(derece_i * PI / 180) - Sin(city.Latitude * PI / 180) * Sin(d8_i * PI / 180)) / (Cos(city.Latitude * PI / 180) * Cos(d8_i * PI / 180))) * 180 / PI) / 15) + city.LongitudeDelta - b4_i) / 24;
            sabah = (12 - ((Acos((Sin(derece_s * PI / 180) - Sin(city.Latitude * PI / 180) * Sin(d8_i * PI / 180)) / (Cos(city.Latitude * PI / 180) * Cos(d8_i * PI / 180))) * 180 / PI) / 15) + city.LongitudeDelta - b4_i) / 24;
            gunes = (12 - ((Acos((Sin(-0.833 * PI / 180) - Sin(city.Latitude * PI / 180) * Sin(d8_g * PI / 180)) / (Cos(city.Latitude * PI / 180) * Cos(d8_g * PI / 180))) * 180 / PI) / 15) + city.LongitudeDelta - b4_g) / 24;
            israk = (12 - ((Acos((Sin(5 * PI / 180) - Sin(city.Latitude * PI / 180) * Sin(d8_is * PI / 180)) / (Cos(city.Latitude * PI / 180) * Cos(d8_is * PI / 180))) * 180 / PI) / 15) + city.LongitudeDelta - b4_is) / 24;
            ogle = (12 - b4_o + city.LongitudeDelta) / 24;
            ikindi = (12 + ((Acos((Sin((90 - (Atan((Tan((Abs(city.Latitude - d8_k)) * PI / 180) + 1)) * 180 / PI)) * PI / 180) - (Sin(city.Latitude * PI / 180) * Sin(d8_k * PI / 180))) / (Cos(city.Latitude * PI / 180) * Cos(d8_k * PI / 180))) * 180 / PI) / 15) + city.LongitudeDelta - b4_k) / 24;
            aksam = (12 + ((Acos((Sin(-0.833 * PI / 180) - Sin(city.Latitude * PI / 180) * Sin(d8_a * PI / 180)) / (Cos(city.Latitude * PI / 180) * Cos(d8_a * PI / 180))) * 180 / PI) / 15) + city.LongitudeDelta - b4_a) / 24;
            yatsi = (12 + ((Acos((Sin(derece_y * PI / 180) - (Sin(city.Latitude * PI / 180) * Sin(d8_y * PI / 180))) / (Cos(city.Latitude * PI / 180) * Cos(d8_y * PI / 180))) * 180 / PI) / 15) + city.LongitudeDelta - b4_y) / 24;
        }

    }
}