using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TimeAndDate.Services;
using TimeAndDate.Services.DataTypes.DST;
using TimeAndDate.Services.DataTypes.Places;
using TimeAndDate.Services.Tests;
using Excel = Microsoft.Office.Interop.Excel;

namespace TimeAndDate
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

       

        IList<place> places;
        String date;
        ExcelYaz Excelyaz;
        int split_count = 190;

       
        public IList<place> read_excell(String File_name)
        {

            label3.Visible = true;
            lb_readexcell.Visible = true;

            IList<place> plcs = new List<place>();
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            string str;
            int rCnt = 0;
            int cCnt = 0;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(File_name, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.ActiveSheet;

            range = xlWorkSheet.UsedRange;

            for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)
            {
                if ((range.Cells[rCnt, 1] as Excel.Range).Value2 != null)
                {

                    place place = new place();
                    //poz.sira = xlWorkSheet.Cells[rCnt, 1].Text.ToString();
                    place.Name = (range.Cells[rCnt, 1] as Excel.Range).Value2.ToString();
                    place.CoordinateX = Convert.ToDouble((range.Cells[rCnt, 2] as Excel.Range).Value2.ToString());
                    place.CoordinateY = Convert.ToDouble((range.Cells[rCnt, 3] as Excel.Range).Value2.ToString());
                    plcs.Add(place);
                }

                //if (rCnt%100 == 0)
                //{
                //    lb_readexcell.Text = rCnt.ToString();
                //}
                lb_readexcell.Text = rCnt.ToString();

                //for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                //{

                //    str = (string)(range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                //    MessageBox.Show(str);
                //}
            }

            lb_readexcell.Text = (rCnt-2).ToString();

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            return plcs;

        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Text Files (.xls)|*.xls|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                date = dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss,fff");
                button1.Enabled = false;
                places = read_excell(openFileDialog1.FileName);

                List<List<place>> placess = Split(places);
                Excelyaz = new ExcelYaz(openFileDialog1.FileName);

                for (int pl =0; pl<placess.Count; pl++)
                {
                    label5.Visible = true;
                    lb_service_count.Visible = true;
                    ConvertedTimes times = get_times(placess[pl]);
                    lb_service_count.Text = (pl * split_count).ToString();                   
                    Excelyaz.times_write(times,pl*split_count);

                }

                lb_service_count.Text = (places.Count).ToString();


                Excelyaz.savefile();
                button1.Enabled = true;
                System.Diagnostics.Process.Start("explorer.exe", openFileDialog1.FileName.Replace(".xls", "_Times.xls"));

            }


            //Calling_ConvertTimeService_WithTimeChanges_Should_ReturnTimeChanges();
            //Calling_TimeService_WithCoordinates_Should_ReturnCorrectLocation();
            //Calling_DstService_WithCountry_Should_ReturnAllDst();
        }


        public  List<List<T>> Split<T>(IList<T> source)
        {
            return  source.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / split_count )
                .Select(x => x.Select(v => v.Value).ToList()).ToList();
        }

        //public IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        //{
        //    while (source.Any())
        //    {
        //        yield return source.Take(chunksize);
        //        source = source.Skip(chunksize);
        //    }
        //}

        public ConvertedTimes get_times (IList<place> _places)
        {
            List<LocationId> locations = new List<LocationId>();
            LocationId f_coordinate = new LocationId(new Coordinates((decimal)_places[0].CoordinateX, (decimal)_places[0].CoordinateY));
            for (int p = 1; p < _places.Count; p++)
            {
                LocationId lid = new LocationId(new Coordinates((decimal)_places[p].CoordinateX, (decimal)_places[p].CoordinateY));
                locations.Add(lid);
            }

            var service = new ConvertTimeService(tb_accesskey.Text, tb_secretkey.Text);
            service.IncludeTimeChanges = true; // Default

            DateTime myDate = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture);
            var result = service.ConvertTime(f_coordinate, myDate.ToString("s", CultureInfo.InvariantCulture), locations);

            return result;
        }


        public void Calling_TimeService_WithCoordinates_Should_ReturnCorrectLocation() // service 1
        {
            // Arrange
            var osloCoords = new Coordinates(41.000m, 29.000m);
            var expectedId = String.Format("+{0}+{1}", osloCoords.Latitude.ToString(CultureInfo.InvariantCulture),
                                            osloCoords.Longitude.ToString(CultureInfo.InvariantCulture));

            // Act
            var timeservice = new TimeService("F8F1be1336", "pHYL2400YdHi8Tg6pm1M");
            timeservice.IncludeListOfTimeChanges = true;
            timeservice.IncludeTimezoneInformation = true;
            
            
            var result = timeservice.CurrentTimeForPlace(new LocationId(osloCoords));
            
            var firstLocation = result.SingleOrDefault();

            // Assert
            Assert.AreEqual(expectedId, firstLocation.Id);

        }


        public void Calling_DstService_WithCountry_Should_ReturnAllDst()
        {
            // Arrage
            var countryCode = "no";
            var country = "Norway";

            // Act
            var service = new DSTService("F8F1be1336", "pHYL2400YdHi8Tg6pm1M");
            var result = service.GetDaylightSavingTime(countryCode);
            var sampleCountry = result.SingleOrDefault(x => x.Region.Country.Name == "Norway");

            // Assert
            Assert.IsFalse(service.IncludeOnlyDstCountries);
            Assert.AreEqual(country, sampleCountry.Region.Country.Name);
            Assert.IsTrue(result.Count == 1);

            HasValidSampleCountry(sampleCountry);
        }

        public void HasValidSampleCountry(DST norway)
        {
            Assert.AreEqual("Oslo", norway.Region.BiggestPlace);
            Assert.AreEqual("no", norway.Region.Country.Id);

            Assert.Greater(norway.DstEnd.Year, 1);
            Assert.Greater(norway.DstStart.Year, 1);
            Assert.AreEqual("CEST", norway.DstTimezone.Abbrevation);
            Assert.AreEqual(3600, norway.DstTimezone.BasicOffset);
            Assert.AreEqual(3600, norway.DstTimezone.DSTOffset);
            Assert.AreEqual(7200, norway.DstTimezone.TotalOffset);
            Assert.AreEqual("Central European Summer Time", norway.DstTimezone.Name);
            Assert.AreEqual(2, norway.DstTimezone.Offset.Hours);
            Assert.AreEqual(0, norway.DstTimezone.Offset.Minutes);

            Assert.AreEqual("CET", norway.StandardTimezone.Abbrevation);
            Assert.AreEqual(3600, norway.StandardTimezone.BasicOffset);
            Assert.AreEqual(0, norway.StandardTimezone.DSTOffset);
            Assert.AreEqual(3600, norway.StandardTimezone.TotalOffset);
            Assert.AreEqual("Central European Time", norway.StandardTimezone.Name);
            Assert.AreEqual(1, norway.StandardTimezone.Offset.Hours);
            Assert.AreEqual(0, norway.StandardTimezone.Offset.Minutes);

            Assert.IsNotNull(norway.Region.Description);
            Assert.IsNotEmpty(norway.Region.Description);
        }

       
        //public static TimeZoneInfo UsTimezone = TimeZoneInfo.FindSystemTimeZoneById("US/Alaska");
        //public static DateTime UsTimestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), UsTimezone);

        //public static TimeZoneInfo ArticTimezone = TimeZoneInfo.FindSystemTimeZoneById("Antarctica/Troll");
        //public static DateTime ArticTimestamp = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), ArticTimezone);

        public const string fromCountry = "Norway";
        public const string fromCity = "Oslo";
        //LocationId fromCoords;
        public static String fromFormat = String.Format("{0}/{1}", fromCountry, fromCity).ToLower();
        //public static LocationId fromId;

        public const string toUsState = "Alaska";
        public const string toUsCountry = "USA";
        public const string toUsCity = "Anchorage";
        public static String toUsFormat = String.Format("{0}/{1}", toUsCountry, toUsCity).ToLower();
        public LocationId toUsId;

        public const string toArticCountry = "Antarctica";
        public const string toArticCity = "Troll";
        public static String toArticFormat = String.Format("{0}/{1}", toArticCountry, toArticCity).ToLower();
        //public LocationId toArticId;

        public static LocationId fromCoords = new LocationId(new Coordinates(33.533418000m, -7.583234000m));
        LocationId fromId = new LocationId(fromFormat);
        LocationId _toUsId = new LocationId(toUsFormat);
        LocationId toArticId = new LocationId(toArticFormat);
        // Arrange
        public void Calling_ConvertTimeService_WithTimeChanges_Should_ReturnTimeChanges()
        {
            var toId = new List<LocationId>();
        toId.Add(new LocationId(new Coordinates(30.044449000m, 31.235737000m)));

            // Act
            var service = new ConvertTimeService("F8F1be1336", "pHYL2400YdHi8Tg6pm1M");
            service.IncludeTimeChanges = true; // Default

            DateTime myDate = DateTime.ParseExact("2017-01-08 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture);
            var result = service.ConvertTime(fromCoords, myDate.ToString("s", CultureInfo.InvariantCulture), toId);
            String date_first = result.Locations[0].TimeChanges[0].NewLocalTime.ToString();
            String date_second = result.Locations[0].TimeChanges[1].NewLocalTime.ToString();

            var anchorage = result.Locations.FirstOrDefault(x => x.Id == "18");
        var oslo = result.Locations.FirstOrDefault(x => x.Id == "187");

        // Assert
        //Assert.AreEqual(toUsState, anchorage.Geography.State);
        //    Assert.AreEqual(toUsCity, anchorage.Geography.Name);

        //    Assert.AreEqual(fromCity, oslo.Geography.Name);
        //    Assert.AreEqual(fromCountry, oslo.Geography.Country.Name);

            Assert.IsTrue(result.Locations.All(x => x.TimeChanges != null));

        }

       
        

        public void HasCorrectLocation(DateTimeOffset date, Location location)
        {
            Assert.AreEqual(date.Year, location.Time.DateTime.Year);
            Assert.AreEqual(date.Month, location.Time.DateTime.Month);
            Assert.AreEqual(date.Day, location.Time.DateTime.Day);
            Assert.AreEqual(date.Hour, location.Time.DateTime.Hour);
            Assert.AreEqual(date.Minute, location.Time.DateTime.Minute);
        }

        public void HasCorrectUtc(DateTimeOffset date)
        {
            Assert.AreEqual(DateTime.UtcNow.Year, date.Year);
            Assert.AreEqual(DateTime.UtcNow.Month, date.Month);
            Assert.AreEqual(DateTime.UtcNow.Day, date.Day);
            Assert.AreEqual(DateTime.UtcNow.Hour, date.Hour);
            Assert.AreEqual(DateTime.UtcNow.Minute, date.Minute);
        }

    }
}
