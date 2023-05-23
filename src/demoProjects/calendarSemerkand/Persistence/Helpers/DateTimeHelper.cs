using System;
using System.Globalization;
using System.Linq;
using Persistence.Contexts;

namespace Persistence.Helpers
{
    public class DateTimeHelper
    {
        private BaseDbContext _db;
        public DateTimeHelper(BaseDbContext db) 
        {
            _db = db;
        }
       
        public double ConvertToJulian(DateTime date)
        {
            return date.ToOADate() + 2415018.5;
        }
        public string ToJulianString(string date)
        {
            //var date2 = Convert.ToDateTime(date, new CultureInfo("en-US"));
            var date2 = Convert.ToDateTime(date, new CultureInfo("tr-TR"));
            return ConvertToJulian(date2).ToString(CultureInfo.InvariantCulture);
        }
        // Farkın sadece o tarihlerdeki Güneş değerlerinden olan zaman denkleminden kaynaklanıp kaynaklanmadığını teyit edebilir miyiz?
        // bu degerleri hep greenwiche gore hesapliyoruz
        // en1: simdilik sifirdan buyuk bir deger verelim
        // sm: standart meridyen
        // b4: zaman denklemi
        // d8: günesin egimi
        public Tuple<double, double> get_b4_d8__(double julian_date, int en1, double enlem, double boylam, double sm)
        {
            const double pi = 3.1415926535898;
            var k = pi / 180;       
            var k1 = 180 / pi;
            var t1 = (julian_date - 2415020) / 36525;

            var t2 = Math.Pow(t1, 2); //karesini alır
            var t3 = Math.Pow(t1, 3); //küpü

            var o1 = 259.18328 - (1934.142 * t1) + (0.002078 * t2);
            o1 = o1 - Math.Floor((o1 / 360)) * 360; //değeri kendisinden küçük olan tamsayıya yuvarlar.

            if (o1 < 0)
            {
                o1 = o1 + 360;
            }

            var m = 358.47583333 + (129596579.1 * t1 - 0.54 * t2 - 0.012 * t3) / 3600;
            m = m - Math.Floor(m / 360) * 360;

            var l2 = 279.69667778 + (129602768.13 * t1 + 1.089 * t2) / 3600;
            l2 = l2 - Math.Floor(l2 / 360) * 360;

            var c1 = (6910.057 - 17.24 * t1 - 0.052 * t2) * Math.Sin(m * k) + 0.018 * Math.Sin(4 * m * k);
            c1 = c1 + (72.338 - 0.361 * t1) * Math.Sin(2 * m * k) + (1.54 - 0.001 * t1) * Math.Sin(3 * m * k); //sinüsünü alır
            c1 = c1 / 3600;
            var y = l2 + c1;

            var e1 = 23.45229444 - (46.845 * t1 + 0.0059 * t2 - 0.00181 * t3) / 3600;
            var e2 = (9.21 + 0.00091 * t1) * Convert.ToDouble(Math.Cos(o1 * k)) - (0.0904 - 0.00004 * t1) * Convert.ToDouble(Math.Cos(2 * o1 * k));
            e2 = e2 + 0.552 * Convert.ToDouble(Math.Cos(2 * l2 * k)) + 0.0884 * Convert.ToDouble(Math.Cos(2 * l2 * k)) + 0.0216 * Convert.ToDouble(Math.Cos((2 * l2 + m) * k));
            e2 = e2 - 0.0093 * Convert.ToDouble(Math.Cos((2 * l2 - m) * k));
            e2 = e2 / 3600;
            var e = e1 + e2;

            var a8 = Math.Atan(Math.Tan(y * k) * Math.Cos(e * k)) * k1;
            var a1 = (y - a8 + 5) / 90;
            a8 = a8 + Math.Floor(a1) * 90;
            a8 = a8 + 360;
            a8 = a8 - Math.Floor(a8 / 360) * 360;
            var d8 = Math.Sin(y * k) * Math.Sin(e * k);

            d8 = Math.Atan(d8 / Math.Sqrt(1 - d8 * d8)) * k1;

            if (en1 < 0)
            {
                d8 = -1 * d8;
            }

            var e3 = 6.646065556 + (8640184.542 * t1 + 0.0929 * t2) / 3600;
            e3 = e3 - Math.Floor(e3 / 24) * 24;

            if (e3 < 0)
            {
                e3 = e3 + 24;
            }

            double b4 = a8 / 15 - e3;
            if (b4 < 0)
            {
                b4 = b4 + 24;
            }

            var b5 = b4 + (sm - boylam) / 15;
            var e5 = Math.Cos(enlem * k) * Math.Cos(d8 * k);
            var b7 = Math.Tan(enlem * k) * Math.Tan(d8 * k);

            // todo: check
            // var b4_ = (b4%1)*-1;
            var b4_ = 12 - b4;
            return Tuple.Create(b4_, d8);
        }

        public Tuple<double, double> get_b4_d8(DateTime date, int en1, double enlem, double boylam, double sm)
        {
            var tarih = date.ToString("dd.MM.yyyy");
            var saat = date.Hour;
            Meyl_Cetveli meyl_Cetveli = new Meyl_Cetveli();
            //var row = _db.MeylCetveli.Where(x => x.Tarih == tarih && x.Saat == saat).FirstOrDefault();
            //var b4 = Convert.ToDouble(row.Zaman_Denklemi);
            //var d8 = Convert.ToDouble(row.Gunesin_Egimi);
            return Tuple.Create(5.5, 5.5);
        }

    }
}