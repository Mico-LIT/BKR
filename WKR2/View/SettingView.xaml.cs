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
        private List<int> DelRow = new List<int>();
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
            DelCol.Remove(listColumn.SelectedItem.ToString());
            listColumn.Items.Remove(listColumn.SelectedItem);
        }
        private void MenuItem_Click_Delete_Row(object sender, RoutedEventArgs e)
        {
            DelRow.Remove(int.Parse(listRow.SelectedItem.ToString()));
            listRow.Items.Remove(listRow.SelectedItem);
        }

        private void Button_Click_new(object sender, RoutedEventArgs e)
        {
            DataView ff = (DataView)DataG.ItemsSource;
            foreach (string item in DelCol)ff.Table.Columns.Remove(item);
            foreach (int item in DelRow) ff.Table.Rows.RemoveAt(item);
            DataG.ItemsSource = null;
            DataG.ItemsSource = ff;
            listColumn.Items.Clear();
            listRow.Items.Clear();
            this.Close();
        }

        private void Delete_Collumn(object sender, RoutedEventArgs e)
        {
            foreach (DataGridCellInfo item in DataG.SelectedCells)
            {
            string df1 = item.Column.Header.ToString();
            var hh = DelCol.Find(x => x == df1);
            if (hh == null) { DelCol.Add(df1); listColumn.Items.Add(df1); }
            }
        }

        private void delete(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete) del_row();
        }

        public void del_row()
        {
            DataView gg = (DataView)DataG.ItemsSource;
            foreach (DataGridCellInfo item in DataG.SelectedCells)
            {
                int gggh = item.Column.DisplayIndex;
                string gggh1 = item.Column.Header.ToString();
            }
            //gg.Table.Rows.RemoveAt(DataG.SelectedIndex);
            DataG.ItemsSource = null;
            DataG.ItemsSource = gg;
        }

        private void Delete_Row(object sender, RoutedEventArgs e)
        {
            foreach (DataGridCellInfo item in DataG.SelectedCells)
            {
                DataRowView pp = (DataRowView)item.Item;
                int INT_ROW=DataG.Items.IndexOf(pp);
                var hh = DelRow.FindIndex(x => x == INT_ROW);
                if (hh == -1) { DelRow.Add(INT_ROW); listRow.Items.Add(INT_ROW); }
            }
        }
    }

}
