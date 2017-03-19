using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tool
{
    static public class Print
    {
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

            if (setupDlg.ShowDialog()==DialogResult.OK)
            {

                printDoc.DefaultPageSettings =
                setupDlg.PageSettings;
                printDoc.PrinterSettings =
                setupDlg.PrinterSettings;
            }
        }
    }
}
