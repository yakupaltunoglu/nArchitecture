using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Persistence.Contexts;
using Persistence.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;


namespace Persistence.Repositories
{
    public class CityRepository : EfRepositoryBase<City, BaseDbContext>, ICityRepository
    {

        private BaseDbContext _context;
      
        public CityRepository(BaseDbContext context) : base(context)
        {
            _context = context;
           
        }

        

        #region CalculatePraytime
        public void CalculatePraytime()
        {
            DateTime startDate = new DateTime(2022, 04, 01);
            DateTime endDate = new DateTime(2024, 03, 30);
            var cityList = _context.Cities.Where(x => x.Id == 3995).ToList();
            var row = 0;
            foreach (var city in cityList)
            {
                row++;
                var time = new PrayTimeHelper(_context);
                var result = time.Calculate(city, startDate, endDate);
                var prayTimeTakdirs = SehirTakdirOlayi(result); //Nanlı değerleri yıldızlı olarak yazmak
                _context.PrayTimes.AddRange(prayTimeTakdirs);

                if (row % 3000 == 0)
                {
                    _context.SaveChanges();
                }
            }
            _context.SaveChanges();
        }
        List<PrayTime> SehirTakdirOlayi(List<PrayTime> prayTimes)
        {
            var newPrayTimes = new List<PrayTime>();
            var tarih = new DateTime();
            var nanDurum = false;
            var imsakTakdir = new PrayTime();
            var yatsiTakdir = new PrayTime();
            string imsakTakdirSet = "", yatsiTakdirSet = "";
            foreach (var item in prayTimes)
            {
                if (!nanDurum)
                {
                    if (item.Imsak == "NaN")
                    {
                        tarih = item.Date;
                        nanDurum = true;
                        imsakTakdir = prayTimes.FirstOrDefault(x => x.Date == tarih.AddDays(-1));
                        yatsiTakdir = prayTimes.FirstOrDefault(x => x.Date == tarih.AddDays(-2));
                        imsakTakdirSet = imsakTakdir?.Imsak;
                        yatsiTakdirSet = yatsiTakdir?.Isha;
                        if (imsakTakdir != null)
                        {
                            imsakTakdir.Isha = "*" + yatsiTakdirSet;
                        }
                        else
                        {
                            imsakTakdir = null;
                        }
                    }
                }
                if (nanDurum)
                {
                    var sonrakiDurum = prayTimes.FirstOrDefault(x => x.Date == item.Date.AddDays(+1));

                    if (sonrakiDurum != null)
                    {
                        if (sonrakiDurum.Imsak != "NaN")
                        {
                            nanDurum = false;
                        }
                    }
                    item.Imsak = "*" + imsakTakdirSet;
                    if (nanDurum)
                    {
                        item.Isha = "*" + yatsiTakdirSet;
                    }
                }
            }
            newPrayTimes = prayTimes.Select(item => new PrayTime()
            {
                Date = item.Date,
                Imsak = DakikayiSaateCevirme(item.Imsak),
                Fajr = DakikayiSaateCevirme(item.Fajr),
                Tulu = DakikayiSaateCevirme(item.Tulu),
                Israk = DakikayiSaateCevirme(item.Israk),
                Zuhr = DakikayiSaateCevirme(item.Zuhr),
                Asr = DakikayiSaateCevirme(item.Asr),
                Maghrib = DakikayiSaateCevirme(item.Maghrib),
                Isha = DakikayiSaateCevirme(item.Isha),
                CityName = item.CityName,
                CityId = item.CityId,
                CountryName = item.CountryName,

            }).ToList();

            return newPrayTimes;
        }
        string DakikayiSaateCevirme(string sayi)
        {
            if (sayi == "NaN") return "NaN";

            int minute, hour;
            double mainMinute;
            var star = "";

            if (sayi.Substring(0, 1) == "*")
            {
                star = sayi.Substring(0, 1);
                sayi = sayi.Substring(1, sayi.Length - 1);
            }

            if (string.IsNullOrEmpty(sayi)) return "";

            mainMinute = Convert.ToDouble(sayi) * 1440;
            hour = Convert.ToInt32(mainMinute) / 60;
            minute = Convert.ToInt32(mainMinute) % 60;

            var hourText = hour < 10 && hour > -1
                ? "0" + hour
                : hour.ToString();

            var minuteText = minute < 10 && minute > -1
                ? "0" + minute
                : minute.ToString(); ;


            return star + hourText + ":" + minuteText;
        }

        #endregion

        #region CalculateTimeAndDate


        #endregion


    }


}