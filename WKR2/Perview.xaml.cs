using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Dr = System.Drawing;

namespace WKR2
{
    /// <summary>
    /// Логика взаимодействия для Perview.xaml
    /// </summary>
    public partial class Perview : Window
    {
        private System.Drawing.Image vie;

        public Perview()
        {
           
        }

        public Perview(System.Drawing.Image vie)
        {
            InitializeComponent();
            Img.Source = Convert(vie);
                
            //    new BitmapImage(
            //new Uri(@"C:\BKR\WKR2\gomer1.jpg"));
        }

        public BitmapImage Convert(Dr.Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
