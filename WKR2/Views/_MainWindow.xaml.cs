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
        Dictionary<int, Drawing.Font> hashCodeButtonsOncanvas = new Dictionary<int, Drawing.Font>();

        readonly AnaliticService analiticService;
        readonly ExcelService excelService;
        readonly PrintService printService;

        public _MainWindow()
        {
            InitializeComponent();
            IssEnabledAllElementsControl();

            Directory.CreateDirectory(AppSettings.PathPattern);
            Directory.CreateDirectory(AppSettings.PathAnalytic);

            analiticService = new AnaliticService(AppSettings.PathAnalytic);
            excelService = new ExcelService();
            printService = new PrintService();

            printService.NotificationPrint += PrintService_NotificationPrint;
            printService.GetImage += (index, useBackgroundImage) => Preview(index, useBackgroundImage);
        }

        private void PrintService_NotificationPrint(int index)
        {
            if (analiticService.GetSettingOnAnalitic == true)
            {
                if (DataGridMain.ItemsSource == null) throw new InvalidOperationException();

                var dataRowCollencion = ((DataView)DataGridMain.ItemsSource).Table.Rows[index];
                int columnIndex1 = analiticService.PARAMS.Params_1;
                int columnIndex2 = analiticService.PARAMS.Params_2;

                string value1 = dataRowCollencion.ItemArray[columnIndex1].ToString();
                string value2 = dataRowCollencion.ItemArray[columnIndex2].ToString();

                string fileName = string.Format("{0}_{1}_{2}_", value1, value2, DateTime.Now.ToShortDateString());

                Drawing.Image image = Preview(index);

                analiticService.Save_Persont(image, fileName);
            }
        }

        private Drawing.Image Preview(int rowIndex = 0, bool useBackgroundImage = true)
        {
            try
            {
                if (rowIndex == -1) return null;

                if (bitmapImageOriginal == null)
                {
                    MessageBox.Show("Нужно загрузить шаблон\\картинку для работы", "Информация");
                    return null;
                }

                Drawing.Image drawingImage;

                using (Drawing.Bitmap bitmap = new Drawing.Bitmap(bitmapImageOriginal))
                {
                    using (Drawing.Graphics graphics = Drawing.Graphics.FromImage(bitmap))
                    {
                        if (!useBackgroundImage) graphics.Clear(Drawing.Color.White);

                        Point pointImage = new Point();

                        foreach (var itemUI in CanvasForImage.Children.OfType<Button>())
                        {
                            Button buttonOnCanvas = itemUI;

                            double pixelWidth = ((BitmapSource)ImageMainControl.Source).PixelWidth;
                            double pixelHeight = ((BitmapSource)ImageMainControl.Source).PixelHeight;
                            pointImage.X = (pixelWidth * buttonOnCanvas.Margin.Left) / ImageMainControl.ActualWidth;
                            pointImage.Y = (pixelHeight * buttonOnCanvas.Margin.Top) / ImageMainControl.ActualHeight;

                            DataView sourceDGM = (DataView)DataGridMain.ItemsSource;
                            int columnIndex = sourceDGM.Table.Columns.IndexOf(buttonOnCanvas.Name);

                            string text = sourceDGM.Table.Rows[rowIndex].ItemArray[columnIndex].ToString();

                            Drawing.Font font = hashCodeButtonsOncanvas.FirstOrDefault(x => x.Key == itemUI.GetHashCode()).Value;

                            if (font == null)
                                font = printService.FontCurrent;

                            const float magicNumber = 3.3f;

                            graphics.DrawString(text, font, Drawing.Brushes.Black, new Drawing.RectangleF
                                (
                                    (float)pointImage.X,
                                    (float)pointImage.Y,
                                    (float)(buttonOnCanvas.Width * magicNumber),
                                    (float)(buttonOnCanvas.Height * magicNumber))
                                );
                        }
                    }

                    bitmap.Save(System.IO.Path.Combine(AppSettings.PathLocal, "PreView.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                    using (MemoryStream tmpStrm = new MemoryStream())
                    {
                        bitmap.Save(tmpStrm, System.Drawing.Imaging.ImageFormat.Png);
                        drawingImage = Drawing.Image.FromStream(tmpStrm);
                    }
                }

                return drawingImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                throw;
            }
        }

        private void ShowMessageBoxAnalitic()
        {
            if (analiticService.GetSettingOnAnalitic == null || analiticService.GetSettingOnAnalitic == false)
            {
                MessageBoxResult result = MessageBox.Show("Включить аналитик?!", "Внимание!", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.No:
                        analiticService.GetSettingOnAnalitic = false;
                        break;
                    case MessageBoxResult.Yes:
                        analiticService.GetSettingOnAnalitic = true;
                        break;
                    default:
                        analiticService.GetSettingOnAnalitic = null;
                        break;
                }
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
                MIOpenImage.IsEnabled = MIDownloadPattern.IsEnabled = MIGroupAnalitic.IsEnabled = MIPreView.IsEnabled = true;

                MISavePattent.IsEnabled = (ImageMainControl.Source != null);

                foreach (MenuItem item in DataGridMain.ContextMenu.Items)
                    item.IsEnabled = true;
            }
            else
            {
                MIOpenImage.IsEnabled = MIDownloadPattern.IsEnabled = MISavePattent.IsEnabled =
                  MIGroupAnalitic.IsEnabled = MIPreView.IsEnabled = false;

                foreach (MenuItem item in DataGridMain.ContextMenu.Items)
                    item.IsEnabled = false;
            }
        }

        private void ButtonAddOnCanvas(string nameButton, Thickness? thickness = null)
        {
            Button button = new Button()
            {
                Name = nameButton,
                Height = 20,
                Width = 100,
                Content = "**" + nameButton + "**",
                Margin = thickness ?? new Thickness(0, 0, 0, 0)
            };

            button.PreviewMouseDown += new MouseButtonEventHandler(MouseDown);
            button.PreviewMouseUp += new MouseButtonEventHandler(MouseUp);
            button.PreviewMouseMove += new MouseEventHandler(MouseMove);
            button.MouseRightButtonDown += (sender, e) =>
            {
                Button buttonCurrent = sender as Button;
                var windowButtonCalibration = new Views.Button_Calibration(buttonCurrent, hashCodeButtonsOncanvas, printService);
                windowButtonCalibration.ShowDialog();
                e.Handled = true;
            };

            CanvasForImage.Children.Add(button);
            hashCodeButtonsOncanvas.Add(button.GetHashCode(), printService.FontCurrent);
        }

        #region Mouse Event

        bool isMoved = false;

        private new void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isMoved = true;
                Point startMovePosition = e.GetPosition(this);
            }
        }
        private new void MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                isMoved = false;
            }
        }
        private new void MouseMove(object sender, MouseEventArgs e)
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
        private void DataGridMainButtonCM_MouseRightButtonUp(object sender, MouseButtonEventArgs e) => e.Handled = true;

        private void DataGridMainButtonCM_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
                button.ContextMenu.IsOpen = true;
        }

        private void DataGridMain_ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            //var contextMenu = (ContextMenu)sender;

            //if (contextMenu == null)
            //    throw new InvalidOperationException();
        }

        private void DataGridMain_Button_PrintItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bitmapImageOriginal == null)
                {
                    MessageBox.Show("Нужно загрузить шаблон\\картинку для работы", "Информация");
                    return;
                }

                var selectedIndex = DataGridMain.SelectedIndex + 1;

                ShowMessageBoxAnalitic();

                printService.Print(selectedIndex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DataGridMain_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DataView dataView = (DataView)DataGridMain.ItemsSource;
                dataView.Table.Rows.RemoveAt(DataGridMain.SelectedIndex);
                DataGridMain.ItemsSource = null;
                DataGridMain.ItemsSource = dataView;
            }
        }

        #endregion

        #region MenuItem

        private void MenuItem_Calibration_Click(object sender, RoutedEventArgs e)
            => new Views.Calibration(printService.CalibrationData).ShowDialog();
        private void MenuItem_Font_click(object sender, RoutedEventArgs e) => printService.Font(updateFontCurrent: true);
        private void MenuItem_PreView_Click(object sender, RoutedEventArgs e)
        {
            var image = Preview();
            Views.PreView pre = new Views.PreView(image) { WindowState = WindowState.Maximized };
            pre.ShowDialog();
        }
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) => this.Close();
        private void MenuItem_GroupFile_MouseMove(object sender, MouseEventArgs e) => IssEnabledAllElementsControl();

        private void MenuItem_DGM_PreView(object sender, RoutedEventArgs e)
        {
            var image = Preview(DataGridMain.SelectedIndex);
            Views.PreView pre = new Views.PreView(image) { WindowState = WindowState.Maximized };
            pre.ShowDialog();
        }
        private void MenuItem_DGM_AddConvasOnButton(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGridMain.CurrentCell.Column == null)
                    return;

                var coll = DataGridMain.CurrentCell.Column;

                if (coll.DisplayIndex <= 0)
                    return;

                string nameButton = (string)coll.Header;
                DataGridMain.SelectedIndex = -1;
                ButtonAddOnCanvas(nameButton);
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
                string nameButton = (string)DataGridMain.CurrentCell.Column.Header;
                var buttons = CanvasForImage.Children.OfType<Button>();

                var buttonOnCanvas = buttons.FirstOrDefault(x => x.Name == nameButton);
                if (buttonOnCanvas == null) return;

                var hashCodeButton = hashCodeButtonsOncanvas.Keys.ToList().FindLast(x => x == buttonOnCanvas.GetHashCode());

                CanvasForImage.Children.Remove(buttonOnCanvas);
                hashCodeButtonsOncanvas.Remove(hashCodeButton);

                DataGridMain.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source);
                DataGridMain.SelectedIndex = -1;
            }
        }

        private void MenuItem_OpenExampleWorkProgram_Click(object sender, RoutedEventArgs e)
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
                using (Stream stream = assembly.GetManifestResourceStream(AppSettings.ResourceNameImageTemplate))
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

                hashCodeButtonsOncanvas.Clear();
                CanvasForImage.Children.Clear();

                this.ButtonAddOnCanvas("Column1", new Thickness(200, 140, 0, 0));
                this.ButtonAddOnCanvas("Column2", new Thickness(200, 160, 0, 0));

                IssEnabledAllElementsControl();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке!");
                throw ex;
            }
        }

        private void MenuItem_OpenImage_Click(object sender, RoutedEventArgs e)
        {
            var windowAnaliticSettings = new Views.AnalliticSetings(analiticService,
                DataGridMain.Columns.OfType<DataGridTextColumn>().Select(x => x.Header.ToString()).ToList());

            windowAnaliticSettings.ShowDialog();

            const string filter = "Jpeg files (*.jpeg)|*.jpeg;*.jpg| PNG files (*.PNG)|*.png|Все файлы (*.*)|*.*";
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = filter };

            if (openFileDialog.ShowDialog() == true)
            {
                Uri uri = new Uri(openFileDialog.FileName);

                ImageMainControl.Source = new BitmapImage(uri);
                bitmapImageOriginal = new Drawing.Bitmap(uri.LocalPath);
                hashCodeButtonsOncanvas.Clear();
                CanvasForImage.Children.Clear();
            }
        }

        private void MenuItem_OpenExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataView dataView = excelService.LoadrExcel();

                if (dataView == null)
                    return;

                Views.SettingView windowSettinView = new Views.SettingView(dataView);
                if (windowSettinView.ShowDialog() == true)
                {
                    CanvasForImage.Children.Clear();
                    DataGridMain.ItemsSource = null;
                    DataGridMain.ItemsSource = dataView;
                }

                IssEnabledAllElementsControl();
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
                const string filter = "Dat files (*.dat)|*.dat";
                string path = AppSettings.PathPattern;

                OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = filter, InitialDirectory = path };

                if (openFileDialog.ShowDialog() == true)
                {
                    hashCodeButtonsOncanvas.Clear();
                    CanvasForImage.Children.Clear();

                    DataPattern dataPatternModel = Helper.DeSerializationDataPattern(openFileDialog.FileName);

                    printService.Font(dataPatternModel.Font, isDialog: false, updateFontCurrent: true);
                    printService.CalibrationData = dataPatternModel.CalibrationData;
                    analiticService.PARAMS = dataPatternModel.Params;

                    BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
                    (
                        dataPatternModel.Image.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions()
                    );

                    ImageMainControl.Source = bitmapSource;

                    foreach (SettingButton item in dataPatternModel.SettingButtons)
                    {
                        Button button = new Button()
                        {
                            Name = item.Name,
                            Height = item.Height,
                            Width = item.Width,
                            Content = "**" + item.Name + "**",
                            Margin = new Thickness(item.MarginL, item.MarginT, item.MarginR, item.MarginB)
                        };

                        button.PreviewMouseDown += new MouseButtonEventHandler(MouseDown);
                        button.PreviewMouseUp += new MouseButtonEventHandler(MouseUp);
                        button.PreviewMouseMove += new MouseEventHandler(MouseMove);
                        button.MouseRightButtonDown += (_sender, _e) =>
                        {
                            Button buttonCurrent = _sender as Button;
                            new Views.Button_Calibration(buttonCurrent, hashCodeButtonsOncanvas, printService).ShowDialog();
                            e.Handled = true;
                        };

                        Drawing.Font font = item.Font ?? printService.FontCurrent;

                        CanvasForImage.Children.Add(button);
                        hashCodeButtonsOncanvas.Add(button.GetHashCode(), font);
                    }

                    bitmapImageOriginal = new Drawing.Bitmap(dataPatternModel.Image);
                    MessageBox.Show("Загрузка прошла успешно");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при загрузки", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_SettingAnalitic_Click(object sender, RoutedEventArgs e)
            => new Views.AnalliticSetings(analiticService,
                DataGridMain.Columns.OfType<DataGridTextColumn>().Select(x => x.Header.ToString()).ToList()).ShowDialog();

        private void MenuItem_ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            excelService.ExportToExcel(Core.AppSettings.PathAnalytic);
            MessageBox.Show("Выгрузка прошла успешно!");
        }

        private void MenuItem_Save_Serial(object sender, RoutedEventArgs e)
        {
            try
            {
                const string filter = "Dat files (*.dat)|*.dat";
                string path = AppSettings.PathPattern;

                SaveFileDialog saveFileDialog = new SaveFileDialog() { InitialDirectory = path, Filter = filter };

                if (saveFileDialog.ShowDialog() == true)
                {
                    List<SettingButton> SettingButtons = new List<SettingButton>();

                    foreach (UIElement item in CanvasForImage.Children)
                    {
                        var button = (Button)item;

                        SettingButtons.Add(new SettingButton()
                        {
                            Font = hashCodeButtonsOncanvas.FirstOrDefault(x => x.Key == button.GetHashCode()).Value,
                            Name = button.Name,

                            Width = button.Width,
                            Height = button.Height,

                            MarginB = button.Margin.Bottom,
                            MarginL = button.Margin.Left,
                            MarginR = button.Margin.Right,
                            MarginT = button.Margin.Top,
                        });
                    }

                    DataPattern dataPattern = new DataPattern()
                    {
                        Font = printService.FontCurrent,
                        Image = new Drawing.Bitmap(bitmapImageOriginal),
                        Params = analiticService.PARAMS,
                        SettingButtons = SettingButtons,
                        CalibrationData = printService.CalibrationData,
                    };

                    Helper.SerializationDataPattern(dataPattern, saveFileDialog.FileName);

                    MessageBox.Show("Сохранение прошло успешно");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                throw ex;
            }
        }

        #endregion

        #region Other Code

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

                        var trt = hashCodeButtonsOncanvas.FirstOrDefault(x => f.GetHashCode() == x.Key).Value;
                        if (trt == null) { trt = printService.FontCurrent; }

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
        }

        private void JJson(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пока нет реализации, извините");
            return;

            //Helper.JsonSerializeObject(((DataView)DataGridMain.ItemsSource).ToTable());
        }

        private void Poisk(object sender, RoutedEventArgs e) => new Views.Poisk(this).Show();

        private void Save_img(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пока нет реализации, извините");
            return;

            //Point pointImage = new Point();

            //Drawing.Bitmap bmp = new Drawing.Bitmap(@"C:\BKR\WKR2\ggh.jpg");
            //Drawing.Bitmap b = new Drawing.Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            //using (Drawing.Graphics g = Drawing.Graphics.FromImage(b))
            //{
            //    g.DrawImage(bmp, 0, 0);
            //}

            //using (Drawing.Graphics g = Drawing.Graphics.FromImage(b))
            //{
            //    using (var font = new Drawing.Font("Arial", 10))  // настроить шрифт
            //    {
            //        foreach (var item in CanvasForImage.Children)
            //        {
            //            if (item is Button)
            //            {
            //                Button f = item as Button;

            //                double pixelWidth = ImageMainControl.Source.Width;
            //                double pixelHeight = ImageMainControl.Source.Height;
            //                pointImage.X = (pixelWidth * f.Margin.Left) / ImageMainControl.ActualWidth;
            //                pointImage.Y = (pixelHeight * f.Margin.Top) / ImageMainControl.ActualHeight;

            //                g.DrawString("Информационная конференция IT Corparation", font, Drawing.Brushes.Black,
            //           (float)pointImage.X, (float)pointImage.Y);
            //            }
            //        }

            //    }
            //}
            //b.Save(@"C:\BKR\WKR2\gomer1.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //bmp.Dispose();
            //b.Dispose();
        }

        private Drawing.Image Print_Item(int rowIndex, int rez = 0)
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
                        string TEXT = (yy.Table.Rows[rowIndex].ItemArray[i]).ToString();

                        var trt = hashCodeButtonsOncanvas.FirstOrDefault(x => f.GetHashCode() == x.Key).Value;
                        if (trt == null) { trt = printService.FontCurrent; }

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

        #endregion

        #region Other Code

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
