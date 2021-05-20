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
using Tool.Services.Excel.Models;

namespace Tool.Services.Excel
{
    public static class ExcelService
    {
        public static DataView LoadrExcel(Stream stream)
        {
            DataSet dateSet;
            using (stream)
            {
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateBinaryReader(stream);

                using (excelDataReader)
                {
                    excelDataReader.IsFirstRowAsColumnNames = false;
                    dateSet = excelDataReader.AsDataSet();
                }
            }
            return dateSet.Tables[0].DefaultView; //внизу экселя есть странички
        }

        public static DataView LoadrExcel()
        {
            DataSet dateSet;

            const string filter = "xls files (*.xls)|*.xls|Xlsx files (*.xlsx)|*.xlsx";
            OpenFileDialog openFileDialog = new OpenFileDialog() { ValidateNames = true, Filter = filter };

            if (openFileDialog.ShowDialog() != true)
                return null;

            using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelDataReader = null;
                if (openFileDialog.FilterIndex == 1) { excelDataReader = ExcelReaderFactory.CreateBinaryReader(fileStream); }
                else
                if (openFileDialog.FilterIndex == 2) { excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream); }
                else throw new InvalidOperationException();

                using (excelDataReader)
                {
                    excelDataReader.IsFirstRowAsColumnNames = false;
                    dateSet = excelDataReader.AsDataSet();
                }

                return dateSet.Tables[0].DefaultView; //внизу экселя есть странички

                //string json = JsonConvert.SerializeObject(dd, Formatting.Indented);
                //using (FileStream gg = new FileStream("DDD12.txt", FileMode.OpenOrCreate))
                //{
                //    byte[] array = System.Text.Encoding.Default.GetBytes(json);
                //    gg.Write(array,0,array.Length);
                //}
            }
        }

        public static void ExportToExcel(string path, string fileName = "Analitic.csv")
        {
            List<Settings> settings = new List<Settings>();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
                Directory.CreateDirectory(path);

            var files = directoryInfo.GetFiles("*.jpeg").ToList();

            foreach (var item in files)
            {
                var mas = item.ToString().Split('_');

                if (item.ToString().Split('_').Count() == 5)
                {
                    settings.Add(new Settings()
                    {
                        Nomer = (mas[1]),
                        FIO = (mas[2]),
                        repeat = (mas[0]),
                        Date = (mas[3])

                    });
                }
                else
                {
                    settings.Add(new Settings()
                    {
                        Nomer = (mas[0]),
                        FIO = (mas[1]),
                        repeat = (" "),
                        Date = (mas[2])
                    });
                }
            }

            using (StreamWriter sw = new StreamWriter(new FileStream(Path.Combine(path, fileName), FileMode.Create), Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ФИО; НОМЕР;ПОВТОР;ДАТА");

                foreach (Settings items in settings)
                    sb.AppendLine(String.Format("{0};{1};{2};{3}", items.FIO, items.Nomer, items.repeat, items.Date));

                sw.Write(sb.ToString());
            }

            #region OtherCode Microsoft.Office

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
