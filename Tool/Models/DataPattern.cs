using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using static Tool.Services.Print.PrintService;
using System.Windows;
using d = System.Windows.Controls;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Tool
{
    [Serializable]
    public class DataPattern
    {
        
        public Bitmap Image { get; set; }
        public Font font { get; set; }
        public Calibration_Data calibration_data { get; set; }
        public List<SettingButton> But_canvas { get; set; }
        public Services.Analitic.Models.Params par { get; set; }

    }

    [Serializable]
    public class SettingButton
    {
        public string Name { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double MarginB { get; set; }
        public double MarginL { get; set; }
        public double MarginT { get; set; }
        public double MarginR { get; set; }
        public Font Font { get; set; }
    }

    [Serializable]
    //Калибровка
    public class Calibration_Data
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
