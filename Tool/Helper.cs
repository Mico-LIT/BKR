using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public static class Helper
    {
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
    }
}
