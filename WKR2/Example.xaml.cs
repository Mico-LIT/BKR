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

namespace WKR2
{
    /// <summary>
    /// Логика взаимодействия для Example.xaml
    /// </summary>
    public partial class Example : Window
    {
        class Date
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
        }
        public Example()
        {
            InitializeComponent();
            d12.ItemsSource = new List<Date>() { new Date() { Name="12",FirstName="14",LastName="15"} };
        }
    }
}
