using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
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

namespace WKR2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Point PointImag;
        public Image imageORig;

        public MainWindow()
        {
            InitializeComponent();
            image.Source = new BitmapImage(new Uri(@"C:\BKR\WKR2\ggh.jpg"));
            for (int i = 1; i < 3; i++)
            {
                var but = new Button() { Name = "T_" + i, Content = "**"+i+"**", Margin = new Thickness(0, 0, 0, 0) };
                but.MouseDown += new MouseButtonEventHandler(MouseDown);
                but.MouseUp += new MouseButtonEventHandler(MouseUp);
                but.MouseMove += new MouseEventHandler(MouseMove);
                grid_imag.Children.Add(but);
            }

            imageORig = image;
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
                using (var font = new Dr.Font("Arial", 10))
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

                            g.DrawString("Привет я сделал вкр!!", font, Dr.Brushes.Black,
                       (float)PointImag.X, (float)PointImag.Y);
                        }
                    }
                   
                }
            }
            Dr.Image vie;
            
            using (MemoryStream tmpStrm = new MemoryStream())
            {
                b.Save(tmpStrm, System.Drawing.Imaging.ImageFormat.Png);
                vie = Dr.Image.FromStream(tmpStrm);
            }


            var df1=Convert(vie);



            Perview dd = new Perview(vie);
            dd.ShowDialog();

            b.Save(@"C:\BKR\WKR2\gomer1.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
            
        }

        public BitmapImage Convert(Dr.Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
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
            //Example df = new Example();
            //df.Show();
            //Window dd = new Window();
            //var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
            //stackPanel.Children.Add(new Image
            //{
            //    Source = new BitmapImage(
            //new Uri(@"c:\users\redga\documents\visual studio 2015\Projects\WKR2\WKR2\gomer1.jpg"))
            //});

            //dd.ShowDialog();

            //foreach (var item in stackPanel.Children)
            //{
            //    if (item is Image)
            //    {
            //        var gg = item as Image;
            //        gg.Source = null;
            //    }
            //}
            Perview dd = new Perview();
            dd.ShowDialog();


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
    }
}
