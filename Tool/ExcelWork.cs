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
                edr.IsFirstRowAsColumnNames =false;
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

        static public void Json(DataTable dd)
        {
            DataSet dsSorted = new DataSet();dsSorted.Tables.Add(dd);

            string json = JsonConvert.SerializeObject(dsSorted, Formatting.Indented);
            using (FileStream gg = new FileStream("DDD12.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(json);
                gg.Write(array, 0, array.Length);
            }
        }

    }
}
