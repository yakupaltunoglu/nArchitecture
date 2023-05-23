using System;
using System.Linq;
using static System.Math;
using System.Collections.Generic;
using System.Globalization; 
using Domain.Entities;
using Persistence.Contexts;


//http://praytimes.org/calculation
//http://praytimes.org/code/git/?a=viewblob&p=PrayTimes&h=cff3aa7fbc9ad958c5bfbb12fd64e5e7b9e26fb4&hb=HEAD&f=v1/csharp/PrayTime.cs
namespace Persistence.Helpers
{
    public class PrayTimeHelper
    {
        private BaseDbContext _db;

        private Dictionary<Vakit, int> _vakitHours = new Dictionary<Vakit, int>{
      {Vakit.imsak,6},
      {Vakit.sabah,6},
      {Vakit.gunes,8},
      {Vakit.israk,9},
      {Vakit.ogle,12},
      {Vakit.ikindi,14},
      {Vakit.aksam,16},
      {Vakit.yatsi,18},
    };

        private Dictionary<Vakit, int> _temkinler = new Dictionary<Vakit, int>{
      {Vakit.imsak,-5},
      {Vakit.sabah,0},
      {Vakit.gunes,-7},
      {Vakit.israk,10},
      {Vakit.ogle,5}, //aslında olan      
      //{Vakit.ogle,7},
      {Vakit.ikindi,4},
      {Vakit.aksam,7},
      {Vakit.yatsi,0},
    };

        private List<DaylightSavingTime> _daylightSavingTimes;

        // calc instance fields
        private int duzeltme1;
        private int duzeltme2;
        private double onceki_gun_imsak;
        private double onceki_gun_yatsi;
        private double greenwich_fark;
        private double enlem, boylam, boylam_fark, sm;
        private string cityName;
        private int cityId;
        private List<PrayTime> results;

        public PrayTimeHelper(BaseDbContext db)
        {
            _db = db;
            _daylightSavingTimes = db.DaylightSavingTimes.ToList();

        }

        public void PrepareCalculate(City city)
        {
            duzeltme1 = 0;
            duzeltme2 = 0;
            onceki_gun_imsak = 0;
            onceki_gun_yatsi = 0;
            greenwich_fark = city.GreenwichDelta;
            enlem = city.Latitude;
            boylam = city.Longitude;
            boylam_fark = city.LongitudeDelta;
            sm = city.StandartMeridian;
            cityName = city.Name;
            cityId = city.Id;
            results = new List<PrayTime>();
        }



        
        public List<PrayTime> Calculate(int cityId, DateTime startDate, DateTime endDate)
        {
            var city = _db.Cities.Where(x => x.Id == cityId).FirstOrDefault();
            if (city == null)
                return results;
            PrepareCalculate(city);
            //var daycount = DateTime.IsLeapYear(year) ? 366 : 365;
            DateTime date = startDate;

            while (true)
            {
                var pt = new PrayTime();
                pt.Date = date;
                pt.CityId = city.Id;
                pt.Imsak = VakitHesapla(Vakit.imsak, date);
                pt.Fajr = VakitHesapla(Vakit.sabah, date);
                pt.Tulu = VakitHesapla(Vakit.gunes, date);
                pt.Israk = VakitHesapla(Vakit.israk, date);
                pt.Zuhr = VakitHesapla(Vakit.ogle, date);
                pt.Asr = VakitHesapla(Vakit.ikindi, date);
                pt.Maghrib = VakitHesapla(Vakit.aksam, date);
                pt.Isha = VakitHesapla(Vakit.yatsi, date);
                //pt.Ulke = city.Ulke;
                pt.CityName=city.Name;
                results.Add(pt);
                //update date
                date = date.AddDays(1);
                if (date == endDate)
                {
                    break;
                }
            }
            return results;
        }

        public List<PrayTime> Calculate(City city, DateTime startDate, DateTime endDate)
        {
            if (city == null)
                return results;
            PrepareCalculate(city);
            //var daycount = DateTime.IsLeapYear(year) ? 366 : 365;
            DateTime date = startDate;

            while (true)
            {
                var pt = new PrayTime();
                pt.Date = date;
                pt.CityId = city.Id;
                pt.Imsak = VakitHesapla(Vakit.imsak, date);
                pt.Fajr = VakitHesapla(Vakit.sabah, date);
                pt.Tulu = VakitHesapla(Vakit.gunes, date);
                pt.Israk = VakitHesapla(Vakit.israk, date);
                pt.Zuhr = VakitHesapla(Vakit.ogle, date);
                pt.Asr = VakitHesapla(Vakit.ikindi, date);
                pt.Maghrib = VakitHesapla(Vakit.aksam, date);
                pt.Isha = VakitHesapla(Vakit.yatsi, date);
                //pt.Ulke = city.Ulke;
                pt.CityName=city.Name;
                results.Add(pt);
                //update date
                date = date.AddDays(1);
                if (date == endDate)
                {
                    break;
                }
            }
            return results;
        }
        //Farkın sadece o tarihlerdeki Güneş değerlerinden olan zaman denkleminden kaynaklanıp kaynaklanmadığını teyit edebilir miyiz?
        private string VakitHesapla(Vakit vakit, DateTime date)
        {
            DateTimeHelper dateTimeHelper = new DateTimeHelper(_db);
            var (b4, d8) = dateTimeHelper.get_b4_d8__(dateTimeHelper.ConvertToJulian(date.AddHours(_vakitHours[vakit])), 1, enlem, boylam, sm);
            var val = VakitHesapla_(vakit, enlem, b4, d8, boylam, boylam_fark);
            var val2 = 0d;
            var s_val = String.Format("{0:n7}", val);
            var s_val2 = string.Empty;
            Iterasyon(vakit, date, ref val, ref val2, ref s_val, ref s_val2);

            s_val2 = String.Format("{0:n7}", val2);

            // takdir
            // todo: yilin ilk gunu takdirse??
            if (vakit == Vakit.imsak)
            {
                if (Double.IsNaN(val2) && duzeltme1 == 0)
                {
                    Takdir_Duzeltme(Takdir_Vakit.imsak_oncesi, vakit, date, ref val, ref val2, ref s_val, ref s_val2);
                }
                onceki_gun_imsak = val2;
            }
            if (vakit == Vakit.yatsi)
            {
                if (Double.IsNaN(val2) && duzeltme2 == 0)
                {
                    Takdir_Duzeltme(Takdir_Vakit.yatsi_oncesi, vakit, date, ref val, ref val2, ref s_val, ref s_val2);
                }
                onceki_gun_yatsi = val2;
            }


            if (val2 < 0)
                val2 += 1;
            if (val2 >= 1)
                val2 -= 1;

            val2 = SaniyeYuvarlama(vakit, val2, enlem);
            val2 = YazSaati(date, val2, cityId);
            if (vakit == Vakit.yatsi && val2 >= 1)
                val2 = val2 - 1;

            s_val2 = String.Format("{0:n7}", val2);
            return s_val2;
        }

        // eskiden kullandigimiz interpolasyon yontemi
        private Tuple<double, double> Meyl_Iterasyon_old(
          double ilk_deger,
          double greenwich_fark,
          DateTime date,
          double enlem,
          double boylam,
          double sm
        )
        {
            DateTimeHelper dateTimeHelper = new DateTimeHelper(_db);
            var gf = ilk_deger - (greenwich_fark / 24);
            var date2 = date;
            if (gf < 0)
            {
                gf = gf + 1;
                date2 = date.AddDays(-1);
            }
            else if (gf >= 1)
            {
                gf = gf - 1;
                date2 = date.AddDays(1);
            }

            var zaman = gf * 24;
            var saat = Floor(zaman);
            var dakika = Floor((zaman - saat) * 60);
            var saniye = Floor((((zaman - saat) * 60) - dakika) * 60);

            var date3 = date2.AddHours(saat + 1);
            var (b4_iterasyon, d8_iterasyon) = dateTimeHelper.get_b4_d8(date2.AddHours(saat), 1, enlem, boylam, sm);
            var (b4_iterasyon2, d8_iterasyon2) = dateTimeHelper.get_b4_d8(date3, 1, enlem, boylam, sm);
            b4_iterasyon2 -= b4_iterasyon;
            d8_iterasyon2 -= d8_iterasyon;

            var b4 = (b4_iterasyon2 * ((dakika / 60) + (saniye / 3600))) + b4_iterasyon;
            var d8 = (d8_iterasyon2 * ((dakika / 60) + (saniye / 3600))) + d8_iterasyon;
            return Tuple.Create(b4, d8);
        }

        private Tuple<double, double> Meyl_Iterasyon_new(
          double ilk_deger,
          double greenwich_fark,
          DateTime date,
          double enlem,
          double boylam,
          double sm
        )
        {
            DateTimeHelper dateTimeHelper = new DateTimeHelper(_db);
            var gf = ilk_deger - (greenwich_fark / 24);
            var jd = dateTimeHelper.ConvertToJulian(date) + gf;

            return dateTimeHelper.get_b4_d8__(jd, 1, enlem, boylam, sm);
        }

        private double VakitHesapla_(Vakit vakit, double enlem, double b4, double d8, double boylam, double boylam_fark)
        {
            switch (vakit)
            {
                case Vakit.imsak:
                    var derece_i = enlem < 45 && enlem > -45 ? -18 : -19; 
                    //var derece_i = -18;
                    return (12 - ((Acos((Sin(derece_i * PI / 180) - Sin(enlem * PI / 180) * Sin(d8 * PI / 180)) / (Cos(enlem * PI / 180) * Cos(d8 * PI / 180))) * 180 / PI) / 15) + boylam_fark - b4) / 24;
                case Vakit.sabah:
                    var derece_s = -17;
                    return (12 - ((Acos((Sin(derece_s * PI / 180) - Sin(enlem * PI / 180) * Sin(d8 * PI / 180)) / (Cos(enlem * PI / 180) * Cos(d8 * PI / 180))) * 180 / PI) / 15) + boylam_fark - b4) / 24;
                case Vakit.gunes:
                    return (12 - ((Acos((Sin(-0.833 * PI / 180) - Sin(enlem * PI / 180) * Sin(d8 * PI / 180)) / (Cos(enlem * PI / 180) * Cos(d8 * PI / 180))) * 180 / PI) / 15) + boylam_fark - b4) / 24;
                case Vakit.israk:
                    return (12 - ((Acos((Sin(5 * PI / 180) - Sin(enlem * PI / 180) * Sin(d8 * PI / 180)) / (Cos(enlem * PI / 180) * Cos(d8 * PI / 180))) * 180 / PI) / 15) + boylam_fark - b4) / 24;
                case Vakit.ogle:
                    return (12 - b4 + boylam_fark) / 24;
                case Vakit.ikindi:
                    return (12 + ((Acos((Sin((90 - (Atan((Tan((Abs(enlem - d8)) * PI / 180) + 1)) * 180 / PI)) * PI / 180) - (Sin(enlem * PI / 180) * Sin(d8 * PI / 180))) / (Cos(enlem * PI / 180) * Cos(d8 * PI / 180))) * 180 / PI) / 15) + boylam_fark - b4) / 24;
                case Vakit.aksam:
                    return (12 + ((Acos((Sin(-0.833 * PI / 180) - Sin(enlem * PI / 180) * Sin(d8 * PI / 180)) / (Cos(enlem * PI / 180) * Cos(d8 * PI / 180))) * 180 / PI) / 15) + boylam_fark - b4) / 24;
                case Vakit.yatsi:
                    var derece_y = -17;
                    //var derece_y = -16;
                    return (12 + ((Acos((Sin(derece_y * PI / 180) - (Sin(enlem * PI / 180) * Sin(d8 * PI / 180))) / (Cos(enlem * PI / 180) * Cos(d8 * PI / 180))) * 180 / PI) / 15) + boylam_fark - b4) / 24;
                default:
                    throw new ArgumentOutOfRangeException(nameof(vakit));
            }
        }

        // todo: neden dakika +1 eklemisiz?
        // bazı vakitleri saniyesine gore asagi yada yukari yuvarliyoruz
        // simdilik sadece imsak ve gunes asagi yuvarlaniyor
        private double SaniyeYuvarlama(Vakit vakit, double val, double enlem)
        {
            var zaman = (val * 24);
            var saat = Floor(zaman);
            var dakika = Floor((zaman - saat) * 60);
            dakika += GetTemkin(vakit, enlem);
            switch (vakit)
            {
                case Vakit.sabah:
                case Vakit.israk:
                case Vakit.ogle:
                case Vakit.ikindi:
                case Vakit.aksam:
                case Vakit.yatsi:
                    dakika += 1;
                    break;
            }

            return ((dakika / 60) + saat) / 24;
        }

        //private int GetTemkin(Vakit vakit, double enlem)
        //{
        //    var derece_i =-18;
           
        //    if (vakit == Vakit.imsak && derece_i == -18)
        //        return 0;
        //    return _temkinler[vakit];
        //}

        private int GetTemkin(Vakit vakit, double enlem)
        {
            var derece_i = enlem < 45 && enlem > -45 ? -18 : -19;
            if (vakit == Vakit.imsak && derece_i != -18)
                return 0;
            return _temkinler[vakit];
        }

        private double YazSaati2(DateTime date, double val, int cityId)
        {
            // todo: cache values
            var dst = _daylightSavingTimes.FirstOrDefault(x => x.CityId == cityId);
            if (dst == null)
                return val;

            var zaman = (val * 24);
            var saat = Floor(zaman);
            var dakika = Floor((zaman - saat) * 60);
            var r = val;
            if (date > Convert.ToDateTime(dst.Start, new CultureInfo("tr-TR")) && date < Convert.ToDateTime(dst.End, new CultureInfo("tr-TR")))
            {
                saat += 1;
                val = (dakika / 60 + saat) / 24;
            }
            else if ((date == Convert.ToDateTime(dst.Start, new CultureInfo("tr-TR")) && saat >= dst.StartDiff) || (date == Convert.ToDateTime(dst.End, new CultureInfo("tr-TR")) && (saat < dst.EndDiff || (saat == dst.EndDiff && dakika == 0))))
            {
                saat += 1;
                val = (dakika / 60 + saat) / 24;
            }
            return val;
        }


        private double YazSaati(DateTime date, double val, int cityId)
        {
            // todo: cache values
            var dstt = _daylightSavingTimes.Where(x => x.CityId == cityId).OrderBy(x=>x.Id).ToList();
            if (dstt.Count < 1)
                return val;
            foreach (var dst in dstt)
            {
                var zaman = (val * 24);
                var saat = Floor(zaman);
                var dakika = Floor((zaman - saat) * 60);
                var r = val;
                if (date > Convert.ToDateTime(dst.Start, new CultureInfo("tr-TR")) && date < Convert.ToDateTime(dst.End, new CultureInfo("tr-TR")))
                {
                    saat += 1;
                    val = (dakika / 60 + saat) / 24;
                }
                else if ((date == Convert.ToDateTime(dst.Start, new CultureInfo("tr-TR")) && saat >= dst.StartDiff) || (date == Convert.ToDateTime(dst.End, new CultureInfo("tr-TR")) && (saat < dst.EndDiff || (saat == dst.EndDiff && dakika == 0))))
                {
                    saat += 1;
                    val = (dakika / 60 + saat) / 24;
                }

            }
            return val;
        }


        //private double YazSaatiRevize(DateTime date, double val, int cityId) //tarih,değer ve id geliyor
        //{
        //    // todo: cache values
        //    var dst = _daylightSavingTimes2.FirstOrDefault(x => x.CityId == cityId);
        //    if (dst == null)
        //        return val;
        //    //yaz saati değerleri çekiliyor
        //    var zaman = (val * 24);  
        //    var saat = Floor(zaman); 
        //    var dakika = Floor((zaman - saat) * 60);
        //    var r = val;  
        //    if (date > Convert.ToDateTime(dst.Start) && date < Convert.ToDateTime(dst.End)) // gelen tarih yaz başlama ve bitiş tarihleri arasında mı? 
        //    {
        //        saat += 1;
        //        val = (dakika / 60 + saat) / 24;  //saat 1 ekliyoruz ve değeri yeniden hesaplıyoruz
        //    }
        //    else if ((date == Convert.ToDateTime(dst.Start) && saat >= dst.StartDiff) || (date == Convert.ToDateTime(dst.End) && (saat < dst.EndDiff || (saat == dst.EndDiff && dakika == 0))))  //(gelen tarih başlama tarihine eşitse ve saat başlama saat farkına eşit veya büyükse) ya da (tarih yaz son tarihe eşitse ve (saat yaz son saat küçükse ya da (saat yaz son tarihe eşitse ve dakika 0 sa) 
        //    {
        //        saat += 1;
        //        val = (dakika / 60 + saat) / 24;
        //    }

        //    if (date > Convert.ToDateTime(dst.StartSecond) && date < Convert.ToDateTime(dst.EndSecond)) // gelen tarih yaz başlama ve bitiş tarihleri arasında mı? 
        //    {
        //        saat += 1;
        //        val = (dakika / 60 + saat) / 24;  //saat 1 ekliyoruz ve değeri yeniden hesaplıyoruz
        //    }
        //    else if ((date == Convert.ToDateTime(dst.StartSecond) && saat >= dst.StartDiffSecond) || (date == Convert.ToDateTime(dst.EndSecond) && (saat < dst.EndDiffSecond || (saat == dst.EndDiffSecond && dakika == 0))))  //(gelen tarih başlama tarihine eşitse ve saat başlama saat farkına eşit veya büyükse) ya da (tarih yaz son tarihe eşitse ve (saat yaz son saat küçükse ya da (saat yaz son tarihe eşitse ve dakika 0 sa) 
        //    {
        //        saat += 1;
        //        val = (dakika / 60 + saat) / 24;
        //    }
        //    return val;
        //}



        private void Takdir_Duzeltme(
          Takdir_Vakit takdir_vakit,
          Vakit vakit,
          DateTime date,
          ref double val,
          ref double val2,
          ref string s_val,
          ref string s_val2)
        {
            if (takdir_vakit == Takdir_Vakit.imsak_oncesi)
            {
                var (b4, d8) = Meyl_Iterasyon_new(onceki_gun_imsak, greenwich_fark, date, enlem, boylam, sm);
                val2 = VakitHesapla_(Vakit.imsak, enlem, b4, d8, boylam, boylam_fark);
                s_val2 = String.Format("{0:n7}", val2);
                duzeltme1 = 1;

                if (!Double.IsNaN(val2))
                {
                    val = val2;
                    s_val = s_val2;
                    Iterasyon(vakit, date, ref val, ref val2, ref s_val, ref s_val2);
                }
            }
            else if (takdir_vakit == Takdir_Vakit.yatsi_oncesi)
            {
                var (b4, d8) = Meyl_Iterasyon_new(onceki_gun_yatsi, greenwich_fark, date, enlem, boylam, sm);
                val2 = VakitHesapla_(Vakit.yatsi, enlem, b4, d8, boylam, boylam_fark);
                s_val2 = String.Format("{0:n7}", val2);
                duzeltme2 = 1;

                if (!Double.IsNaN(val2))
                {
                    val = val2;
                    s_val = s_val2;
                    Iterasyon(vakit, date, ref val, ref val2, ref s_val, ref s_val2);
                }
            }
        }

        private void Iterasyon(
          Vakit vakit, DateTime date,
          ref double val, ref double val2,
          ref string s_val, ref string s_val2
        )
        {
            var sayac = 0;
            var yedek = string.Empty;
            s_val2 = string.Empty;
            while (s_val2 != s_val)
            {
                sayac++;
                if (sayac != 1)
                {
                    val = val2;
                    yedek = s_val;
                    s_val = s_val2;
                }

                //if(!Double.IsNaN(val))
                var (b4, d8) = Meyl_Iterasyon_new(val, greenwich_fark, date, enlem, boylam, sm);
                val2 = VakitHesapla_(vakit, enlem, b4, d8, boylam, boylam_fark);
                s_val2 = String.Format("{0:n7}", val2);

                if (s_val2 == yedek)
                    s_val = s_val2;
            }
        }
    }

    public enum Vakit
    {
        imsak,
        sabah,
        gunes,
        israk,
        ogle,
        ikindi,
        aksam,
        yatsi
    }

    public enum Takdir_Vakit
    {
        imsak_oncesi,
        imsak_sonrasi,
        yatsi_oncesi,
        yatsi_sonrasi,
    }
}