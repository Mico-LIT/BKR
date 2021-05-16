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

namespace WKR2.Views
{
    /// <summary>
    /// Логика взаимодействия для Anallitic_Setings.xaml
    /// </summary>
    public partial class AnalliticSetings : Window
    {
        public AnalliticSetings()
        {
            InitializeComponent();
        }

        public AnalliticSetings(ItemCollection items) : this()
        {
            comboBoxId.ItemsSource = items; //d1.SelectedIndex = 0;
            comboBoxFIO.ItemsSource = items; //d2.SelectedIndex = 0;
            stackPanel.DataContext = Tool.Services.Analitic.AnaliticService.PARAMS;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
