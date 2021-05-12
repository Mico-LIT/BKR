using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Tool.Services.Analitic.Models;

namespace Tool.Services.Analitic
{
    public class AnaliticService
    {
        public static bool GetSettingOnAnalitic { get; set; }

        static public Params PARAMS = new Params();

        internal static void Save_Persont(Image image, string str, string pathLocal)
        {
            DirectoryInfo di = Directory.CreateDirectory(Path.Combine(pathLocal, @"\Analitic"));
            string path = string.Format(di.FullName + @"\{0}.jpeg", str);

            if (File.Exists(path))
            {
                str = "Repeat" + DateTime.Now.TimeOfDay.Milliseconds.ToString() + "_" + str;
                path = string.Format(di.FullName + @"\{0}.jpeg", str);
                image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
                image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
