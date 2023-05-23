using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeAndDate.Services;

namespace TimeAndDate
{
    class ExcelYaz
    {
        String FileName;
        Microsoft.Office.Interop.Excel.Application oXL;
        Microsoft.Office.Interop.Excel._Workbook oWB;
        Microsoft.Office.Interop.Excel._Worksheet Sheet_addresses;
        Microsoft.Office.Interop.Excel._Worksheet Sheet_adres_grup;

        //workbook.Sheets.Add(After: workbook.Sheets[workbook.Sheets.Count]); 
        Microsoft.Office.Interop.Excel.Range oRng;
        object misvalue = System.Reflection.Missing.Value;

        public ExcelYaz(String filename)
        {
            FileName = filename;
            oXL = new Microsoft.Office.Interop.Excel.Application();
            oXL.Visible = false;

            //Get a new workbook.
            oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
        }

        public Boolean times_write(ConvertedTimes times, int first_row)
        {
            try
            {
                if(first_row == 0)
                {
                    Sheet_addresses = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Sheets.Add(After: oWB.Sheets[oWB.Sheets.Count]);
                    Sheet_addresses.Name = "Times";

                }

                //Add table headers going cell by cell.
                Sheet_addresses.Cells[1, 1] = "Coordinate";
                Sheet_addresses.Cells[1, 2] = "Abbrevation";
                Sheet_addresses.Cells[1, 3] = "Basic Offset";
                Sheet_addresses.Cells[1, 4] = "DST Offset";
                Sheet_addresses.Cells[1, 5] = "Total Offset";



                Sheet_addresses.Cells[1, 7] = "Old Local Time";
                Sheet_addresses.Cells[1, 8] = "New Local Time";
                Sheet_addresses.Cells[1, 9] = "New Total Offset";


                ////Format A1:D1 as bold, vertical alignment = center.
                //Sheet_addresses.get_Range("A1", "D1").Font.Bold = true;
                //Sheet_addresses.get_Range("A1", "D1").VerticalAlignment =
                //    Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                int max_range = 0;
                for (int l = 0; l < times.Locations.Count; l++)
                {
                    Sheet_addresses.Cells[first_row + 2 + l, 1] = times.Locations[l].Geography.Coordinates.Latitude.ToString() + " ! " + times.Locations[l].Geography.Coordinates.Longitude.ToString();
                    Sheet_addresses.Cells[first_row + 2 + l, 2] = times.Locations[l].Time.Timezone.Abbrevation.ToString();
                    Sheet_addresses.Cells[first_row + 2 + l, 3] = times.Locations[l].Time.Timezone.BasicOffset.ToString();
                    Sheet_addresses.Cells[first_row + 2 + l, 4] = times.Locations[l].Time.Timezone.DSTOffset.ToString();
                    Sheet_addresses.Cells[first_row + 2 + l, 5] = times.Locations[l].Time.Timezone.TotalOffset.ToString();
                    for (int t = 0; t<times.Locations[l].TimeChanges.Count; t++)
                    {

                        Sheet_addresses.Cells[first_row + 2 + l, 6 + ((t * 4)) + 1] = times.Locations[l].TimeChanges[t].OldLocalTime.ToString();
                        Sheet_addresses.Cells[first_row + 2 + l, 6 + ((t * 4)) + 2] = times.Locations[l].TimeChanges[t].NewLocalTime.ToString();
                        Sheet_addresses.Cells[first_row + 2 + l, 6 + ((t * 4)) + 3] = times.Locations[l].TimeChanges[t].NewTotalOffset.ToString();

                        if(max_range < t)
                        {
                            max_range = t;
                            Sheet_addresses.Cells[1, 6 + ((t * 4)) + 1] = "Old Local Time";
                            Sheet_addresses.Cells[1, 6 + ((t * 4)) + 2] = "New Local Time";
                            Sheet_addresses.Cells[1, 6 + ((t * 4)) + 3] = "New Total Offset";
                        }

                    }
                 
                }


                //AutoFit columns A:D.
                //oRng = Sheet_addresses.get_Range("A1", "D1");
                //oRng.EntireColumn.AutoFit();

                Microsoft.Office.Interop.Excel.Worksheet sheet = oWB.ActiveSheet;
                Microsoft.Office.Interop.Excel.Range oRng = (Microsoft.Office.Interop.Excel.Range)sheet.Range[sheet.Cells[1, 1], sheet.Cells[1, (6 + ((max_range * 4)) + 3)]];

                //AutoFit columns A:D.
                //oRng = (Microsoft.Office.Interop.Excel.Range) Sheet_adres_grup.get_Range(Sheet_adres_grup.Cells[1, 1], Sheet_adres_grup.Cells[1, Address_grup.Count]);
                oRng.EntireColumn.AutoFit();


                //Microsoft.Office.Interop.Excel.Range range = Sheet_adres_grup.UsedRange;
                //string str = (string)(range.Cells[1, Address_grup.Count] as Microsoft.Office.Interop.Excel.Range).Name.Value2;
                //Format A1:D1 as bold, vertical alignment = center.
                oRng.Font.Bold = true;
                oRng.VerticalAlignment =
                    Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;






                oXL.Visible = false;
                oXL.UserControl = false;


                oXL.Visible = false;
                oXL.UserControl = false;



            }
            catch (Exception)
            {
                return false;
                //throw;

            }

            return true;
        }

       
        //public Boolean Address_group_yaz(List<fw_address_group> Address_grup)
        //{

        //    try
        //    {
        //        Sheet_adres_grup = (Microsoft.Office.Interop.Excel._Worksheet)oWB.Sheets.Add(After: oWB.Sheets[oWB.Sheets.Count]);
        //        Sheet_adres_grup.Name = "Address_Group";

        //        for (int i = 0; i < Address_grup.Count; i++)
        //        {
        //            Sheet_adres_grup.Cells[1, 1 + i] = Address_grup[i].Name;
        //            for (int member = 0; member < Address_grup[i].members.Count; member++)
        //            {
        //                Sheet_adres_grup.Cells[2 + member, 1 + i] = Address_grup[i].members[member];
        //            }
        //        }


        //        Microsoft.Office.Interop.Excel.Worksheet sheet = oWB.ActiveSheet;
        //        Microsoft.Office.Interop.Excel.Range oRng = (Microsoft.Office.Interop.Excel.Range)sheet.Range[sheet.Cells[1, 1], sheet.Cells[1, Address_grup.Count]];

        //        //AutoFit columns A:D.
        //        //oRng = (Microsoft.Office.Interop.Excel.Range) Sheet_adres_grup.get_Range(Sheet_adres_grup.Cells[1, 1], Sheet_adres_grup.Cells[1, Address_grup.Count]);
        //        oRng.EntireColumn.AutoFit();


        //        //Microsoft.Office.Interop.Excel.Range range = Sheet_adres_grup.UsedRange;
        //        //string str = (string)(range.Cells[1, Address_grup.Count] as Microsoft.Office.Interop.Excel.Range).Name.Value2;
        //        //Format A1:D1 as bold, vertical alignment = center.
        //        oRng.Font.Bold = true;
        //        oRng.VerticalAlignment =
        //            Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;






        //        oXL.Visible = false;
        //        oXL.UserControl = false;



        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //        //throw;

        //    }

        //    return true;
        //}
        public void savefile()
        {
            oWB.SaveAs(FileName.Replace(".xls", "_Times.xls"), Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
            false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            oWB.Close();
        }

    }
}
