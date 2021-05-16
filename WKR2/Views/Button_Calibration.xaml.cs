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
using Tool.Services.Print;

namespace WKR2.Views
{
    /// <summary>
    /// Логика взаимодействия для Button_Calibration.xaml
    /// </summary>
    public partial class Button_Calibration : Window
    {
        private Button buttonRef;

        public Button_Calibration(ref Button button)
        {
            this.buttonRef = button;
            InitializeComponent();
            gridCallibration.DataContext = button;
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            buttonRef.Margin = new Thickness(buttonRef.Margin.Left, buttonRef.Margin.Top + 1, 0, 0);
            //bu.Height = 500;
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            buttonRef.Margin = new Thickness(buttonRef.Margin.Left, buttonRef.Margin.Top - 1, 0, 0);
        }

        private void rigth_Click(object sender, RoutedEventArgs e)
        {
            buttonRef.Margin = new Thickness(buttonRef.Margin.Left + 1, buttonRef.Margin.Top, 0, 0);
        }

        private void left_Click(object sender, RoutedEventArgs e)
        {
            buttonRef.Margin = new Thickness(buttonRef.Margin.Left - 1, buttonRef.Margin.Top, 0, 0);
        }

        private void But_font_Click(object sender, RoutedEventArgs e)
        {
            var button = PrintService.ButtonFontDictionary.FirstOrDefault(x => buttonRef == x.Key);

            if (button.Key != null)
            {
                Font font = PrintService.Font(PrintService.ButtonFontDictionary[buttonRef]);
                if (font != null) PrintService.ButtonFontDictionary[buttonRef] = font;

            }
            else
            {
                Font font = PrintService.Font();
                if (font != null) PrintService.ButtonFontDictionary.Add(buttonRef, font);
            }
        }
    }
}
