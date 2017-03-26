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

namespace WKR2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Point PointImag;     // координаты кнопки 
        public Image imageORig;     //оригинал загружен
        public Dr.Bitmap bitmapORig;//оригинал загружен

        public MainWindow()
        {
            InitializeComponent();
            //List<Date> fd = new List<Date>() { new Date() { jsdkf = "423", NAme = "dsf4", NAme1 = "fs43" } };

            image.Source = ImageWork.Load();
            imageORig = image;
            bitmapORig = new Dr.Bitmap(ImageWork.Load().UriSource.LocalPath);

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
                    foreach (var item in grid_imag.Children)
                    {
                        if(item is Button)
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

        private void imag_click(object sender, MouseButtonEventArgs e)
        {
            PointImag = e.GetPosition(image);
            double pixelWidth = image.Source.Width;
            double pixelHeight = image.Source.Height;
            PointImag.X = (pixelWidth * PointImag.X ) / image.ActualWidth;
            PointImag.Y = (pixelHeight * PointImag.Y )/ image.ActualHeight;

            point.Content = String.Format("x={0}  Y={1}", PointImag.X, PointImag.Y);
        }

        
        //новое окно 
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Previwe();
        }

        public void Previwe()
        {
            int tt = 0;
            //Dr.Bitmap bitmapORig = this.bitmapORig;
            Dr.Bitmap b = new Dr.Bitmap(bitmapORig.Width+tt, bitmapORig.Height+tt, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Dr.Image vie;
            
             using (Dr.Graphics g = Dr.Graphics.FromImage(b)) { g.DrawImage(bitmapORig, 0, 0);}

            using (Dr.Graphics g = Dr.Graphics.FromImage(b))
            {
                //g.Clear(System.Drawing.Color.White); 
                //g.FillRectangle(Dr.Brushes.White, -50, -50, b.Width, b.Height); //белый листок

                //using (var font = new Dr.Font("Arial", 15))
                //{
                    int i = 0;
                    foreach (var item in grid_imag.Children)
                    {
                        if (item is Button)
                        {
                            Button f = item as Button;

                            double pixelWidth = image.Source.Width;
                            double pixelHeight = image.Source.Height;
                            PointImag.X = (pixelWidth * f.Margin.Left) / image.ActualWidth;
                            PointImag.Y = (pixelHeight * f.Margin.Top) / image.ActualHeight;
                            var yy = (DataView)d12.ItemsSource;
                            string TEXT=(yy.Table.Rows[0].ItemArray[i++]).ToString();

                            g.DrawString(TEXT/*+"123456786543213453421224234234"*/, Tool.Print.font, Dr.Brushes.Black,
                                new Dr.RectangleF((float)PointImag.X, (float)PointImag.Y,200,500));

                       //     g.DrawString(TEXT+"</br> 3efdvgt4", font, Dr.Brushes.Black,
                       //(float)PointImag.X, (float)PointImag.Y);
                        }
                    }

                //}
            }
            b.Save(@"C:\BKR\WKR2\new12.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            using (MemoryStream tmpStrm = new MemoryStream())
            {
                b.Save(tmpStrm, System.Drawing.Imaging.ImageFormat.Png);
                vie = Dr.Image.FromStream(tmpStrm);
                
            }
            b.Dispose();

            Tool.Print.iii = vie;
            View.PreView pre = new View.PreView(vie);
            pre.ShowDialog();
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
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoved)
            {
                Point currentPoint = e.GetPosition(grid_imag);

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

        private void open(object sender, RoutedEventArgs e)
        {
            DataView dd = Tool.ExcelWork.LoadrExcel();
            //if (dd != null) d12.ItemsSource = dd;

            View.SettingView vv = new View.SettingView(dd);
            vv.ShowDialog();
            d12.ItemsSource = null;
            d12.ItemsSource = dd;

            DataView colbot = dd;
            
         
            // сколько кнопок надо на разментку 
            foreach (DataColumn item in colbot.Table.Columns)
           {
                var but = new Button() { Name = item.ColumnName, Content = "**" + item.ColumnName + "**", Margin = new Thickness(0, 0, 0, 0) };
                but.MouseDown += new MouseButtonEventHandler(MouseDown);
                but.MouseUp += new MouseButtonEventHandler(MouseUp);
                but.MouseMove += new MouseEventHandler(MouseMove);
                grid_imag.Children.Add(but);
            }

            //Сохранить в Json Парметр Excel измененого
            //Tool.ExcelWork.Json(((DataView)d12.ItemsSource).ToTable());
        }

        private void JJson(object sender, RoutedEventArgs e)
        {
            Tool.ExcelWork.Json(((DataView)d12.ItemsSource).ToTable());
        }

        private void delete_row(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Delete)
            {
                DataView gg =(DataView)d12.ItemsSource;
                gg.Table.Rows.RemoveAt(d12.SelectedIndex);
                d12.ItemsSource = null;
                d12.ItemsSource = gg;
                //d12.Items.Remove(d12.SelectedItem);// напрямую с ним работать нельзя надо посредника DataTable
            }
        }

        private void Pehat(object sender, RoutedEventArgs e)
        {
           
            Tool.Print.dd();

        }

        private void Calibration_Click(object sender, RoutedEventArgs e)
        {
            WKR2.View.Calibration gf = new View.Calibration( Tool.Print.calibration_data);
            gf.ShowDialog();
        }

        private void Font_click(object sender, RoutedEventArgs e)
        {
            Tool.Print.Font();
        }
    }
}
