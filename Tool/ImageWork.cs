using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using D = System.Drawing;

namespace Tool
{
    public static class ImageWork
    {
        public static string MyProperty { get; }

        public static BitmapImage Load() { return new BitmapImage(new Uri(@"C:\BKR\WKR2\ggh.jpg")); }

        public static void Save() {}
          
    }
}
