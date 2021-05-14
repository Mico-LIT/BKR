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
    /// Логика взаимодействия для Poisk.xaml
    /// </summary>
    public partial class Poisk : Window
    {
        private _MainWindow mainWindow;

        public Poisk()
        {
            InitializeComponent();
        }

        public Poisk(_MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            //mainWindow.Print_Item(3);
        }
    }
}
