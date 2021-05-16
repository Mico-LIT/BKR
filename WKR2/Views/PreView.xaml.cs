using System;
using System.Collections.Generic;
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

namespace WKR2.Views
{
    /// <summary>
    /// Логика взаимодействия для PreView.xaml
    /// </summary>
    public partial class PreView : Window
    {
        public PreView(Dr.Image image) 
        {
            InitializeComponent();
            img.Source = Convert(image);
        }

        public BitmapImage Convert(Dr.Image img)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = memory;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();

                return bi;
            }
        }
    }
}
