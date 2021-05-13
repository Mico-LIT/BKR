using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tool
{
    public static class Helper
    {
        public static void JsonSerializeObject(System.Data.DataTable dataTable)
        {
            DataSet dsSorted = new DataSet(); dsSorted.Tables.Add(dataTable);

            string json = JsonConvert.SerializeObject(dsSorted, Formatting.Indented);
            using (FileStream gg = new FileStream("DDD12.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(json);
                gg.Write(array, 0, array.Length);
            }
        }

        public static void SerializationDataPattern(DataPattern dataPattern, string path)
        {
            if (dataPattern == null) throw new ArgumentNullException(nameof(dataPattern));

            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, dataPattern);
            }
        }

        public static DataPattern DeSerializationDataPattern(string path)
        {
            DataPattern dataPattern;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                dataPattern = (DataPattern)formatter.Deserialize(fs);
            }

            return dataPattern;
        }
    }
}
