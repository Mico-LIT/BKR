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
        private Dictionary<int, Font> hashCodeButtonsOncanvas;

        public Button_Calibration(Button button, Dictionary<int, Font> hashCodeButtonsOncanvas)
        {
            this.buttonRef = button;
            this.hashCodeButtonsOncanvas = hashCodeButtonsOncanvas;

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
            int buttonGetHashCode = buttonRef.GetHashCode();
            var fontButton = hashCodeButtonsOncanvas[buttonGetHashCode];

            Font font = PrintService.Font(fontButton);
            if (font != null)
                hashCodeButtonsOncanvas[buttonGetHashCode] = font;
        }

        protected override void OnClosed(EventArgs e)
        {
            this.buttonRef = null;
            this.hashCodeButtonsOncanvas = null;
            base.OnClosed(e);
        }
    }
}
