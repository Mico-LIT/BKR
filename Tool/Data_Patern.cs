using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using static Tool.Print;
using System.Windows;
using d = System.Windows.Controls;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Tool
{
    [Serializable]
    public class Data_Patern
    {
        
        public Bitmap Image { get; set; }
        public Font font { get; set; }
        public Calibration_Data calibration_data { get; set; }
        public List<Setting_Button> But_canvas { get; set; }
        
    }
    [Serializable]
    public class Setting_Button
    {
        public string Name { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double MarginB { get; set; }
        public double MarginL { get; set; }
        public double MarginT { get; set; }
        public double MarginR { get; set; }
    }
}
