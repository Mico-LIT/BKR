using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using Microsoft.Win32;
using System.IO;
using Excel;
using Newtonsoft.Json;
using Microsoft.Office.Interop.Excel;

namespace Tool
{
    static public class ExcelWork
    {
        static public DataView LoadrExcel()
        {
            DataSet dd;
            OpenFileDialog df = new OpenFileDialog() { ValidateNames = true };
            df.Filter = "Xlsx files (*.xlsx)|*.xlsx|xls files (*.xls)|*.xls";

            if (df.ShowDialog() == true)
            {
                FileStream fs = new FileStream(df.FileName, FileMode.Open, FileAccess.Read);
                IExcelDataReader edr = null;
                if (df.FilterIndex == 1) { edr = ExcelReaderFactory.CreateOpenXmlReader(fs); }
                if (df.FilterIndex == 2) { edr = ExcelReaderFactory.CreateBinaryReader(fs); }
                edr.IsFirstRowAsColumnNames = false;
                dd = edr.AsDataSet();
                edr.Close();

                //string json = JsonConvert.SerializeObject(dd, Formatting.Indented);
                //using (FileStream gg = new FileStream("DDD12.txt", FileMode.OpenOrCreate))
                //{
                //    byte[] array = System.Text.Encoding.Default.GetBytes(json);
                //    gg.Write(array,0,array.Length);
                //}
                return dd.Tables[0].DefaultView; //внизу экселя есть странички
            }
            return null;
        }

        static public void Json(System.Data.DataTable dd)
        {
            DataSet dsSorted = new DataSet(); dsSorted.Tables.Add(dd);

            string json = JsonConvert.SerializeObject(dsSorted, Formatting.Indented);
            using (FileStream gg = new FileStream("DDD12.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(json);
                gg.Write(array, 0, array.Length);
            }
        }

        class asd
        {
            public string FIO { get; set; }
            public string Nomer { get; set; }
            public string repeat { get; set; }
            public string Date { get; set; }
        }

        public static void ExportToExcel(string pathLocal)
        {
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(Path.Combine(pathLocal, @"\Analitic"));

            List<asd> fff = new List<asd>();
            var files = info.GetFiles("*.jpeg").ToList();
            foreach (var item in files)
            {
                var mas = item.ToString().Split('_');

                if (item.ToString().Split('_').Count() == 5)
                {
                    fff.Add(new asd()
                    {
                        Nomer = (mas[1]),
                        FIO = (mas[2]),
                        repeat = (mas[0]),
                        Date = (mas[3])

                    });
                }
                else
                {
                    fff.Add(new asd()
                    {
                        Nomer = (mas[0]),
                        FIO = (mas[1]),
                        repeat = (" "),
                        Date = (mas[2])
                    });
                }
            }
            using (StreamWriter sw = new StreamWriter(new FileStream(info.FullName + @"\Analitic.csv", FileMode.Create), Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ФИО; НОМЕР;ПОВТОР;ДАТА");
                foreach (asd items in fff)
                {
                    sb.AppendLine(String.Format("{0};{1};{2};{3}", items.FIO, items.Nomer, items.repeat, items.Date));
                }
                sw.Write(sb.ToString());
            }

            #region 33
            //// Creating a Excel object. 
            //Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            //Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            //Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            //try
            //{

            //    worksheet = workbook.ActiveSheet;

            //    worksheet.Name = "ExportedFromDatGrid";

            //    int cellRowIndex = 1;
            //    int cellColumnIndex = 1;

            //    //Loop through each row and read value from each column. 
            //    for (int i = 0; i < 10 - 1; i++)
            //    {
            //        for (int j = 0; j < 10; j++)
            //        {
            //            // Excel index starts from 1,1. As first Row would have the Column headers, adding a condition check. 
            //            if (cellRowIndex == 1)
            //            {
            //                worksheet.Cells[cellRowIndex, cellColumnIndex] = i;
            //                worksheet.Cells[cellRowIndex, cellColumnIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);
            //            }
            //            else
            //            {
            //                worksheet.Cells[cellRowIndex, cellColumnIndex] = j;
            //            }
            //            cellColumnIndex++;
            //        }
            //        cellColumnIndex = 1;
            //        cellRowIndex++;
            //    }

            //    //Getting the location and file name of the excel to save from user. 
            //    SaveFileDialog saveDialog = new SaveFileDialog();
            //    saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            //    saveDialog.FilterIndex = 2;

            //    if (saveDialog.ShowDialog() == true)
            //    {
            //        workbook.SaveAs(saveDialog.FileName);
            //       // MessageBox.Show("Export Successful");
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    //MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    excel.Quit();
            //    workbook = null;
            //    excel = null;
            //}
            #endregion
        }

    }
}
