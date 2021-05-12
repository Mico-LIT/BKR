using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для Anallitic_Setings.xaml
    /// </summary>
    public partial class Anallitic_Setings : Window
    {
        

        public Anallitic_Setings()
        {
            InitializeComponent();

        }

        public Anallitic_Setings(ItemCollection items):this()
        {
            d1.ItemsSource = items; //d1.SelectedIndex = 0;
            d2.ItemsSource = items; //d2.SelectedIndex = 0;
            so.DataContext = Tool.Analitic.PARAMS;
        }
    }
}
