using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tool
{
    static public class Print
    {
        static public Image iii;
        static private int width;
        static private int height;
        static public void dd()
        {

            var setupDlg = new PageSetupDialog();
            var printDlg = new PrintDialog();
            var printDoc = new PrintDocument();
            printDoc.DocumentName = "Print Document";
           
            //PageSetupDialog settings
            setupDlg.Document = printDoc;

            setupDlg.AllowMargins = false;
            setupDlg.AllowOrientation = false;
            setupDlg.AllowPaper = false;
            setupDlg.AllowPrinter = false;
            setupDlg.Reset();

            setupDlg.PageSettings = new System.Drawing.Printing.PageSettings();

            setupDlg.PrinterSettings =  new System.Drawing.Printing.PrinterSettings();

            if (setupDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.DefaultPageSettings = setupDlg.PageSettings;
                printDoc.PrinterSettings = setupDlg.PrinterSettings;
            }

            var fggg=printDoc.PrinterSettings.LandscapeAngle;
            if (printDlg.ShowDialog() == DialogResult.OK){
                printDoc.PrinterSettings = printDlg.PrinterSettings; 

            }
            //width = printDoc.DefaultPageSettings.PaperSize.Width;
            //height = printDoc.DefaultPageSettings.PaperSize.Height;

            // bip = new Bitmap(printDoc.DefaultPageSettings.PaperSize.Width+1000, printDoc.DefaultPageSettings.PaperSize.Height);
            //using (Graphics ff = Graphics.FromImage(bip))
            //{
            //    ff.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //    ff.DrawImage(iii, 0, 0);
            //    ff.Dispose();
            //}

            printDoc.PrintPage += PrintDoc_PrintPage;
            printDoc.Print();

        }

        private static void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.PageSettings.Margins = new Margins(0,0,0,0);
            var df=e.PageBounds;
            //e.Graphics.DrawString("Приведд уродец", new System.Drawing.Font("Arial", 12),System.Drawing.Brushes.Black,1800,100);
            //e.Graphics.DrawImage(ResizeOrigImg(iii,(int)(df.Width*1.5),(int)(df.Height*1.5) ), 0,0);
            e.Graphics.DrawImage(iii, e.MarginBounds);
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
        public static Bitmap ResizeImage(Image image, int width, int height)
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

        public static Image ResizeOrigImg(Image image, int nWidth, int nHeight)
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
    }
}
