using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Calibration.xaml
    /// </summary>
    public partial class Calibration : Window
    {

        public Calibration(Tool.Calibration_Data calibration_Data )
        {
            InitializeComponent();
            so.DataContext = calibration_Data;
        }
    }
}
