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
        public static bool? GetSettingOnAnalitic { get; set; }

        static public Params PARAMS = new Params();

        internal static void Save_Persont(Image image, string nameFile, string pathLocal)
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory(pathLocal);
            string path = string.Format(@"{0}\{1}.jpeg", directoryInfo.FullName, nameFile);

            if (File.Exists(path))
            {
                nameFile = "Repeat" + DateTime.Now.TimeOfDay.Milliseconds.ToString() + "_" + nameFile;
                path = string.Format(@"{0}\{1}.jpeg", directoryInfo.FullName, nameFile);
            }

            image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
