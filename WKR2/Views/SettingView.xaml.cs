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

namespace WKR2.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingView.xaml
    /// </summary>
    public partial class SettingView : Window
    {
        //private DataView dataView;
        private List<string> DelCol = new List<string>();
        private List<int> DelRow = new List<int>();
        public SettingView(DataView dataView)
        {
            InitializeComponent();
            DataCurrent.ItemsSource = dataView;

            //this.dataView = dataView;

            //List<string> columnNameList = new List<string>();
            //foreach (DataColumn item in dd.Table.Columns)
            //{
            //    columnNameList.Add(item.ColumnName);
            //}
            //listV.ItemsSource =new List<string> ();
        }

        private void MenuItem_Click_Delete_Column(object sender, RoutedEventArgs e)
        {
            DelCol.Remove(listColumn.SelectedItem.ToString());
            listColumn.Items.Remove(listColumn.SelectedItem);
        }
        private void MenuItem_Click_Delete_Row(object sender, RoutedEventArgs e)
        {
            DelRow.Remove(int.Parse(listRow.SelectedItem.ToString()));
            listRow.Items.Remove(listRow.SelectedItem);
        }

        private void Button_Click_Ready(object sender, RoutedEventArgs e)
        {
            DataView dataView = (DataView)DataCurrent.ItemsSource;
            if (dataView == null)
                throw new InvalidOperationException();

            foreach (string item in DelCol) dataView.Table.Columns.Remove(item); // Удаление по имени
            DelRow.Reverse();
            foreach (int item in DelRow) dataView.Table.Rows.RemoveAt(item); // Удаление по индексу

            DataCurrent.ItemsSource = null;
            DataCurrent.ItemsSource = dataView;

            this.DialogResult = true;
            this.Close();
        }

        private void Delete_Collumn(object sender, RoutedEventArgs e)
        {
            foreach (DataGridCellInfo item in DataCurrent.SelectedCells)
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
            DataView dataView = (DataView)DataCurrent.ItemsSource;
            foreach (DataGridCellInfo item in DataCurrent.SelectedCells)
            {
                int tmp = item.Column.DisplayIndex;
                string tmp1 = item.Column.Header.ToString();
            }
            //gg.Table.Rows.RemoveAt(DataG.SelectedIndex);
            DataCurrent.ItemsSource = null;
            DataCurrent.ItemsSource = dataView;
        }

        private void Delete_Row(object sender, RoutedEventArgs e)
        {
            foreach (DataGridCellInfo item in DataCurrent.SelectedCells)
            {
                DataRowView pp = (DataRowView)item.Item;
                int index = DataCurrent.Items.IndexOf(pp);
                var find = DelRow.FindIndex(x => x == index);

                if (find == -1)
                {
                    DelRow.Add(index);
                    listRow.Items.Add(index);
                }
            }
        }

        private void DataCurrent_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = $"{e.Row.GetIndex()}    ";
        }

        protected override void OnClosed(EventArgs e)
        {
            listColumn.Items.Clear();
            listRow.Items.Clear();

            base.OnClosed(e);
        }
    }
}
