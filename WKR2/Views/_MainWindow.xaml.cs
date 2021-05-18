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
using Drawing = System.Drawing;
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
        Drawing.Bitmap bitmapImageOriginal;//оригинал загружен
        List<UIElement> canvasOnButtons = new List<UIElement>();

        public _MainWindow()
        {
            InitializeComponent(); IssEnabledAllElementsControl();
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

        private void Previwe(int row = 0)
        {
            try
            {
                if (row == -1) return;

                if (bitmapImageOriginal == null)
                {
                    MessageBox.Show("Нужно загрузить шаблон\\картинку для работы", "Информация");
                    return;
                }

                //int tt = 0;
                //Dr.Bitmap bitmapORig = this.bitmapORig;
                //using (Drawing.Graphics g = Drawing.Graphics.FromImage(b)) { g.DrawImage(bitmapImageOriginal, 0, 0); }

                Drawing.Image drawingImage;

                using (Drawing.Bitmap bitmap = new Drawing.Bitmap(bitmapImageOriginal))
                {
                    using (Drawing.Graphics graphics = Drawing.Graphics.FromImage(bitmap))
                    {
                        //g.Clear(System.Drawing.Color.White); 
                        //g.FillRectangle(Dr.Brushes.White, -50, -50, b.Width, b.Height); //белый листок

                        //using (var font = new Dr.Font("Arial", 15))
                        //{

                        Point pointImage = new Point();

                        foreach (var itemUI in CanvasForImage.Children.OfType<Button>())
                        {
                            //if (itemUI is Button)
                            //{
                                int i = 0;
                                Button buttonOnCanvas = itemUI;

                                double pixelWidth = ((BitmapImage)ImageMainControl.Source).PixelWidth;
                                double pixelHeight = ((BitmapImage)ImageMainControl.Source).PixelHeight;
                                pointImage.X = (pixelWidth * buttonOnCanvas.Margin.Left) / ImageMainControl.ActualWidth;
                                pointImage.Y = (pixelHeight * buttonOnCanvas.Margin.Top) / ImageMainControl.ActualHeight;

                                DataView sourceDGM = (DataView)DataGridMain.ItemsSource;
                                foreach (DataColumn dataColumn in sourceDGM.Table.Columns)
                                {
                                    if (dataColumn.ColumnName == buttonOnCanvas.Name) break;
                                    i++;
                                }
                                string text = (sourceDGM.Table.Rows[row].ItemArray[i]).ToString();

                                Drawing.Font font = PrintService.ButtonFontDictionary.FirstOrDefault(x => buttonOnCanvas == x.Key).Value;

                                if (font == null)
                                    font = PrintService.FontCurrent;

                                const float magicNumber = 3.3f;

                                graphics.DrawString(text, font, Drawing.Brushes.Black, new Drawing.RectangleF
                                    (
                                        (float)pointImage.X,
                                        (float)pointImage.Y,
                                        (float)(buttonOnCanvas.Width * magicNumber),
                                        (float)(buttonOnCanvas.Height * magicNumber))
                                    );

                                //     g.DrawString(TEXT+"</br> 3efdvgt4", font, Dr.Brushes.Black,
                                //(float)PointImag.X, (float)PointImag.Y);
                            //}
                        }

                        //}
                    }

                    string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    bitmap.Save(path + "\\PreView.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);

                    using (MemoryStream tmpStrm = new MemoryStream())
                    {
                        bitmap.Save(tmpStrm, System.Drawing.Imaging.ImageFormat.Png);
                        drawingImage = Drawing.Image.FromStream(tmpStrm);
                    }
                }

                PrintService.ImageCurrent = drawingImage;
                Views.PreView pre = new Views.PreView(drawingImage);
                pre.WindowState = WindowState.Maximized;
                pre.ShowDialog();

            }
            catch (Exception ex)
            {
                var tmp = ex;
                MessageBox.Show("Error");
            }
        }

        private void ShowMessageBoxAnalitic()
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
            var dataRowCollencion = ((DataView)DataGridMain.ItemsSource).Table.Rows[Row];

            return string.Format("{0}_{1}_{2}_",
                dataRowCollencion.ItemArray[AnaliticService.PARAMS.Params_1].ToString(),
                dataRowCollencion.ItemArray[AnaliticService.PARAMS.Params_2].ToString(),
                DateTime.Now.ToShortDateString()
                );
        }
        //TODO !
        private Drawing.Image Print_Item2(int Item_Row, int rez = 0)
        {
            Drawing.Bitmap b = new Drawing.Bitmap(bitmapImageOriginal.Width, bitmapImageOriginal.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Drawing.Image vie;
            if (rez != 0) using (Drawing.Graphics g = Drawing.Graphics.FromImage(b)) { g.DrawImage(bitmapImageOriginal, 0, 0); }

            using (Drawing.Graphics g = Drawing.Graphics.FromImage(b))
            {
                if (rez == 0) g.Clear(System.Drawing.Color.White);

                Point pointImage = new Point();

                foreach (var item in CanvasForImage.Children)
                {
                    if (item is Button)
                    {
                        Button f = item as Button;
                        int i = 0;
                        double pixelWidth = ImageMainControl.Source.Width;
                        double pixelHeight = ImageMainControl.Source.Height;
                        pointImage.X = (pixelWidth * f.Margin.Left) / ImageMainControl.ActualWidth;
                        pointImage.Y = (pixelHeight * f.Margin.Top) / ImageMainControl.ActualHeight;
                        var yy = (DataView)DataGridMain.ItemsSource;
                        foreach (DataColumn ii in yy.Table.Columns)
                        {
                            if (ii.ColumnName == f.Name) break;
                            i++;
                        }
                        string TEXT = (yy.Table.Rows[Item_Row].ItemArray[i]).ToString();

                        var trt = PrintService.ButtonFontDictionary.FirstOrDefault(x => f == x.Key).Value;
                        if (trt == null) { trt = PrintService.FontCurrent; }

                        g.DrawString(TEXT, trt, Drawing.Brushes.Black,
                            new Drawing.RectangleF(
                                (float)pointImage.X,
                                (float)pointImage.Y,
                                (float)(f.Width * 3.3),
                                (float)(f.Height * 3.3)));
                    }
                }

            }

            using (MemoryStream tmpStrm = new MemoryStream())
            {
                b.Save(tmpStrm, System.Drawing.Imaging.ImageFormat.Png);
                vie = Drawing.Image.FromStream(tmpStrm);
            }
            b.Dispose();
            return vie;

        }

        private void Button_SERi_Canvas(IList<SettingButton> settingButtons)
        {
            foreach (SettingButton item in settingButtons)
            {
                Button button = new Button()
                {
                    Name = item.Name,
                    Height = item.Height,
                    Width = item.Width,
                    Content = "**" + item.Name + "**",
                    Margin = new Thickness(item.MarginL, item.MarginT, item.MarginR, item.MarginB)
                };

                button.MouseDown += new MouseButtonEventHandler(MouseDown);
                button.MouseUp += new MouseButtonEventHandler(MouseUp);
                button.MouseMove += new MouseEventHandler(MouseMove);
                button.Click += (sender, e) =>
                {
                    Button buttonCurrent = sender as Button;
                    Views.Button_Calibration windowButtonCalibration = new Views.Button_Calibration(ref buttonCurrent);
                    windowButtonCalibration.ShowDialog();
                };

                //var df = But_canvas.Find(x => ((Button)x).Name == str);
                //if (df != null) throw new Exception() { Source = " Елемент такой уже добавлен!" };
                if (item.Font != null) 
                    PrintService.ButtonFontDictionary.Add(button, item.Font);

                CanvasForImage.Children.Add(button);
                //TODO !
                canvasOnButtons.Add(button);
            }
        }

        //Вывод кардинат где нажал 
        private void ImageMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
#if DEBUG
            Point pointOnImageClick = new Point();

            pointOnImageClick = e.GetPosition(ImageMainControl);
            double pixelWidth = ImageMainControl.Source.Width;
            double pixelHeight = ImageMainControl.Source.Height;
            pointOnImageClick.X = (pixelWidth * pointOnImageClick.X) / ImageMainControl.ActualWidth;
            pointOnImageClick.Y = (pixelHeight * pointOnImageClick.Y) / ImageMainControl.ActualHeight;

            point.Content = string.Format("x={0}  Y={1}", pointOnImageClick.X, pointOnImageClick.Y);

#endif
        }

        private void IssEnabledAllElementsControl()
        {
            if (DataGridMain.Items.Count > 0)
            {
                MIOpenImage.IsEnabled = MIDownloadPattern.IsEnabled = MISavePattent.IsEnabled =
                MIGroupAnalitic.IsEnabled = MIPreView.IsEnabled = poisk.IsEnabled = true;

                MISavePattent.IsEnabled = (ImageMainControl.Source == null) ? false : true;
            }
            else
            {
                MIOpenImage.IsEnabled = MIDownloadPattern.IsEnabled = MISavePattent.IsEnabled =
                  MIGroupAnalitic.IsEnabled = MIPreView.IsEnabled = poisk.IsEnabled = false;
            }
        }

        private void ButtonAddOnCanvas(string nameButton)
        {
            DataView dataView = (DataView)DataGridMain.ItemsSource;

            //foreach (DataColumn item in colbot.Table.Columns)
            //{
            //var but = new Button() { Name = item.ColumnName, Height = 20, Width = 100, Content = "**" + item.ColumnName + "**", Margin = new Thickness(0, 0, 0, 0) };

            Button button = new Button()
            {
                Name = nameButton,
                Height = 20,
                Width = 100,
                Content = "**" + nameButton + "**",
                Margin = new Thickness(0, 0, 0, 0)
            };

            button.MouseDown += new MouseButtonEventHandler(MouseDown);
            button.MouseUp += new MouseButtonEventHandler(MouseUp);
            button.MouseMove += new MouseEventHandler(MouseMove);
            button.Click += (sender, e) =>
            {
                Button buttonCurrent = sender as Button;
                Views.Button_Calibration windowButtonCalibration = new Views.Button_Calibration(ref buttonCurrent);
                windowButtonCalibration.ShowDialog();
            };

            //var df=But_canvas.Find(x => ((Button)x).Name == str);
            //if (df != null) but.Content = "Копия " + but.Name;//throw new Exception() { Source = " Елемент такой уже добавлен!" };


            CanvasForImage.Children.Add(button);
            canvasOnButtons.Add(button);
            //}
        }

        #region Mouse Event

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
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoved)
            {
                Point currentPoint = e.GetPosition(CanvasForImage);

                Button rect = sender as Button;
                rect.Margin = new Thickness(currentPoint.X - 10, currentPoint.Y - 10, 0, 0);
            }
        }

        #endregion

        #region DataGridMain

        private void DataGridMain_LoadingRow(object sender, DataGridRowEventArgs e) => e.Row.Header = $"{(e.Row.GetIndex() + 1)}    ";

        private void DataGridMain_ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var contextMenu = (System.Windows.Controls.ContextMenu)sender;

            if (contextMenu == null)
                throw new InvalidOperationException();

            foreach (MenuItem item in ((ContextMenu)e.Source).Items)
                item.IsEnabled = (DataGridMain.Columns.Count <= 1) ? false : true;
        }

        //новое окно 
        private void DataGridMain_Button_PrintItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bitmapImageOriginal == null)
                {
                    MessageBox.Show("Нужно загрузить шаблон\\картинку для работы", "Информация");
                    return;
                }

                var ddd = DataGridMain.SelectedIndex;
                //Print_Item(ddd);
                ShowMessageBoxAnalitic();
                PrintService.dd(Print_Item2, GetParametrAnalitic, Core.AppSettings.PathLocal, ddd + 1);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DataGridMain_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void DataGridMain_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //e.Column.Header =;
        }

        #endregion

        #region MenuItem

        private void MenuItem_Calibration_Click(object sender, RoutedEventArgs e) => new Views.Calibration(PrintService.CalibrationData).ShowDialog();
        private void MenuItem_Font_click(object sender, RoutedEventArgs e) => PrintService.FontCurrent = PrintService.Font();
        private void MenuItem_PreView_Click(object sender, RoutedEventArgs e) => Previwe();
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) => this.Close();
        private void MenuItem_GroupFile_MouseMove(object sender, MouseEventArgs e) => IssEnabledAllElementsControl();

        private void MenuItem_DGM_PreView(object sender, RoutedEventArgs e) => Previwe(DataGridMain.SelectedIndex);
        private void MenuItem_DGM_AddConvasOnButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridMain.CurrentCell.Column == null)
                    return;

                string fg = (string)DataGridMain.CurrentCell.Column.Header;
                DataGridMain.SelectedIndex = -1;
                ButtonAddOnCanvas(fg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                DataGridMain.SelectedIndex = -1;
            }

        }
        private void MenuItem_DGM_ButtonRemoveOnConvas(object sender, RoutedEventArgs e)
        {
            try
            {
                string fg = (string)DataGridMain.CurrentCell.Column.Header;
                var h = canvasOnButtons.FindLast(x => ((Button)x).Name == fg);
                DataGridMain.SelectedIndex = -1;
                if (h == null) return;

                PrintService.ButtonFontDictionary.Remove((Button)h);
                CanvasForImage.Children.Remove(h);
                canvasOnButtons.Remove(h);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source);
                DataGridMain.SelectedIndex = -1;
            }
        }


        private void MenuItem_OpenExample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                //загругка данных
                using (Stream stream = assembly.GetManifestResourceStream(AppSettings.ResourceNameTestData))
                {
                    DataView dataView = ExcelService.LoadrExcel(stream);
                    CanvasForImage.Children.Clear();
                    DataGridMain.ItemsSource = null;
                    DataGridMain.ItemsSource = dataView;
                }

                //загруска картинки\шаблона
                using (var stream = assembly.GetManifestResourceStream(AppSettings.ResourceNameImageTemplate))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();

                    ImageMainControl.Source = bitmapImage;

                    bitmapImageOriginal = new Drawing.Bitmap(stream);
                }

                canvasOnButtons.Clear();
                CanvasForImage.Children.Clear();

                // для поиска
                foreach (var item in DataGridMain.Columns)
                    if (item is DataGridTextColumn) Com.Items.Add((string)item.Header);

                IssEnabledAllElementsControl();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке!");
            }
        }

        private void MenuItem_OpenImage_Click(object sender, RoutedEventArgs e)
        {
            Views.AnalliticSetings AS = new Views.AnalliticSetings(Com.Items);
            AS.ShowDialog();
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Jpeg files (*.jpeg)|*.jpeg;*.jpg| PNG files (*.PNG)|*.png|Все файлы (*.*)|*.*";
            if (OFD.ShowDialog() == true)
            {

                var uri = new Uri(OFD.FileName);
                //Clear_button();
                ImageMainControl.Source = new BitmapImage(uri);
                bitmapImageOriginal = new Drawing.Bitmap(uri.LocalPath);
                canvasOnButtons.Clear();
                CanvasForImage.Children.Clear();
            }
        }

        private void MenuItem_OpenExcel_Click(object sender, RoutedEventArgs e)
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

                IssEnabledAllElementsControl();

                //Сохранить в Json Парметр Excel измененого
                //Tool.ExcelWork.Json(((DataView)d12.ItemsSource).ToTable());
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при загрузке!");
            }
        }

        private void MenuItem_DownloadPattern_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.Filter = "Dat files (*.dat)|*.dat";
                OFD.InitialDirectory = Core.AppSettings.PathLocal;
                if (OFD.ShowDialog() == true)
                {
                    canvasOnButtons.Clear();
                    CanvasForImage.Children.Clear();

                    DataPattern dataPatternModel = Helper.DeSerializationDataPattern(OFD.FileName);

                    //Tool.Print.But_font = ds.BF;
                    PrintService.FontCurrent = dataPatternModel.font;
                    PrintService.CalibrationData = dataPatternModel.calibration_data;
                    AnaliticService.PARAMS = dataPatternModel.par;

                    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
                    (
                        dataPatternModel.Image.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions()
                    );

                    ImageMainControl.Source = bitmapSource;
                    Button_SERi_Canvas(dataPatternModel.But_canvas);
                    bitmapImageOriginal = dataPatternModel.Image;
                    MessageBox.Show("Загрузка прошла успешно");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при загрузки", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_SettingAnalitic_Click(object sender, RoutedEventArgs e)
        {
            Views.AnalliticSetings an = new Views.AnalliticSetings(Com.Items);
            an.ShowDialog();
        }

        private void MenuItem_ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            ExcelService.ExportToExcel(Core.AppSettings.PathLocal);
            MessageBox.Show("Выгрузка прошла успешно!");
        }

        private void MenuItem_Save_Serial(object sender, RoutedEventArgs e)
        {
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(Core.AppSettings.PathLocal + @"\Patern");
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = "Dat files (*.dat)|*.dat";
                SFD.DefaultExt = di.FullName;
                if (SFD.ShowDialog() == true)
                {
                    List<SettingButton> but = new List<SettingButton>();
                    foreach (UIElement item in CanvasForImage.Children)
                    {
                        but.Add(new SettingButton()
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
                        Image = bitmapImageOriginal,
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



        #endregion

        #region Код для 2-ой версии проекта

        //забросил метод
        private void Print_Item(int Item_Row)
        {
            Drawing.Bitmap b = new Drawing.Bitmap(bitmapImageOriginal.Width, bitmapImageOriginal.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Drawing.Image vie;

            using (Drawing.Graphics g = Drawing.Graphics.FromImage(b))
            {
                g.Clear(System.Drawing.Color.White);

                Point pointImage = new Point();

                foreach (var item in CanvasForImage.Children)
                {
                    if (item is Button)
                    {
                        Button f = item as Button;
                        int i = 0;
                        double pixelWidth = ImageMainControl.Source.Width;
                        double pixelHeight = ImageMainControl.Source.Height;
                        pointImage.X = (pixelWidth * f.Margin.Left) / ImageMainControl.ActualWidth;
                        pointImage.Y = (pixelHeight * f.Margin.Top) / ImageMainControl.ActualHeight;
                        var yy = (DataView)DataGridMain.ItemsSource;
                        foreach (DataColumn ii in yy.Table.Columns)
                        {
                            if (ii.ColumnName == f.Name) break;
                            i++;
                        }
                        string TEXT = (yy.Table.Rows[Item_Row].ItemArray[i]).ToString();

                        var trt = PrintService.ButtonFontDictionary.FirstOrDefault(x => f == x.Key).Value;
                        if (trt == null) { trt = PrintService.FontCurrent; }

                        g.DrawString(TEXT, trt, Drawing.Brushes.Black,
                            new Drawing.RectangleF(
                                (float)pointImage.X,
                                (float)pointImage.Y,
                                (float)(f.Width * 3.3),
                                (float)(f.Height * 3.3)));
                    }
                }

            }

            using (MemoryStream tmpStrm = new MemoryStream())
            {
                b.Save(tmpStrm, System.Drawing.Imaging.ImageFormat.Png);
                vie = Drawing.Image.FromStream(tmpStrm);
            }
            b.Dispose();
            PrintService.ImageCurrent = vie;
            //Tool.Print.dd();
        }

        private void JJson(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пока нет реализации, извините");
            return;

            Helper.JsonSerializeObject(((DataView)DataGridMain.ItemsSource).ToTable());
        }

        private void Pehat(object sender, RoutedEventArgs e)
        {
            ShowMessageBoxAnalitic();
            PrintService.dd(Print_Item2, GetParametrAnalitic, Core.AppSettings.PathLocal);
        }

        private void Poisk(object sender, RoutedEventArgs e) => new Views.Poisk(this).Show();

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

            Point pointImage = new Point();

            Drawing.Bitmap bmp = new Drawing.Bitmap(@"C:\BKR\WKR2\ggh.jpg");
            Drawing.Bitmap b = new Drawing.Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            using (Drawing.Graphics g = Drawing.Graphics.FromImage(b))
            {
                g.DrawImage(bmp, 0, 0);
            }

            using (Drawing.Graphics g = Drawing.Graphics.FromImage(b))
            {
                using (var font = new Drawing.Font("Arial", 10))  // настроить шрифт
                {
                    foreach (var item in CanvasForImage.Children)
                    {
                        if (item is Button)
                        {
                            Button f = item as Button;

                            double pixelWidth = ImageMainControl.Source.Width;
                            double pixelHeight = ImageMainControl.Source.Height;
                            pointImage.X = (pixelWidth * f.Margin.Left) / ImageMainControl.ActualWidth;
                            pointImage.Y = (pixelHeight * f.Margin.Top) / ImageMainControl.ActualHeight;

                            g.DrawString("Информационная конференция IT Corparation", font, Drawing.Brushes.Black,
                       (float)pointImage.X, (float)pointImage.Y);
                        }
                    }

                }
            }
            b.Save(@"C:\BKR\WKR2\gomer1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            bmp.Dispose();
            b.Dispose();
        }

        #endregion

        #region Other Code

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

        //
        //public void Clear_button()
        //{
        //    for (int i = grid_imag.Children.Count-1; i >= 0 ; i--)
        //    {
        //        Button pp = grid_imag.Children[i] as Button;
        //        if (pp != null) grid_imag.Children.Remove(pp);
        //    }
        //}

        #endregion
    }
}
