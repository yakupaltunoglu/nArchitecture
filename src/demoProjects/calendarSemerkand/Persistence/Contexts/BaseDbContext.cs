
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<DaylightSavingTime> DaylightSavingTimes { get; set; }
        public DbSet<PrayTime> PrayTimes { get; set; }
      

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //    base.OnConfiguring(
            //        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SomeConnectionString")));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(a =>
            {
                a.ToTable("Cities").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.Name).HasColumnName("Name");
                a.Property(p => p.Latitude).HasColumnName("Latitude");
                a.Property(p => p.Longitude).HasColumnName("Longitude");
                a.Property(p => p.LongitudeDelta).HasColumnName("LongitudeDelta");
                a.Property(p => p.GreenwichDelta).HasColumnName("GreenwichDelta");
                a.Property(p => p.Offset).HasColumnName("Offset");
                a.Property(p => p.StandartMeridian).HasColumnName("StandartMeridian");
                a.Property(p => p.LongitudeDeltaFromService).HasColumnName("LongitudeDeltaFromService");
                a.Property(p => p.GreenwichtenZamanCinsindenFarki).HasColumnName("GreenwichtenZamanCinsindenFarki");
                a.Property(p => p.IhlasTemkin).HasColumnName("IhlasTemkin");
                a.Property(p => p.SehirKisa).HasColumnName("SehirKisa");
                a.Property(p => p.StandartBoylamQuery).HasColumnName("StandartBoylamQuery");
                a.Property(p => p.CreatedOn).HasColumnName("CreatedOn");
                a.Property(p => p.CountryName).HasColumnName("CountryName");
                a.HasMany(p => p.PrayTimes);
                a.HasMany(p => p.DaylightSavingTimes);
            });


            modelBuilder.Entity<DaylightSavingTime>(a =>
            {
                a.ToTable("DaylightSavingTimes").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.CityName).HasColumnName("CityName");
                a.Property(p => p.Start).HasColumnName("Start");
                a.Property(p => p.End).HasColumnName("End");
                a.Property(p => p.StartDiff).HasColumnName("StartDiff");
                a.Property(p => p.EndDiff).HasColumnName("EndDiff");
                a.Property(p => p.CityId).HasColumnName("CityId");
                a.Property(p => p.CountryName).HasColumnName("CountryName");

                a.HasOne(p => p.City);
            });

            modelBuilder.Entity<PrayTime>(a =>
            {
                a.ToTable("PrayTimes").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.Date).HasColumnName("Date");
                a.Property(p => p.Imsak).HasColumnName("Imsak");
                a.Property(p => p.Fajr).HasColumnName("Fajr");
                a.Property(p => p.Tulu).HasColumnName("Tulu");
                a.Property(p => p.Israk).HasColumnName("Israk");
                a.Property(p => p.Zuhr).HasColumnName("Zuhr");
                a.Property(p => p.Asr).HasColumnName("Asr");
                a.Property(p => p.Maghrib).HasColumnName("Maghrib");
                a.Property(p => p.Isha).HasColumnName("Isha");
                a.Property(p => p.CityName).HasColumnName("CityName");
                a.Property(p => p.IsDeleted).HasColumnName("IsDeleted");
                a.Property(p => p.CityId).HasColumnName("CityId");
                a.Property(p => p.CountryName).HasColumnName("CountryName");

                a.HasOne(p => p.City);
            });


            modelBuilder.Entity<Time>(a =>
            {
                a.ToTable("Times").HasKey(k => k.Id);
                a.Property(p => p.Id).HasColumnName("Id");
                a.Property(p => p.Coordinate).HasColumnName("Coordinate");
                a.Property(p => p.Abbrevation).HasColumnName("Abbrevation");
                a.Property(p => p.BasicOffset).HasColumnName("BasicOffset");
                a.Property(p => p.DstOffset).HasColumnName("DstOffset");
                a.Property(p => p.TotalOffset).HasColumnName("TotalOffset");
                a.Property(p => p.F6).HasColumnName("F6");
                a.Property(p => p.OldLocalTime).HasColumnName("OldLocalTime");
                a.Property(p => p.NewLocalTime).HasColumnName("NewLocalTime");
                a.Property(p => p.NewTotalOffset).HasColumnName("NewTotalOffset");
                a.Property(p => p.CityId).HasColumnName("CityId");
                a.Property(p => p.OldLocalTime1).HasColumnName("OldLocalTime1");
                a.Property(p => p.NewLocalTime1).HasColumnName("NewLocalTime1");
                a.Property(p => p.NewTotalOffset1).HasColumnName("NewTotalOffset1");
                a.Property(p => p.NewTotalOffset1).HasColumnName("NewTotalOffset1");
                a.Property(p => p.Latitude).HasColumnName("Latitude");
                a.Property(p => p.Longitude).HasColumnName("Longitude");
                a.Property(p => p.CityName).HasColumnName("CityName");
                a.Property(p => p.CountryName).HasColumnName("CountryName");
                a.HasOne(p => p.City);
            });
        }
    }
}
