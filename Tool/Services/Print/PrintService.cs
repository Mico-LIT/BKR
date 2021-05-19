﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Tool.Services.Analitic;

namespace Tool.Services.Print
{
    public class PrintService
    {
        private static Font fontDefault = new Font("Arial", 15);
        private static Font fontCurrent;

        private static Dictionary<System.Windows.Controls.Button, Font>
            buttonFontDictionary = new Dictionary<System.Windows.Controls.Button, Font>();

        private static string pathLocal;
        private static int Start = 0;
        private static int Stop = 0;
        private static Func<int, int, Image> print_Item2_;
        private static Func<int, string> getParametrAnalitic_;

        public static Calibration_Data CalibrationData { get; set; } = new Calibration_Data() { X = 0, Y = 0 };
        public static Image ImageCurrent { get; set; } //Картинка на печать
        public static Font FontCurrent  //Шрифт
        {
            get { return fontCurrent ?? fontDefault; }
            set { fontCurrent = value; }
        }
        public static Dictionary<System.Windows.Controls.Button, Font> ButtonFontDictionary => buttonFontDictionary;

        public static Font Font(Font font = null)
        {
            using (FontDialog Fontdialog = new FontDialog())
            {
                Fontdialog.Font = font ?? fontDefault;

                if (Fontdialog.ShowDialog() == DialogResult.OK)
                    return Fontdialog.Font;

                return fontDefault;
            }
        }

        public static void Print(Func<int, int, Image> print_Item2, Func<int, string> getParametrAnalitic, string pathLocal, int ii = 1)
        {
            PrintService.pathLocal = pathLocal;

            print_Item2_ = print_Item2;
            getParametrAnalitic_ = getParametrAnalitic;
            var setupDlg = new PageSetupDialog();
            var printDlg = new PrintDialog();
            var printDoc = new PrintDocument();
            printDoc.DocumentName = "Print Document";

            setupDlg.PageSettings = new System.Drawing.Printing.PageSettings();
            setupDlg.PrinterSettings = new System.Drawing.Printing.PrinterSettings();

            setupDlg.PageSettings.Landscape = true;
            setupDlg.PageSettings.Margins = new Margins(0, 0, 0, 0);

            if (setupDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.DefaultPageSettings = setupDlg.PageSettings;
                printDoc.PrinterSettings = setupDlg.PrinterSettings;
            }
            else return;
            printDlg.AllowSomePages = true;
            printDlg.UseEXDialog = true;

            printDlg.PrinterSettings.FromPage = printDlg.PrinterSettings.ToPage = ii;
            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.PrinterSettings = printDlg.PrinterSettings;

            }
            else return;
            Start = printDlg.PrinterSettings.FromPage - 1;
            Stop = printDlg.PrinterSettings.ToPage - 1;
            printDoc.PrintPage += PrintDoc_PrintPage1; ;
            printDoc.Print();
        }

        static void PrintDoc_PrintPage1(object sender, PrintPageEventArgs e)
        {
            if (AnaliticService.GetSettingOnAnalitic)
                AnaliticService.Save_Persont(print_Item2_(Start, 1), getParametrAnalitic_(Start), pathLocal);

            e.Graphics.DrawImage(print_Item2_(Start, 0), new Rectangle()
            {
                Height = e.PageSettings.PaperSize.Width,
                Width = e.PageSettings.PaperSize.Height,
                X = CalibrationData.X,
                Y = CalibrationData.Y,
            });

            if (Start == Stop)
            {
                e.HasMorePages = false;
                Start = Stop = 0;
            }
            else e.HasMorePages = true;
            Start++;
        }

        #region OtherCode

        static void dd(int ii)
        {
            Start = Stop = ii;
            var setupDlg = new PageSetupDialog();
            var printDlg = new PrintDialog();
            var printDoc = new PrintDocument();
            printDoc.DocumentName = "Print Document";
            ////PageSetupDialog settings
            //setupDlg.Document = printDoc;

            //setupDlg.AllowMargins = false;
            //setupDlg.AllowOrientation = false;
            //setupDlg.AllowPaper = false;
            //setupDlg.AllowPrinter = false;
            //setupDlg.Reset();

            setupDlg.PageSettings = new System.Drawing.Printing.PageSettings();
            setupDlg.PrinterSettings = new System.Drawing.Printing.PrinterSettings();

            setupDlg.PageSettings.Landscape = true;
            setupDlg.PageSettings.Margins = new Margins(0, 0, 0, 0);

            if (setupDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.DefaultPageSettings = setupDlg.PageSettings;
                printDoc.PrinterSettings = setupDlg.PrinterSettings;
            }
            else return;
            //printDlg.AllowSomePages = true;
            //printDlg.UseEXDialog = true;
            //var fggg=printDoc.PrinterSettings.LandscapeAngle;
            if (printDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.PrinterSettings = printDlg.PrinterSettings;

            }
            else return;
            //width = printDoc.DefaultPageSettings.PaperSize.Width;
            //height = printDoc.DefaultPageSettings.PaperSize.Height;

            // bip = new Bitmap(printDoc.DefaultPageSettings.PaperSize.Width+1000, printDoc.DefaultPageSettings.PaperSize.Height);
            //using (Graphics ff = Graphics.FromImage(bip))
            //{
            //    ff.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //    ff.DrawImage(iii, 0, 0);
            //    ff.Dispose();
            //}
            //printDoc.DefaultPageSettings.Landscape = true; //Горизонт
            //printDoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);// отступы
            printDoc.PrintPage += PrintDoc_PrintPage1;
            printDoc.Print();

        }

        static void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (ImageCurrent == null)
                throw new ArgumentNullException(nameof(ImageCurrent));

            //e.PageSettings.Margins = new Margins(0,0,0,0);
            //var df=e.PageBounds;
            //e.Graphics.DrawString("Приведд уродец", new System.Drawing.Font("Arial", 12),System.Drawing.Brushes.Black,1800,100);
            //e.Graphics.DrawImage(ResizeOrigImg(iii,(int)(df.Width*1.5),(int)(df.Height*1.5) ), 0,0);
            //e.Graphics.DrawImage(iii, e.MarginBounds);
            e.Graphics.DrawImage(ImageCurrent, new Rectangle()
            {
                Height = e.PageSettings.PaperSize.Width,
                Width = e.PageSettings.PaperSize.Height,
                X = CalibrationData.X,
                Y = CalibrationData.Y,
            });
            //Rectangle m = e.MarginBounds;

            //if ((double)iii.Width / (double)iii.Height > (double)m.Width / (double)m.Height) // image is wider
            //{
            //    m.Height = (int)((double)iii.Height / (double)iii.Width * (double)m.Width);
            //}
            //else
            //{
            //    m.Width = (int)((double)iii.Width / (double)iii.Height * (double)m.Height);
            //}
            //e.Graphics.DrawImage(iii, m);

        }

        static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        static Image ResizeOrigImg(Image image, int nWidth, int nHeight)
        {
            int newWidth, newHeight;
            var coefH = (double)nHeight / (double)image.Height;
            var coefW = (double)nWidth / (double)image.Width;
            if (coefW >= coefH)
            {
                newHeight = (int)(image.Height * coefH);
                newWidth = (int)(image.Width * coefH);
            }
            else
            {
                newHeight = (int)(image.Height * coefW);
                newWidth = (int)(image.Width * coefW);
            }

            Image result = new Bitmap(nWidth, nHeight);
            using (var g = Graphics.FromImage(result))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(image, 0, 0, nWidth, nHeight);
                g.Dispose();
            }
            return result;
        }

        #endregion

    }
}
