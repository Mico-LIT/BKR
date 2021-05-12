using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace WKR2.View
{
    /// <summary>
    /// Логика взаимодействия для Button_Calibration.xaml
    /// </summary>
    public partial class Button_Calibration : Window
    {
        private Button bu;

        public Button_Calibration( ref Button bu)
        {
            this.bu = bu;
            InitializeComponent();
            d12.DataContext = bu;
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            bu.Margin = new Thickness(bu.Margin.Left, bu.Margin.Top + 1, 0, 0);
            //bu.Height = 500;
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            bu.Margin = new Thickness(bu.Margin.Left, bu.Margin.Top - 1, 0, 0);
        }

        private void rigth_Click(object sender, RoutedEventArgs e)
        {
            bu.Margin = new Thickness(bu.Margin.Left+1, bu.Margin.Top , 0, 0);
        }

        private void left_Click(object sender, RoutedEventArgs e)
        {
            bu.Margin = new Thickness(bu.Margin.Left-1, bu.Margin.Top , 0, 0);
        }

        private void But_font_Click(object sender, RoutedEventArgs e)
        {

            var tt= Tool.Print.But_font.FirstOrDefault(x => bu == x.Key);
            if (tt.Key != null)
            {
                Font f = Tool.Print.Font(Tool.Print.But_font[bu]);
                if (f!=null)Tool.Print.But_font[bu] = f;

            }
            else {
                Font f = Tool.Print.Font();
                if (f != null) Tool.Print.But_font.Add(bu, f);
            }
            
        



        }
    }
}
