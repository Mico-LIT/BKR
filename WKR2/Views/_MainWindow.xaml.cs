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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dr = System.Drawing;
using Tool;
using System.IO;
using Microsoft.Win32;
using System.Data;
using System.ComponentModel;
using Tool.Services.Analitic;
using Tool.Services.Excel;
using Tool.Services;
using Tool.Services.Print;
using WKR2.Core;
using System.Reflection;

namespace WKR2.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class _MainWindow : Window
    {
        public Point PointImag;     // координаты кнопки 
        public Image imageORig;     //оригинал загружен
        public Dr.Bitmap bitmapORig;//оригинал загружен
        private List<UIElement> But_canvas = new List<UIElement>();

        public _MainWindow()
        {
            InitializeComponent(); IssEnabled();
            //List<Date> fd = new List<Date>() { new Date() { jsdkf = "423", NAme = "dsf4", NAme1 = "fs43" } };

            //image.Source = ImageWork.Load();                                   //генирится ошибка вот тут 
            //imageORig = image;
            //bitmapORig = new Dr.Bitmap(ImageWork.Load().UriSource.LocalPath); //генирится ошибка вот тут 

            // сколько кнопок надо на разментку 
            //for (int i = 1; i < 3; i++)
            //{
            //    var but = new Button() { Name = "T_" + i, Content = "**"+i+"**", Margin = new Thickness(0, 0, 0, 0) };
            //    but.MouseDown += new MouseButtonEventHandler(MouseDown);
            //    but.MouseUp += new MouseButtonEventHandler(MouseUp);
            //    but.MouseMove += new MouseEventHandler(MouseMove);
            //    grid_imag.Children.Add(but);
            //}
            //d12.ItemsSource = fd;

        }

        private void Save_img(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пока нет реализации, извините");
            return;

            // белый листок
            //Dr.Bitmap b = new Dr.Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            ////Dr.Bitmap bmp = new Dr.Bitmap(@"c: \users\redga\documents\visual studio 2015\Projects\WKR2\WKR2\ggh.jpg");

            ////Dr.Bitmap b = new Dr.Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            ////using (Dr.Graphics g = Dr.Graphics.FromImage(b))
            ////{
            ////    g.FillRectangle(Dr.Brushes.White, 0, 0, b.Width, b.Height);
            ////}

            //b.Save(@"c:\users\redga\documents\visual studio 2015\Projects\WKR2\WKR2\111.jpg", System.Drawing.Imaging.ImageFormat.Png);

            //Dr.Bitmap bmp = new Dr.Bitmap(@"c: \users\redga\documents\visual studio 2015\Projects\WKR2\WKR2\ggh.png");
            ////bmp = b;


            Dr.Bitmap bmp = new Dr.Bitmap(@"C:\BKR\WKR2\ggh.jpg");
            Dr.Bitmap b = new Dr.Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            using (Dr.Graphics g = Dr.Graphics.FromImage(b))
            {
                g.DrawImage(bmp, 0, 0);
            }

            using (Dr.Graphics g = Dr.Graphics.FromImage(b))
            {
                using (var font = new Dr.Font("Arial", 10))  // настроить шрифт
                {
                    foreach (var item in CanvasForImage.Children)
                    {
                        if (item is Button)
                        {
                            Button f = item as Button;

                            double pixelWidth = image.Source.Width;
                            double pixelHeight = image.Source.Height;
                            PointImag.X = (pixelWidth * f.Margin.Left) / image.ActualWidth;
                            PointImag.Y = (pixelHeight * f.Margin.Top) / image.ActualHeight;

                            g.DrawString("Информационная конференция IT Corparation", font, Dr.Brushes.Black,
                       (float)PointImag.X, (float)PointImag.Y);
                        }
                    }

                }
            }
            b.Save(@"C:\BKR\WKR2\gomer1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            bmp.Dispose();
            b.Dispose();
        }

        //Вывод кардинат где нажал 
        private void imag_click(object sender, MouseButtonEventArgs e)
        {
            //PointImag = e.GetPosition(image);
            //double pixelWidth = image.Source.Width;
            //double pixelHeight = image.Source.Height;
            //PointImag.X = (pixelWidth * PointImag.X ) / image.ActualWidth;
            //PointImag.Y = (pixelHeight * PointImag.Y )/ image.ActualHeight;


            //point.Content = String.Format("x={0}  Y={1}", PointImag.X, PointImag.Y); 
        }


        //новое окно 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bitmapORig == null)
                {
                    MessageBox.Show("Нужно загрузить шаблон\\картинку для работы", "Информация");
                    return;
                }

                var ddd = DataGridMain.SelectedIndex;
                //Print_Item(ddd);
                ShowMessAnalitic();
                PrintService.dd(Print_Item2, GetParametrAnalitic, Core.AppSettings.PathLocal, ddd + 1);

            }
            catch (Exception)
            {
                throw;
            }
        }
        //забросил метод
        public void Print_Item(int Item_Row)
        {
            Dr.Bitmap b = new Dr.Bitmap(bitmapORig.Width, bitmapORig.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Dr.Image vie;

            using (Dr.Graphics g = Dr.Graphics.FromImage(b))
            {
                g.Clear(System.Drawing.Color.White);


                foreach (var item in CanvasForImage.Children)
                {
                    if (item is Button)
                    {
                        Button f = item as Button;
                        int i = 0;
                        double pixelWidth = image.Source.Width;
                        double pixelHeight = image.Source.Height;
                        PointImag.X = (pixelWidth * f.Margin.Left) / image.ActualWidth;
                        PointImag.Y = (pixelHeight * f.Margin.Top) / image.ActualHeight;
                        var yy = (DataView)DataGridMain.ItemsSource;
                        foreach (DataColumn ii in yy.Table.Columns)
                        {
                            if (ii.ColumnName == f.Name) break;
                            i++;
                        }
                        string TEXT = (yy.Table.Rows[Item_Row].ItemArray[i]).ToString();

                        var trt = PrintService.ButtonFontDictionary.FirstOrDefault(x => f == x.Key).Value;
                        if (trt == null) { trt = PrintService.FontCurrent; }

                        g.DrawString(TEXT, trt, Dr.Brushes.Black,
                            new Dr.RectangleF(
                                (float)PointImag.X,
                                (float)PointImag.Y,
                                (float)(f.Width * 3.3),
                                (float)(f.Height * 3.3)));
                    }
                }

            }

            using (MemoryStream tmpStrm = new MemoryStream())
            {
                b.Save(tmpStrm, System.Drawing.Imaging.ImageFormat.Png);
                vie = Dr.Image.FromStream(tmpStrm);
            }
            b.Dispose();
            PrintService.ImageCurrent = vie;
            //Tool.Print.dd();
        }
        public void Previwe(int row = 0)
        {
            try
            {

                if (bitmapORig == null)
                {
                    MessageBox.Show("Нужно загрузить шаблон\\картинку для работы", "Информация");
                    return;
                }

                int tt = 0;
                //Dr.Bitmap bitmapORig = this.bitmapORig;
                Dr.Bitmap b = new Dr.Bitmap(bitmapORig.Width + tt, bitmapORig.Height + tt, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                Dr.Image vie;

                using (Dr.Graphics g = Dr.Graphics.FromImage(b)) { g.DrawImage(bitmapORig, 0, 0); }

                using (Dr.Graphics g = Dr.Graphics.FromImage(b))
                {
                    //g.Clear(System.Drawing.Color.White); 
                    //g.FillRectangle(Dr.Brushes.White, -50, -50, b.Width, b.Height); //белый листок

                    //using (var font = new Dr.Font("Arial", 15))
                    //{

                    foreach (var item in CanvasForImage.Children)
                    {
                        if (item is Button)
                        {
                            Button f = item as Button;
                            int i = 0;
                            double pixelWidth = image.Source.Width;
                            double pixelHeight = image.Source.Height;
                            PointImag.X = (pixelWidth * f.Margin.Left) / image.ActualWidth;
                            PointImag.Y = (pixelHeight * f.Margin.Top) / image.ActualHeight;
                            var yy = (DataView)DataGridMain.ItemsSource;
                            foreach (DataColumn ii in yy.Table.Columns)
                            {
                                if (ii.ColumnName == f.Name) break;
                                i++;
                            }
                            string TEXT = (yy.Table.Rows[row].ItemArray[i]).ToString();

                            var trt = PrintService.ButtonFontDictionary.FirstOrDefault(x => f == x.Key).Value;
                            if (trt == null) { trt = PrintService.FontCurrent; }

                            g.DrawString(TEXT/*+"123456786543213453421224234234"*/, trt, Dr.Brushes.Black,
                                new Dr.RectangleF((float)PointImag.X, (float)PointImag.Y, (float)(f.Width * 3.3), (float)(f.Height * 3.3)));

                            //     g.DrawString(TEXT+"</br> 3efdvgt4", font, Dr.Brushes.Black,
                            //(float)PointImag.X, (float)PointImag.Y);
                        }
                    }

                    //}
                }

                string ff = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                b.Save(ff + "\\PreView.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                using (MemoryStream tmpStrm = new MemoryStream())
                {
                    b.Save(tmpStrm, System.Drawing.Imaging.ImageFormat.Png);
                    vie = Dr.Image.FromStream(tmpStrm);

                }
                b.Dispose();

                PrintService.ImageCurrent = vie;
                Views.PreView pre = new Views.PreView(vie);
                pre.WindowState = WindowState.Maximized;
                pre.ShowDialog();

            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
        }

        #region Движение мыши

        bool isMoved = false;
        Point startMovePosition;
        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                isMoved = true;
                startMovePosition = e.GetPosition(this);
            }
        }
        private void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Released)
            {
                isMoved = false;

            }
        }
        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoved)
            {
                Point currentPoint = e.GetPosition(CanvasForImage);

                Button rect = sender as Button;
                rect.Margin = new Thickness(currentPoint.X - 10, currentPoint.Y - 10, 0, 0);
            }
        }

        #endregion

        #region xren
        //bool m_IsPressed;
        //private void Button_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (m_IsPressed)
        //    {
        //        Button bb = sender as Button;
        //        TranslateTransform transform = new TranslateTransform();
        //        transform.X = Mouse.GetPosition(GLABN_GRID).X;
        //        transform.Y = Mouse.GetPosition(GLABN_GRID).Y;
        //        bb.RenderTransform = transform;
        //    }
        //}

        //private void MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    m_IsPressed = true;
        //}

        //private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        //{

        //}
        #endregion


        private void d12_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var fg = e.Row.Header = (e.Row.GetIndex() + 1).ToString() + "    ";

        }
        // сколько кнопок надо на разментку 
        private void Button_ADD_Canvas(string str)
        {
            DataView colbot = (DataView)DataGridMain.ItemsSource;

            //foreach (DataColumn item in colbot.Table.Columns)
            //{
            //var but = new Button() { Name = item.ColumnName, Height = 20, Width = 100, Content = "**" + item.ColumnName + "**", Margin = new Thickness(0, 0, 0, 0) };

            Button but = new Button() { Name = str, Height = 20, Width = 100, Content = "**" + str + "**", Margin = new Thickness(0, 0, 0, 0) };
            but.MouseDown += new MouseButtonEventHandler(MouseDown);
            but.MouseUp += new MouseButtonEventHandler(MouseUp);
            but.MouseMove += new MouseEventHandler(MouseMove);
            but.Click += (r, t) =>
            {
                Button bu = r as Button;
                Views.Button_Calibration ff = new Views.Button_Calibration(ref bu);
                ff.ShowDialog();
            };
            //var df=But_canvas.Find(x => ((Button)x).Name == str);
            //if (df != null) but.Content = "Копия " + but.Name;//throw new Exception() { Source = " Елемент такой уже добавлен!" };


            CanvasForImage.Children.Add(but);
            But_canvas.Add(but);
            //}
        }

        private void JJson(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пока нет реализации, извините");
            return;

            Tool.Services.Helper.JsonSerializeObject(((DataView)DataGridMain.ItemsSource).ToTable());
        }

        private void delete_row(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DataView gg = (DataView)DataGridMain.ItemsSource;
                gg.Table.Rows.RemoveAt(DataGridMain.SelectedIndex);
                DataGridMain.ItemsSource = null;
                DataGridMain.ItemsSource = gg;
                //d12.Items.Remove(d12.SelectedItem);// напрямую с ним работать нельзя надо посредника DataTable
            }
        }
        private void ShowMessAnalitic()
        {
            MessageBoxResult result = MessageBox.Show("Включить аналитик?!", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            switch (result)
            {
                case MessageBoxResult.No:
                    AnaliticService.GetSettingOnAnalitic = false;
                    break;
                case MessageBoxResult.Yes:
                    AnaliticService.GetSettingOnAnalitic = true;
                    break;
                default:
                    break;
            }
        }

        private string GetParametrAnalitic(int Row)
        {
            string str = String.Format("{0}_{1}_{2}_",
                ((DataView)DataGridMain.ItemsSource).Table.Rows[Row].ItemArray[AnaliticService.PARAMS.Params_1].ToString(),
                ((DataView)DataGridMain.ItemsSource).Table.Rows[Row].ItemArray[AnaliticService.PARAMS.Params_2].ToString(),
                DateTime.Now.ToShortDateString()
                );
            return str;
        }

        private void Pehat(object sender, RoutedEventArgs e)
        {
            ShowMessAnalitic();
            PrintService.dd(Print_Item2, GetParametrAnalitic, Core.AppSettings.PathLocal);
        }


        public Dr.Image Print_Item2(int Item_Row, int rez = 0)
        {
            Dr.Bitmap b = new Dr.Bitmap(bitmapORig.Width, bitmapORig.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Dr.Image vie;
            if (rez != 0) using (Dr.Graphics g = Dr.Graphics.FromImage(b)) { g.DrawImage(bitmapORig, 0, 0); }

            using (Dr.Graphics g = Dr.Graphics.FromImage(b))
            {
                if (rez == 0) g.Clear(System.Drawing.Color.White);


                foreach (var item in CanvasForImage.Children)
                {
                    if (item is Button)
                    {
                        Button f = item as Button;
                        int i = 0;
                        double pixelWidth = image.Source.Width;
                        double pixelHeight = image.Source.Height;
                        PointImag.X = (pixelWidth * f.Margin.Left) / image.ActualWidth;
                        PointImag.Y = (pixelHeight * f.Margin.Top) / image.ActualHeight;
                        var yy = (DataView)DataGridMain.ItemsSource;
                        foreach (DataColumn ii in yy.Table.Columns)
                        {
                            if (ii.ColumnName == f.Name) break;
                            i++;
                        }
                        string TEXT = (yy.Table.Rows[Item_Row].ItemArray[i]).ToString();

                        var trt = PrintService.ButtonFontDictionary.FirstOrDefault(x => f == x.Key).Value;
                        if (trt == null) { trt = PrintService.FontCurrent; }

                        g.DrawString(TEXT, trt, Dr.Brushes.Black,
                            new Dr.RectangleF(
                                (float)PointImag.X,
                                (float)PointImag.Y,
                                (float)(f.Width * 3.3),
                                (float)(f.Height * 3.3)));
                    }
                }

            }

            using (MemoryStream tmpStrm = new MemoryStream())
            {
                b.Save(tmpStrm, System.Drawing.Imaging.ImageFormat.Png);
                vie = Dr.Image.FromStream(tmpStrm);
            }
            b.Dispose();
            return vie;

        }


        private void Calibration_Click(object sender, RoutedEventArgs e)
        {
            WKR2.Views.Calibration gf = new Views.Calibration(PrintService.CalibrationData);
            gf.ShowDialog();
        }

        private void Font_click(object sender, RoutedEventArgs e)
        {
            PrintService.FontCurrent = PrintService.Font();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Previwe();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //Зачем сука???
        //public void Clear_button()
        //{
        //    for (int i = grid_imag.Children.Count-1; i >= 0 ; i--)
        //    {
        //        Button pp = grid_imag.Children[i] as Button;
        //        if (pp != null) grid_imag.Children.Remove(pp);
        //    }

        //}

        private void Add_Convas(object sender, RoutedEventArgs e)
        {
            try
            {
                string fg = (string)DataGridMain.CurrentCell.Column.Header;
                DataGridMain.SelectedIndex = -1;
                Button_ADD_Canvas(fg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                DataGridMain.SelectedIndex = -1;
            }

        }

        private void Del_Button_Convas(object sender, RoutedEventArgs e)
        {
            try
            {

                string fg = (string)DataGridMain.CurrentCell.Column.Header;
                var h = But_canvas.FindLast(x => ((Button)x).Name == fg);
                DataGridMain.SelectedIndex = -1;
                if (h == null) return;

                PrintService.ButtonFontDictionary.Remove((Button)h);
                CanvasForImage.Children.Remove(h);
                But_canvas.Remove(h);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source);
                DataGridMain.SelectedIndex = -1;
            }
        }

        private void Save_Serial(object sender, RoutedEventArgs e)
        {
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(Core.AppSettings.PathLocal + @"\Patern");
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "Dat files (*.dat)|*.dat";
                SFD.DefaultExt = di.FullName;
                if (SFD.ShowDialog() == true)
                {
                    List<Setting_Button> but = new List<Setting_Button>();
                    foreach (UIElement item in CanvasForImage.Children)
                    {
                        but.Add(new Setting_Button()
                        {
                            Name = ((Button)item).Name,
                            Height = ((Button)item).Height,
                            Width = ((Button)item).Width,
                            MarginB = ((Button)item).Margin.Bottom,
                            MarginL = ((Button)item).Margin.Left,
                            MarginR = ((Button)item).Margin.Right,
                            MarginT = ((Button)item).Margin.Top,
                            Font = PrintService.ButtonFontDictionary.FirstOrDefault(x => x.Key == (Button)item).Value
                        });
                    }

                    DataPattern dataPattern = new DataPattern()
                    {
                        Image = bitmapORig,
                        But_canvas = but,
                        font = PrintService.FontCurrent,
                        calibration_data = PrintService.CalibrationData,
                        par = AnaliticService.PARAMS
                    };

                    Helper.SerializationDataPattern(dataPattern, SFD.FileName);

                    MessageBox.Show("Сохранение прошло успешно");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при сохранении", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public BitmapSource ConvertToBitmapSource(Dr.Bitmap bitmap)
        {
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
            (
            bitmap.GetHbitmap(),
            IntPtr.Zero,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions()
            );
            return bitmapSource;
        }

        private void Button_SERi_Canvas(List<Setting_Button> lis)
        {
            foreach (Setting_Button item in lis)
            {


                Button but = new Button()
                {
                    Name = item.Name,
                    Height = item.Height,
                    Width = item.Width,
                    Content = "**" + item.Name + "**",
                    Margin = new Thickness(item.MarginL, item.MarginT, item.MarginR, item.MarginB)
                };
                but.MouseDown += new MouseButtonEventHandler(MouseDown);
                but.MouseUp += new MouseButtonEventHandler(MouseUp);
                but.MouseMove += new MouseEventHandler(MouseMove);
                but.Click += (r, t) =>
                {
                    Button bu = r as Button;
                    Views.Button_Calibration ff = new Views.Button_Calibration(ref bu);
                    ff.ShowDialog();
                };
                //var df = But_canvas.Find(x => ((Button)x).Name == str);
                //if (df != null) throw new Exception() { Source = " Елемент такой уже добавлен!" };
                if (item.Font != null) PrintService.ButtonFontDictionary.Add(but, item.Font);
                CanvasForImage.Children.Add(but);
                But_canvas.Add(but);

            }
        }
        private void MenuItem_Click_1(object sender, MouseEventArgs e)
        {
            IssEnabled();
        }
        private void IssEnabled()
        {
            if (DataGridMain.Items.Count > 0)
            {
                MIOpenImage.IsEnabled = MIDownloadPattern.IsEnabled = MISavePattent.IsEnabled
                    = MIGroupAnalitic.IsEnabled = D4.IsEnabled = poisk.IsEnabled = true;
                MISavePattent.IsEnabled = (image.Source == null) ? false : true;
            }
            else
                MIOpenImage.IsEnabled = MIDownloadPattern.IsEnabled = MISavePattent.IsEnabled = MIGroupAnalitic.IsEnabled = D4.IsEnabled = poisk.IsEnabled = false;
        }

        private void Poisk(object sender, RoutedEventArgs e)
        {
            var fg = new Views.Poisk(this);
            fg.Show();
        }

        private void d12_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            //e.Column.Header =;

        }

        private void SettingAnalitic_Click(object sender, RoutedEventArgs e)
        {
            Views.AnalliticSetings an = new Views.AnalliticSetings(Com.Items);

            an.ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Previwe(DataGridMain.SelectedIndex);
        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            ExcelService.ExportToExcel(Core.AppSettings.PathLocal);
            MessageBox.Show("Выгрузка прошла успешно!");
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var contextMenu = (System.Windows.Controls.ContextMenu)sender;

            if (contextMenu == null)
                throw new InvalidOperationException();

            foreach (MenuItem item in ((ContextMenu)e.Source).Items)
                item.IsEnabled = (DataGridMain.Columns.Count <= 1) ? false : true;
        }

        private void OpenExample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                //загругка данных
                using (Stream stream = assembly.GetManifestResourceStream(AppSettings.ResourceNameTestData))
                {
                    DataView dd = ExcelService.LoadrExcel(stream);
                    CanvasForImage.Children.Clear();
                    DataGridMain.ItemsSource = null;
                    DataGridMain.ItemsSource = dd;
                }

                //загруска картинки\шаблона
                using (var stream = assembly.GetManifestResourceStream(AppSettings.ResourceNameImageTemplate))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    image.Source = bitmap;
                    imageORig = image;

                    bitmapORig = new Dr.Bitmap(stream);
                }

                But_canvas.Clear();
                CanvasForImage.Children.Clear();

                // для поиска
                foreach (var item in DataGridMain.Columns)
                    if (item is DataGridTextColumn) Com.Items.Add((string)item.Header);

                IssEnabled();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке!");
            }
        }

        private void OpenImageMI_Click(object sender, RoutedEventArgs e)
        {
            Views.AnalliticSetings AS = new Views.AnalliticSetings(Com.Items);
            AS.ShowDialog();
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Jpeg files (*.jpeg)|*.jpeg;*.jpg| PNG files (*.PNG)|*.png|Все файлы (*.*)|*.*";
            if (OFD.ShowDialog() == true)
            {
                //Clear_button();
                image.Source = new BitmapImage(new Uri(OFD.FileName));                                   //генирится ошибка вот тут 
                imageORig = image;
                bitmapORig = new Dr.Bitmap(new BitmapImage(new Uri(OFD.FileName)).UriSource.LocalPath); //генирится ошибка вот тут 
                But_canvas.Clear();
                CanvasForImage.Children.Clear();
            }
        }

        private void OpenExcelMI_Click(object sender, RoutedEventArgs e)
        {
            //Clear_button();
            try
            {
                DataView dd = ExcelService.LoadrExcel();
                //if (dd != null) d12.ItemsSource = dd;

                Views.SettingView vv = new Views.SettingView(dd);
                if (dd != null)
                    if (vv.ShowDialog() == true)
                    {
                        CanvasForImage.Children.Clear();
                        DataGridMain.ItemsSource = null;
                        DataGridMain.ItemsSource = dd;
                        DataView colbot = dd;
                        var ff = DataGridMain.Columns;
                        foreach (var item in ff)
                        {
                            if (item is DataGridTextColumn) Com.Items.Add((string)item.Header);
                        }
                    }

                IssEnabled();

                //Сохранить в Json Парметр Excel измененого
                //Tool.ExcelWork.Json(((DataView)d12.ItemsSource).ToTable());
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при загрузке!");
            }
        }

        private void DownloadPatternMI_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.Filter = "Dat files (*.dat)|*.dat";
                OFD.InitialDirectory = Core.AppSettings.PathLocal;
                if (OFD.ShowDialog() == true)
                {
                    But_canvas.Clear();
                    CanvasForImage.Children.Clear();

                    DataPattern ds = Helper.DeSerializationDataPattern(OFD.FileName);

                    //Tool.Print.But_font = ds.BF;
                    PrintService.FontCurrent = ds.font;
                    PrintService.CalibrationData = ds.calibration_data;
                    AnaliticService.PARAMS = ds.par;
                    image.Source = ConvertToBitmapSource(ds.Image);
                    Button_SERi_Canvas(ds.But_canvas);
                    bitmapORig = ds.Image;
                    MessageBox.Show("Загрузка прошла успешно");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при загрузки", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
