using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для SettingView.xaml
    /// </summary>
    public partial class SettingView : Window
    {
        private DataView dd;
        private List<string>  DelCol =new List<string> ();
        public SettingView(DataView dd)
        {
            InitializeComponent();
            this.dd = dd;
            DataG.ItemsSource = dd;

            //List<string> columnNameList = new List<string>();
            //foreach (DataColumn item in dd.Table.Columns)
            //{
            //    columnNameList.Add(item.ColumnName);
            //}
            //listV.ItemsSource =new List<string> ();
        }

        private void MenuItem_Click_Delete(object sender, RoutedEventArgs e)
        {
            List<string> default1 = (List<string>)listV.ItemsSource;
            foreach (string eachItem in listV.SelectedItems)
            {
                default1.Remove(eachItem);
            }
            listV.ItemsSource = null;
            listV.ItemsSource = default1;
        }

        private void Button_Click_new(object sender, RoutedEventArgs e)
        {
            DataView ff = (DataView)DataG.ItemsSource;
            foreach (string item in listV.Items)ff.Table.Columns.Remove(item);

            DataG.ItemsSource = null;
            DataG.ItemsSource = ff;
            listV.Items.Clear();
        }

        private void Delete_Collumn(object sender, RoutedEventArgs e)
        {
            string df1 = (string)DataG.CurrentColumn.Header;
            var hh = DelCol.Find(x => x == df1);
            if (hh == null) { DelCol.Add(df1); listV.Items.Add(df1); }
        }
    }
}
