using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace Tool
{
    public class Serialization_Data
    {
        private Bitmap image;
        private Font font;
        private Print.Calibration_Data calibration_data;
        private List<Setting_Button> but;
        private Tool.Services.Analitic.Models.Params par;

        public Serialization_Data(Bitmap image, Font font, 
            Print.Calibration_Data calibration_data, List<Setting_Button> but, Tool.Services.Analitic.Models.Params par)
        {
            this.image = image;
            this.font = font;
            this.calibration_data = calibration_data;
            this.but = but;
            this.par = par;
        }
        public Serialization_Data()
        {

        }

        public void Serialization(string Url)
        {
            Data_Patern dd = new Data_Patern() {
                Image = this.image ,
                But_canvas= but,
                font = this.font,
                calibration_data = this.calibration_data,
                par=this.par
            };

            BinaryFormatter formatter = new BinaryFormatter();
            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(Url, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, dd);
            }
        }
        public Data_Patern DeSerialization(string Url)
        {
            Data_Patern dd;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(Url, FileMode.OpenOrCreate))
            {
                dd = (Data_Patern)formatter.Deserialize(fs);
            }

            return dd;
        }
    }
}
